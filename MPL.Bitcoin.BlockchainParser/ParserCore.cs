using System;
using System.IO;

namespace MPL.Bitcoin.BlockchainParser
{
    /// <summary>
    /// A class that defines the core functionality of a parser.
    /// </summary>
    /// <typeparam name="T">A T that is the type of the object being parser.</typeparam>
    internal class ParserCore<T>
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of the class with the specified parameters.
        /// </summary>
        /// <param name="parser">A Func of input BlockchainStream and output T that is the parser.</param>
        /// <param name="objectName">A string containing the name of the object being parsed.</param>
        internal ParserCore(Func<BlockchainStream, T> parser, string objectName)
        {
            _parser = parser;
            _objectName = objectName;
        }

        #endregion

        #region Declarations
        #region _Members_
        private readonly string _objectName;
        private readonly Func<BlockchainStream, T> _parser;

        #endregion
        #endregion

        #region Methods
        #region _Internal_
        /// <summary>
        /// Parses a T from the specified data.
        /// </summary>
        /// <param name="data">An array of byte containing the data to parse.</param>
        /// <exception cref="System.ArgumentException">The specified data is NULL, zero-length, or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The object could not be parsed from the specified data.</exception>
        /// <returns>A T parsed from the data.</returns>
        internal T Parse(byte[] data)
        {
            T returnValue = default;

            // Verify params
            if (data == null || data.Length == 0) throw new ArgumentException("The specified data is NULL or zero-length", nameof(data));

            try
            {
                using MemoryStream stream = new MemoryStream(data);
                returnValue = Parse(stream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unable to parse the {_objectName} from the specified data", ex);
            }

            return returnValue;
        }

        /// <summary>
        /// Parses a T from the specified stream.
        /// </summary>
        /// <param name="stream">A Stream containing the data to parse.</param>
        /// <exception cref="System.ArgumentException">The specified stream is NULL or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The object could not be parsed from the specified stream.</exception>
        /// <returns>A T parsed from the stream.</returns>
        internal T Parse(Stream stream)
        {
            T returnValue = default;

            // Verify params
            if (stream == null) throw new ArgumentException("The specified stream is NULL", nameof(stream));

            try
            {
                BlockchainStream blockDataStream;

                // Wrap the stream
                using (blockDataStream = new BlockchainStream(stream))
                    returnValue = _parser(blockDataStream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unable to parse the {_objectName} from the specified stream", ex);
            }

            return returnValue;
        }

        /// <summary>
        /// Tries to parse a T from the specified stream.
        /// </summary>
        /// <param name="stream">A BlockchainStream to read data from.</param>
        /// <param name="output">A T that will be set to the parsed object.</param>
        /// <returns>A bool indicating whether the T was parsed from the stream.</returns>
        internal bool TryParse(BlockchainStream stream, out T output)
        {
            bool returnValue = false;

            // Defaults
            output = default;

            try
            {
                output = _parser(stream);
                returnValue = true;
            }
            catch (Exception)
            { }

            return returnValue;
        }

        #endregion
        #endregion
    }
}
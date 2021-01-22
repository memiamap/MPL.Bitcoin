using System;
using System.IO;

namespace MPL.Bitcoin.BlockchainParser
{
    /// <summary>
    /// A class that provides functionality to parse an output.
    /// </summary>
    public static class OutputParser
    {
        #region Constructors
        static OutputParser()
        {
            _parserCore = new ParserCore<Output>(Parse, "output");
        }

        #endregion

        #region Declarations
        #region _Members_
        private readonly static ParserCore<Output> _parserCore;

        #endregion
        #endregion

        #region Methods
        #region _Internal_
        /// <summary>
        /// Parses an output from the specified stream.
        /// </summary>
        /// <param name="stream">A BlockDataStream containing the data to load.</param>
        /// <exception cref="System.ArgumentException">The specified stream is NULL or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The output could not be parsed from the specified stream.</exception>
        /// <returns>An Output parsed from the stream.</returns>
        internal static Output Parse(BlockchainStream stream)
        {
            Output returnValue;

            // Verify params
            if (stream == null) throw new ArgumentException("The specified stream is NULL", nameof(stream));

            // Get the value of the output
            if (stream.TryReadLong(out long value))
            {
                // Get the size of the redeem script
                if (stream.TryReadVarInt(out VarInt scriptPubKeySize) && scriptPubKeySize.AsInt32 > 0)
                {
                    // Get the signature script
                    if (stream.TryReadBytes(scriptPubKeySize.AsInt32, out byte[] scriptPubKey))
                    {
                        returnValue = new Output(value, scriptPubKey);
                    }
                    else
                        throw new InvalidOperationException("The redeem script could not be parsed");
                }
                else
                    throw new InvalidOperationException("The size of the redeem script could not be parsed");
            }
            else
                throw new InvalidOperationException("The output value could not be parsed");

            return returnValue;
        }


        /// <summary>
        /// Tries to parse an output from the specified stream.
        /// </summary>
        /// <param name="stream">A BlockchainStream to read data from.</param>
        /// <param name="parsedObject">An Output that will be set to the parsed output.</param>
        /// <returns>A bool indicating whether the output was parsed from the stream.</returns>
        internal static bool TryParse(BlockchainStream stream, out Output parsedObject)
        {
            return _parserCore.TryParse(stream, out parsedObject);
        }

        #endregion
        #region _Public_
        /// <summary>
        /// Parses an output from the specified data.
        /// </summary>
        /// <param name="data">An array of byte containing the data to parse.</param>
        /// <exception cref="System.ArgumentException">The specified data is NULL, zero-length, or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The output could not be parsed from the specified data.</exception>
        /// <returns>An Output parsed from the data.</returns>
        public static Output Parse(byte[] data)
        {
            return _parserCore.Parse(data);
        }

        /// <summary>
        /// Parses an output from the specified stream.
        /// </summary>
        /// <param name="stream">A Stream containing the data to parse.</param>
        /// <exception cref="System.ArgumentException">The specified stream is NULL or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The output could not be parsed from the specified stream.</exception>
        /// <returns>An Output parsed from the stream.</returns>
        public static Output Parse(Stream stream)
        {
            return _parserCore.Parse(stream);
        }

        #endregion
        #endregion
    }
}
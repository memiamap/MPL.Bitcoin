using System;
using System.IO;

namespace MPL.Bitcoin.BlockchainParser
{
    /// <summary>
    /// A class that provides functionality to parse an input.
    /// </summary>
    public static class InputParser
    {
        #region Constructors
        static InputParser()
        {
            _parserCore = new ParserCore<Input>(Parse, "input");
        }

        #endregion

        #region Declarations
        #region _Members_
        private readonly static ParserCore<Input> _parserCore;

        #endregion
        #endregion

        #region Methods
        #region _Internal_
        /// <summary>
        /// Parses an input from the specified stream.
        /// </summary>
        /// <param name="stream">A BlockDataStream containing the data to load.</param>
        /// <exception cref="System.ArgumentException">The specified stream is NULL or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The input could not be parsed from the specified stream.</exception>
        /// <returns>An Input parsed from the stream.</returns>
        internal static Input Parse(BlockchainStream stream)
        {
            Input returnValue;

            // Verify params
            if (stream == null) throw new ArgumentException("The specified stream is NULL", nameof(stream));

            // Get the previous transaction
            if (stream.TryReadBytes(32, out byte[] transaction))
            {
                // Get output index
                if (stream.TryReadUInt(out uint outputID))
                {
                    // Get the size of the signature script
                    if (stream.TryReadVarInt(out VarInt scriptSigSize) && scriptSigSize.AsInt32 > 0)
                    {
                        // Get the signature script
                        if (stream.TryReadBytes(scriptSigSize.AsInt32, out byte[] scriptSig))
                        {
                            // Get the sequence
                            if (stream.TryReadUInt(out uint sequence))
                            {
                                returnValue = new Input(transaction, outputID, scriptSig, sequence);
                            }
                            else
                                throw new InvalidOperationException("The sequence could not be parsed");
                        }
                        else
                            throw new InvalidOperationException("The signature script could not be parsed");
                    }
                    else
                        throw new InvalidOperationException("The size of the signature script could not be parsed");
                }
                else
                    throw new InvalidOperationException("The number of inputs could not be parsed");
            }
            else
                throw new InvalidOperationException("The previous transaction could not be parsed");

            return returnValue;
        }

        /// <summary>
        /// Tries to parse an input from the specified stream.
        /// </summary>
        /// <param name="stream">A BlockchainStream to read data from.</param>
        /// <param name="parsedObject">An Input that will be set to the parsed input.</param>
        /// <returns>A bool indicating whether the input was parsed from the stream.</returns>
        internal static bool TryParse(BlockchainStream stream, out Input parsedObject)
        {
            return _parserCore.TryParse(stream, out parsedObject);
        }

        #endregion
        #region _Public_
        /// <summary>
        /// Parses an input from the specified data.
        /// </summary>
        /// <param name="data">An array of byte containing the data to parse.</param>
        /// <exception cref="System.ArgumentException">The specified data is NULL, zero-length, or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The input could not be parsed from the specified data.</exception>
        /// <returns>An Input parsed from the data.</returns>
        public static Input Parse(byte[] data)
        {
            return _parserCore.Parse(data);
        }

        /// <summary>
        /// Parses an input from the specified stream.
        /// </summary>
        /// <param name="stream">A Stream containing the data to parse.</param>
        /// <exception cref="System.ArgumentException">The specified stream is NULL or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The input could not be parsed from the specified stream.</exception>
        /// <returns>An Input parsed from the stream.</returns>
        public static Input Parse(Stream stream)
        {
            return _parserCore.Parse(stream);
        }

        #endregion
        #endregion
    }
}
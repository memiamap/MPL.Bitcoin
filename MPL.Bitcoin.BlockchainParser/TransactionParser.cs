using System;
using System.IO;

namespace MPL.Bitcoin.BlockchainParser
{
    /// <summary>
    /// A class that provides functionality to parse a transaction.
    /// </summary>
    public static class TransactionParser
    {
        #region Constructors
        static TransactionParser()
        {
            _parserCore = new ParserCore<Transaction>(Parse, "transaction");
        }

        #endregion

        #region Declarations
        #region _Members_
        private readonly static ParserCore<Transaction> _parserCore;

        #endregion
        #endregion

        #region Methods
        #region _Internal_
        /// <summary>
        /// Parses a transaction from the specified stream.
        /// </summary>
        /// <param name="stream">A BlockchainStream containing the data to load.</param>
        /// <exception cref="System.ArgumentException">The specified stream is NULL or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The transaction could not be parsed from the specified stream.</exception>
        /// <returns>A Transaction parsed from the stream.</returns>
        internal static Transaction Parse(BlockchainStream stream)
        {
            Transaction returnValue;

            // Verify params
            if (stream == null) throw new ArgumentException("The specified stream is NULL", nameof(stream));

            // Get the transaction version
            if (stream.TryReadInt(out int version))
            {
                // Get number of inputs
                if (stream.TryReadVarInt(out VarInt inputCount) && inputCount.AsInt64 > 0)
                {
                    InputList inputs;

                    // Load inputs
                    inputs = new InputList();
                    for (long i = 0; i < inputCount.AsInt64; i++)
                    {
                        Input nextInput;
                        
                        nextInput = InputParser.Parse(stream);
                        inputs.Add(nextInput);
                    }

                    // Get number of outputs
                    if (stream.TryReadVarInt(out VarInt outputCount) && outputCount.AsInt64 > 0)
                    {
                        OutputList outputs;

                        // Load outputs
                        outputs = new OutputList();
                        for (long i = 0; i < outputCount.AsInt64; i++)
                        {
                            Output nextOutput;

                            nextOutput = OutputParser.Parse(stream);
                            outputs.Add(nextOutput);
                        }

                        if (stream.TryReadUInt(out uint rawLockTime))
                        {
                            LockTime lockTime;

                            lockTime = ParseLockTime(rawLockTime);
                            returnValue = new Transaction(version, lockTime, inputs, outputs);
                        }
                        else
                            throw new InvalidOperationException("The locktime could not be parsed");
                    }
                    else
                        throw new InvalidOperationException("The number of outputs could not be parsed");
                }
                else
                    throw new InvalidOperationException("The number of inputs could not be parsed");
            }
            else
                throw new InvalidOperationException("The transaction version could not be parsed");

            return returnValue;
        }

        /// <summary>
        /// Tries to parse a transaction from the specified stream.
        /// </summary>
        /// <param name="stream">A BlockchainStream to read data from.</param>
        /// <param name="parsedObject">A Transaction that will be set to the parsed transaction.</param>
        /// <returns>A bool indicating whether the transaction was parsed from the stream.</returns>
        internal static bool TryParse(BlockchainStream stream, out Transaction parsedObject)
        {
            return _parserCore.TryParse(stream, out parsedObject);
        }

        #endregion
        #region _Private_
        private static LockTime ParseLockTime(byte[] data)
        {
            if (data == null || data.Length == 0 || data.Length > 4) throw new ArgumentException("The specified data is NULL, empty, or invalid", nameof(data));
            return ParseLockTime(BitConverter.ToUInt32(data, 0));
        }
        private static LockTime ParseLockTime(uint data)
        {
            LockTime returnValue;
            LockTimeType targetLockTime;

            if (data == 0)
                targetLockTime = LockTimeType.NoLockTime;
            else if (data < 500000000)
                targetLockTime = LockTimeType.BlockHeight;
            else
                targetLockTime = LockTimeType.Timestamp;

            returnValue = new LockTime(targetLockTime, data);

            return returnValue;
        }

        #endregion
        #region _Public_
        /// <summary>
        /// Parses a transaction from the specified data.
        /// </summary>
        /// <param name="data">An array of byte containing the data to parse.</param>
        /// <exception cref="System.ArgumentException">The specified data is NULL, zero-length, or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The transaction could not be parsed from the specified data.</exception>
        /// <returns>A Transaction parsed from the data.</returns>
        public static Transaction Parse(byte[] data)
        {
            return _parserCore.Parse(data);
        }

        /// <summary>
        /// Parses a transaction from the specified stream.
        /// </summary>
        /// <param name="stream">A Stream containing the data to parse.</param>
        /// <exception cref="System.ArgumentException">The specified stream is NULL or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The transaction could not be parsed from the specified stream.</exception>
        /// <returns>A Transaction parsed from the stream.</returns>
        public static Transaction Parse(Stream stream)
        {
            return _parserCore.Parse(stream);
        }

        #endregion
        #endregion
    }
}
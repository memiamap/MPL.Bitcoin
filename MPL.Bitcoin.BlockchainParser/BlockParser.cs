using System;
using System.IO;

namespace MPL.Bitcoin.BlockchainParser
{
    /// <summary>
    /// A class that provides functionality to parse a block.
    /// </summary>
    public static class BlockParser
    {
        #region Constructors
        static BlockParser()
        {
            _parserCore = new ParserCore<Block>(Parse, "block");
        }

        #endregion

        #region Declarations
        #region _Members_
        private readonly static ParserCore<Block> _parserCore;

        #endregion
        #endregion

        #region Methods
        #region _Internal_
        /// <summary>
        /// Parses a block from the specified stream.
        /// </summary>
        /// <param name="stream">A BlockchainStream to read data from.</param>
        /// <exception cref="System.ArgumentException">The specified stream is NULL or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The block could not be parsed from the specified stream.</exception>
        /// <returns>A Block parsed from the stream.</returns>
        internal static Block Parse(BlockchainStream stream)
        {
            Block returnValue;

            // Verify params
            if (stream == null) throw new ArgumentException("The specified stream is NULL", nameof(stream));

            // Read the magic number
            if (stream.TryReadUInt(out uint magicNumber))
            {
                BitcoinNetwork network;

                // Test the magic number
                network = ParseMagicNumber(magicNumber);
                if (network != BitcoinNetwork.Undefined)
                {
                    // Get block data
                    if (stream.TryReadInt(out int blockSize) && blockSize > 80)
                    {
                        // Try to load the block header
                        if (TryParseBlockHeader(stream, network, blockSize, out returnValue))
                        {
                            // Get transaction count
                            if (stream.TryReadVarInt(out VarInt transactionCount) && transactionCount.AsInt64 > 0)
                            {
                                // Process transactions
                                for (long i = 0; i < transactionCount.AsInt64; i++)
                                {
                                    Transaction nextTransaction;

                                    nextTransaction = TransactionParser.Parse(stream);
                                    returnValue.Transactions.Add(nextTransaction);
                                }
                            }
                            else
                                throw new InvalidOperationException("The transaction count is invalid");
                        }
                        else
                            throw new InvalidOperationException("The block header is invalid");
                    }
                    else
                        throw new InvalidOperationException("The stream does not contain a valid block");
                }
                else
                    throw new InvalidOperationException("The stream does not start with a valid magic number");
            }
            else
                throw new InvalidOperationException("The stream does not start with a valid magic number");

            return returnValue;
        }

        /// <summary>
        /// Tries to parse a block from the specified stream.
        /// </summary>
        /// <param name="stream">A BlockchainStream to read data from.</param>
        /// <param name="parsedObject">A Block that will be set to the parsed block.</param>
        /// <returns>A bool indicating whether the block was parsed from the stream.</returns>
        internal static bool TryParse(BlockchainStream stream, out Block parsedObject)
        {
            return _parserCore.TryParse(stream, out parsedObject);
        }
        
        #endregion
        #region _Private_
        private static BitcoinNetwork ParseMagicNumber(uint magicNumber)
        {
            BitcoinNetwork returnValue = BitcoinNetwork.Undefined;

            if (magicNumber == 0xd9b4bef9)
                returnValue = BitcoinNetwork.Mainnet;
            else if (magicNumber == 0x0709110b)
                returnValue = BitcoinNetwork.Testnet3;
            else if (magicNumber == 0xdab5bffa)
                returnValue = BitcoinNetwork.Regtest;
            else if (magicNumber == 0x40cf030a)
                returnValue = BitcoinNetwork.Signet;
            else if (magicNumber == 0xf9beb4fe)
                returnValue = BitcoinNetwork.Namecoin;

            return returnValue;
        }

        private static bool TryParseBlockHeader(BlockchainStream stream, BitcoinNetwork network, int blockSize, out Block block)
        {
            bool returnValue = false;

            // Defaults
            block = null;

            try
            {
                int version = stream.ReadInt();
                byte[] previousBlock = stream.ReadBytes(32);
                byte[] merkleRoot = stream.ReadBytes(32);
                uint timestamp = stream.ReadUInt();
                uint bits = stream.ReadUInt();
                uint nonce = stream.ReadUInt();

                block = new Block(network, blockSize, version, previousBlock, merkleRoot, timestamp, bits, nonce);
                returnValue = true;
            }
            catch (Exception)
            { }

            return returnValue;
        }

        #endregion
        #region _Public_
        /// <summary>
        /// Parses a block from the specified data.
        /// </summary>
        /// <param name="data">An array of byte containing the data to parse.</param>
        /// <exception cref="System.ArgumentException">The specified data is NULL, zero-length, or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The block could not be parsed from the specified data.</exception>
        /// <returns>A Block parsed from the data.</returns>
        public static Block Parse(byte[] data)
        {
            return _parserCore.Parse(data);
        }

        /// <summary>
        /// Parses a block from the specified stream.
        /// </summary>
        /// <param name="stream">A Stream containing the data to parse.</param>
        /// <exception cref="System.ArgumentException">The specified stream is NULL or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The block could not be parsed from the specified stream.</exception>
        /// <returns>A Block parsed from the stream.</returns>
        public static Block Parse(Stream stream)
        {
            return _parserCore.Parse(stream);
        }
     
        #endregion
        #endregion
    }
}
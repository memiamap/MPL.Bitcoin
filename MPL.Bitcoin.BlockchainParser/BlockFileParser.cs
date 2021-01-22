using System;
using System.IO;

namespace MPL.Bitcoin.BlockchainParser
{
    /// <summary>
    /// A class that can be used to parse Bitcoin core blk (block) files.
    /// </summary>
    public static class BlockFileParser
    {
        #region Constructors
        static BlockFileParser()
        {
            _parserCore = new ParserCore<BlockFile>(Parse, "block file");
        }

        #endregion

        #region Declarations
        #region _Members_
        private readonly static ParserCore<BlockFile> _parserCore;

        #endregion
        #endregion

        #region Methods
        #region _Internal_
        /// <summary>
        /// Parses a block file from the specified stream.
        /// </summary>
        /// <param name="stream">A BlockchainStream to read data from.</param>
        /// <exception cref="System.ArgumentException">The specified stream is NULL or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The block file could not be parsed from the specified stream.</exception>
        /// <returns>A BlockFile parsed from the stream.</returns>
        internal static BlockFile Parse(BlockchainStream stream)
        {
            BlockFile returnValue;

            // Verify params
            if (stream == null) throw new ArgumentException("The specified stream is NULL", nameof(stream));

            try
            {
                // Defaults
                returnValue = new BlockFile();

                // Load all blocks from the stream
                while (BlockParser.TryParse(stream, out Block block))
                    returnValue.Blocks.Add(block);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to load the block file from the specified stream", ex);
            }

            return returnValue;
        }

        /// <summary>
        /// Tries to parse a block file from the specified stream.
        /// </summary>
        /// <param name="stream">A BlockchainStream to read data from.</param>
        /// <param name="parsedObject">A BlockFile that will be set to the parsed block file.</param>
        /// <returns>A bool indicating whether the block file was parsed from the stream.</returns>
        internal static bool TryParse(BlockchainStream stream, out BlockFile parsedObject)
        {
            return _parserCore.TryParse(stream, out parsedObject);
        }

        #endregion
        #region _Public_
        /// <summary>
        /// Parses a block file from the specified data.
        /// </summary>
        /// <param name="data">An array of byte containing the data to parse.</param>
        /// <exception cref="System.ArgumentException">The specified data is NULL, zero-length, or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The block file could not be parsed from the specified data.</exception>
        /// <returns>A BlockFile parsed from the data.</returns>
        public static BlockFile Parse(byte[] data)
        {
            return _parserCore.Parse(data);
        }

        /// <summary>
        /// Parses a block file from the specified stream.
        /// </summary>
        /// <param name="stream">A Stream containing the data to parse.</param>
        /// <exception cref="System.ArgumentException">The specified stream is NULL or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The block file could not be parsed from the specified stream.</exception>
        /// <returns>A BlockFile parsed from the stream.</returns>
        public static BlockFile Parse(Stream stream)
        {
            return _parserCore.Parse(stream);
        }

        /// <summary>
        /// Parses a block file from the specified file.
        /// </summary>
        /// <param name="filePath">A string containing the path of the file.</param>
        /// <exception cref="System.ArgumentException">The specified filePath is NULL or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The block file could not be parsed from the specified file.</exception>
        /// <returns>A BlockFile parsed from the file.</returns>
        public static BlockFile ParseFile(string filePath)
        {
            BlockFile returnValue = null;

            // Verify params
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("The specified file is NULL or empty", nameof(filePath));
            if (!File.Exists(filePath)) throw new ArgumentException("The specified file does not exist", nameof(filePath));

            try
            {
                using FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                returnValue = Parse(stream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to parse the block file from the specified file", ex);
            }

            return returnValue;
        }

        #endregion
        #endregion
    }
}
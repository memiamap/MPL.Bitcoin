using System;

namespace MPL.Bitcoin
{
    /// <summary>
    /// A class that defines a Bitcoin block fle.
    /// </summary>
    public class Block
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of the class using the specified parameters.
        /// </summary>
        /// <param name="network">A BitcoinNetwork indicating the network the block belongs to.</param>
        /// <param name="size">An int indicating the size of the block.</param>
        /// <param name="version">An int indicating the block version.</param>
        /// <param name="previousBlock">An array of byte containing the hash of the previous block.</param>
        /// <param name="merkleRoot">An array of byte containing the merkle root of the block.</param>
        /// <param name="timestamp">An uint indicating the timestamp of the block creation.</param>
        /// <param name="bits">An uint indicating the difficulty target for the block.</param>
        /// <param name="nonce">An uint indicating the nonce used to generate the block.</param>
        public Block(BitcoinNetwork network, int size, int version, byte[] previousBlock, byte[] merkleRoot, uint timestamp, uint bits, uint nonce)
        {
            Network = network;
            Size = size;
            Version = version;
            PreviousBlock = previousBlock;
            MerkleRoot = merkleRoot;
            Timestamp = timestamp;
            Bits = bits;
            Nonce = nonce;

            PreviousBlockHex = HelperFunctions.ConvertHex(previousBlock);
            MerkleRootHex = HelperFunctions.ConvertHex(merkleRoot);
            TimestampDateTime = HelperFunctions.ConvertTimestamp(timestamp);
            Transactions = new TransactionList();
        }

        #endregion

        #region Methods
        #region _Public_
        public override string ToString()
        {
            return $"{TimestampDateTime}: {Transactions.Count} Transactions";
        }

        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Gets the difficulty target for the block.
        /// </summary>
        public uint Bits { get; }

        /// <summary>
        /// Gets the merkle root of the block.
        /// </summary>
        public byte[] MerkleRoot { get; }

        /// <summary>
        /// Gets the merkle root of the block as a hexadecimal string.
        /// </summary>
        public string MerkleRootHex { get; }

        /// <summary>
        /// Gets the network for the block.
        /// </summary>
        public BitcoinNetwork Network { get; }

        /// <summary>
        /// Gets the nonce used to generate the block.
        /// </summary>
        public uint Nonce { get; }

        /// <summary>
        /// Gets the previous block hash.
        /// </summary>
        public byte[] PreviousBlock { get; }

        /// <summary>
        /// Gets the previous block hash as a hexadecimal string.
        /// </summary>
        public string PreviousBlockHex { get; }

        /// <summary>
        /// Gets the size of the block.
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Gets the timestamp of the block creation.
        /// </summary>
        public uint Timestamp { get; }

        /// <summary>
        /// Gets the timestamp of the block creation in DateTime format.
        /// </summary>
        public DateTime TimestampDateTime { get; }

        /// <summary>
        /// Gets the transactions in this block.
        /// </summary>
        public TransactionList Transactions { get; }

        /// <summary>
        /// Gets the version of the block.
        /// </summary>
        public int Version { get; }

        #endregion
    }
}
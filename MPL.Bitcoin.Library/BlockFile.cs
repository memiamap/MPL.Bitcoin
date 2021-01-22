using System;

namespace MPL.Bitcoin
{
    /// <summary>
    /// A class that defines the contents of a block file.
    /// </summary>
    public class BlockFile
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        public BlockFile()
        {
            Blocks = new BlockList();
        }

        #endregion

        #region Methods
        #region _Public_
        public override string ToString()
        {
            return $"{Blocks.Count} Blocks";
        }

        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Gets a list of blocks from this block file.
        /// </summary>
        public BlockList Blocks { get; }

        #endregion
    }
}
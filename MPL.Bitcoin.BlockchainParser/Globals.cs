using System;

namespace MPL.Bitcoin.BlockchainParser
{
    #region Declarations
    #region _Enumerations_
    /// <summary>
    /// An enumeration that defines a byte order.
    /// </summary>
    internal enum ByteOrder : int
    {
        /// <summary>
        /// The byte order is undefined.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The byte order is big endian.
        /// </summary>
        BigEndian,

        /// <summary>
        /// The default byte order should be used.
        /// </summary>
        Default,

        /// <summary>
        /// The byte order is little endian.
        /// </summary>
        LittleEndian
    }

    #endregion
    #endregion
}
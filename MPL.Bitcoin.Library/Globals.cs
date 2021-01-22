using System;

namespace MPL.Bitcoin
{
    #region Declarations
    #region _Enumerations_
    /// <summary>
    /// An enumeration that defines a Bitcoin network.
    /// </summary>
    public enum BitcoinNetwork : int
    {
        /// <summary>
        /// The network is undefined.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The Mainnet network.
        /// </summary>
        Mainnet,

        /// <summary>
        /// The Namecoin network.
        /// </summary>
        Namecoin,

        /// <summary>
        /// The Regtest network.
        /// </summary>
        Regtest,

        /// <summary>
        /// The signet network.
        /// </summary>
        Signet,

        /// <summary>
        /// The Testnet3 network.
        /// </summary>
        Testnet3
    }

    /// <summary>
    /// An enumeration that defines the type of a locktime.
    /// </summary>
    public enum LockTimeType : int
    {
        /// <summary>
        /// The locktime type is undefined.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The locktime is the target block height.
        /// </summary>
        BlockHeight,

        /// <summary>
        /// There is no locktime.
        /// </summary>
        NoLockTime,

        /// <summary>
        /// The locktime is the target timestamp.
        /// </summary>
        Timestamp
    }

    /// <summary>
    /// An enumeration that defines the size of a variable integer.
    /// </summary>
    public enum VariableIntegerSize : int
    {
        /// <summary>
        /// The variable integer size is undefined.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The variable integer is 16-bits.
        /// </summary>
        Int16,

        /// <summary>
        /// The variable integer is 32-bits.
        /// </summary>
        Int32,

        /// <summary>
        /// The variable integer is 64-bits.
        /// </summary>
        Int64,


        /// <summary>
        /// The variable integer is 8-bits.
        /// </summary>
        Int8
    }

    #endregion
    #endregion
}
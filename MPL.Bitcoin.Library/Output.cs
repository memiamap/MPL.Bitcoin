using System;

namespace MPL.Bitcoin
{
    /// <summary>
    /// A class that defines a transaction output.
    /// </summary>
    public class Output
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of the class with the specified parameters.
        /// </summary>
        /// <param name="value">A long indicating the value of the output in Satoshis.</param>
        /// <param name="scriptPubKey">An array of byte containing the redeem script for the output.</param>
        public Output(long value, byte[] scriptPubKey)
        {
            Value = value;
            ScriptPubKey = scriptPubKey;
        }

        #endregion

        #region Methods
        #region _Public_
        public override string ToString()
        {
            return $"{Value}";
        }

        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Gets the redeem script for the output.
        /// </summary>
        public byte[] ScriptPubKey { get; }

        /// <summary>
        /// Gets the value of the output in Satoshis.
        /// </summary>
        public long Value { get; }

        #endregion
    }
}
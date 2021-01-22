using System;

namespace MPL.Bitcoin
{
    /// <summary>
    /// A class that defines a transaction input.
    /// </summary>
    public class Input
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of the class with the specified parameters.
        /// </summary>
        /// <param name="transaction">An array of byte containing the previous transaction used for this input.</param>
        /// <param name="outputID">An uint indicating the output in the referenced transaction.</param>
        /// <param name="scriptSig">An array of byte containing the input's ScriptSig.</param>
        /// <param name="sequence">An uint indicating the sequence number for this input.</param>
        public Input(byte[] transaction, uint outputID, byte[] scriptSig, uint sequence)
        {
            Transaction = transaction;
            OutputID = outputID;
            ScriptSig = scriptSig;
            Sequence = sequence;

            TransactionHex = HelperFunctions.ConvertHex(transaction);
        }

        #endregion

        #region Methods
        #region _Public_
        public override string ToString()
        {
            return $"{TransactionHex}-{OutputID}";
        }

        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Gets the index of the output in the referenced transaction.
        /// </summary>
        public uint OutputID { get; }

        /// <summary>
        /// Gets the signaute script of the transaction input.
        /// </summary>
        public byte[] ScriptSig { get; }

        /// <summary>
        /// Gets the sequence number for this input.
        /// </summary>
        public uint Sequence { get; }

        /// <summary>
        /// Gets the previous transaction used for this input.
        /// </summary>
        public byte[] Transaction { get; }

        /// <summary>
        /// Gets the hexadecimal representation of the previous transaction used for this input.
        /// </summary>
        public string TransactionHex { get; }

        #endregion
    }
}
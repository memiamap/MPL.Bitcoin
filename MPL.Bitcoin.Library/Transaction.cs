using System;

namespace MPL.Bitcoin
{
    /// <summary>
    /// A class that defines a Bitcoin transaction.
    /// </summary>
    public class Transaction
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of the class with the specified parameters.
        /// </summary>
        /// <param name="version">An int indicating the version of the transaction.</param>
        /// <param name="lockTime">A LockTime containing the transaction locktime.</param>
        public Transaction(int version, LockTime lockTime)
            : this(version, lockTime, new InputList(), new OutputList())
        { }

        /// <summary>
        /// Creates a new instance of the class with the specified parameters.
        /// </summary>
        /// <param name="version">An int indicating the version of the transaction.</param>
        /// <param name="lockTime">A LockTime containing the transaction locktime.</param>
        /// <param name="inputs">An InputList containing a list of inputs.</param>
        /// <param name="outputs">An OutputList containing a list of outputs.</param>
        public Transaction(int version, LockTime lockTime, InputList inputs, OutputList outputs)
        {
            Version = version;
            LockTime = lockTime;
            Inputs = inputs;
            Outputs = outputs;
        }

        #endregion

        #region Methods
        #region _Public_
        public override string ToString()
        {
            return $"Inputs: {Inputs.Count} Outputs: {Outputs.Count}";
        }

        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Gets a list of inputs in  this transaction.
        /// </summary>
        public InputList Inputs { get; }

        /// <summary>
        /// Gets the transaction locktime.
        /// </summary>
        public LockTime LockTime { get; }

        /// <summary>
        /// Gets a list of outputs in this transaction.
        /// </summary>
        public OutputList Outputs { get; }

        /// <summary>
        /// Gets the version number of the transaction.
        /// </summary>
        public int Version{ get; }
        
        #endregion
    }
}
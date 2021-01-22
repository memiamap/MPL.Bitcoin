using System;

namespace MPL.Bitcoin
{
    /// <summary>
    /// A class that defines a variable integer.
    /// </summary>
    public class VarInt
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of the class with the specified parameters.
        /// </summary>
        /// <param name="data">An array of byte containing the raw data for the </param>
        /// <param name="size">A VariableIntegerSize indicating the size of the variable integer.</param>
        /// <param name="data8">A byte indicating the 8-bit value of the variable integer.</param>
        /// <param name="data16">A short indicating the 16-bit value of the variable integer.</param>
        /// <param name="data32">An int indicating the 32-bit value of the variable integer.</param>
        /// <param name="data64">A long indicating the 64-bit value of the variable integer.</param>
        public VarInt(VariableIntegerSize size, byte data8, short data16, int data32, long data64)
        {
            Size = size;
            AsInt8 = data8;
            AsInt16 = data16;
            AsInt32 = data32;
            AsInt64 = data64;
        }

        #endregion

        #region Methods
        #region _Public_
        public override string ToString()
        {
            return Size switch
            {
                VariableIntegerSize.Int16 => $"Int16: {AsInt16}",
                VariableIntegerSize.Int32 => $"Int32: {AsInt32}",
                VariableIntegerSize.Int64 => $"Int64: {AsInt64}",
                VariableIntegerSize.Int8 => $"Int8: {AsInt8}",
                _ => base.ToString(),
            };
        }

        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Gets the variable integre as 16-bit.
        /// </summary>
        public short AsInt16 { get; }

        /// <summary>
        /// Gets the variable integer as 32-bit.
        /// </summary>
        public int AsInt32 { get; }

        /// <summary>
        /// Gets the variable integer as 64-bit.
        /// </summary>
        public long AsInt64 { get; }

        /// <summary>
        /// Gets the variable integer as 8-bit.
        /// </summary>
        public byte AsInt8 { get; }

        /// <summary>
        /// Gets the computed size of the variable integer.
        /// </summary>
        public VariableIntegerSize Size { get; }

        #endregion
    }
}
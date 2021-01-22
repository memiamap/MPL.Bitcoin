using System;
using System.IO;
using System.Linq;

namespace MPL.Bitcoin.BlockchainParser
{
    /// <summary>
    /// A class that defines a stream to read blockchain data.
    /// </summary>
    internal class BlockchainStream : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of the class with the specified parameters.
        /// </summary>
        /// <param name="sourceData">An array of byte containing the source data to read.</param>
        /// <param name="defaultByteOrder">A ByteOrder indicating the byte order to use when reading from the stream.</param>
        internal BlockchainStream(byte[] sourceData, ByteOrder defaultByteOrder = ByteOrder.Default)
        {
            if (sourceData == null || sourceData.Length == 0) throw new ArgumentException("The specified source data is NULL or zero-length", nameof(sourceData));
            if (defaultByteOrder == ByteOrder.Undefined) throw new ArgumentException("The specified default byte order is invalid", nameof(defaultByteOrder));

            _sourceStream = new MemoryStream(sourceData);
            DefaultByteOrder = defaultByteOrder == ByteOrder.Default ? cDEFAULT_BYTE_ORDER : defaultByteOrder;
        }

        /// <summary>
        /// Creates a new instance of the class with the specified parameters.
        /// </summary>
        /// <param name="sourceStream">A Stream containing the source stream to read.</param>
        /// <param name="defaultByteOrder">A ByteOrder indicating the byte order to use when reading from the stream.</param>
        internal BlockchainStream(Stream sourceStream, ByteOrder defaultByteOrder = ByteOrder.Default)
        {
            _sourceStream = sourceStream ?? throw new ArgumentException("The specified source stream is NULL", nameof(sourceStream));
            if (defaultByteOrder == ByteOrder.Undefined) throw new ArgumentException("The specified default byte order is invalid", nameof(defaultByteOrder));
            DefaultByteOrder = defaultByteOrder == ByteOrder.Default ? cDEFAULT_BYTE_ORDER : defaultByteOrder;
        }

        #endregion

        #region Declarations
        #region _Constants_
        private const ByteOrder cDEFAULT_BYTE_ORDER = ByteOrder.LittleEndian;

        #endregion
        #region _Members_
        private ByteOrder _defaultByteOrder;
        private bool _isDisposed;
        private readonly Stream _sourceStream;

        #endregion
        #endregion

        #region Methods
        #region _Internal_
        /// <summary>
        /// Reads the specified number of bytes from the stream.
        /// </summary>
        /// <param name="length">An int indicating the number of bytes to read.</param>
        /// <param name="byteOrder">A ByteOrder indicating the byte order to use.</param>
        /// <returns>An array of byte containing the result.</returns>
        internal byte[] ReadBytes(int length, ByteOrder byteOrder = ByteOrder.Default)
        {
            byte[] returnValue;

            // Verify params
            if (length <= 0) throw new ArgumentException("The length is invalid", nameof(length));
            if (byteOrder == ByteOrder.Undefined) throw new ArgumentException("The specified default byte order is invalid", nameof(byteOrder));

            // Try to get the bytes
            if (!TryReadBytes(length, out returnValue, byteOrder))
                throw new InvalidOperationException("Unable to read the bytes from the block file stream");

            return returnValue;
        }

        /// <summary>
        /// Reads an integer from the stream.
        /// </summary>
        /// <param name="byteOrder">A ByteOrder indicating the byte order to use.</param>
        /// <returns>An int containing the result.</returns>
        internal int ReadInt(ByteOrder byteOrder = ByteOrder.Default)
        {
            return ReadAsType(4, BitConverter.ToInt32, byteOrder);
        }

        /// <summary>
        /// Reads an unsigned integer from the stream.
        /// </summary>
        /// <param name="byteOrder">A ByteOrder indicating the byte order to use.</param>
        /// <returns>An uint containing the result.</returns>
        internal uint ReadUInt(ByteOrder byteOrder = ByteOrder.Default)
        {
            return ReadAsType(4, BitConverter.ToUInt32, byteOrder);
        }

        /// <summary>
        /// Reads a variable integer from the stream.
        /// </summary>
        /// <returns>A VarInt containing the result.</returns>
        internal VarInt ReadVarInt()
        {
            return VarIntParser.Parse(this);
        }

        /// <summary>
        /// Tries to read the specified number of bytes from the stream.
        /// </summary>
        /// <param name="length">An int indicating the number of bytes to read.</param>
        /// <param name="data">An array of byte that will contain the read data.</param>
        /// <param name="byteOrder">A ByteOrder indicating the byte order to use.</param>
        /// <returns>A bool that indicates success.</returns>
        internal bool TryReadBytes(int length, out byte[] data, ByteOrder byteOrder = ByteOrder.Default)
        {
            bool returnValue = false;

            // Verify params
            if (length <= 0) throw new ArgumentException("The length is invalid", nameof(length));
            byteOrder = GetByteOrder(byteOrder);
            data = new byte[length];

            try
            {
                int readBytes;

                readBytes = _sourceStream.Read(data, 0, length);
                if (readBytes == length)
                {
                    if ((BitConverter.IsLittleEndian && byteOrder != ByteOrder.LittleEndian) ||
                        (!BitConverter.IsLittleEndian && byteOrder == ByteOrder.LittleEndian))
                        data = data.Reverse().ToArray();
                    returnValue = true;
                }
                else
                    throw new InvalidOperationException($"Unable to read {length} bytes from stream - {readBytes} byte(s) returned");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred whilst reading bytes from source stream: {ex.Message}");
            }

            return returnValue;
        }

        /// <summary>
        /// Tries to read a long from the stream.
        /// </summary>
        /// <param name="output">A long that will be set to the output.</param>
        /// <param name="byteOrder">A ByteOrder indicating the byte order to use.</param>
        /// <returns>A bool that indicates success.</returns>
        internal bool TryReadLong(out long output, ByteOrder byteOrder = ByteOrder.Default)
        {
            return TryReadAsType(8, BitConverter.ToInt64, byteOrder, out output);
        }

        /// <summary>
        /// Tries to read an integer from the stream.
        /// </summary>
        /// <param name="output">An int that will be set to the output.</param>
        /// <param name="byteOrder">A ByteOrder indicating the byte order to use.</param>
        /// <returns>A bool that indicates success.</returns>
        internal bool TryReadInt(out int output, ByteOrder byteOrder = ByteOrder.Default)
        {
            return TryReadAsType(4, BitConverter.ToInt32, byteOrder, out output);
        }

        /// <summary>
        /// Tries to read an unsigned integer from the stream.
        /// </summary>
        /// <param name="output">An int that will be set to the output.</param>
        /// <param name="byteOrder">A ByteOrder indicating the byte order to use.</param>
        /// <returns>A bool that indicates success.</returns>
        internal bool TryReadUInt(out uint output, ByteOrder byteOrder = ByteOrder.Default)
        {
            return TryReadAsType(4, BitConverter.ToUInt32, byteOrder, out output);
        }

        /// <summary>
        /// Tries to read a variable integer from the stream.
        /// </summary>
        /// <param name="output">A VarInt that will be set to the output.</param>
        /// <returns>A bool that indicates success.</returns>
        internal bool TryReadVarInt(out VarInt output)
        {
            bool returnValue = false;

            // Defaults
            output = null;

            try
            {
                output = ReadVarInt();
                returnValue = true;
            }
            catch (Exception)
            { }

            return returnValue;
        }

        #endregion
        #region _Private_
        private ByteOrder GetByteOrder(ByteOrder byteOrder)
        {
            ByteOrder returnValue = byteOrder;

            if (byteOrder == ByteOrder.Undefined)
                throw new ArgumentException("The specified default byte order is invalid", nameof(byteOrder));
            else if (byteOrder == ByteOrder.Default)
                returnValue = DefaultByteOrder;

            return returnValue;
        }

        private T ReadAsType<T>(int length, Func<byte[], int, T> converterFunction, ByteOrder byteOrder)
        {
            T returnValue;

            // Verify params
            if (length <= 0) throw new ArgumentException("The specified length is invalid", nameof(length));
            if (converterFunction == null) throw new ArgumentException("The specified type converter function is invalid", nameof(converterFunction));
            byteOrder = GetByteOrder(byteOrder);

            // Read the data
            if (TryReadBytes(length, out byte[] data, byteOrder))
            {
                try
                {
                    returnValue = converterFunction(data, 0);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("The specified type converter produced an exception", nameof(converterFunction), ex);
                }
            }
            else
                throw new InvalidOperationException("Unable to read the data from the block file stream");

            return returnValue;
        }

        private bool TryReadAsType<T>(int length, Func<byte[], int, T> converterFunction, ByteOrder byteOrder, out T output)
        {
            bool returnValue = false;

            // Verify params
            if (length <= 0) throw new ArgumentException("The specified length is invalid", nameof(length));
            if (converterFunction == null) throw new ArgumentException("The specified type converter function is invalid", nameof(converterFunction));
            byteOrder = GetByteOrder(byteOrder);

            // Defaults
            output = default;

            try
            {
                output = ReadAsType(length, converterFunction, byteOrder);
                returnValue = true;
            }
            catch (Exception)
            { }

            return returnValue;
        }

        #endregion
        #region _Protected_
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                    _sourceStream.Dispose();
                _isDisposed = true;
            }
        }

        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the default byte order for the stream.
        /// </summary>
        internal ByteOrder DefaultByteOrder
        {
            get { return _defaultByteOrder; }
            set
            {
                if (value == ByteOrder.Undefined)
                    throw new ArgumentException("The specified default byte order is invalid", nameof(value));
                else
                    _defaultByteOrder = value == ByteOrder.Default ? cDEFAULT_BYTE_ORDER : value;
            }
        }

        #endregion

        #region Interfaces
        #region _IDisposable_
        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
        #endregion
    }
}
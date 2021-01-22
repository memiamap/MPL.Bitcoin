using System;
using System.Collections.Generic;
using System.Linq;

namespace MPL.Bitcoin
{
    /// <summary>
    /// A class that provides helper functions.
    /// </summary>
    public static class HelperFunctions
    {
        #region Constructors
        static HelperFunctions()
        {
            _hexValues = new Dictionary<char, byte>();
            for (byte i = 48; i < 58; i++)
                _hexValues.Add((char)i, (byte)(i - 48));
            for (byte i = 65; i < 71; i++)
                _hexValues.Add((char)i, (byte)(i - 55));
            for (byte i = 97; i < 103; i++)
                _hexValues.Add((char)i, (byte)(i - 87));
        }

        #endregion

        #region Declarations
        #region _Members_
        private static readonly Dictionary<char, byte> _hexValues;
        private static readonly uint[] _lookup32 = CreateLookup32();

        #endregion
        #endregion

        #region Methods
        #region _Private_
        private static uint[] CreateLookup32()
        {
            uint[] result = new uint[256];

            for (int i = 0; i < 256; i++)
            {
                string s = i.ToString("X2");
                result[i] = s[0] + ((uint)s[1] << 16);
            }

            return result;
        }

        #endregion
        #region _Public_
        /// <summary>
        /// Uses BitConverter to convert from the offset in a byte array into a value, automatically compensating for the endianness of the environment.
        /// </summary>
        /// <param name="data">An array of bytes that is the array to convert from.</param>
        /// <param name="value">An int that will be set to the converted value.</param>
        /// <param name="offset">An int indicating the offset in the array from which to start the conversion.</param>
        public static void BitConvert(byte[] data, out int value, int offset = 0)
        {
            value = GetValue(BitConverter.ToInt32, data, offset, 4);
        }
        /// <summary>
        /// Uses BitConverter to convert from the offset in a byte array into a value, automatically compensating for the endianness of the environment.
        /// </summary>
        /// <param name="data">An array of bytes that is the array to convert from.</param>
        /// <param name="value">A long that will be set to the converted value.</param>
        /// <param name="offset">An int indicating the offset in the array from which to start the conversion.</param>
        public static void BitConvert(byte[] data, out long value, int offset = 0)
        {
            value = GetValue(BitConverter.ToInt64, data, offset, 8);
        }
        /// <summary>
        /// Uses BitConverter to convert from the offset in a byte array into a value, automatically compensating for the endianness of the environment.
        /// </summary>
        /// <param name="data">An array of bytes that is the array to convert from.</param>
        /// <param name="value">A short that will be set to the converted value.</param>
        /// <param name="offset">An int indicating the offset in the array from which to start the conversion.</param>
        public static void BitConvert(byte[] data, out short value, int offset = 0)
        {
            value = GetValue(BitConverter.ToInt16, data, offset, 2);
        }
        /// <summary>
        /// Uses BitConverter to convert from the offset in a byte array into a value, automatically compensating for the endianness of the environment.
        /// </summary>
        /// <param name="data">An array of bytes that is the array to convert from.</param>
        /// <param name="value">An uint that will be set to the converted value.</param>
        /// <param name="offset">An int indicating the offset in the array from which to start the conversion.</param>
        public static void BitConvert(byte[] data, out uint value, int offset = 0)
        {
            value = GetValue(BitConverter.ToUInt32, data, offset, 4);
        }
        /// <summary>
        /// Uses BitConverter to convert from the offset in a byte array into a value, automatically compensating for the endianness of the environment.
        /// </summary>
        /// <param name="data">An array of bytes that is the array to convert from.</param>
        /// <param name="value">An ulong that will be set to the converted value.</param>
        /// <param name="offset">An int indicating the offset in the array from which to start the conversion.</param>
        public static void BitConvert(byte[] data, out ulong value, int offset = 0)
        {
            value = GetValue(BitConverter.ToUInt64, data, offset, 8);
        }
        /// <summary>
        /// Uses BitConverter to convert from the offset in a byte array into a value, automatically compensating for the endianness of the environment.
        /// </summary>
        /// <param name="data">An array of bytes that is the array to convert from.</param>
        /// <param name="value">An ushort that will be set to the converted value.</param>
        /// <param name="offset">An int indicating the offset in the array from which to start the conversion.</param>
        public static void BitConvert(byte[] data, out ushort value, int offset = 0)
        {
            value = GetValue(BitConverter.ToUInt16, data, offset, 2);
        }

        /// <summary>
        /// Converts a byte array to it's hexadecimal representation.
        /// </summary>
        /// <param name="source">An array of byte to be converted.</param>
        /// <param name="outputUpperCase">A bool indicating whether to output upper case characters.</param>
        /// <returns>A string that is the converted hex.</returns>
        public static string ConvertHex(byte[] source, bool outputUpperCase = false)
        {
            uint[] lookup32 = _lookup32;
            char[] result;
            string returnValue;

            result = new char[source.Length * 2];
            for (int i = 0; i < source.Length; i++)
            {
                var val = lookup32[source[i]];
                result[2 * i] = (char)val;
                result[2 * i + 1] = (char)(val >> 16);
            }

            returnValue = new string(result);
            if (outputUpperCase)
                returnValue = returnValue.ToUpper();

            return returnValue;
        }
        /// <summary>
        /// Converts a hexadecimal string to an array of bytes.
        /// </summary>
        /// <param name="source">A string to be converted.</param>
        /// <exception cref="System.ArgumentNullException">The specified source is NULL or empty.</exception>
        /// <exception cref="System.ArgumentException">The specified source is invalid.</exception>
        /// <returns>An array of bytes that is the converted hex.</returns>
        public static byte[] ConvertHex(string source)
        {
            byte[] returnValue;

            if (string.IsNullOrWhiteSpace(source)) throw new ArgumentNullException("The specified source is NULL or empty", nameof(source));
            if (source.Length % 2 == 1) throw new ArgumentException("The source hexadecimal string cannot have an odd number of digits", nameof(source));

            returnValue = new byte[source.Length >> 1];

            for (int i = 0; i < source.Length >> 1; ++i)
            {
                var high = source[i << 1];
                var low = source[(i << 1) + 1];

                if (_hexValues.ContainsKey(high) && _hexValues.ContainsKey(low))
                    returnValue[i] = (byte)((_hexValues[high] << 4) + _hexValues[low]);
                else
                    throw new ArgumentException("The source hexadecimal string is invalid");
            }

            return returnValue;
        }

        /// <summary>
        /// Converts a Timestamp to a DateTime.
        /// </summary>
        /// <param name="timestamp">An uint indicating the timestamp to convert.</param>
        /// <returns>A DateTime that is the converted timestamp.</returns>
        public static DateTime ConvertTimestamp(uint timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
        }

        /// <summary>
        /// Gets bytes for the specified value, automatically ensuring little-endian output.
        /// </summary>
        /// <typeparam name="T">The type of the input value.</typeparam>
        /// <param name="converter">A Func that takes input of type T and outputs an array of byte that converts the bytes.</param>
        /// <param name="value">A T that is the value to convert.</param>
        /// <returns>An array of byte containing the converted value.</returns>
        public static byte[] GetBytes<T>(Func<T, byte[]> converter, T value)
        {
            byte[] returnValue;

            returnValue = converter(value);
            if (!BitConverter.IsLittleEndian)
                returnValue = returnValue.Reverse().ToArray();

            return returnValue;
        }

        /// <summary>
        /// Gets the value for the specified data, automatically compensating for the endianness of source data.
        /// </summary>
        /// <typeparam name="T">The type of the output value.</typeparam>
        /// <param name="converter">A Func that takes input of an array of byte and outputs a type of T from the converted the bytes.</param>
        /// <param name="data">An array of byte containing the data to convert.</param>
        /// <param name="offset">An int indicating the offset of the data in the array.</param>
        /// <param name="length">An int indicating the length of the data (or 0 for all of the data).</param>
        /// <returns>A T that was converted from the data.</returns>
        public static T GetValue<T>(Func<byte[], int, T> converter, byte[] data, int offset = 0, int length = 0)
        {
            byte[] fixedData;
            T returnValue;

            // Check params
            if (data == null || data.Length == 0) throw new ArgumentException("The specified data is NULL or empty", nameof(data));
            if (offset < 0 || offset >= data.Length) throw new ArgumentException("The specified offset is invalid", nameof(offset));
            if (length < 0 || offset + length > data.Length) throw new ArgumentException("The specified length is invalid", nameof(length));
            if (length == 0)
                length = data.Length;

            // Fix the endianness of the data
            fixedData = new byte[length];
            Array.Copy(data, offset, fixedData, 0, length);
            if (!BitConverter.IsLittleEndian)
                fixedData = fixedData.Reverse().ToArray();

            // Convert output
            returnValue = converter(fixedData, 0);

            return returnValue;

        }

        /// <summary>
        /// Slices an array from the specified start position.
        /// </summary>
        /// <param name="source">An array of bytes that is the source array to slice.</param>
        /// <param name="start">An int indicating the start position for the array slice.</param>
        /// <returns></returns>
        public static byte[] Slice(this byte[] source, int start)
        {
            byte[] returnValue;

            if (source.Length > start)
                returnValue = Slice(source, start, source.Length - start);
            else
                throw new ArgumentException("The specified start is invalid", nameof(start));

            return returnValue;
        }
        /// <summary>
        /// Slices an array for the specified length from the specified start position.
        /// </summary>
        /// <param name="source">An array of bytes that is the source array to slice.</param>
        /// <param name="start">An int indicating the start position for the array slice.</param>
        /// <param name="length">An int indicating the length of the slice.</param>
        /// <returns></returns>
        public static byte[] Slice(this byte[] source, int start, int length)
        {
            if (source != null)
            {
                if (source.Length > start)
                {
                    if (source.Length >= start + length)
                    {
                        return source.Skip(start).Take(length).ToArray();
                    }
                    else
                        throw new ArgumentException("The specified length is invalid", nameof(length));
                }
                else
                    throw new ArgumentException("The specified start is invalid", nameof(start));
            }
            else
                throw new ArgumentException("The specified array is NULL or empty", nameof(source));
        }

        #endregion
        #endregion
    }
}
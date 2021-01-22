using System;
using System.IO;

namespace MPL.Bitcoin.BlockchainParser
{
    /// <summary>
    /// A class that provides functionality to parse a variable integer.
    /// </summary>
    public static class VarIntParser
    {
        #region Constructors
        static VarIntParser()
        {
            _parserCore = new ParserCore<VarInt>(Parse, "variable integer");
        }

        #endregion

        #region Declarations
        #region _Members_
        private readonly static ParserCore<VarInt> _parserCore;

        #endregion
        #endregion

        #region Methods
        #region _Internal_
        /// <summary>
        /// Parses a variable integer from the specified stream.
        /// </summary>
        /// <param name="stream">A BlockDataStream to pasre the variable integer from.</param>
        /// <returns>A VarInt pasred from the stream.</returns>
        internal static VarInt Parse(BlockchainStream stream)
        {
            VarInt returnValue;

            // Verify params
            if (stream == null) throw new ArgumentException("The specified stream is NULL", nameof(stream));

            // Get the leading byte
            if (stream.TryReadBytes(1, out byte[] data))
            {
                bool isValid = false;

                if (data[0] > 0xfc)
                {
                    int extraBytes = 0;
                    byte marker = data[0];

                    if (marker == 0xfd)
                        extraBytes = 2;
                    else if (marker == 0xfe)
                        extraBytes = 4;
                    else if (marker == 0xff)
                        extraBytes = 8;

                    if (stream.TryReadBytes(extraBytes, out byte[] temp))
                    {
                        data = new byte[extraBytes + 1];
                        data[0] = marker;
                        Array.Copy(temp, 0, data, 1, extraBytes);
                        isValid = true;
                    }
                }
                else
                    isValid = true;

                if (isValid)
                    returnValue = Parse(data);
                else
                    throw new InvalidOperationException("The variable integer read from the stream is invalid");
            }
            else
                throw new InvalidOperationException("The leading byte could not be read from the stream");

            return returnValue;
        }

        /// <summary>
        /// Tries to parse a variable integer from the specified stream.
        /// </summary>
        /// <param name="stream">A BlockchainStream to read data from.</param>
        /// <param name="parsedObject">A VarInt that will be set to the parsed variable integer.</param>
        /// <returns>A bool indicating whether the variable integer was parsed from the stream.</returns>
        internal static bool TryParse(BlockchainStream stream, out VarInt parsedObject)
        {
            return _parserCore.TryParse(stream, out parsedObject);
        }

        #endregion
        #region _Private_
        private static void ValuesFrom(byte value, out short value16, out int value32, out long value64)
        {
            value16 = value;
            value32 = value;
            value64 = value;
        }
        private static void ValuesFrom(int value, out byte value8, out short value16, out long value64)
        {
            value8 = (byte)(value & 0xff);
            value16 = (short)(value & 0xffff);
            value64 = value;
        }
        private static void ValuesFrom(long value, out byte value8, out short value16, out int value32)
        {
            value8 = (byte)(value & 0xff);
            value16 = (short)(value & 0xffff);
            value32 = (int)(value & 0xffffffff);
        }
        private static void ValuesFrom(short value, out byte value8, out int value32, out long value64)
        {
            value8 = (byte)(value & 0xff);
            value32 = value;
            value64 = value;
        }

        #endregion
        #region _Public_
        /// <summary>
        /// Parses a variable integer from the specified data.
        /// </summary>
        /// <param name="data">An array of byte containing the data to parse.</param>
        /// <exception cref="System.ArgumentException">The specified data is NULL, zero-length, or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The variable integer could not be parsed from the specified data.</exception>
        /// <returns>A VarInt parsed from the data.</returns>
        public static VarInt Parse(byte[] data)
        {
            VarInt returnValue;
            VariableIntegerSize size = VariableIntegerSize.Undefined;
            byte value8 = 0;
            short value16 = 0;
            int value32 = 0;
            long value64 = 0;

            // Verify params
            if (data == null || data.Length == 0) throw new ArgumentException("The specified data is NULL or empty", nameof(data));

            if (data.Length == 1 && data[0] <= 0xfc)
            {
                size = VariableIntegerSize.Int8;
                value8 = data[0];
                ValuesFrom(value8, out value16, out value32, out value64);
            }
            else if (data.Length == 3 && data[0] == 0xfd)
            {
                size = VariableIntegerSize.Int16;
                HelperFunctions.BitConvert(data, out value16, 1);
                ValuesFrom(value16, out value8, out value32, out value64);
            }
            else if (data.Length == 5 && data[0] == 0xfe)
            {
                size = VariableIntegerSize.Int32;
                HelperFunctions.BitConvert(data, out value32, 1);
                ValuesFrom(value32, out value8, out value16, out value64);
            }
            else if (data.Length == 9 && data[0] == 0xff)
            {
                size = VariableIntegerSize.Int64;
                HelperFunctions.BitConvert(data, out value64, 1);
                ValuesFrom(value64, out value8, out value16, out value32);
            }

            if (size != VariableIntegerSize.Undefined)
                returnValue = new VarInt(size, value8, value16, value32, value64);
            else
                throw new ArgumentException("The specified data is invalid", nameof(data));

            return returnValue;
        }

        /// <summary>
        /// Parses a variable integer from the specified stream.
        /// </summary>
        /// <param name="stream">A Stream containing the data to parse.</param>
        /// <exception cref="System.ArgumentException">The specified stream is NULL or invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The variable integer could not be parsed from the specified stream.</exception>
        /// <returns>A VarInt parsed from the stream.</returns>
        public static VarInt Parse(Stream stream)
        {
            return _parserCore.Parse(stream);
        }

        #endregion
        #endregion
    }
}
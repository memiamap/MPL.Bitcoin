using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace MPL.Bitcoin.BlockchainParser.Tests
{
    [TestClass]
    public class VarIntParserTests
    {
        #region Methods
        #region _Private Methods_
        private void CheckValues(VarInt source, VariableIntegerSize size, byte int8, short int16, int int32, long int64)
        {
            Assert.AreEqual(source.Size, size);
            Assert.AreEqual(source.AsInt8, int8);
            Assert.AreEqual(source.AsInt16, int16);
            Assert.AreEqual(source.AsInt32, int32);
            Assert.AreEqual(source.AsInt64, int64);
        }

        private void ParseAndCheckValues(byte[] data, VariableIntegerSize size, byte int8, short int16, int int32, long int64)
        {
            VarInt varInt;
            
            varInt = VarIntParser.Parse(data);
            Assert.IsNotNull(varInt);
            CheckValues(varInt, size, int8, int16, int32, int64);
        }

        private void ParseAndCheckValuesAsStream(byte[] data, VariableIntegerSize size, byte int8, short int16, int int32, long int64)
        {
            VarInt varInt;

            using MemoryStream stream = new MemoryStream(data);
            varInt = VarIntParser.Parse(stream);
            Assert.IsNotNull(varInt);
            CheckValues(varInt, size, int8, int16, int32, int64);
        }

        #endregion
        #region _Test Methods_
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseData_EmptyData_ThrowsException()
        {
            VarIntParser.Parse(new byte[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseData_Invalid0xFCFlagLong_ThrowsException()
        {
            VarIntParser.Parse(new byte[] { 0xFC, 0x00 });
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseData_Invalid0xFDFlagLong_ThrowsException()
        {
            VarIntParser.Parse(new byte[] { 0xFD, 0x00, 0x00, 0x00 });
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseData_Invalid0xFDFlagShort_ThrowsException()
        {
            VarIntParser.Parse(new byte[] { 0xFD, 0x00 });
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseData_Invalid0xFEFlagLong_ThrowsException()
        {
            VarIntParser.Parse(new byte[] { 0xFE, 0x00, 0x00, 0x00, 0x00, 0x00 });
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseData_Invalid0xFEFlagShort_ThrowsException()
        {
            VarIntParser.Parse(new byte[] { 0xFE, 0x00 });
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseData_Invalid0xFFFlagLong_ThrowsException()
        {
            VarIntParser.Parse(new byte[] { 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseData_Invalid0xFFFlagShort_ThrowsException()
        {
            VarIntParser.Parse(new byte[] { 0xFF, 0x00 });
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseData_NullData_ThrowsException()
        {
            VarIntParser.Parse(null as byte[]);
        }

        [TestMethod]
        public void ParseData_ValueAbove0xFC_IsValid()
        {
            ParseAndCheckValues(new byte[] { 0xFD, 0xFF, 0x00 }, VariableIntegerSize.Int16, 0xFF, 0xFF, 0xFF, 0xFF);
        }

        [TestMethod]
        public void ParseData_ValueAbove0xFF_IsValid()
        {
            ParseAndCheckValues(new byte[] { 0xFD, 0x19, 0x34 }, VariableIntegerSize.Int16, 0x19, 0x3419, 0x3419, 0x3419);
        }

        [TestMethod]
        public void ParseData_ValueAbove0xFFFF_IsValid()
        {
            ParseAndCheckValues(new byte[] { 0xFE, 0x91, 0x45, 0xDC, 0x00 }, VariableIntegerSize.Int32, 0x91, 0x4591, 0xDC4591, 0xDC4591);
            ParseAndCheckValues(new byte[] { 0xFE, 0xE5, 0x81, 0x00, 0x08 }, VariableIntegerSize.Int32, 0xE5, -32283, 0x080081E5, 0x080081E5);
        }

        [TestMethod]
        public void ParseData_ValueAbove0xFFFFFFFF_IsValid()
        {
            ParseAndCheckValues(new byte[] { 0xFF, 0x57, 0x28, 0x4E, 0x56, 0xDA, 0xB4, 0x00, 0x00 }, VariableIntegerSize.Int64, 0x57, 0x2857, 0x564E2857, 0xB4DA564E2857);
            ParseAndCheckValues(new byte[] { 0xFF, 0x58, 0xC1, 0x59, 0x7D, 0xA1, 0x83, 0xF5, 0x4B }, VariableIntegerSize.Int64, 0x58, -16040, 0x7D59C158, 0x4BF583A17D59C158);
        }

        [TestMethod]
        public void ParseData_ValueBelow0xFC_IsValid()
        {
            ParseAndCheckValues(new byte[] { 0xBB }, VariableIntegerSize.Int8, 0xBB, 0xBB, 0xBB, 0xBB);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ParseStream_EmptyData_ThrowsException()
        {
            MemoryStream stream = new MemoryStream { Capacity = 0 };
            VarIntParser.Parse(stream);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseStream_NullData_ThrowsException()
        {
            VarIntParser.Parse(null as Stream);
        }

        [TestMethod]
        public void ParseStream_ValueAbove0xFC_IsValid()
        {
            ParseAndCheckValuesAsStream(new byte[] { 0xFD, 0xFF, 0x00 }, VariableIntegerSize.Int16, 0xFF, 0xFF, 0xFF, 0xFF);
        }

        [TestMethod]
        public void ParseStream_ValueAbove0xFF_IsValid()
        {
            ParseAndCheckValuesAsStream(new byte[] { 0xFD, 0x19, 0x34 }, VariableIntegerSize.Int16, 0x19, 0x3419, 0x3419, 0x3419);
        }

        [TestMethod]
        public void ParseStream_ValueAbove0xFFFF_IsValid()
        {
            ParseAndCheckValuesAsStream(new byte[] { 0xFE, 0x91, 0x45, 0xDC, 0x00 }, VariableIntegerSize.Int32, 0x91, 0x4591, 0xDC4591, 0xDC4591);
            ParseAndCheckValuesAsStream(new byte[] { 0xFE, 0xE5, 0x81, 0x00, 0x08 }, VariableIntegerSize.Int32, 0xE5, -32283, 0x080081E5, 0x080081E5);
        }

        [TestMethod]
        public void ParseStream_ValueAbove0xFFFFFFFF_IsValid()
        {
            ParseAndCheckValuesAsStream(new byte[] { 0xFF, 0x57, 0x28, 0x4E, 0x56, 0xDA, 0xB4, 0x00, 0x00 }, VariableIntegerSize.Int64, 0x57, 0x2857, 0x564E2857, 0xB4DA564E2857);
            ParseAndCheckValuesAsStream(new byte[] { 0xFF, 0x58, 0xC1, 0x59, 0x7D, 0xA1, 0x83, 0xF5, 0x4B }, VariableIntegerSize.Int64, 0x58, -16040, 0x7D59C158, 0x4BF583A17D59C158);
        }

        [TestMethod]
        public void ParseStream_ValueBelow0xFC_IsValid()
        {
            ParseAndCheckValuesAsStream(new byte[] { 0xBB }, VariableIntegerSize.Int8, 0xBB, 0xBB, 0xBB, 0xBB);
        }

        #endregion
        #endregion
    }
}
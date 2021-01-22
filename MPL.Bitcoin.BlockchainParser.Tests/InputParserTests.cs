using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace MPL.Bitcoin.BlockchainParser.Tests
{
    [TestClass]
    public class InputParserTests
    {
        #region Declarations
        #region _Constants_
        private const string cDATA_INVALID = "0102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f201020304007041d011aaabbccdd";
        private const string cDATA_VALID = "0102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f20102030400704ffff001d011aaabbccdd";

        private const uint cVALID_OUTPUT = 0x40302010U;
        private const string cVALID_SCRIPTSIG = "04ffff001d011a";
        private const uint cVALID_SEQUENCE = 0xddccbbaaU;
        private const string cVALID_TRANSACTION = "0102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f20";

        #endregion
        #endregion

        #region Methods
        #region _Test Methods_
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseData_EmptyData_ThrowsException()
        {
            InputParser.Parse(new byte[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ParseData_InvalidDataTooShort_ThrowsException()
        {
            byte[] data;

            data = HelperFunctions.ConvertHex(cDATA_INVALID);
            InputParser.Parse(data);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseData_NullData_ThrowsException()
        {
            InputParser.Parse(null as byte[]);
        }

        [TestMethod]
        public void ParseData_ValidData_IsValid()
        {
            byte[] data;
            Input result;
            byte[] scriptSig;
            byte[] transactionID;

            data = HelperFunctions.ConvertHex(cDATA_VALID);
            result = InputParser.Parse(data);
            Assert.IsNotNull(result);

            Assert.AreEqual(result.OutputID, cVALID_OUTPUT);

            scriptSig = HelperFunctions.ConvertHex(cVALID_SCRIPTSIG);
            Assert.AreEqual(result.ScriptSig.Length, scriptSig.Length);
            Assert.IsTrue(result.ScriptSig.SequenceEqual(scriptSig));

            Assert.AreEqual(result.Sequence, cVALID_SEQUENCE);

            transactionID = HelperFunctions.ConvertHex(cVALID_TRANSACTION);
            Assert.AreEqual(result.Transaction.Length, transactionID.Length);
            Assert.IsTrue(result.Transaction.SequenceEqual(transactionID));
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ParseStream_EmptyData_ThrowsException()
        {
            MemoryStream stream = new MemoryStream { Capacity = 0 };
            InputParser.Parse(stream);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ParseStream_InvalidDataTooShort_ThrowsException()
        {
            byte[] data;
            MemoryStream stream;

            data = HelperFunctions.ConvertHex(cDATA_INVALID);
            stream = new MemoryStream(data);
            InputParser.Parse(stream);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseStream_NullData_ThrowsException()
        {
            InputParser.Parse(null as Stream);
        }

        [TestMethod]
        public void ParseStream_ValidData_IsValid()
        {
            byte[] data;
            Input result;
            MemoryStream stream;
            byte[] scriptSig;
            byte[] transactionID;

            data = HelperFunctions.ConvertHex(cDATA_VALID);
            stream = new MemoryStream(data);
            result = InputParser.Parse(stream);
            Assert.IsNotNull(result);

            Assert.AreEqual(result.OutputID, cVALID_OUTPUT);

            scriptSig = HelperFunctions.ConvertHex(cVALID_SCRIPTSIG);
            Assert.AreEqual(result.ScriptSig.Length, scriptSig.Length);
            Assert.IsTrue(result.ScriptSig.SequenceEqual(scriptSig));

            Assert.AreEqual(result.Sequence, cVALID_SEQUENCE);

            transactionID = HelperFunctions.ConvertHex(cVALID_TRANSACTION);
            Assert.AreEqual(result.Transaction.Length, transactionID.Length);
            Assert.IsTrue(result.Transaction.SequenceEqual(transactionID));
        }

        #endregion
        #endregion
    }
}
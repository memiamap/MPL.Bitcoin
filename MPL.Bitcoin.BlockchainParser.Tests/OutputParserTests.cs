using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace MPL.Bitcoin.BlockchainParser.Tests
{
    [TestClass]
    public class OutputParserTests
    {
        #region Declarations
        #region _Constants_
        private const string cDATA_INVALID = "010203040506070843410435d66d6cef63a3461110c810975b8816308372b58274d88436a974b478d98d8d972f7233ea8a5242d151de9d4b1ac11a6f7f8460e8f9b146d97c7bad980cc5";
        private const string cDATA_VALID = "010203040506070843410435d66d6cef63a3461110c810975b8816308372b58274d88436a974b478d98d8d972f7233ea8a5242d151de9d4b1ac11a6f7f8460e8f9b146d97c7bad980cc5ceac";

        private const string cVALID_SCRIPTSIG = "410435d66d6cef63a3461110c810975b8816308372b58274d88436a974b478d98d8d972f7233ea8a5242d151de9d4b1ac11a6f7f8460e8f9b146d97c7bad980cc5ceac";
        private const long cVALID_VALUE = 0x0807060504030201;

        #endregion
        #endregion

        #region Methods
        #region _Test Methods_
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseData_EmptyData_ThrowsException()
        {
            OutputParser.Parse(new byte[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ParseData_InvalidDataTooShort_ThrowsException()
        {
            byte[] data;

            data = HelperFunctions.ConvertHex(cDATA_INVALID);
            OutputParser.Parse(data);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseData_NullData_ThrowsException()
        {
            OutputParser.Parse(null as byte[]);
        }

        [TestMethod]
        public void ParseData_ValidData_IsValid()
        {
            byte[] data;
            Output result;
            byte[] scriptSig;

            data = HelperFunctions.ConvertHex(cDATA_VALID);
            result = OutputParser.Parse(data);
            Assert.IsNotNull(result);

            scriptSig = HelperFunctions.ConvertHex(cVALID_SCRIPTSIG);
            Assert.AreEqual(result.ScriptPubKey.Length, scriptSig.Length);
            Assert.IsTrue(result.ScriptPubKey.SequenceEqual(scriptSig));

            Assert.AreEqual(result.Value, cVALID_VALUE);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ParseStream_EmptyData_ThrowsException()
        {
            MemoryStream stream = new MemoryStream { Capacity = 0 };
            OutputParser.Parse(stream);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ParseStream_InvalidDataTooShort_ThrowsException()
        {
            byte[] data;
            MemoryStream stream;

            data = HelperFunctions.ConvertHex(cDATA_INVALID);
            stream = new MemoryStream(data);
            OutputParser.Parse(stream);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseStream_NullData_ThrowsException()
        {
            OutputParser.Parse(null as Stream);
        }

        [TestMethod]
        public void ParseStream_ValidData_IsValid()
        {
            byte[] data;
            Output result;
            byte[] scriptSig;
            MemoryStream stream;

            data = HelperFunctions.ConvertHex(cDATA_VALID);
            stream = new MemoryStream(data);
            result = OutputParser.Parse(stream);
            Assert.IsNotNull(result);

            scriptSig = HelperFunctions.ConvertHex(cVALID_SCRIPTSIG);
            Assert.AreEqual(result.ScriptPubKey.Length, scriptSig.Length);
            Assert.IsTrue(result.ScriptPubKey.SequenceEqual(scriptSig));

            Assert.AreEqual(result.Value, cVALID_VALUE);
        }

        #endregion
        #endregion
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace MPL.Bitcoin.BlockchainParser.Tests
{
    [TestClass]
    public class TransactionParserTests
    {
        #region Declarations
        #region _Constants_
        private const string cDATA_INVALID = "01000000010000000000000000000000000000000000000000000000000000000000000000ffffffff0704ffff001d011affffffff0100f2052a0100000043410435d66d6cef63a3461110c810975b8816308372b58274d88436a974b478d98d8d972f7233ea8a5242d151";
        private const string cDATA_VALID_1_1 = "01000000010000000000000000000000000000000000000000000000000000000000000000ffffffff0704ffff001d011affffffff0100f2052a0100000043410435d66d6cef63a3461110c810975b8816308372b58274d88436a974b478d98d8d972f7233ea8a5242d151de9d4b1ac11a6f7f8460e8f9b146d97c7bad980cc5ceac00000000";

        #endregion
        #endregion

        #region Methods
        #region _Test Methods_
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseData_EmptyData_ThrowsException()
        {
            TransactionParser.Parse(new byte[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ParseData_InvalidDataTooShort_ThrowsException()
        {
            byte[] data;

            data = HelperFunctions.ConvertHex(cDATA_INVALID);
            TransactionParser.Parse(data);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseData_NullData_ThrowsException()
        {
            TransactionParser.Parse(null as byte[]);
        }

        [TestMethod]
        public void ParseData_ValidData_IsValid()
        {
            byte[] data;
            Transaction result;

            data = HelperFunctions.ConvertHex(cDATA_VALID_1_1);
            result = TransactionParser.Parse(data);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Inputs.Count, 1);
            Assert.AreEqual(result.LockTime.Type, LockTimeType.NoLockTime);
            Assert.AreEqual(result.Outputs.Count, 1);
            Assert.AreEqual(result.Version, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ParseStream_EmptyData_ThrowsException()
        {
            MemoryStream stream = new MemoryStream { Capacity = 0 };
            TransactionParser.Parse(stream);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ParseStream_InvalidDataTooShort_ThrowsException()
        {
            byte[] data;
            MemoryStream stream;

            data = HelperFunctions.ConvertHex(cDATA_INVALID);
            stream = new MemoryStream(data);
            TransactionParser.Parse(stream);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ParseStream_NullData_ThrowsException()
        {
            TransactionParser.Parse(null as Stream);
        }

        [TestMethod]
        public void ParseStream_ValidData_IsValid()
        {
            byte[] data;
            Transaction result;
            MemoryStream stream;

            data = HelperFunctions.ConvertHex(cDATA_VALID_1_1);
            stream = new MemoryStream(data);
            result = TransactionParser.Parse(stream);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Inputs.Count, 1);
            Assert.AreEqual(result.LockTime.Type, LockTimeType.NoLockTime);
            Assert.AreEqual(result.Outputs.Count, 1);
            Assert.AreEqual(result.Version, 1);
        }

        #endregion
        #endregion
    }
}
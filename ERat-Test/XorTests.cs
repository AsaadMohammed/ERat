using Microsoft.VisualStudio.TestTools.UnitTesting;
using ERat.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERat.Encryption.Tests {
    [TestClass()]
    public class XorTests {
        [TestMethod()]
        public void EncryptTest() {
            Xor xor = new Xor(0xab);
            Assert.AreEqual<byte>(xor.Encrypt(new byte[] { 0xba, 0xda, 0xff })[0], 0x11);
            Assert.AreEqual<byte>(xor.Encrypt(new byte[] { 0xba, 0xda, 0xff })[1], 0x71);
            Assert.AreEqual<byte>(xor.Encrypt(new byte[] { 0xba, 0xda, 0xff })[2], 0x54);
        }

        [TestMethod()]
        public void DecryptTest() {
            Xor xor = new Xor(0xab);
            Assert.AreEqual<byte>(xor.Decrypt(new byte[] { 0x11, 0x71, 0x54 })[0], 0xba);
            Assert.AreEqual<byte>(xor.Decrypt(new byte[] { 0x11, 0x71, 0x54 })[1], 0xda);
            Assert.AreEqual<byte>(xor.Decrypt(new byte[] { 0x11, 0x71, 0x54 })[2], 0xff);
        }
    }
}
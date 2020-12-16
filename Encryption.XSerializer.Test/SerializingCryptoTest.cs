using Cactus.Blade.Encryption;
using Cactus.Blade.Encryption.XSerializer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Text;
using XSerializer.Encryption;

namespace Encryption.XSerializer.Test
{
    [TestClass]
    public class SerializingCryptoTest
    {
        private const int Bar = 123;
        private const string Baz = "abc";
        private const double Qux = 543.21;

        private const string FooXmlFormat = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Foo xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Bar>{0}</Bar><Baz>{1}</Baz><Qux>{2}</Qux></Foo>";
        private static readonly string FooXml = string.Format(FooXmlFormat, EncryptRaw(Bar), EncryptRaw(Baz), EncryptRaw(Qux));

        private const string FooJsonFormat = "{{\"Bar\":\"{0}\",\"Baz\":\"{1}\",\"Qux\":\"{2}\"}}";
        private static readonly string FooJson = string.Format(FooJsonFormat, EncryptRaw(Bar), EncryptJsonString(Baz), EncryptRaw(Qux));

        private static readonly Mock<Base64Crypto> MockCrypto = new Mock<Base64Crypto>() { CallBase = true };

        static SerializingCryptoTest() => Crypto.SetCurrent(MockCrypto.Object);

        [TestMethod]
        public void TheDefaultEncryptionMechanismHasItsCryptoSetFromCryptoCurrent()
        {
            Assert.AreEqual(SerializingCrypto.EncryptionMechanism.Crypto, Crypto.Current);
        }

        [TestMethod]
        public void ToXmlWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            MockCrypto.Invocations.Clear();

            var foo = new Foo { Bar = Bar, Baz = Baz, Qux = Qux };

            SerializingCrypto.ToXml(foo);

            MockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(o => o == null)), Times.Once());
        }

        [TestMethod]
        public void ToXmlWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            MockCrypto.Invocations.Clear();

            var foo = new Foo { Bar = Bar, Baz = Baz, Qux = Qux };

            const string credentialName = "foobar";

            SerializingCrypto.ToXml(foo, credentialName);

            MockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(o => o == credentialName)), Times.Once());
        }

        [TestMethod]
        public void FromXmlWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            MockCrypto.Invocations.Clear();

            SerializingCrypto.FromXml<Foo>(FooXml);

            MockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(o => o == null)), Times.Once());
        }

        [TestMethod]
        public void FromXmlWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            MockCrypto.Invocations.Clear();

            var credentialName = "foobar";

            SerializingCrypto.FromXml<Foo>(FooXml, credentialName);

            MockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(o => o == credentialName)), Times.Once());
        }

        [TestMethod]
        public void ToJsonWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            MockCrypto.Invocations.Clear();

            var foo = new Foo { Bar = Bar, Baz = Baz, Qux = Qux };

            SerializingCrypto.ToJson(foo);

            MockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(o => o == null)), Times.Once());
        }

        [TestMethod]
        public void ToJsonWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            MockCrypto.Invocations.Clear();

            var foo = new Foo { Bar = Bar, Baz = Baz, Qux = Qux };

            const string credentialName = "foobar";

            SerializingCrypto.ToJson(foo, credentialName);

            MockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(o => o == credentialName)), Times.Once());
        }

        [TestMethod]
        public void FromJsonWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            MockCrypto.Invocations.Clear();

            SerializingCrypto.FromJson<Foo>(FooJson);

            MockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(o => o == null)), Times.Once());
        }

        [TestMethod]
        public void FromJsonWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            MockCrypto.Invocations.Clear();

            const string credentialName = "foobar";

            SerializingCrypto.FromJson<Foo>(FooJson, credentialName);

            MockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(o => o == credentialName)), Times.Once());
        }

        [TestMethod]
        public void CryptoToXmlWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            MockCrypto.Invocations.Clear();

            var foo = new Foo { Bar = Bar, Baz = Baz, Qux = Qux };

            Crypto.Current.ToXml(foo);

            MockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(o => o == null)), Times.Once());
        }

        [TestMethod]
        public void CryptoToXmlWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            MockCrypto.Invocations.Clear();

            var foo = new Foo { Bar = Bar, Baz = Baz, Qux = Qux };

            const string credentialName = "foobar";

            Crypto.Current.ToXml(foo, credentialName);

            MockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(o => o == credentialName)), Times.Once());
        }

        [TestMethod]
        public void CryptoFromXmlWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            MockCrypto.Invocations.Clear();

            Crypto.Current.FromXml<Foo>(FooXml);

            MockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(o => o == null)), Times.Once());
        }

        [TestMethod]
        public void CryptoFromXmlWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            MockCrypto.Invocations.Clear();

            const string credentialName = "foobar";

            Crypto.Current.FromXml<Foo>(FooXml, credentialName);

            MockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(o => o == credentialName)), Times.Once());
        }

        [TestMethod]
        public void CryptoToJsonWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            MockCrypto.Invocations.Clear();

            var foo = new Foo { Bar = Bar, Baz = Baz, Qux = Qux };

            Crypto.Current.ToJson(foo);

            MockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(o => o == null)), Times.Once());
        }

        [TestMethod]
        public void CryptoToJsonWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            MockCrypto.Invocations.Clear();

            var foo = new Foo { Bar = Bar, Baz = Baz, Qux = Qux };

            var credentialName = "foobar";

            Crypto.Current.ToJson(foo, credentialName);

            MockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(o => o == credentialName)), Times.Once());
        }

        [TestMethod]
        public void CryptoFromJsonWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            MockCrypto.Invocations.Clear();

            Crypto.Current.FromJson<Foo>(FooJson);

            MockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(o => o == null)), Times.Once());
        }

        [TestMethod]
        public void CryptoFromJsonWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            MockCrypto.Invocations.Clear();

            const string credentialName = "foobar";

            Crypto.Current.FromJson<Foo>(FooJson, credentialName);

            MockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(o => o == credentialName)), Times.Once());
        }

        [TestMethod]
        public void ToXmlSerializesCorrectly()
        {
            var foo = new Foo { Bar = Bar, Baz = Baz, Qux = Qux };

            var xml = SerializingCrypto.ToXml(foo);
            var expectedXml = string.Format(FooXmlFormat,
                EncryptRaw(foo.Bar), EncryptRaw(foo.Baz), EncryptRaw(foo.Qux));

            Assert.AreEqual(xml, expectedXml);
        }

        [TestMethod]
        public void FromXmlDeserializesCorrectly()
        {
            var foo = SerializingCrypto.FromXml<Foo>(FooXml);

            Assert.AreEqual(foo.Bar, Bar);
            Assert.AreEqual(foo.Baz, Baz);
            Assert.AreEqual(foo.Qux, Qux);
        }

        [TestMethod]
        public void ToJsonSerializesCorrectly()
        {
            var foo = new Foo { Bar = Bar, Baz = Baz, Qux = Qux };

            var json = SerializingCrypto.ToJson(foo);
            var expectedJson = string.Format(FooJsonFormat,
                EncryptRaw(foo.Bar), EncryptJsonString(foo.Baz), EncryptRaw(foo.Qux));

            Assert.AreEqual(json, expectedJson);
        }

        [TestMethod]
        public void FromJsonDeserializesCorrectly()
        {
            var foo = SerializingCrypto.FromJson<Foo>(FooJson);

            Assert.AreEqual(foo.Bar, Bar);
            Assert.AreEqual(foo.Baz, Baz);
            Assert.AreEqual(foo.Qux, Qux);
        }

        [TestMethod]
        public void CryptoToXmlSerializesCorrectly()
        {
            var foo = new Foo { Bar = Bar, Baz = Baz, Qux = Qux };

            var xml = Crypto.Current.ToXml(foo);
            var expectedXml = string.Format(FooXmlFormat,
                EncryptRaw(foo.Bar), EncryptRaw(foo.Baz), EncryptRaw(foo.Qux));

            Assert.AreEqual(xml, expectedXml);
        }

        [TestMethod]
        public void CryptoFromXmlDeserializesCorrectly()
        {
            var foo = Crypto.Current.FromXml<Foo>(FooXml);

            Assert.AreEqual(foo.Bar, Bar);
            Assert.AreEqual(foo.Baz, Baz);
            Assert.AreEqual(foo.Qux, Qux);
        }

        [TestMethod]
        public void CryptoToJsonSerializesCorrectly()
        {
            var foo = new Foo { Bar = Bar, Baz = Baz, Qux = Qux };

            var json = Crypto.Current.ToJson(foo);
            var expectedJson = string.Format(FooJsonFormat,
                EncryptRaw(foo.Bar), EncryptJsonString(foo.Baz), EncryptRaw(foo.Qux));

            Assert.AreEqual(json, expectedJson);
        }

        [TestMethod]
        public void CryptoFromJsonDeserializesCorrectly()
        {
            var foo = Crypto.Current.FromJson<Foo>(FooJson);

            Assert.AreEqual(foo.Bar, Bar);
            Assert.AreEqual(foo.Baz, Baz);
            Assert.AreEqual(foo.Qux, Qux);
        }

        public class Foo
        {
            [Encrypt]
            public int Bar { get; set; }

            [Encrypt]
            public string Baz { get; set; }

            [Encrypt]
            public double Qux { get; set; }
        }

        public class Base64Crypto : ICrypto
        {
            public virtual bool CanDecrypt(string credentialName) => true;
            public virtual bool CanEncrypt(string credentialName) => true;
            public virtual string Decrypt(string cipherText, string credentialName) => Base64.Decrypt(cipherText);
            public virtual byte[] Decrypt(byte[] cipherText, string credentialName) => throw new NotImplementedException();
            public virtual string Encrypt(string plainText, string credentialName) => Base64.Encrypt(plainText);
            public virtual byte[] Encrypt(byte[] plainText, string credentialName) => throw new NotImplementedException();
            public virtual IDecryptor GetDecryptor(string credentialName) => MockDecryptor.Object;
            public virtual IEncryptor GetEncryptor(string credentialName) => MockEncryptor.Object;
            public Mock<Base64Encryptor> MockEncryptor { get; } = new Mock<Base64Encryptor>() { CallBase = true };
            public Mock<Base64Decryptor> MockDecryptor { get; } = new Mock<Base64Decryptor>() { CallBase = true };
        }

        public class Base64Encryptor : IEncryptor
        {
            public void Dispose() { }
            public string Encrypt(string plainText) => Base64.Encrypt(plainText);
            public byte[] Encrypt(byte[] plainText) => throw new NotImplementedException();
        }

        public class Base64Decryptor : IDecryptor
        {
            public void Dispose() { }
            public string Decrypt(string cipherText) => Base64.Decrypt(cipherText);
            public byte[] Decrypt(byte[] cipherText) => throw new NotImplementedException();
        }

        private static class Base64
        {
            public static string Encrypt(string plainText) => Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
            public static string Decrypt(string cipherText) => Encoding.UTF8.GetString(Convert.FromBase64String(cipherText));
        }

        private static string EncryptJsonString(string value) => Base64.Encrypt("\"" + value + "\"");
        private static string EncryptRaw(object value) => Base64.Encrypt(value.ToString());
    }
}

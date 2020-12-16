using Cactus.Blade.Encryption;
using Cactus.Blade.Encryption.XSerializer.XSerializer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using XSerializer;

namespace Encryption.XSerializer.Test
{
    [TestClass]
    public class CryptoEncryptionMechanismTest
    {
        [TestMethod]
        public void CryptoPropertyIsSetFromConstructorParameter()
        {
            var mockCrypto = new Mock<ICrypto>();

            var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

            Assert.AreEqual(encryptionMechanism.Crypto, mockCrypto.Object);
        }

        [TestMethod]
        public void EncryptCallsCryptoGetEncryptorWhenSerializationStateIsEmpty()
        {
            var mockCrypto = new Mock<ICrypto>();
            var mockEncryptor = new Mock<IEncryptor>();

            mockCrypto.Setup(c => c.GetEncryptor(It.IsAny<string>())).Returns(() => mockEncryptor.Object);

            var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

            const string credentialName = "foobar";
            var serializationState = new SerializationState();

            encryptionMechanism.Encrypt("foo", credentialName, serializationState);

            mockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(obj => obj == credentialName)), Times.Once());
        }

        [TestMethod]
        public void GetEncryptorIsNotCalledWhenSerializationStateIsNotEmpty()
        {
            var mockCrypto = new Mock<ICrypto>();
            var mockEncryptor = new Mock<IEncryptor>();

            mockCrypto.Setup(c => c.GetEncryptor(It.IsAny<string>())).Returns(() => mockEncryptor.Object);

            var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

            const string credentialName = "foobar";
            var serializationState = new SerializationState();

            serializationState.Get(() => mockEncryptor.Object);

            encryptionMechanism.Encrypt("foo", credentialName, serializationState);

            mockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(obj => obj == credentialName)), Times.Never());
        }

        [TestMethod]
        public void TheCachedEncryptorReturnsTheReturnValue()
        {
            var mockCrypto = new Mock<ICrypto>();
            var mockEncryptor = new Mock<IEncryptor>();

            mockCrypto.Setup(c => c.GetEncryptor(It.IsAny<string>())).Returns(() => mockEncryptor.Object);
            mockEncryptor.Setup(e => e.Encrypt(It.IsAny<string>())).Returns("bar");

            var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

            var credentialName = "foobar";
            var serializationState = new SerializationState();

            serializationState.Get(() => mockEncryptor.Object);

            var encrypted = encryptionMechanism.Encrypt("foo", credentialName, serializationState);

            Assert.AreEqual(encrypted, "bar");
        }

        [TestMethod]
        public void DecryptCallsCryptoGetDecryptorWhenSerializationStateIsEmpty()
        {
            var mockCrypto = new Mock<ICrypto>();
            var mockDecryptor = new Mock<IDecryptor>();

            mockCrypto.Setup(c => c.GetDecryptor(It.IsAny<string>())).Returns(() => mockDecryptor.Object);

            var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

            const string credentialName = "foobar";
            var serializationState = new SerializationState();

            encryptionMechanism.Decrypt("foo", credentialName, serializationState);

            mockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(obj => obj == credentialName)), Times.Once());
        }

        [TestMethod]
        public void GetDecryptorIsNotCalledWhenSerializationStateIsNotEmpty()
        {
            var mockCrypto = new Mock<ICrypto>();
            var mockDecryptor = new Mock<IDecryptor>();

            mockCrypto.Setup(c => c.GetDecryptor(It.IsAny<string>())).Returns(() => mockDecryptor.Object);

            var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

            const string credentialName = "foobar";
            var serializationState = new SerializationState();

            serializationState.Get(() => mockDecryptor.Object);

            encryptionMechanism.Decrypt("foo", credentialName, serializationState);

            mockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(obj => obj == credentialName)), Times.Never());
        }

        [TestMethod]
        public void TheCachedDecryptorReturnsTheReturnValue()
        {
            var mockCrypto = new Mock<ICrypto>();
            var mockDecryptor = new Mock<IDecryptor>();

            mockCrypto.Setup(c => c.GetDecryptor(It.IsAny<string>())).Returns(() => mockDecryptor.Object);
            mockDecryptor.Setup(e => e.Decrypt(It.IsAny<string>())).Returns("bar");

            var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

            const string credentialName = "foobar";
            var serializationState = new SerializationState();

            serializationState.Get(() => mockDecryptor.Object);

            var decrypted = encryptionMechanism.Decrypt("foo", credentialName, serializationState);

            Assert.AreEqual(decrypted, "bar");
        }
    }
}

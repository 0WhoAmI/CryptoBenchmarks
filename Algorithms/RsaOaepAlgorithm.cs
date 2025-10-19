using CryptoBenchmarks.Interfaces;
using System.Security.Cryptography;

namespace CryptoBenchmarks.Algorithms
{
    public class RsaOaepAlgorithm : IEncryptionAlgorithm
    {
        public string Name => "RSA-OAEP";
        public int KeySize { get; }

        private RSA rsa;

        public RsaOaepAlgorithm(int keySize)
        {
            KeySize = keySize;
        }

        public void GenerateKey()
        {
            rsa = RSA.Create(KeySize);
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            return rsa.Encrypt(plaintext, RSAEncryptionPadding.OaepSHA256);
        }

        public byte[] Decrypt(byte[] ciphertext)
        {
            return rsa.Decrypt(ciphertext, RSAEncryptionPadding.OaepSHA256);
        }
    }
}

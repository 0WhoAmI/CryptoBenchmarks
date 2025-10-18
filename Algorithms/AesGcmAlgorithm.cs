using CryptoBenchmarks.Algorithms.Interfaces;
using System.Security.Cryptography;

namespace CryptoBenchmarks.Algorithms
{
    public class AesGcmAlgorithm : IEncryptionAlgorithm
    {
        public string Name => "AES-GCM";
        public int KeySize { get; }

        private byte[] key;
        private byte[] nonce;

        public AesGcmAlgorithm(int keySize)
        {
            KeySize = keySize;
        }

        public void GenerateKey()
        {
            key = RandomNumberGenerator.GetBytes(KeySize / 8);
            nonce = RandomNumberGenerator.GetBytes(12);
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            byte[] ciphertext = new byte[plaintext.Length];
            byte[] tag = new byte[16];

            using var aesGcm = new AesGcm(key);
            aesGcm.Encrypt(nonce, plaintext, ciphertext, tag);
            return ciphertext.Concat(tag).ToArray();
        }

        public byte[] Decrypt(byte[] ciphertext)
        {
            var tag = ciphertext[^16..];
            var data = ciphertext[..^16];
            byte[] plaintext = new byte[data.Length];

            using var aesGcm = new AesGcm(key);
            aesGcm.Decrypt(nonce, data, tag, plaintext);
            return plaintext;
        }
    }
}

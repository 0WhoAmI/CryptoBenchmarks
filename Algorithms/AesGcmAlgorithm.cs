using CryptoBenchmarks.Interfaces;
using System.Security.Cryptography;

namespace CryptoBenchmarks.Algorithms
{
    public class AesGcmAlgorithm : IEncryptionAlgorithm
    {
        public string Name => "AES-GCM";
        public int KeySize { get; }

        private AesGcm aesGcm;
        private byte[] key;
        private byte[] nonce;
        private byte[] tag;

        public AesGcmAlgorithm(int keySize)
        {
            KeySize = keySize;
        }

        public void GenerateKey()
        {
            key = RandomNumberGenerator.GetBytes(KeySize / 8);  // konwertuje bity na bajty
            aesGcm = new AesGcm(key);
            nonce = RandomNumberGenerator.GetBytes(12); // Nonce to 12 bajtów zgodnie ze standardem AES-GCM. (standardowy rozmiar nonce = 96 bitów)
            tag = new byte[16]; // AES-GCM używa 128-bitowego tagu domyślnie (16 B).
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            byte[] ciphertext = new byte[plaintext.Length];
            aesGcm.Encrypt(nonce, plaintext, ciphertext, tag);
            return ciphertext.Concat(tag).ToArray();
        }

        public byte[] Decrypt(byte[] ciphertext)
        {
            byte[] tag = ciphertext[^16..]; // wybiera ostatnie 16 bajtów tablicy
            byte[] data = ciphertext[..^16]; // wybiera wszystko oprócz ostatnich 16 bajtów tablicy,
            byte[] plaintext = new byte[data.Length];

            aesGcm.Decrypt(nonce, data, tag, plaintext);
            return plaintext;
        }
    }
}

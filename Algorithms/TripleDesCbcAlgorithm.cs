using CryptoBenchmarks.Algorithms.Interfaces;
using System.Security.Cryptography;

namespace CryptoBenchmarks.Algorithms
{
    internal class TripleDesCbcAlgorithm : IEncryptionAlgorithm
    {
        public string Name => "3DES-CBC";
        public int KeySize => 168;

        private byte[] key;
        private byte[] iv;

        public void GenerateKey()
        {
            using var tdes = TripleDES.Create();
            tdes.GenerateKey();
            tdes.GenerateIV();
            key = tdes.Key;
            iv = tdes.IV;
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            using var tdes = TripleDES.Create();
            tdes.Key = key;
            tdes.IV = iv;
            tdes.Mode = CipherMode.CBC;
            tdes.Padding = PaddingMode.PKCS7;
            using var enc = tdes.CreateEncryptor();
            return enc.TransformFinalBlock(plaintext, 0, plaintext.Length);
        }

        public byte[] Decrypt(byte[] ciphertext)
        {
            using var tdes = TripleDES.Create();
            tdes.Key = key;
            tdes.IV = iv;
            tdes.Mode = CipherMode.CBC;
            tdes.Padding = PaddingMode.PKCS7;
            using var dec = tdes.CreateDecryptor();
            return dec.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
        }
    }
}

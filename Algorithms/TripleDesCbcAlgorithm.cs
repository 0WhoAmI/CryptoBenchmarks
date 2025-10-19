using CryptoBenchmarks.Interfaces;
using System.Security.Cryptography;

namespace CryptoBenchmarks.Algorithms
{
    internal class TripleDesCbcAlgorithm : IEncryptionAlgorithm
    {
        public string Name => "3DES-CBC";
        public int KeySize => 168;

        private TripleDES tdes;

        public void GenerateKey()
        {
            tdes = TripleDES.Create();
            tdes.GenerateKey();
            tdes.GenerateIV(); // ustawiamy wektor inicjalizacyjny CBC
            tdes.Mode = CipherMode.CBC; // blokowy tryb szyfrowania (każdy blok XOR z poprzednim)
            tdes.Padding = PaddingMode.PKCS7; // dopełnienie danych do pełnych bloków DES
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            using ICryptoTransform enc = tdes.CreateEncryptor();
            return enc.TransformFinalBlock(plaintext, 0, plaintext.Length);
        }

        public byte[] Decrypt(byte[] ciphertext)
        {
            using ICryptoTransform dec = tdes.CreateDecryptor();
            return dec.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
        }
    }
}

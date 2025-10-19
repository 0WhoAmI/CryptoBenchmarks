namespace CryptoBenchmarks.Interfaces
{
    public interface IEncryptionAlgorithm
    {
        string Name { get; }
        int KeySize { get; }

        void GenerateKey();
        byte[] Encrypt(byte[] plaintext);
        byte[] Decrypt(byte[] ciphertext);
    }
}

namespace ERat.Encryption {
    interface EncryptorInterface {
        byte[] Encrypt(byte[] buffer);
        byte[] Decrypt(byte[] buffer);
    }
}

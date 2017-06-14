namespace ERat.Encryption {
    public class Xor : EncryptorInterface {
        public byte EncryptionKey { get; set; }

        public Xor(byte key) {
            EncryptionKey = key;
        }

        public byte[] Decrypt(byte[] buffer) {
            return Encrypt(buffer);
        }

        public byte[] Encrypt(byte[] buffer) {
            byte[] buf = new byte[buffer.Length];

            int pos = 0;
            foreach(byte b in buffer) {
                buf[pos] = (byte)(b ^ EncryptionKey);
                pos += 1;
            }

            return buf;
        }

        public byte GenerateKey() {
            return (byte)(new System.Random().Next(0x00, 0xff));
        }
    }
}

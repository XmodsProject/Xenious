
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Xbox360.Crypto
{
    public class Rijndael
    {
        private RijndaelManaged AES_H;
        private byte[] Key;
        private int BlockSize = 128;
        public Rijndael(byte[] KeyIn, int BlockSizeBits = 128)
        {
            this.Key = KeyIn;
            this.BlockSize = BlockSizeBits;
        }

        public byte[] Encrypt(byte[] plainData)
        {
            byte[] result = new byte[plainData.Length];
            byte[] iv = new byte[16]
            {
                0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
            };

            this.AES_H = new RijndaelManaged();
            this.AES_H.BlockSize = this.BlockSize;
            this.AES_H.KeySize = 128;
            this.AES_H.Padding = PaddingMode.None;

            MemoryStream ms = new MemoryStream(result);
            ICryptoTransform cth = this.AES_H.CreateEncryptor(this.Key, iv);
            CryptoStream cs = new CryptoStream(ms, cth, CryptoStreamMode.Write);
            cs.Write(plainData, 0, plainData.Length);
            cs.Close();
            cs = null;
            return ms.ToArray();
        }

        public byte[] Decrypt(byte[] encData)
        {
            byte[] result = new byte[encData.Length];
            byte[] iv = new byte[16]
            {
                0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
            };

            this.AES_H = new RijndaelManaged();
            this.AES_H.BlockSize = this.BlockSize;
            this.AES_H.KeySize = 128;
            this.AES_H.Padding = PaddingMode.None;

            MemoryStream ms = new MemoryStream(result);
            ICryptoTransform cth = this.AES_H.CreateDecryptor(this.Key, iv);
            CryptoStream cs = new CryptoStream(ms, cth, CryptoStreamMode.Write);
            cs.Write(encData, 0, encData.Length);
            cs.Close();
            cs = null;
            return ms.ToArray();
        }

    }
}

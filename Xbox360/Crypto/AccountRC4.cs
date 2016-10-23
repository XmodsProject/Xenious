using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Xbox360.Crypto
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RC4Session
    {
        public byte[] Key;
        public int SBoxLen;
        public byte[] SBox;
        public int I;
        public int J;
    }

    public abstract class AccountRC4
    {
        protected AccountRC4()
        {
        }

        public static RC4Session RC4CreateSession(byte[] key)
        {
            RC4Session session = new RC4Session
            {
                Key = key,
                I = 0,
                J = 0,
                SBoxLen = 0x100,
                SBox = new byte[0x100]
            };
            for (int i = 0; i < session.SBoxLen; i++)
            {
                session.SBox[i] = (byte)i;
            }
            int index = 0;
            for (int j = 0; j < session.SBoxLen; j++)
            {
                index = ((index + session.SBox[j]) + key[j % key.Length]) % session.SBoxLen;
                byte num4 = session.SBox[index];
                session.SBox[index] = session.SBox[j];
                session.SBox[j] = num4;
            }
            return session;
        }

        public static void RC4Decrypt(ref RC4Session session, byte[] data, int index, int count)
        {
            RC4Encrypt(ref session, data, index, count);
        }

        public static void RC4Encrypt(ref RC4Session session, byte[] data, int index, int count)
        {
            int num = index;
            do
            {
                session.I = (session.I + 1) % 0x100;
                session.J = (session.J + session.SBox[session.I]) % 0x100;
                byte num2 = session.SBox[session.I];
                session.SBox[session.I] = session.SBox[session.J];
                session.SBox[session.J] = num2;
                byte num3 = data[num];
                byte num4 = session.SBox[(session.SBox[session.I] + session.SBox[session.J]) % 0x100];
                data[num] = (byte)(num3 ^ num4);
                num++;
            }
            while (num != (index + count));
        }

        public static void ResetSession(ref RC4Session session)
        {
            session = RC4CreateSession(session.Key);
        }
    }
}

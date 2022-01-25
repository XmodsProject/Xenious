/*
 * With thanks to the team at RGLoader, this was (Util/XCompress.cs) taken and rewritten to suit the xex decompression process.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Xbox360.Compression
{
    public class LZX
    {
        [DllImport("LZX.dll", EntryPoint = "Decompress", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Decompress(byte[] CompData, int CDSize, [In][Out] byte[] OutputData, int ODSize, uint WindowSize);
        [DllImport("LZX.dll", EntryPoint = "Version", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 Version();

    }
}


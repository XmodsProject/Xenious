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
    public class XCompress
    {
        private static readonly bool IsMachine64Bit = (IntPtr.Size == 8);

        public static int LDICreateDecompression(ref int pcbDataBlockMax, ref LzxDecompress pvConfiguration, int pfnma, int pfnmf, IntPtr pcbSrcBufferMin, ref int unknown, ref int ldiContext)
        {
            if (Is64Bit)
            {
                return LDICreateDecompression64(ref pcbDataBlockMax, ref pvConfiguration, pfnma, pfnmf, pcbSrcBufferMin, ref unknown, ref ldiContext);
            }
            return LDICreateDecompression32(ref pcbDataBlockMax, ref pvConfiguration, pfnma, pfnmf, pcbSrcBufferMin, ref unknown, ref ldiContext);
        }

        [DllImport("xcompress32.dll", EntryPoint = "LDICreateDecompression")]
        private static extern int LDICreateDecompression32(ref int pcbDataBlockMax, ref LzxDecompress pvConfiguration, int pfnma, int pfnmf, IntPtr pcbSrcBufferMin, ref int unknown, ref int ldiContext);
        [DllImport("xcompress64.dll", EntryPoint = "LDICreateDecompression")]
        private static extern int LDICreateDecompression64(ref int pcbDataBlockMax, ref LzxDecompress pvConfiguration, int pfnma, int pfnmf, IntPtr pcbSrcBufferMin, ref int unknown, ref int ldiContext);
        public static int LDIDecompress(int context, byte[] pbSrc, int cbSrc, byte[] pbDst, ref int pcbDecompressed)
        {
            if (Is64Bit)
            {
                return LDIDecompress64(context, pbSrc, cbSrc, pbDst, ref pcbDecompressed);
            }
            return LDIDecompress32(context, pbSrc, cbSrc, pbDst, ref pcbDecompressed);
        }

        [DllImport("xcompress32.dll", EntryPoint = "LDIDecompress")]
        private static extern int LDIDecompress32(int context, byte[] pbSrc, int cbSrc, byte[] pbDst, ref int pcbDecompressed);
        [DllImport("xcompress64.dll", EntryPoint = "LDIDecompress")]
        private static extern int LDIDecompress64(int context, byte[] pbSrc, int cbSrc, byte[] pbDst, ref int pcbDecompressed);
        public static int LDIDestroyDecompression(int context)
        {
            if (Is64Bit)
            {
                return LDIDestroyDecompression64(context);
            }
            return LDIDestroyDecompression32(context);
        }

        [DllImport("xcompress32.dll", EntryPoint = "LDIDestroyDecompression")]
        private static extern int LDIDestroyDecompression32(int context);
        [DllImport("xcompress64.dll", EntryPoint = "LDIDestroyDecompression")]
        private static extern int LDIDestroyDecompression64(int context);
        public static int LDIResetDecompression(int context)
        {
            if (Is64Bit)
            {
                return LDIResetDecompression64(context);
            }
            return LDIResetDecompression32(context);
        }

        [DllImport("xcompress32.dll", EntryPoint = "LDIResetDecompression")]
        private static extern int LDIResetDecompression32(int context);
        [DllImport("xcompress64.dll", EntryPoint = "LDIResetDecompression")]
        private static extern int LDIResetDecompression64(int context);
        public static int LDISetWindowData(int context, byte[] window, int size)
        {
            if (Is64Bit)
            {
                return LDISetWindowData64(context, window, size);
            }
            return LDISetWindowData32(context, window, size);
        }

        [DllImport("xcompress32.dll", EntryPoint = "LDISetWindowData")]
        private static extern int LDISetWindowData32(int context, byte[] window, int size);
        [DllImport("xcompress64.dll", EntryPoint = "LDISetWindowData")]
        private static extern int LDISetWindowData64(int context, byte[] window, int size);

        private static bool Is64Bit
        {
            get
            {
                return IsMachine64Bit;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LzxDecompress
        {
            public long WindowSize;
            public long CpuType;
        }

        public byte[] decompress_in_chunks(byte[] data, long window_size = 0x20000L)
        {
            LzxDecompress decomp;
            int pcb_data_block_max = 0x800000;
            int ldi_context = -1;
            int unknown = 0;
            decomp.CpuType = 0x1103L;
            decomp.WindowSize = window_size;

            IntPtr pcb_src_buf_min = Marshal.AllocHGlobal(0x23200);
            if(LDICreateDecompression(ref pcb_data_block_max, ref decomp, 0, 0, pcb_src_buf_min, ref unknown, ref ldi_context) != 0)
            {
                throw new Exception("Failed to create decompression method !");
            }

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            int pos = 0;
            byte[] buf, pb_src, pb_dest;
            ushort count = 0;
            int pcb_decompressed = 0;
            while(pos < data.Length)
            {
                #region Read Count
                buf = new byte[2]
                {
                    data[pos],
                    data[pos + 1]
                };
                if(BitConverter.IsLittleEndian)
                {
                    Array.Reverse(buf);
                }
                count = BitConverter.ToUInt16(buf, 0);
                #endregion
                if(count == 0)
                {
                    break;
                }

                #region Read PCB Decompressed
                buf = new byte[2]
                {
                    data[pos + 2],
                    data[pos + 3]
                };
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(buf);
                }
                pcb_decompressed = BitConverter.ToUInt16(buf, 0);
                #endregion
                if (pcb_decompressed == 0)
                {
                    break;
                }

                pb_src = new byte[count];
                Array.Copy(data, pos + 4, pb_src, 0, count);

                pb_dest = new byte[pcb_decompressed];
                
                if(LDIDecompress(ldi_context, pb_src, count, pb_dest, ref pcb_decompressed) != 0)
                {
                    throw new Exception("Failed to decompress data !");
                }
                stream.Write(pb_dest, 0, pcb_decompressed);
            }
            if(LDIDestroyDecompression(ldi_context) != 0)
            {
                throw new Exception("Failed to destroy decompression method !");
            }
            Marshal.FreeHGlobal(pcb_src_buf_min);
            data = null;
            return stream.ToArray();
            
        }
    }
}

/*
 * Document Written by [Hect0r]
 * With thanks to http://www.nxp.com/files/32bit/doc/ref_manual/ALTIVECPEM.pdf
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenon.PowerPC.CPU
{
    // This class attempts to emulate locally a cpu.
    // Needs some work haha.
    public class VMX128Register
    {
        public UInt64 Lower;
        public UInt64 Upper;

        public VMX128Register(UInt64 Upper, UInt64 Lower)
        {
            this.Upper = Upper;
            this.Lower = Lower;
        }
        public VMX128Register(byte[] RegisterData)
        {
            // Extract Upper in LittleEndian.
            byte[] UpBuf = new byte[8]
            {
                RegisterData[15],
                RegisterData[14],
                RegisterData[13],
                RegisterData[12],
                RegisterData[11],
                RegisterData[10],
                RegisterData[9],
                RegisterData[8],
            };

            // Extract Lower in LittleEndian.
            byte[] LowBuf = new byte[8]
            {
                RegisterData[7],
                RegisterData[6],
                RegisterData[5],
                RegisterData[4],
                RegisterData[3],
                RegisterData[2],
                RegisterData[1],
                RegisterData[0]
            };

            // Reverse if is Big Endian.
            if (!BitConverter.IsLittleEndian) { Array.Reverse(UpBuf); Array.Reverse(LowBuf); }

            // Now convert from byte[] to UInt64.
            this.Upper = BitConverter.ToUInt64(UpBuf, 0);
            this.Lower = BitConverter.ToUInt64(LowBuf, 0);
        }

         
    }
    public class XenonCPU
    {
        public List<ulong> GPRS; // General Purpose Registers.
        public List<double> FPRS; // Floating Point Registers.
        public List<ulong> SPRs; // Special Purpose Registers.
        public List<ulong> SRs; // Segment Reigsters.
        public List<VMX128Register> VMXs;

        public XenonCPU()
        {
            GPRS = new List<ulong>(32);
            FPRS = new List<double>(32);
            SPRs = new List<ulong>(1024);
            SRs = new List<ulong>(16);
            VMXs = new List<VMX128Register>(128);
        }

        // Altivec PowerPC Instructions.
        public void AddImmediate(byte d, byte a, UInt16 simm) // addi
        {
            GPRS[d] = (GPRS[a] + simm);
        }
        public void MoveFromLinkRegister(byte ra)
        {
            //LR = GPRS[ra];
            // TODO Branch to link reigter.
        } 
        //public void StoreDoubleword()
    }
}

/*
 * Document Written by [Hect0r]
 * With thanks to http://www.nxp.com/files/32bit/doc/ref_manual/ALTIVECPEM.pdf
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenonPowerPC.CPU
{
    // This class attempts to emulate locally a cpu.
    // Needs some work haha.
    public class VMX128Register
    {
        public ulong Lower;
        public ulong Upper;
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

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

    public class XenonCPU
    {
        public List<UInt32> GPRS; // General Purpose Registers.
        public List<float> FPRS; // Floating Point Registers.
        public List<UInt32> SPRs; // Special Purpose Registers.
        public List<UInt32> SRs; // Segment Reigsters.

        // Missing TODO Vector Registers.

        public XenonCPU()
        {
            GPRS = new List<uint>(32);
            FPRS = new List<float>(32);
            SPRs = new List<UInt32>(1024);
            SRs = new List<uint>(16);
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

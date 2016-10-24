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

        // Count Register.
        public UInt32 CTR
        {
            get { return SPRs[9]; }
            set { SPRs[9] = value; }
        }
        // Link Register
        public UInt32 LR
        {
            get { return SPRs[8]; }
            set { SPRs[8] = value; }
        }
        // XER
        public UInt32 XER
        {
            get { return SPRs[1]; }
            set { SPRs[1] = value; }
        }
        // AltiVec Save Register
        public UInt32 VRSAVE
        {
            get { return SPRs[256]; }
            set { SPRs[256] = value; }
        }
        // Instruction BAT Reigsters.
        public UInt32 IBAT0U
        {
            get { return SPRs[528]; }
            set { SPRs[528] = value; }
        }
        public UInt32 IBAT0L
        {
            get { return SPRs[529]; }
            set { SPRs[529] = value; }
        }
        public UInt32 IBAT1U
        {
            get { return SPRs[530]; }
            set { SPRs[530] = value; }
        }
        public UInt32 IBAT1L
        {
            get { return SPRs[531]; }
            set { SPRs[531] = value; }
        }
        public UInt32 IBAT2U
        {
            get { return SPRs[532]; }
            set { SPRs[532] = value; }
        }
        public UInt32 IBAT2L
        {
            get { return SPRs[533]; }
            set { SPRs[533] = value; }
        }
        public UInt32 IBAT3U
        {
            get { return SPRs[534]; }
            set { SPRs[534] = value; }
        }
        public UInt32 IBAT3L
        {
            get { return SPRs[535]; }
            set { SPRs[535] = value; }
        }
        // SDR1
        public UInt32 SDR1
        {
            get { return SPRs[25]; }
            set { SPRs[25] = value; }
        }
        //Data Address Register.
        public UInt32 DAR
        {
            get { return SPRs[19]; }
            set { SPRs[19] = value; }
        }
        // SPRGs
        public UInt32 SPRG0
        {
            get { return SPRs[272]; }
            set { SPRs[272] = value; }
        }
        public UInt32 SPRG1
        {
            get { return SPRs[273]; }
            set { SPRs[273] = value; }
        }
        public UInt32 SPRG2
        {
            get { return SPRs[274]; }
            set { SPRs[274] = value; }
        }
        public UInt32 SPRG3
        {
            get { return SPRs[275]; }
            set { SPRs[275] = value; }
        }
        // Decrementer.
        public UInt32 DEC
        {
            get { return SPRs[22]; }
            set { SPRs[22] = value; }
        }
        // Processor Version Register
        public UInt32 PVR
        {
            get { return SPRs[287]; }
            set { SPRs[287] = value; }
        }
        // Data BAT Registers
        public UInt32 DBAT0U
        {
            get { return SPRs[536]; }
            set { SPRs[536] = value; }
        }
        public UInt32 DBAT0L
        {
            get { return SPRs[537]; }
            set { SPRs[537] = value; }
        }
        public UInt32 DBAT1U
        {
            get { return SPRs[538]; }
            set { SPRs[538] = value; }
        }
        public UInt32 DBAT1L
        {
            get { return SPRs[539]; }
            set { SPRs[539] = value; }
        }
        public UInt32 DBAT2U
        {
            get { return SPRs[540]; }
            set { SPRs[540] = value; }
        }
        public UInt32 DBAT2L
        {
            get { return SPRs[541]; }
            set { SPRs[541] = value; }
        }
        public UInt32 DBAT3U
        {
            get { return SPRs[542]; }
            set { SPRs[542] = value; }
        }
        public UInt32 DBAT3L
        {
            get { return SPRs[543]; }
            set { SPRs[543] = value; }
        }
        // DSISR
        public UInt32 DSISR
        {
            get { return SPRs[18]; }
            set { SPRs[18] = value; }
        }
        // Save and Restore Registers.
        public UInt32 SRR0
        {
            get { return SPRs[26]; }
            set { SPRs[26] = value; }
        }
        public UInt32 SRR1
        {
            get { return SPRs[27]; }
            set { SPRs[27] = value; }
        }
        // Floating Point Exception Cause Register.
        public UInt32 FPECR
        {
            get { return SPRs[1022]; }
            set { SPRs[1022] = value; }
        }
        // Data Address Breakpoint Register.
        public UInt32 BABR
        {
            get { return SPRs[1013]; }
            set { SPRs[1013] = value; }
        }
        // External Address Register.
        public UInt32 EAR
        {
            get { return SPRs[282]; }
            set { SPRs[282] = value; }
        }
        // Processor ID Register.
        public UInt32 PIR
        {
            get { return SPRs[1023]; }
            set { SPRs[1023] = value; }
        }

        // Floating Point Status and Control Register.
        public float FPSCR = 0;
        // Condition Register.
        public UInt32 CR = 0;
        // Vect0r Status and Control Register.
        public UInt32 VSCR = 0;
        // Machine State Register.
        public UInt32 MSR = 0;

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
            LR = GPRS[ra];
            // TODO Branch to link reigter.
        } 
        //public void StoreDoubleword()
    }
}

/*
 * Taken from the PowerPC Altivec/VMX Extension Module for IDA,
 * Original Plugin provided and coded by Dean and xorloser (and others). 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPC
{
    public enum AltivecOpID
    {
        NO_OPERAND,
        VA,
        VB,
        VC,
        VD, VS = VD,
        SIMM,
        UIMM,
        SHB,
        RA,
        RB,
        STRM,

        // Takires: Added operand identifiers
        RS, RT = RS,
        L15,
        L9_10, LS = L9_10,
        L10, L = L10,
        VD128, VS128 = VD128,
        CRM,
        VA128,
        VB128,
        VC128,
        VPERM128,
        VD3D0,
        VD3D1,
        VD3D2,
        RA0,
        SPR,

        // gekko specific
        FA,
        FB,
        FC,
        FD,
        FS = FD,

        crfD,

        WB,
        IB,
        WC,
        IC,
        DRA,
        DRB,
    }
    public class cbea_sprg
    {
        public int sprg;
        public string shortName;
        public string comment;
    }

    public class altivec_operand
    {
        public int bits;
        public int shift;
    }

    class PowerPC
    {
        public static List<altivec_operand> altivec_operands()
        {
            List<altivec_operand> g_altivecOperands = new List<altivec_operand>();
            g_altivecOperands.Add(new altivec_operand(){ bits = 0, shift = 0 });// No Operand
            g_altivecOperands.Add(new altivec_operand() { bits = 5, shift = 16 });	// VA
            g_altivecOperands.Add(new altivec_operand() { bits = 5, shift = 11 });	// VB
            g_altivecOperands.Add(new altivec_operand() { bits = 5, shift = 6 });	// VC
            g_altivecOperands.Add(new altivec_operand() { bits = 5, shift = 21 });	// VD / VS
            g_altivecOperands.Add(new altivec_operand() { bits = 5, shift = 16 });	// SIMM
            g_altivecOperands.Add(new altivec_operand() { bits = 5, shift = 16 });	// UIMM
            g_altivecOperands.Add(new altivec_operand() { bits = 4, shift = 6 });	// SHB
            g_altivecOperands.Add(new altivec_operand() { bits = 5, shift = 16 });	// RA
            g_altivecOperands.Add(new altivec_operand() { bits = 5, shift = 11 });	// RB
            g_altivecOperands.Add(new altivec_operand() { bits = 2, shift = 21 });  // STRM

            // Takires: Added operands
            g_altivecOperands.Add(new altivec_operand() { bits = 5, shift = 21 });	// RS / RT
            g_altivecOperands.Add(new altivec_operand() { bits = 1, shift = 16 });	// L15
            g_altivecOperands.Add(new altivec_operand() { bits = 2, shift = 21 });	// L9_10
            g_altivecOperands.Add(new altivec_operand() { bits = 1, shift = 21 });	// L10
            g_altivecOperands.Add(new altivec_operand() { bits = 0, shift = 0 });	// VD128 / VS128
            g_altivecOperands.Add(new altivec_operand() { bits = 8, shift = 12 });	// CRM
            g_altivecOperands.Add(new altivec_operand() { bits = 0, shift = 0 });	// VA128
            g_altivecOperands.Add(new altivec_operand() { bits = 0, shift = 0 });	// VB128
            g_altivecOperands.Add(new altivec_operand() { bits = 3, shift = 8 });	// VC128
            g_altivecOperands.Add(new altivec_operand() { bits = 0, shift = 0 });	// VPERM128
            g_altivecOperands.Add(new altivec_operand() { bits = 3, shift = 18 });	// VD3D0
            g_altivecOperands.Add(new altivec_operand() { bits = 2, shift = 16 });	// VD3D1
            g_altivecOperands.Add(new altivec_operand() { bits = 2, shift = 6 });	// VD3D2
            g_altivecOperands.Add(new altivec_operand() { bits = 5, shift = 16 });	// RA0
            g_altivecOperands.Add(new altivec_operand() { bits = 10, shift = 11 });	// SPR


	        // gekko specific
            g_altivecOperands.Add(new altivec_operand() { bits = 5, shift = 16 });	// FA
            g_altivecOperands.Add(new altivec_operand() { bits = 5, shift = 11 });	// FB
            g_altivecOperands.Add(new altivec_operand() { bits = 5, shift = 6 });	// FC
            g_altivecOperands.Add(new altivec_operand() { bits = 5, shift = 21 });	// FD/FS

            g_altivecOperands.Add(new altivec_operand() { bits = 3, shift = 23 });	//crfD,


            g_altivecOperands.Add(new altivec_operand() { bits = 1, shift = 16 });	//WB,
            g_altivecOperands.Add(new altivec_operand() { bits = 3, shift = 12 });	//IB,
            g_altivecOperands.Add(new altivec_operand() { bits = 1, shift = 10 });	//WC,
            g_altivecOperands.Add(new altivec_operand() { bits = 3, shift = 7 });	//IC,
            g_altivecOperands.Add(new altivec_operand() { bits = 5, shift = 16 });//DRA,
            g_altivecOperands.Add(new altivec_operand() { bits = 5, shift = 11 });//DRB
            return g_altivecOperands;
        }
        public static List<cbea_sprg> cbeaSprgs()
        {
            List<cbea_sprg> g_cbeaSprgs = new List<cbea_sprg>();
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 1023, shortName = "PIR", comment = "Processor Identification Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 1022, shortName = "BP_VR", comment = "CBEA-Compliant Processor Version Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 1017, shortName = "HID6", comment = "Hardware Implementation Register 6" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 1015, shortName = "DABRX", comment = "Data Address Breakpoint Register Extension" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 1013, shortName = "DABR", comment = "Data Address Breakpoint Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 1012, shortName = "HID4", comment = "Hardware Implementation Register 4" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 1009, shortName = "HID1", comment = "Hardware Implementation Register 1" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 1008, shortName = "HID0", comment = "Hardware Implementation Register 0" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 981, shortName = "ICIDR", comment = "Instruction Class ID Register 1" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 980, shortName = "IRMR1", comment = "Instruction Range Mask Register 1" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 979, shortName = "IRSR1", comment = "Instruction Range Start Register 1" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 978, shortName = "ICIDR0", comment = "Instruction Class ID Register 0" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 977, shortName = "IRMR0", comment = "Instruction Range Mask Register 0" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 976, shortName = "IRSR0", comment = "Instruction Range Start Register 0" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 957, shortName = "DCIDR1", comment = "Data Class ID Register 1" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 956, shortName = "DRMR1", comment = "Data Range Mask Register 1" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 955, shortName = "DRSR1", comment = "Data Range Start Register 1" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 954, shortName = "DCIDR0", comment = "Data Class ID Register 0" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 953, shortName = "DRMR0", comment = "Data Range Mask Register 0" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 952, shortName = "DRSR0", comment = "Data Range Start Register 0" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 951, shortName = "PPE_TLB_RMT", comment = "PPE Translation Lookaside Buffer RMT Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 949, shortName = "PPE_TLB_RPN", comment = "PPE Translation Lookaside Buffer Real-Page Number" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 948, shortName = "PPE_TLB_VPN", comment = "PPE Translation Lookaside Buffer Virtual-Page Number" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 947, shortName = "PPE_TLB_Index", comment = "PPE Translation Lookaside Buffer Index Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 946, shortName = "PPE_TLB_Index_Hint", comment = "PPE Translation Lookaside Buffer Index Hint Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 922, shortName = "TTR", comment = "Thread Switch Timeout Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 921, shortName = "TSCR", comment = "Thread Switch Control Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 897, shortName = "TSRR", comment = "Thread Status Register Remote" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 896, shortName = "TSRL", comment = "Thread Status Register Local" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 319, shortName = "LPIDR", comment = "Logical Partition Identity Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 318, shortName = "LPCR", comment = "Logical Partition Control Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 315, shortName = "HSRR1", comment = "Hypervisor Machine Status Save/Restore Register 1" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 314, shortName = "HSRR0", comment = "Hypervisor Machine Status Save/Restore Register 0" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 313, shortName = "HRMOR", comment = "Hypervisor Real Mode Offset Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 312, shortName = "RMOR", comment = "Real Mode Offset Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 310, shortName = "HDEC", comment = "Hypervisor Decrementer Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 305, shortName = "HSPRG1", comment = "Hypervisor Software Use Special Purpose Register 1" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 304, shortName = "HSPRG0", comment = "Hypervisor Software Use Special Purpose Register 0" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 287, shortName = "PVR", comment = "PPE Processor Version Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 285, shortName = "TBU", comment = "Time Base Upper Register - Write Only" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 284, shortName = "TBL", comment = "Time Base Lower Register - Write Only" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 275, shortName = "SPRG3", comment = "Software Use Special Purpose Register 3" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 274, shortName = "SPRG2", comment = "Software Use Special Purpose Register 2" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 273, shortName = "SPRG1", comment = "Software Use Special Purpose Register 1" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 272, shortName = "SPRG0", comment = "Software Use Special Purpose Register 0" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 269, shortName = "TBU", comment = "Time Base Upper Register - Read Only" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 268, shortName = "TB", comment = "Time Base Register - Read Only" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 259, shortName = "SPRG3", comment = "Software Use Special Purpose Register 3" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 256, shortName = "VRSAVE", comment = "VXU Register Save" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 152, shortName = "CTRL", comment = "Control Register Write" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 136, shortName = "CTRL", comment = "Control Register Read" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 29, shortName = "ACCR", comment = "Address Compare Control Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 27, shortName = "SRR1", comment = "Machine Status Save/Restore Register 1" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 26, shortName = "SRR0", comment = "Machine Status Save/Restore Register 0" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 25, shortName = "SDR1", comment = "Storage Description Register 1" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 22, shortName = "DEC", comment = "Decrementer Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 19, shortName = "DAR", comment = "Data Address Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 18, shortName = "DSISR", comment = "Data Storage Interrupt Status Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 9, shortName = "CTR", comment = "Count Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 8, shortName = "LR", comment = "Link Register" });
            g_cbeaSprgs.Add(new cbea_sprg(){ sprg = 1, shortName = "XER", comment = "Fixed-Point exception Register" });
            return g_cbeaSprgs;
        }
       

    }
}

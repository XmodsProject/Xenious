using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenonPowerPC.PowerPC
{
    public enum op_codes : UInt32
    {
        addi = 939524096,
        beq = 1099038720,
        bne = 0x40820000,
        cmpwi = 738197504,
        extsb = 0x7c000774,
        extsb_condition = 0x7c000775,
        li = 939524096,
        lis= 1006632960,
        lwz = 0x80000000,
        bl = 1207959553,
        mflr = 2080899750,
        mtlr = 0x7c0803a6,
        stw = 2415919104,
        stwu = 2483027968,
        nop = 1610612736,
        unknown = 0
    }
}

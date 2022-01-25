using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenon.PowerPC.CPU
{
    public enum SpecialPurposeRegister : int
    {
        esr = 62,
        ivpr = 63,
        pid = 48,
        ctrlrd = 136,
        ctrlwr = 152,
        pvr = 287,
        hsprg0 = 304,
        hsprg1 = 305,
        hdsisr = 306,
        hdar = 307,
        dbcr0 = 308,
        dbcr1 = 309,
        hdec = 310,
        hior = 311,
        rmor = 312,
        hrmor = 313,
        hsrr0 = 314,
        hsrr1 = 315,
        dac1 = 316,
        dac2 = 317,
        lpcr = 318,
        lpidr = 319,
        tsr = 336,
        tcr = 340,
        tsrl = 896,
        tsrr = 897,
        tscr = 921,
        ttr = 922,
        PpeTlbIndexHint = 946,
        PpeTlbIndex = 947,
        PpeTlbVpn = 948,
        PpeTlbRpn = 949,
        PpeTlbRmt = 951,
        dsr0 = 952,
        drmr0 = 953,
        dcidr0 = 954,
        drsr1 = 955,
        drmr1 = 956,
        dcidr1 = 957,
        issr0 = 976,
        irmr0 = 977,
        icidr0 = 978,
        irsr1 = 979,
        irmr1 = 980,
        icidr1 = 981,
        hid0 = 1008,
        hid1 = 1009,
        hid4 = 1012,
        iabr = 1010,
        dabr = 1013,
        dabrx = 1015,
        buscsr = 1016,
        hid6 = 1017,
        l2sr = 1018,
        BpVr = 1022,
        pir = 1023
    }
}

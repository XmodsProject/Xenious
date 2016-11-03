using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenious.Xecutable
{
    public class Misc
    {
        public static Xbox360.XEX.XeGame_Ratings get_ratings_template()
        {
            Xbox360.XEX.XeGame_Ratings xgr = new Xbox360.XEX.XeGame_Ratings();
            xgr.bbfc = Xbox360.XEX.XeRating_bbfc.UNRATED;
            xgr.brazil = Xbox360.XEX.XeRating_brazil.UNRATED;
            xgr.cero = Xbox360.XEX.XeRating_cero.UNRATED;
            xgr.esrb = Xbox360.XEX.XeRating_esrb.UNRATED;
            xgr.fpb = Xbox360.XEX.XeRating_fpb.UNRATED;
            xgr.kmrb = Xbox360.XEX.XeRating_kmrb.UNRATED;
            xgr.oflcau = Xbox360.XEX.XeRating_oflc_au.UNRATED;
            xgr.oflcnz = Xbox360.XEX.XeRating_oflc_nz.UNRATED;
            xgr.pegi = Xbox360.XEX.XeRating_pegi.UNRATED;
            xgr.pegifi = Xbox360.XEX.XeRating_pegi_fi.UNRATED;
            xgr.pegipt = Xbox360.XEX.XeRating_pegi_pt.UNRATED;
            xgr.usk = Xbox360.XEX.XeRating_usk.UNRATED;
            xgr.reserved = new byte[52]
            {
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF
            };

            return xgr;
        }

        public static bool ratings_data_is_null(Xbox360.XEX.XeGame_Ratings xgr)
        {
            if (xgr.bbfc == Xbox360.XEX.XeRating_bbfc.UNRATED &&
               xgr.brazil == Xbox360.XEX.XeRating_brazil.UNRATED &&
               xgr.cero == Xbox360.XEX.XeRating_cero.UNRATED &&
               xgr.esrb == Xbox360.XEX.XeRating_esrb.UNRATED &&
               xgr.fpb == Xbox360.XEX.XeRating_fpb.UNRATED &&
               xgr.kmrb == Xbox360.XEX.XeRating_kmrb.UNRATED &&
               xgr.oflcau == Xbox360.XEX.XeRating_oflc_au.UNRATED &&
               xgr.oflcnz == Xbox360.XEX.XeRating_oflc_nz.UNRATED &&
               xgr.pegi == Xbox360.XEX.XeRating_pegi.UNRATED &&
               xgr.pegifi == Xbox360.XEX.XeRating_pegi_fi.UNRATED &&
               xgr.pegipt == Xbox360.XEX.XeRating_pegi_pt.UNRATED &&
               xgr.usk == Xbox360.XEX.XeRating_usk.UNRATED &&
               xgr.reserved == new byte[52] {
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF
               })
            {
                return true;
            }
            return false;
        }
    }
}

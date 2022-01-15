using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenonPowerPC.PowerPC.Format
{
    public  class OPF_XO
    {
        public OpCode Op;
        public OPF_XO(OpCode in_op)
        {
            Op = in_op;
        }
        public bool Rc
        {
            get
            {
                return (bool)((UInt32)(this.Op._OpData & (UInt32)0x80000000) == (UInt32)0x80000000);
            }
        }
        public bool OE
        {
            get
            {
                return (bool)((UInt32)(this.Op._OpData & (UInt32)0x200000) == (UInt32)0x200000);
            }
        }

        public UInt32 RB
        {
            get
            {
                return (UInt32)(this.Op._OpData & (UInt32)0x1F0000) >> 16;
            }
        }
        public UInt32 RA
        {
            get
            {
                return (UInt32)(this.Op._OpData & (UInt32)0xF800) >> 11;
            }
        }
        public UInt32 RD
        {
            get
            {
                return (UInt32)(this.Op._OpData & (UInt32)0x7C0) >> 6;
            }
        }

        public override string ToString()
        {
            string Buf = string.Format("[0x{0}]", this.Op._OpData.ToString("X8"));

            #region Addx
            if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C000214) == (UInt32)0x7C000214)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("add %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("add. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("addo %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("addo. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
            }
            #endregion
            #region Addex
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C000114) == (UInt32)0x7C000114)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("adde %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("adde. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("addeo %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("addeo. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
            }
            #endregion
            #region Addcx
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C000014) == (UInt32)0x7C000014)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("addc %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("addc. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("addco %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("addco. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
            }
            #endregion
            #region Addmex
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C0001D4) == (UInt32)0x7C0001D4)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("addme %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("addme. %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
                else if (this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("addmeo %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
                else if (this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("addmeo. %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
            }
            #endregion
            #region Addzex
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C000194) == (UInt32)0x7C000194)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("addze %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("addze. %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
                else if (this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("addzeo %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
                else if (this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("addzeo. %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
            }
            #endregion
            #region Divdx
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C0003D2) == (UInt32)0x7C0003D2)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("divd %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("divd. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("divdo %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("divdo. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
            }
            #endregion
            #region Divdux
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C000392) == (UInt32)0x7C000392)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("divdu %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("divdu. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("divduo %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("divduo. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
            }
            #endregion
            #region Divwx
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C0003D6) == (UInt32)0x7C0003D6)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("divw %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("divw. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("divwo %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("divwo. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
            }
            #endregion
            #region Divwux
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C000396) == (UInt32)0x7C000396)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("divuw %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("divwu. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("divwuo %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("divwuo. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
            }
            #endregion
            #region Mulhdx
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C000092) == (UInt32)0x7C000092)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("mulhd %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("mulhd. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
            }
            #endregion
            #region Mulhdux
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C000012) == (UInt32)0x7C000012)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("mulhdu %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("mulhdu. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
            }
            #endregion
            #region Mulhwx
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C000096) == (UInt32)0x7C000096)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("mulhw %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("mulhw. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
            }
            #endregion
            #region Mulhwux
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C000016) == (UInt32)0x7C000016)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("mulhwu %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("mulhwu. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
            }
            #endregion
            #region Mulldx
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C0001D2) == (UInt32)0x7C0001D2)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("mulld %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("mulld. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("mulldo %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("mulldo. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
            }
            #endregion
            #region Mullwx
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C0001D6) == (UInt32)0x7C0001D6)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("mullw %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("mullw. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("mullwo %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("mullwo. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
            }
            #endregion
            #region Negx 
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C0000D0) == (UInt32)0x7C0000D0)
            {
                if (this.RB == (UInt32)0 && this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("neg %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
                else if (this.RB == (UInt32)0 && this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("neg. %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
                else if (this.RB == (UInt32)0 && this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("nego %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
                else if (this.RB == (UInt32)0 && this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("nego. %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
            }
            #endregion
            #region Subfx
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C000050) == (UInt32)0x7C000050)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("subf %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("subf. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("subfo %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("subfo. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
            }
            #endregion
            #region Subfcx
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C000010) == (UInt32)0x7C000010)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("subfc %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("subfc. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("subfco %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("subfco. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
            }
            #endregion
            #region Subfex
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C000110) == (UInt32)0x7C000110)
            {
                if (this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("subfe %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("subfe. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("subfeo %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
                else if (this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("subfeo. %r{0}, %r{1}, %r{2}", this.RD.ToString(), this.RA.ToString(), this.RB.ToString());
                }
            }
            #endregion
            #region Subfmex
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C0001D0) == (UInt32)0x7C0001D0)
            {
                if (this.RB == (UInt32)0 && this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("subfme %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
                else if (this.RB == (UInt32)0 && this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("subfme. %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
                else if (this.RB == (UInt32)0 && this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("subfmeo %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
                else if (this.RB == (UInt32)0 && this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("subfmeo. %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
            }
            #endregion
            #region Subfzex
            else if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x7C000190) == (UInt32)0x7C000190)
            {
                if (this.RB == (UInt32)0 && this.OE == false && this.Rc == false)
                {
                    Buf = string.Format("subfze %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
                else if (this.RB == (UInt32)0 && this.OE == false && this.Rc == true)
                {
                    Buf = string.Format("subfze. %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
                else if (this.RB == (UInt32)0 && this.OE == true && this.Rc == false)
                {
                    Buf = string.Format("subfzeo %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
                else if (this.RB == (UInt32)0 && this.OE == true && this.Rc == true)
                {
                    Buf = string.Format("subfzeo. %r{0}, %r{1}", this.RD.ToString(), this.RA.ToString());
                }
            }
            #endregion

            return Buf;
        }

    }
}

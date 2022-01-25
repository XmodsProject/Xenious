using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenon.PowerPC.PPC64.Format
{
    public class OPF_I
    {
        private OpCode Op;

        public OPF_I(OpCode in_op)
        {
            Op = in_op;
        }
        public bool LK
        {
            get
            {
                return (bool)((UInt32)(this.Op._OpData & (UInt32)0x80000000) == (UInt32)0x80000000);
            }
        }
        public bool AA
        {
            get
            {
                return (bool)((UInt32)(this.Op._OpData & (UInt32)0x40000000) == (UInt32)0x40000000);
            }
        }
        public UInt64 LI
        {
            get
            {
                byte[] Buf = BitConverter.GetBytes((UInt32)((UInt32)(this.Op._OpData & (UInt32)0x3FFFFFC0) >> 6));
                if (BitConverter.IsLittleEndian) { Array.Reverse(Buf); }
                return (UInt64)BitConverter.ToUInt32(Buf, 0); // Sign Extend to UInt64.
            }
        }

        public override string ToString()
        {
            string Buf = string.Format("[0x{0}]", this.Op._OpData.ToString("X8"));

            #region bx
            if ((UInt32)((UInt32)this.Op._OpData & (UInt32)0x48000000) == (UInt32)0x48000000)
            {
                if (this.AA == false && this.LK == false)
                {
                    Buf = string.Format("b 0x{0}", (((UInt64)this.LI & (UInt32)0xFFFF) | (UInt64)0x00b0).ToString("X16"));
                }
                else if (this.AA == true && this.LK == false)
                {
                    Buf = string.Format("ba 0x{0}", (((UInt64)this.LI & (UInt32)0xFFFF) | (UInt64)0x00b0).ToString("X16"));
                }
                else if (this.AA == false && this.LK == true)
                {
                    Buf = string.Format("bl 0x{0}", (((UInt64)this.LI & (UInt32)0xFFFF) | (UInt64)0x00b0).ToString("X16"));
                }
                else if (this.AA == true && this.LK == true)
                {
                    Buf = string.Format("bla 0x{0}", (((UInt64)this.LI & (UInt32)0xFFFF) | (UInt64)0x00b0).ToString("X16"));
                }
            }
            #endregion

            return Buf;
        }
    }
}

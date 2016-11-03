/*
 * Reversed by [ Hect0r ] @ www.staticpi.net
 * With thanks to : http://www.free60.org/wiki/XEX
 * For inital stuff :)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360.XEX
{
    /*
     * Thanks Xorloser
     * http://www.xboxhacker.org/index.php?topic=9344.msg60570#msg60570
     */
    public class XeBaseFileInfoHeader
    {
        public Int32 info_size;
        public byte[] data;

        public XeEncryptionType enc_type
        {
            get
            {
                byte[] buf = new byte[2]
                {
                    data[5],
                    data[6]
                };

                if(BitConverter.IsLittleEndian)
                {
                    Array.Reverse(buf);
                }

                return (XeEncryptionType)BitConverter.ToUInt16(buf, 0);
            }
        }
        public XeCompressionType comp_type
        {
            get
            {
                byte[] buf = new byte[2]
                {
                    data[7],
                    data[8]
                };

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(buf);
                }

                return (XeCompressionType)BitConverter.ToUInt16(buf, 0);
            }
        }
    }
}

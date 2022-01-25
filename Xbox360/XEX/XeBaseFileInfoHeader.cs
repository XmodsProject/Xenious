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
                    data[0],
                    data[1]
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
                    data[2],
                    data[3]
                };

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(buf);
                }

                return (XeCompressionType)BitConverter.ToUInt16(buf, 0);
            }
        }
        public XeRawBaseFileInfo raw_base_file_info
        {
            get
            {
                XeRawBaseFileInfo info = new XeRawBaseFileInfo();
                info.block = new List<XeRawBaseFileBlock>();
                for(int i = 0; i < (info_size - 8) / 8; i++)
                {
                    #region Read Datasize.
                    byte[] buf = new byte[4] {
                        data[4], data[5], data[6], data[7]
                    };

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(buf);
                    }
                    #endregion
                    Int32 datasize = BitConverter.ToInt32(buf, 0);

                    #region Read Zerosize.
                    buf = new byte[4] {
                        data[8], data[9], data[10], data[11]
                    };

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(buf);
                    }
                    #endregion
                    Int32 zerosize = BitConverter.ToInt32(buf, 0);
                    info.block.Add(new XeRawBaseFileBlock()
                    {
                        data_size = datasize,
                        zero_size = zerosize
                    });
                }
                return info;
            }
        }
        public XeCompBaseFileInfo compressed_base_file_info
        {
            get
            {
                XeCompBaseFileInfo info = new XeCompBaseFileInfo();

                byte[] buf = null;
                #region Read Window Size
                buf = new byte[4] {
                    data[4], data[5], data[6], data[7]     
                };

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(buf);
                }
                #endregion
                info.compression_window = BitConverter.ToUInt32(buf, 0);
                #region Read Block Size
                buf = new byte[4] {
                        data[8], data[9], data[10], data[11]
                    };

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(buf);
                }
                #endregion
                info.block = new XeCompBaseFileBlock();
                info.block.block_size = BitConverter.ToUInt32(buf, 0);
                info.block.hash = new byte[20];
                Array.Copy(data, 12, info.block.hash, 0, 20);
                return info;
            }
        }
    }
}

// Big thanks to stoker25 & others for RGLoader
// Couldent of done this without the source so thank you
// To all RGloader contributers :)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbox360
{
    public class XenonNand
    {
        public UInt16 magic;
        public byte[] unknown;
        public byte copyright_sig;
        public string copyright;
        public UInt16 rgbp_indicator;
        public byte[] reserved;
        public UInt32 kv_size;
        public UInt32 sys_upd_address;
        public UInt16 sys_upd_count;
        public UInt16 kv_version;
        public UInt32 kv_address;
        public UInt32 fs_address;
        public UInt32 smc_cfg_address;
        public UInt32 smc_size;
        public UInt32 smc_address;
        public UInt16 build;
        public byte[] kd_net_data;
        public byte[] kd_net_ip;

        public int HeaderVersion
        {
            get
            {
                if (this.kv_version != 0x712)
                {
                    return 0;
                }
                return 1;
            }
        }
        public bool ContainsRGBP
        {
            get
            {
                return (this.rgbp_indicator == 0x1337);
            }
        }
    }
}

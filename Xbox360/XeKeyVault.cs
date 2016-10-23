using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenious.IO;

namespace Xbox360
{
    public class XeKeyVault
    {
        public byte[] HMAC_SHA1;
        public byte[] console_id;

        public XeKeyVault(FileIO IO)
        {
            IO.position = 0;
            HMAC_SHA1 = IO.read_bytes(16);
            IO.position = 0x9ca;
            console_id = IO.read_bytes(5);
        }
    }
}

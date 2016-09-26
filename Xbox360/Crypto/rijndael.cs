using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xenios.Crypto
{
    public class rijndael
    {
        byte[] seed_key;

        public void set_key(byte[] data)
        {
            this.seed_key = data;
        }

    }
}

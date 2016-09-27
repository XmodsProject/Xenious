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
    public class XeVersion
    {
        public UInt32 version;

        public UInt32 major
        {
            get { return (version >> 28); }
        }
        public UInt32 minor
        {
            get { return (version >> 24) & 0xF0; }
        }
        public UInt32 build
        {
            get { return (version >> 8) & 0x00FF; }
        }
        public UInt32 qfe
        {
            get { return (version & 0xFF00) & 0xFF; }
        }
        public string get_string
        {
            get { return major + "." + minor + "." + build + "." + qfe; }
        }
    }
}

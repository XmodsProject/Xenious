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
    public struct XeSection
    {
        public UInt32 page_size;
        public XeSectionType type
        {
            get
            {
                return (XeSectionType)(byte)(value >> 28);
            }
        }
        public UInt32 page_count
        {
            get
            {
                return (UInt32)(value & 0xF0000000);
            }
        }
        public UInt32 value;
        public byte[] digest;
    }
}

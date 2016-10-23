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
    public class XeExecutionInfo
    {
        public UInt32 media_id;
        public byte[] version;
        public byte[] base_version;
        public UInt32 title_id;
        public byte platform;
        public byte executable_table;
        public byte disc_number;
        public byte disc_count;
        public UInt32 savegame_id;
    }
}

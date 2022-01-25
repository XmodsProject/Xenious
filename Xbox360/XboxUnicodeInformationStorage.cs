using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbox360
{
    public  class XboxUnicodeInformationStorage
    {
        public Hect0rs.IO.IOH IO;
        public XUIS.Header Header;
        public List<string> Entrys;
        private long StartPos = 0;
        public XboxUnicodeInformationStorage(string File)
        {
            this.IO = new Hect0rs.IO.IOH(File, System.IO.FileMode.Open);
        }
        public XboxUnicodeInformationStorage(byte[] in_data)
        {
            this.IO = new Hect0rs.IO.IOH(new System.IO.MemoryStream(in_data));
        }

        public XboxUnicodeInformationStorage(Hect0rs.IO.IOH in_io)
        {
            IO = in_io;
        }

        public void SetStartPos(long Start)
        {
            if(Start < 0 || Start > this.IO.Length)
            {
                throw new Exception("Couldent Set Starting Position of the XUIS IO Handle.");
            }

            this.StartPos = Start;
        }

        public void Parse()
        {
            this.IO.Position = StartPos;

            this.Header = new XUIS.Header();
            this.Header.Magic = IO.ReadString(4);
            this.Header.Flags = IO.ReadByte();
            this.Header.FileVersion = IO.ReadByte();
            this.Header.FileEnd = IO.ReadUInt32(Hect0rs.IO.Endian.High);
            this.Header.StringsEmbedded = IO.ReadUInt16(Hect0rs.IO.Endian.High);

            string buf;
            this.Entrys = new List<string>();
            if ((this.Header.Flags & 1) == 1)
            {
                ushort len = 0;
                for(int i = 0; i < this.Header.StringsEmbedded; i++)
                {
                    // Read len
                    len = IO.ReadUInt16(Hect0rs.IO.Endian.High);
                    this.Entrys.Add(IO.ReadUnicodeStringFixedLength(((int)len * 2), Hect0rs.IO.Endian.High));
                }
                 
            }
            else
            {
                buf = IO.ReadString((int)this.Header.FileEnd - 12);

                int x = 0;
                for(int i = 0; i < this.Header.StringsEmbedded; i++)
                {
                    int len = 0;
                    while(x < buf.Length &&
                          buf.Substring(x, 1) != "\0")
                    {
                        x++;
                        len++;
                    }

                    this.Entrys.Add(buf.Substring(x - len, len));
                    x++;
                }

                
            }
            buf = null;
        }
    }
}

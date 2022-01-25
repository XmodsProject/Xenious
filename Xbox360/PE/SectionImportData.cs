using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hect0rs.IO;
using System.IO;

namespace Xbox360.PE
{
    public class SectionImportData
    {
        string filename;
        public List<UInt32> imports;

        public SectionImportData(string file)
        {
            filename = file;
        }

        public void read()
        {
            IOH io = new IOH(filename, System.IO.FileMode.Open);

            int num = (int)(io.Length / 4);
            imports = new List<uint>();
            for (int i = 0; i < num; i++)
            {
                imports.Add(io.ReadUInt32(Endian.High));
            }

            io.close();
            io = null;
        }
        public void write(IOH io)
        {
            for (int i = 0; i < imports.Count; i++)
            {
                io.Write(imports[i], Endian.High);
            }
        }
        public void overwrite()
        {
            File.Delete(filename);

            IOH io = new IOH(filename, FileMode.Create);
            io.Position = 0;
            write(io);
        }
    }
}

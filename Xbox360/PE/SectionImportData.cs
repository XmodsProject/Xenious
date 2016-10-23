using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xenious.IO;
using System.IO;
using Xbox360.IO;

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
            FileIO io = new FileIO(filename, System.IO.FileMode.Open);

            int num = (int)(io.length / 4);
            imports = new List<uint>();
            for (int i = 0; i < num; i++)
            {
                imports.Add(io.read_uint32(Endian.High));
            }

            io.close();
            io = null;
        }
        public void write(FileIO io)
        {
            for (int i = 0; i < imports.Count; i++)
            {
                io.write(imports[i], Endian.High);
            }
        }
        public void overwrite()
        {
            File.Delete(filename);

            FileIO io = new FileIO(filename, FileMode.Create);
            io.position = 0;
            write(io);
        }
    }
}

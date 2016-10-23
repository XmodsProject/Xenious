using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbox360.Kernal
{
    public enum MemoryType
    {
        XecutableOnly,
        XecutableAndKernal
    }
    public class XenonMemory
    {
        XenonExecutable app_xex;
        FileStream handle;
        long size;
        long address_start = 0x80000000;
        List<XenonExecutable> imports; // The App Imports such as something.dll. read only, get load_address from here.

        public MemoryType memory_type
        {
            get
            {
                return (size > 0x10000000 ? MemoryType.XecutableAndKernal : MemoryType.XecutableOnly);
            }
        }

        public XenonMemory(string filename)
        {
            handle = new FileStream(filename, FileMode.Open);
            size = handle.Length > 0x10000000 ? 0x20000000 : 0x10000000;
        }

        public long position
        {
            get
            {
                return (address_start + handle.Position);
            }
            set
            {
                if (value <= address_start)
                {
                    handle.Position = 0;
                }
                else { handle.Position = (value - address_start); }
            }
        }
        public byte[] Hypervisor
        {
            set
            {
                handle.Position = 0;

            }
        }
        public byte[] Kernal
        {
            set
            {
                handle.Position = 0x400000;
                this.write(value);
            }
        }
        public XenonExecutable App
        {
            set
            {
                app_xex = value;
            }
        }
        public List<XenonExecutable> get_app_imports {
            get
            {
                return imports;
            }
        }

        public void write(byte[] data)
        {
            long x = handle.Position;
            handle.Write(data, 0, data.Length);
            
            handle.Flush();
            handle.Position += (x + data.Length);
        }
        public byte[] read_bytes(int count, bool flip_bytes = false)
        {
            byte[] data = new byte[count];
            long x = handle.Position;
            handle.Read(data, 0, count);
            handle.Position = (x + count);
            return data;
        }

        public string[] get_app_imports_list()
        {
            string[] buf = new string[app_xex.import_libs.Count];
            int x = 0;
            foreach (Xbox360.XEX.XeImportLibary lib in app_xex.import_libs)
            {
                buf[x] = lib.name;
                x++;
            }
            return buf;
        }

        public bool add_new_import(XenonExecutable import)
        {
            // First check if imports needs a init.
            if(imports == null)
            {
                imports = new List<XenonExecutable>();
            }

            // Check import wont cross any paths.
            if (imports.Count > 0) {
                for (int x = 0; x < imports.Count; x++)
                {
                    // First check if it loads into its path.
                    if(import.cert.load_address <= (imports[x].cert.load_address + imports[x].cert.image_size))
                    {
                        return false;
                    }
                }
            }

            // Read PE.
            import.IO.position = import.pe_data_offset;
            byte[] data = import.IO.read_bytes((int)import.cert.image_size);

            // Write PE to its load address.
            this.position = import.cert.load_address;
            this.write(data);

            // Now add it to the list of imports.
            imports.Add(import);
            return true;
        }
        public string get_app_filename()
        {
            return (app_xex.orig_pe_name == null) ? app_xex.IO.file : app_xex.orig_pe_name;
        }
    }

}

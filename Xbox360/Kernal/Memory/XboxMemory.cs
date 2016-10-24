
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbox360.IO;
using Xenious.IO;

namespace Xbox360.Kernal.Memory
{
    public class XboxMemory 
    {
        private System.IO.FileStream input;
        private FileIO handle;
        private Int64 start_address = 0x80000000;
        private XenonExecutable main_app;
        private List<XenonExecutable> main_app_imports;

        public Int64 Position
        {
            get
            {
                return (start_address + handle.position);
            }
            set
            {
                Int64 val = (value - start_address);
                handle.position = val;
            }
        }
        public Int64 Length
        {
            get
            {
                return (Int64)handle.length;
            }
        }
        public XenonExecutable MainApp
        {
            get { return main_app; }
        }

        public void set_length(int length)
        {
            /*byte[] blank = new byte[16]
            {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            for(int i = 0; i < (length / 16); i++)
            {
                this.input.Position = (i == 0 ? 0 : (16 * i));
                this.input.Write(blank, 0, 16);
            }*/
            this.input.SetLength(length);
        }
        public XboxMemory(int length)
        {
            if(System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/cache/xbox_memory.bin"))
            {
                System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/cache/xbox_memory.bin");
            }
            this.input = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory + "/cache/xbox_memory.bin", System.IO.FileMode.Create);
            this.set_length(length);
            this.handle = new FileIO(input);
        }

        public void Write(byte[] input)
        {
            handle.write(input);
        }
        public byte[] ReadBytes(int count, bool reverse)
        {
            return handle.read_bytes(count, reverse);
        }

        public void LoadApp(XenonExecutable in_xex, List<XenonExecutable> imports)
        {
            // First, Load Main App.
            main_app = in_xex;

            // Next Extract PE.
            // The code previous should check for a XUIZ or other, 
            // Make sure it is a XEX package with pe only.
            main_app.IO.position = main_app.pe_data_offset;
            byte[] pe = this.handle.read_bytes((int)main_app.cert.image_size);

            // Write PE to its resulting load address.
            this.Position = main_app.cert.load_address;
            this.Write(pe);

            // Next Load Imports.
            if(imports.Count > 0)
            {
                main_app_imports = new List<XenonExecutable>();
                foreach(XenonExecutable import in imports)
                {
                    // Read its PE or Resource.
                    import.IO.position = import.pe_data_offset;
                    byte[] bin = import.IO.read_bytes((int)import.cert.image_size);

                    // Write it to the memory.
                    this.Position = import.cert.load_address;
                    this.Write(bin);

                    // Add to imports.
                    main_app_imports.Add(import);
                }
            }
        }
    }
}

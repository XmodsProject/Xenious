
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hect0rs.IO;

namespace Xbox360.Kernal.Memory
{
    public class XboxMemory 
    {
        private System.IO.FileStream input;
        private IOH Handle;
        private Int64 start_address = 0x80000000;
        private XenonExecutable main_app;
        private List<XenonExecutable> main_app_imports;
        

        public Int64 Position
        {
            get
            {
                return (start_address + Handle.Position);
            }
            set
            {
                Int64 val = (value - start_address);
                Handle.Position = val;
            }
        }
        public Int64 Length
        {
            get
            {
                return (Int64)Handle.Length;
            }
        }
        public XenonExecutable MainApp
        {
            get { return main_app; }
        }
        public List<XenonExecutable> AppImports
        {
            get { return main_app_imports; }
        }

        public void set_Length(int Length)
        {
            /*byte[] blank = new byte[16]
            {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            for(int i = 0; i < (Length / 16); i++)
            {
                this.inputPosition = (i == 0 ? 0 : (16 * i));
                this.input.Write(blank, 0, 16);
            }*/
            this.input.SetLength(Length);
        }
        public XboxMemory(int Length)
        {
            this.input = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory + "/cache/xbox_memory.bin", System.IO.FileMode.Create);
            this.set_Length(Length);
            this.Handle = new IOH(input);
        }

        public void Write(byte[] input)
        {
            Handle.Write(input);
        }
        public byte[] ReadBytes(int count, bool reverse)
        {
            return Handle.ReadBytes(count, reverse);
        }

        public void LoadApp(XenonExecutable in_xex, List<XenonExecutable> imports)
        {
            // First, Load Main App.
            main_app = in_xex;

            // Next Extract PE.
            // The code previous should check for a XUIZ or other, 
            // Make sure it is a XEX package with pe only.
            main_app.IO.Position = main_app.pe_data_offset;
            byte[] pe = main_app.IO.ReadBytes((int)main_app.cert.image_size);

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
                    import.IO.Position = import.pe_data_offset;
                    byte[] bin = import.IO.ReadBytes((int)import.cert.image_size);

                    // Write it to the memory.
                    this.Position = import.cert.load_address;
                    this.Write(bin);

                    // Add to imports.
                    main_app_imports.Add(import);
                }
            }
        }

        public void close()
        {
            this.main_app.IO.close();

            if(this.main_app_imports != null && this.main_app_imports.Count > 0)
            {
                foreach (XenonExecutable xex in this.main_app_imports)
                {
                    xex.IO.close();
                }
            }

            this.Handle.close();
        }
    }
}

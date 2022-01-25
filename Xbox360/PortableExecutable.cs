using Hect0rs.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbox360.PE;

namespace Xbox360
{
    public class PortableExecutable
    {
        public IOH IO;

        /* PE Stuff */
        public ImageDosHeader img_dos_h;
        public ImageFileHeader img_file_h;
        public ImageOptHeader img_opt_h;
        public List<ImageSectionHeader> img_sections;
        public SectionImportData pe_sec_idata;

        public PortableExecutable(string Input)
        {
            this.IO = new IOH(Input, System.IO.FileMode.Open);
        }
        public bool load_pe()
        {
            IO.Position = 0;

            string magic = IO.ReadString(2);

            if (magic == "MZ")
            {
                this.read_dos_header();
                this.read_file_header();
                this.read_image_opt_header();
                this.read_image_sections();
            }
            else { return false; }

            return true;
        }
        public void read_dos_header()
        {
            IO.Position = 0;
            img_dos_h = new ImageDosHeader();
            img_dos_h.e_magic = IO.ReadUInt16(Endian.Low);
            img_dos_h.e_cblp = IO.ReadUInt16(Endian.Low);
            img_dos_h.e_cp = IO.ReadUInt16(Endian.Low);
            img_dos_h.e_crlc = IO.ReadUInt16(Endian.Low);
            img_dos_h.e_cparhdr = IO.ReadUInt16(Endian.Low);
            img_dos_h.e_minalloc = IO.ReadUInt16(Endian.Low);
            img_dos_h.e_maxalloc = IO.ReadUInt16(Endian.Low);
            img_dos_h.e_ss = IO.ReadUInt16(Endian.Low);
            img_dos_h.e_sp = IO.ReadUInt16(Endian.Low);
            img_dos_h.e_csum = IO.ReadUInt16(Endian.Low);
            img_dos_h.e_ip = IO.ReadUInt16(Endian.Low);
            img_dos_h.e_cs = IO.ReadUInt16(Endian.Low);
            img_dos_h.e_lfarlc = IO.ReadUInt16(Endian.Low);
            img_dos_h.e_ovno = IO.ReadUInt16(Endian.Low);
            img_dos_h.e_res = new List<UInt16>();
            for (int i = 0; i < 4; i++)
            {
                img_dos_h.e_res.Add(IO.ReadUInt16(Endian.Low));
            }

            img_dos_h.e_oemid = IO.ReadUInt16(Endian.Low);
            img_dos_h.e_oeminfo = IO.ReadUInt16(Endian.Low);

            img_dos_h.e_res2 = new List<UInt16>();
            for (int i = 0; i < 10; i++)
            {
                img_dos_h.e_res2.Add(IO.ReadUInt16(Endian.Low));
            }
            img_dos_h.e_lfanew = IO.ReadInt32(Endian.Low);
            img_dos_h.e_rstub = IO.ReadBytes(img_dos_h.e_lfanew - 64);
        }
        public void read_file_header()
        {
            IO.Position = img_dos_h.e_lfanew;
            img_file_h = new ImageFileHeader();
            img_file_h.magic = IO.ReadUInt32(Endian.Low);
            img_file_h.Machine = IO.ReadUInt16(Endian.Low);
            img_file_h.NumberOfSections = IO.ReadUInt16(Endian.Low);
            img_file_h.TimeDateStamp = IO.ReadUInt32(Endian.Low);
            img_file_h.PointerToSymbolTable = IO.ReadUInt32(Endian.Low);
            img_file_h.NumberOfSymbols = IO.ReadUInt32(Endian.Low);
            img_file_h.SizeOfOptionalHeader = IO.ReadUInt16(Endian.Low);
            img_file_h.Characteristics = IO.ReadUInt16(Endian.Low);
        }
        public void read_image_opt_header()
        {
            IO.Position = img_dos_h.e_lfanew + 24;
            ImageOptHeader opt = new ImageOptHeader();
            opt.Magic = IO.ReadUInt16(Endian.Low);
            opt.MajorLinkerVersion = IO.ReadByte();
            opt.MinorLinkerVersion = IO.ReadByte();
            opt.SizeOfCode = IO.ReadUInt32(Endian.Low);
            opt.SizeOfInitializedData = IO.ReadUInt32(Endian.Low);
            opt.SizeOfUninitializedData = IO.ReadUInt32(Endian.Low);
            opt.AddressOfEntryPoint = IO.ReadUInt32(Endian.Low);
            opt.BaseOfCode = IO.ReadUInt32(Endian.Low);
            opt.BaseOfData = IO.ReadUInt32(Endian.Low);
            opt.ImageBase = IO.ReadUInt32(Endian.Low);
            opt.SectionAlignment = IO.ReadUInt32(Endian.Low);
            opt.FileAlignment = IO.ReadUInt32(Endian.Low);
            opt.MajorOperatingSystemVersion = IO.ReadUInt16(Endian.Low);
            opt.MinorOperatingSystemVersion = IO.ReadUInt16(Endian.Low);
            opt.MajorImageVersion = IO.ReadUInt16(Endian.Low);
            opt.MinorImageVersion = IO.ReadUInt16(Endian.Low);
            opt.MajorSubsystemVersion = IO.ReadUInt16(Endian.Low);
            opt.MinorOperatingSystemVersion = IO.ReadUInt16(Endian.Low);
            opt.Reserved1 = IO.ReadUInt32(Endian.Low);
            opt.SizeOfImage = IO.ReadUInt32(Endian.Low);
            opt.CheckSum = IO.ReadUInt32(Endian.Low);
            opt.Subsystem = IO.ReadUInt16(Endian.Low);
            opt.DllCharacteristics = IO.ReadUInt16(Endian.Low);
            opt.SizeOfStackReserve = IO.ReadUInt32(Endian.Low);
            opt.SizeOfStackCommit = IO.ReadUInt32(Endian.Low);
            opt.SizeOfHeapReserve = IO.ReadUInt32(Endian.Low);
            opt.SizeOfHeapCommit = IO.ReadUInt32(Endian.Low);
            opt.LoaderFlags = IO.ReadUInt32(Endian.Low);
            opt.NumberOfRvaAndSizes = IO.ReadUInt32(Endian.Low);
            img_opt_h = opt;
        }
        public void read_image_sections()
        {
            IO.Position = img_dos_h.e_lfanew + 248;
            img_sections = new List<ImageSectionHeader>();

            for (int i = 0; i < img_file_h.NumberOfSections; i++)
            {
                ImageSectionHeader sec = new ImageSectionHeader();
                sec.Name = IO.ReadString(8);
                sec.Misc = IO.ReadUInt32(Endian.Low);
                sec.VirtualAddress = IO.ReadUInt32(Endian.Low);
                sec.SizeOfRawData = IO.ReadUInt32(Endian.Low);
                sec.RawDataPtr = IO.ReadUInt32(Endian.Low);
                sec.RelocationsPtr = IO.ReadUInt32(Endian.Low);
                sec.LineNumsPtr = IO.ReadUInt32(Endian.Low);
                sec.NumRelocations = IO.ReadUInt16(Endian.Low);
                sec.NUmLineNumbers = IO.ReadUInt16(Endian.Low);
                sec.Characteristics = IO.ReadUInt32(Endian.Low);
                img_sections.Add(sec);
            }
        }

        public void extract_pe_section(string output_file, ImageSectionHeader sec)
        {
            IO.Position = sec.RawDataPtr;
            byte[] data = IO.ReadBytes((int)sec.SizeOfRawData);

            System.IO.FileStream outio = new System.IO.FileStream(output_file, System.IO.FileMode.Create);
            outio.Position = 0;
            outio.Write(data, 0, data.Length);
            outio.Flush();
            outio.Close();
            outio = null;
            data = null;
        }
    }
}

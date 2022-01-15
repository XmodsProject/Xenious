using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbox360.IO;
using Xbox360.PE;
using Xenious.IO;

namespace Xbox360
{
    public class PortableExecutable
    {
        public FileIO IO;

        /* PE Stuff */
        public ImageDosHeader img_dos_h;
        public ImageFileHeader img_file_h;
        public ImageOptHeader img_opt_h;
        public List<ImageSectionHeader> img_sections;
        public SectionImportData pe_sec_idata;

        public PortableExecutable(string Input)
        {
            this.IO = new FileIO(Input, System.IO.FileMode.Open);
        }
        public bool load_pe()
        {
            IO.position = 0;

            string magic = IO.read_string(2);

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
            IO.position = 0;
            img_dos_h = new ImageDosHeader();
            img_dos_h.e_magic = IO.read_uint16(Endian.Low);
            img_dos_h.e_cblp = IO.read_uint16(Endian.Low);
            img_dos_h.e_cp = IO.read_uint16(Endian.Low);
            img_dos_h.e_crlc = IO.read_uint16(Endian.Low);
            img_dos_h.e_cparhdr = IO.read_uint16(Endian.Low);
            img_dos_h.e_minalloc = IO.read_uint16(Endian.Low);
            img_dos_h.e_maxalloc = IO.read_uint16(Endian.Low);
            img_dos_h.e_ss = IO.read_uint16(Endian.Low);
            img_dos_h.e_sp = IO.read_uint16(Endian.Low);
            img_dos_h.e_csum = IO.read_uint16(Endian.Low);
            img_dos_h.e_ip = IO.read_uint16(Endian.Low);
            img_dos_h.e_cs = IO.read_uint16(Endian.Low);
            img_dos_h.e_lfarlc = IO.read_uint16(Endian.Low);
            img_dos_h.e_ovno = IO.read_uint16(Endian.Low);
            img_dos_h.e_res = new List<UInt16>();
            for (int i = 0; i < 4; i++)
            {
                img_dos_h.e_res.Add(IO.read_uint16(Endian.Low));
            }

            img_dos_h.e_oemid = IO.read_uint16(Endian.Low);
            img_dos_h.e_oeminfo = IO.read_uint16(Endian.Low);

            img_dos_h.e_res2 = new List<UInt16>();
            for (int i = 0; i < 10; i++)
            {
                img_dos_h.e_res2.Add(IO.read_uint16(Endian.Low));
            }
            img_dos_h.e_lfanew = IO.read_int32(Endian.Low);
            img_dos_h.e_rstub = IO.read_bytes(img_dos_h.e_lfanew - 64);
        }
        public void read_file_header()
        {
            IO.position = img_dos_h.e_lfanew;
            img_file_h = new ImageFileHeader();
            img_file_h.magic = IO.read_uint32(Endian.Low);
            img_file_h.Machine = IO.read_uint16(Endian.Low);
            img_file_h.NumberOfSections = IO.read_uint16(Endian.Low);
            img_file_h.TimeDateStamp = IO.read_uint32(Endian.Low);
            img_file_h.PointerToSymbolTable = IO.read_uint32(Endian.Low);
            img_file_h.NumberOfSymbols = IO.read_uint32(Endian.Low);
            img_file_h.SizeOfOptionalHeader = IO.read_uint16(Endian.Low);
            img_file_h.Characteristics = IO.read_uint16(Endian.Low);
        }
        public void read_image_opt_header()
        {
            IO.position = img_dos_h.e_lfanew + 24;
            ImageOptHeader opt = new ImageOptHeader();
            opt.Magic = IO.read_uint16(Endian.Low);
            opt.MajorLinkerVersion = IO.read_byte();
            opt.MinorLinkerVersion = IO.read_byte();
            opt.SizeOfCode = IO.read_uint32(Endian.Low);
            opt.SizeOfInitializedData = IO.read_uint32(Endian.Low);
            opt.SizeOfUninitializedData = IO.read_uint32(Endian.Low);
            opt.AddressOfEntryPoint = IO.read_uint32(Endian.Low);
            opt.BaseOfCode = IO.read_uint32(Endian.Low);
            opt.BaseOfData = IO.read_uint32(Endian.Low);
            opt.ImageBase = IO.read_uint32(Endian.Low);
            opt.SectionAlignment = IO.read_uint32(Endian.Low);
            opt.FileAlignment = IO.read_uint32(Endian.Low);
            opt.MajorOperatingSystemVersion = IO.read_uint16(Endian.Low);
            opt.MinorOperatingSystemVersion = IO.read_uint16(Endian.Low);
            opt.MajorImageVersion = IO.read_uint16(Endian.Low);
            opt.MinorImageVersion = IO.read_uint16(Endian.Low);
            opt.MajorSubsystemVersion = IO.read_uint16(Endian.Low);
            opt.MinorOperatingSystemVersion = IO.read_uint16(Endian.Low);
            opt.Reserved1 = IO.read_uint32(Endian.Low);
            opt.SizeOfImage = IO.read_uint32(Endian.Low);
            opt.CheckSum = IO.read_uint32(Endian.Low);
            opt.Subsystem = IO.read_uint16(Endian.Low);
            opt.DllCharacteristics = IO.read_uint16(Endian.Low);
            opt.SizeOfStackReserve = IO.read_uint32(Endian.Low);
            opt.SizeOfStackCommit = IO.read_uint32(Endian.Low);
            opt.SizeOfHeapReserve = IO.read_uint32(Endian.Low);
            opt.SizeOfHeapCommit = IO.read_uint32(Endian.Low);
            opt.LoaderFlags = IO.read_uint32(Endian.Low);
            opt.NumberOfRvaAndSizes = IO.read_uint32(Endian.Low);
            img_opt_h = opt;
        }
        public void read_image_sections()
        {
            IO.position = img_dos_h.e_lfanew + 248;
            img_sections = new List<ImageSectionHeader>();

            for (int i = 0; i < img_file_h.NumberOfSections; i++)
            {
                ImageSectionHeader sec = new ImageSectionHeader();
                sec.Name = IO.read_string(8);
                sec.Misc = IO.read_uint32(Endian.Low);
                sec.VirtualAddress = IO.read_uint32(Endian.Low);
                sec.SizeOfRawData = IO.read_uint32(Endian.Low);
                sec.RawDataPtr = IO.read_uint32(Endian.Low);
                sec.RelocationsPtr = IO.read_uint32(Endian.Low);
                sec.LineNumsPtr = IO.read_uint32(Endian.Low);
                sec.NumRelocations = IO.read_uint16(Endian.Low);
                sec.NUmLineNumbers = IO.read_uint16(Endian.Low);
                sec.Characteristics = IO.read_uint32(Endian.Low);
                img_sections.Add(sec);
            }
        }
    }
}

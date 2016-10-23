using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbox360.PE;
using Xenious.IO;

namespace Xbox360
{
    public class PortableExecutable
    {
        public FileIO IO;
        public ImageDosHeader img_dos_h;
        public ImageFileHeader img_file_h;
        public ImageOptHeader img_opt_h;
        public List<ImageSectionHeader> img_sections;
        public SectionImportData pe_sec_idata;
        public int pe_data_offset;

        public PortableExecutable(string File)
        {
            IO = new FileIO(File, System.IO.FileMode.Open);
            IO.position = 0;
            pe_data_offset = 0;
        }

        public PortableExecutable(FileIO in_io, int pe_pos)
        {
            IO = in_io;
            pe_data_offset = pe_pos;
        }

        public void read_dos_header()
        {
            IO.position = pe_data_offset;
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
            IO.position = pe_data_offset + img_dos_h.e_lfanew;
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
            IO.position = pe_data_offset + img_dos_h.e_lfanew + 24;
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
            IO.position = pe_data_offset + img_dos_h.e_lfanew + 248;
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
        public void extract_section(string output_file, ImageSectionHeader sec)
        {
            IO.position = pe_data_offset + sec.RawDataPtr;
            byte[] data = IO.read_bytes((int)sec.SizeOfRawData);

            FileStream outio = new FileStream(output_file, FileMode.Create);
            outio.Position = 0;
            outio.Write(data, 0, data.Length);
            outio.Flush();
            outio.Close();
            outio = null;
            data = null;
        }

        public void save(FileIO in_io)
        {
            // Write out dos header.
            in_io.position = pe_data_offset;
            in_io.write(img_dos_h.e_magic, Endian.Low);
            in_io.write(img_dos_h.e_cblp, Endian.Low);
            in_io.write(img_dos_h.e_cp, Endian.Low);
            in_io.write(img_dos_h.e_crlc, Endian.Low);
            in_io.write(img_dos_h.e_cparhdr, Endian.Low);
            in_io.write(img_dos_h.e_minalloc, Endian.Low);
            in_io.write(img_dos_h.e_maxalloc, Endian.Low);
            in_io.write(img_dos_h.e_ss, Endian.Low);
            in_io.write(img_dos_h.e_sp, Endian.Low);
            in_io.write(img_dos_h.e_csum, Endian.Low);
            in_io.write(img_dos_h.e_ip, Endian.Low);
            in_io.write(img_dos_h.e_cs, Endian.Low);
            in_io.write(img_dos_h.e_lfarlc, Endian.Low);
            in_io.write(img_dos_h.e_ovno, Endian.Low);
            for (int i = 0; i < 4; i++)
            {
                in_io.write(img_dos_h.e_res[i], Endian.Low);
            }

            in_io.write(img_dos_h.e_oemid, Endian.Low);
            in_io.write(img_dos_h.e_oeminfo, Endian.Low);

            for (int i = 0; i < 10; i++)
            {
                in_io.write(img_dos_h.e_res2[i], Endian.Low);
            }
            in_io.write(img_dos_h.e_lfanew, Endian.Low);
            in_io.write(img_dos_h.e_rstub);

            // Write out file header.
            in_io.position = pe_data_offset + img_dos_h.e_lfanew;
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
    }
}

/*
 * Reversed by [ Hect0r ] @ www.staticpi.net
 * With thanks to : http://www.free60.org/wiki/XEX
 * For inital stuff :)
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xenious.IO;
using System.IO;
using Xbox360.PE;
using Xbox360.XEX;
using Xbox360.XUIZ;

namespace Xbox360
{
    public class XenonExecutable
    {
        public FileIO IO;

        public bool is_xex {
            get { return (bool)(magic == "XEX2"); }
        }

        /* Header info */
        public string magic;
        public uint module_flags;
        public uint pe_data_offset;
        public uint reserved;
        public uint certificate_pos;
        public uint opt_header_count;
        public List<XeOptHeader> opt_headers;
        public List<XeOptHeader> unk_headers;
        public XeCertificate cert;
        public List<XeSection> sections;

        /* Opt Headers */
        public List<XeResourceInfo> resources;
        public List<XeStaticLib> static_libs;
        public XeGame_Ratings ratings;
        public XeExecutionInfo xeinfo;
        public XeBaseFileInfoHeader base_file_info_h;
        public XeRawBaseFileInfo raw_file_info_h;
        public XeCompBaseFileInfo comp_file_info_h;
        public List<XeImportLibary> import_libs;
        public XeDeltaPatch delta_patch;
        public XeTLSInfo tls_info;
        public XeExportsByName exports_named;
        public XUIZHeader xuiz_h;
        public string bound_path;
        public string orig_pe_name;
        public UInt32 orig_base_addr;
        public UInt32 exe_entry_point;
        public UInt32 img_base_addr;
        public UInt32 system_flags;
        public UInt32 Unknown_OPT_Data;
        public UInt32 default_stack_size;
        public UInt32 title_workspace_size;
        public UInt32 default_fs_cache_size;
        public UInt32 default_heap_size;
        public UInt64 checksum_timestamp;
        public byte[] callcap_data;
        public byte[] xgd3_media_id;
        public byte[] xbox_360_logo;
        public byte[] lan_key;
        public byte[] device_id;
        public List<byte[]> alternative_title_ids;

        /* PE Stuff */
        public ImageDosHeader img_dos_h;
        public ImageFileHeader img_file_h;
        public ImageOptHeader img_opt_h;
        public List<ImageSectionHeader> img_sections;
        public SectionImportData pe_sec_idata;

        public XenonExecutable(string file)
        {
            IO = new FileIO(file, System.IO.FileMode.Open);
            IO.position = 0;
        }
        public void read_header()
        {
            if (IO.Opened)
            {
                try {
                    magic = IO.read_string(4);

                    if (is_xex)
                    {
                        module_flags = IO.read_uint32(Endian.High);
                        pe_data_offset = IO.read_uint32(Endian.High);
                        reserved = IO.read_uint32(Endian.High);
                        certificate_pos = IO.read_uint32(Endian.High);
                        opt_header_count = IO.read_uint32(Endian.High);

                        XeOptHeader bh;
                        opt_headers = new List<XeOptHeader>();
                        for (int i = 0; i < opt_header_count; i++)
                        {
                            bh = new XeOptHeader();
                            bh.key = (XeHeaderKeys)IO.read_uint32(Endian.High);
                            bh.data = IO.read_uint32(Endian.High);

                            switch ((UInt32)bh.key & 0xFF)
                            {
                                case 0x01:
                                    bh.len = 0;
                                    break;
                                case 0xFF:
                                    bh.pos = bh.data;
                                    break;
                                default:
                                    bh.len = ((UInt32)bh.key & 0xFF) * 4;
                                    bh.pos = bh.data;
                                    break;
                            }

                            opt_headers.Add(bh);
                        }
                    }
                }
                catch(Exception exp)
                {
                    throw exp;
                }
            }
        }
        public void parse_certificate()
        {
            try
            {
                IO.position = certificate_pos;
                cert = new XeCertificate();
                cert.header_size = IO.read_uint32(Endian.High);
                cert.image_size = IO.read_uint32(Endian.High);
                cert.rsa_sig = IO.read_bytes(256);
                cert.UnkLen = IO.read_uint32(Endian.High);
                cert.image_flags = IO.read_uint32(Endian.High);
                cert.load_address = IO.read_uint32(Endian.High);
                cert.section_disgest = IO.read_bytes(20);
                cert.import_table_count = IO.read_uint32(Endian.High);
                cert.import_table_digest = IO.read_bytes(20);
                cert.xgd2_media_id = IO.read_bytes(16);
                cert.seed_key = IO.read_bytes(16);
                cert.export_table_pos = IO.read_uint32(Endian.High);
                cert.header_digest = IO.read_bytes(20);
                cert.game_regions = IO.read_uint32(Endian.High);
                cert.media_flags = IO.read_uint32(Endian.High);
            }
            catch(Exception exp)
            {
                throw exp;
            }
        }
        public void parse_sections()
        {
            try
            {
                IO.position = certificate_pos + 0x180;
                uint num = IO.read_uint32(Endian.High);
                sections = new List<XeSection>();

                for (uint i = 0; i < num; i++)
                {
                    XeSection sec = new XeSection();
                    if (pe_data_offset <= 0x90000000) { sec.page_size = 64 * 1024; }
                    else { sec.page_size = 4 * 1024; }
                    sec.value = IO.read_uint32(Endian.High);
                    sec.digest = IO.read_bytes(20);
                    sections.Add(sec);
                }
            }
            catch(Exception exp)
            {
                throw exp;
            }
        }
        public int parse_optional_headers()
        {
            uint len;
            unk_headers = new List<XeOptHeader>();
            resources = new List<XeResourceInfo>();

            for (int i = 0; i < opt_header_count; i++)
            {
                switch ((uint)opt_headers[i].key)
                {
                    case (uint)XeHeaderKeys.RESOURCE_INFO:
                        #region Resources
                        try
                        {
                            IO.position = (opt_headers[i].pos);
                            uint num = IO.read_uint32(Endian.High);
                            num = (num - 4) / 16;

                            XeResourceInfo res;
                            for (int x = 0; x < num; x++)
                            {
                                res = new XeResourceInfo();
                                res.name = IO.read_string(8);
                                res.address = IO.read_uint32(Endian.High);
                                res.size = IO.read_uint32(Endian.High);
                                resources.Add(res);
                            }
                        }
                        catch
                        {
                            return -1;
                        }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.FILE_FORMAT_INFO:
                        #region BaseFileFormat
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            base_file_info_h = new XeBaseFileInfoHeader();
                            base_file_info_h.info_size = IO.read_int32(Endian.High);
                            base_file_info_h.enc_type = (XeEncryptionType)IO.read_uint16(Endian.High);
                            base_file_info_h.comp_type = (XeCompressionType)IO.read_uint16(Endian.High);

                            #region XeCompression
                            switch (base_file_info_h.comp_type)
                            {
                                case XeCompressionType.Raw:
                                    int num3 = (base_file_info_h.info_size - 8) / 8;
                                    raw_file_info_h = new XeRawBaseFileInfo();
                                    raw_file_info_h.info_size = IO.read_int32(Endian.High);
                                    raw_file_info_h.enc_type = (XeEncryptionType)IO.read_uint16(Endian.High);
                                    raw_file_info_h.comp_type = (XeCompressionType)IO.read_uint16(Endian.High);
                                    raw_file_info_h.block = new List<XeRawBaseFileBlock>();

                                    for (int x = 0; x < num3; x++)
                                    {
                                        XeRawBaseFileBlock b = new XeRawBaseFileBlock();
                                        b.data_size = IO.read_int32(Endian.High);
                                        b.zero_size = IO.read_int32(Endian.High);
                                        raw_file_info_h.block.Add(b);
                                    }

                                    break;
                                case XeCompressionType.Compressed:

                                    break;
                                case XeCompressionType.DeltaCompressed:
                                    break;
                            }
                            #endregion
                        }
                        catch
                        {
                            return -2;
                        }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.DELTA_PATCH_DESCRIPTOR:
                        #region DeltaPatchDescriptor
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            delta_patch = new XeDeltaPatch();
                            delta_patch.size = IO.read_uint32(Endian.High);
                            delta_patch.target_version = IO.read_uint32(Endian.High);
                            delta_patch.source_version = IO.read_uint32(Endian.High);
                            delta_patch.hash_source = IO.read_bytes(20);
                            delta_patch.key_source = IO.read_bytes(16);
                            delta_patch.target_headers_size = IO.read_uint32(Endian.High);
                            delta_patch.headers_source_offset = IO.read_uint32(Endian.High);
                            delta_patch.headers_source_size = IO.read_uint32(Endian.High);
                            delta_patch.headers_target_offset = IO.read_uint32(Endian.High);
                            delta_patch.image_source_offset = IO.read_uint32(Endian.High);
                            delta_patch.image_source_size = IO.read_uint32(Endian.High);
                            delta_patch.image_target_offset = IO.read_uint32(Endian.High);

                            delta_patch.header_data = IO.read_bytes((int)(delta_patch.size - 76));
                        }
                        catch
                        {
                            return -3;
                        }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.XGD3_MEDIA_KEY:
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            xgd3_media_id = IO.read_bytes((int)opt_headers[i].len);
                        }
                        catch { return -4; }
                        break;
                    case (uint)XeHeaderKeys.BOUNDING_PATH:
                        try
                        {
                            IO.position = (opt_headers[i].pos);
                            len = IO.read_uint32(Endian.High);
                            bound_path = IO.read_string((int)len);
                        }
                        catch { return -5; }
                        break;
                    case (uint)XeHeaderKeys.DEVICE_ID:
                        #region DeviceID
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            device_id = IO.read_bytes((int)opt_headers[i].len);
                        }
                        catch { return -6; }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.ORIGINAL_BASE_ADDRESS:
                        orig_base_addr = opt_headers[i].data;
                        break;
                    case (uint)XeHeaderKeys.ENTRY_POINT:
                        exe_entry_point = opt_headers[i].data;
                        break;
                    case (uint)XeHeaderKeys.IMAGE_BASE_ADDRESS:
                        img_base_addr = opt_headers[i].data;
                        break;
                    case (uint)XeHeaderKeys.IMPORT_LIBRARIES:
                        #region ImportLibs
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            uint ils = IO.read_uint32(Endian.High);
                            uint libs_len = IO.read_uint32(Endian.High);
                            uint num_libs = IO.read_uint32(Endian.High);
                            import_libs = new List<XeImportLibary>();
                            if (num_libs == 1)
                            {
                                string buf = IO.read_string((int)libs_len);
                                XeImportLibary impl = new XeImportLibary();
                                impl.name = buf;
                                impl.read(IO);
                                import_libs.Add(impl);
                            }
                            else
                            {
                                string[] buf = IO.read_string((int)libs_len).Split('\0');
                                string[] kernals = new string[num_libs];
                                int g = 0;
                                // Get Import libary names.
                                for (int x = 0; x < buf.Length; x++)
                                {
                                    if (buf[x] != "")
                                    {
                                        kernals[g] = buf[x];
                                        g++;
                                    }
                                }

                                // Now get the import libs.
                                for (int x = 0; x < num_libs; x++)
                                {
                                    XeImportLibary impl = new XeImportLibary();
                                    impl.name = kernals[x];
                                    impl.read(IO);
                                    import_libs.Add(impl);
                                }
                            }
                        }
                        catch { return -7; }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.CHECKSUM_TIMESTAMP:
                        #region ChecksumTimestamp
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            checksum_timestamp = IO.read_uint64(Endian.High);
                        }
                        catch { return -8; }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.ENABLED_FOR_CALLCAP:
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            callcap_data = IO.read_bytes((int)opt_headers[i].len);
                        }
                        catch { return -9; }
                        break;
                    case (uint)XeHeaderKeys.ORIGINAL_PE_NAME:
                        #region OriginalPEName
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            len = IO.read_uint32(Endian.High);
                            orig_pe_name = IO.read_string((int)len);
                        }
                        catch { return -10; }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.STATIC_LIBRARIES:
                        #region StaticLibs
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            len = ((IO.read_uint32(Endian.High) - 4) / 16);
                            static_libs = new List<XeStaticLib>();

                            for (int x = 0; x < len; x++)
                            {
                                XeStaticLib sl = new XeStaticLib();
                                sl.name = IO.read_string(8);
                                sl.major = IO.read_uint16(Endian.High);
                                sl.minor = IO.read_uint16(Endian.High);
                                sl.build = IO.read_uint16(Endian.High);
                                sl.qfe = IO.read_uint16(Endian.High);
                                sl.approval = (XeApprovalType)(sl.qfe & 0x8000);
                                static_libs.Add(sl);
                            }
                        }
                        catch { return -11; }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.TLS_INFO:
                        #region TLSInfo
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            tls_info = new XeTLSInfo();
                            tls_info.slot_count = IO.read_uint32(Endian.High);
                            tls_info.raw_data_addr = IO.read_uint32(Endian.High);
                            tls_info.data_size = IO.read_uint32(Endian.High);
                            tls_info.raw_data_size = IO.read_uint32(Endian.High);
                        }
                        catch { return -12; }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.DEFAULT_STACK_SIZE:
                        default_stack_size = opt_headers[i].data;
                        break;
                    case (uint)XeHeaderKeys.DEFAULT_FILESYSTEM_CACHE_SIZE:
                        default_fs_cache_size = opt_headers[i].data;
                        break;
                    case (uint)XeHeaderKeys.DEFAULT_HEAP_SIZE:
                        default_heap_size = opt_headers[i].data;
                        break;
                    case (uint)XeHeaderKeys.SYSTEM_FLAGS:
                        #region SystemFlags
                        system_flags = opt_headers[i].data;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.UNKNOWN1:
                        Unknown_OPT_Data = opt_headers[i].data;
                        break; // Found in dash.xex...
                    case (uint)XeHeaderKeys.EXECUTION_INFO:

                        #region Execution Info
                        try
                        {
                            
                            IO.position = opt_headers[i].pos;
                            xeinfo = new XeExecutionInfo();
                            xeinfo.media_id = IO.read_uint32(Endian.High);
                            xeinfo.version = IO.read_uint32(Endian.High);
                            xeinfo.base_version = IO.read_uint32(Endian.High);
                            xeinfo.title_id = IO.read_uint32(Endian.High);
                            xeinfo.platform = IO.read_byte();
                            xeinfo.executable_table = IO.read_byte();
                            xeinfo.disc_number = IO.read_byte();
                            xeinfo.disc_count = IO.read_byte();
                            xeinfo.savegame_id = IO.read_uint32(Endian.High);
                        }
                        catch { return -13; }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.TITLE_WORKSPACE_SIZE:
                        title_workspace_size = opt_headers[i].data;
                        break;
                    case (uint)XeHeaderKeys.GAME_RATINGS:
                        #region GameRatings
                        try
                        {
                            ratings = new XeGame_Ratings();
                            IO.position = opt_headers[i].pos;
                            ratings.esrb = (XeRating_esrb)IO.read_byte();
                            ratings.pegi = (XeRating_pegi)IO.read_byte();
                            ratings.pegifi = (XeRating_pegi_fi)IO.read_byte();
                            ratings.pegipt = (XeRating_pegi_pt)IO.read_byte();
                            ratings.bbfc = (XeRating_bbfc)IO.read_byte();
                            ratings.cero = (XeRating_cero)IO.read_byte();
                            ratings.usk = (XeRating_usk)IO.read_byte();
                            ratings.oflcau = (XeRating_oflc_au)IO.read_byte();
                            ratings.oflcnz = (XeRating_oflc_nz)IO.read_byte();
                            ratings.kmrb = (XeRating_kmrb)IO.read_byte();
                            ratings.brazil = (XeRating_brazil)IO.read_byte();
                            ratings.fpb = (XeRating_fpb)IO.read_byte();
                            ratings.reserved = IO.read_bytes(52);
                        }
                        catch { return -14; }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.LAN_KEY:
                        #region LanKey
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            lan_key = IO.read_bytes((int)opt_headers[i].len);
                        }
                        catch { return -15; }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.XBOX360_LOGO:
                        #region Xbox360Logo
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            uint x360l_len = IO.read_uint32(Endian.High);
                            xbox_360_logo = IO.read_bytes((int)x360l_len);
                        }
                        catch { return -16; }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.ALTERNATE_TITLE_IDS:
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            uint len2 = IO.read_uint32(Endian.High);
                            alternative_title_ids = new List<byte[]>();
                            for (int x = 0; x < (int)(len2 - 4) / 4; x++)
                            {
                                alternative_title_ids.Add(IO.read_bytes(4));
                            }
                        }
                        catch { return -17; }
                        break;
                    case (uint)XeHeaderKeys.EXPORTS_BY_NAME:
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            exports_named = new XeExportsByName();
                            exports_named.raw_size = IO.read_uint32(Endian.High);
                            exports_named.num_exports = IO.read_uint32(Endian.High);
                        }
                        catch { return -18; }
                        break;

                    case (uint)XeHeaderKeys.BASE_REFERENCE:
                    case (uint)XeHeaderKeys.ENABLED_FOR_FASTCAP:
                    case (uint)XeHeaderKeys.ADDITIONAL_TITLE_MEMORY:
                    case (uint)XeHeaderKeys.PAGE_HEAP_SIZE_AND_FLAGS:
                    case (uint)XeHeaderKeys.MULTIDISC_MEDIA_IDS:
                    default:
                        unk_headers.Add(opt_headers[i]);
                        break;
                }
            }
            return 0;
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

        public void read_xuiz_header()
        {
            IO.position = pe_data_offset;

            xuiz_h = new XUIZHeader();
            xuiz_h.magic = IO.read_string(4);
            xuiz_h.unknown = IO.read_uint32(Endian.High);
            xuiz_h.unknown2 = IO.read_uint32(Endian.High);
            xuiz_h.unknown3 = IO.read_uint32(Endian.High);
            xuiz_h.unknown4 = IO.read_uint32(Endian.High);
            xuiz_h.unknown5 = IO.read_uint16(Endian.High);
            xuiz_h.name_len = IO.read_uint32(Endian.High);
            xuiz_h.name = IO.read_string((int)xuiz_h.name_len);
        }

        public void save_xex()
        {
            byte[] hash_blank = new byte[20] {
                0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00
            };
            // Save Module Flags.
            IO.position = 4;
            IO.write(module_flags, Endian.High);

            // Save Certificate.
            IO.position = certificate_pos;
            IO.write(cert.header_size, Endian.High);
            IO.write(cert.image_size, Endian.High);
            IO.write(cert.rsa_sig);
            IO.write(cert.UnkLen, Endian.High);
            IO.write(cert.image_flags, Endian.High);
            IO.write(cert.load_address, Endian.High);
            IO.write(hash_blank); // Will hash later.
            IO.write(cert.import_table_count, Endian.High);
            IO.write(cert.import_table_digest);
            IO.write(cert.xgd2_media_id);
            IO.write(cert.seed_key);
            IO.write(cert.export_table_pos, Endian.High);
            IO.write(hash_blank); // Will hash later.
            IO.write(cert.game_regions, Endian.High);
            IO.write(cert.media_flags, Endian.High);

            // Writeout Optional Headers.
            int num = 0;
            foreach(XeOptHeader opt in opt_headers)
            {
                switch(opt.key)
                {
                    case XeHeaderKeys.ORIGINAL_PE_NAME:
                        IO.position = opt.pos;
                        IO.write((UInt32)orig_pe_name.Length, Endian.High);
                        IO.write(Encoding.ASCII.GetBytes(orig_pe_name));
                        break;
                    case XeHeaderKeys.BOUNDING_PATH:
                        IO.position = opt.pos;
                        IO.write((UInt32)bound_path.Length, Endian.High);
                        IO.write(Encoding.ASCII.GetBytes(bound_path));
                        break;
                    case XeHeaderKeys.XGD3_MEDIA_KEY:
                        IO.position = opt.pos;
                        IO.write(xgd3_media_id);
                        break;
                    case XeHeaderKeys.DEVICE_ID:
                        IO.position = opt.pos;
                        IO.write(xgd3_media_id);
                        break;
                    case XeHeaderKeys.ORIGINAL_BASE_ADDRESS:
                        IO.position = 24 + ((8 * (num + 1))  - 4);
                        IO.write(orig_base_addr, Endian.High);
                        break;
                    case XeHeaderKeys.ENTRY_POINT:
                        IO.position = 24 + ((8 * (num + 1))  - 4);
                        IO.write(exe_entry_point, Endian.High);
                        break;
                    case XeHeaderKeys.IMAGE_BASE_ADDRESS:
                        IO.position = 24 + ((8 * (num + 1))  - 4);
                        IO.write(img_base_addr, Endian.High);
                        break;
                    case XeHeaderKeys.CHECKSUM_TIMESTAMP:
                        IO.position = opt.pos;
                        IO.write(checksum_timestamp, Endian.High);
                        break;
                    case XeHeaderKeys.DEFAULT_STACK_SIZE:
                        IO.position = 24 + ((8 * (num + 1))  - 4);
                        IO.write(default_stack_size, Endian.High);
                        break;
                    case XeHeaderKeys.DEFAULT_FILESYSTEM_CACHE_SIZE:
                        IO.position = 24 + ((8 * (num + 1))  - 4);
                        IO.write(default_fs_cache_size, Endian.High);
                        break;
                    case XeHeaderKeys.DEFAULT_HEAP_SIZE:
                        IO.position = 24 + ((8 * (num + 1))  - 4);
                        IO.write(default_heap_size, Endian.High);
                        break;
                    case XeHeaderKeys.SYSTEM_FLAGS:
                        IO.position = 24 + ((8 * (num + 1))  - 4);
                        IO.write(system_flags, Endian.High);
                        break;
                    case XeHeaderKeys.UNKNOWN1:
                        IO.position = 24 + ((8 * (num + 1))  - 4);
                        IO.write(Unknown_OPT_Data, Endian.High);
                        break;
                    case XeHeaderKeys.EXECUTION_INFO:
                        IO.position = opt.pos;
                        IO.write(xeinfo.media_id, Endian.High);
                        IO.write(xeinfo.version, Endian.High);
                        IO.write(xeinfo.base_version, Endian.High);
                        IO.write(xeinfo.title_id, Endian.High);
                        IO.write(xeinfo.platform);
                        IO.write(xeinfo.executable_table);
                        IO.write(xeinfo.disc_number);
                        IO.write(xeinfo.disc_count);
                        IO.write(xeinfo.savegame_id, Endian.High);
                        break;
                    case XeHeaderKeys.TITLE_WORKSPACE_SIZE:
                        IO.position = 24 + ((8 * (num + 1))  - 4);
                        IO.write(title_workspace_size, Endian.High);
                        break;
                    case XeHeaderKeys.GAME_RATINGS:
                        IO.position = opt.pos;
                        IO.write((byte)ratings.esrb);
                        IO.write((byte)ratings.pegi);
                        IO.write((byte)ratings.pegifi);
                        IO.write((byte)ratings.pegipt);
                        IO.write((byte)ratings.bbfc);
                        IO.write((byte)ratings.cero);
                        IO.write((byte)ratings.usk);
                        IO.write((byte)ratings.oflcau);
                        IO.write((byte)ratings.oflcnz);
                        IO.write((byte)ratings.kmrb);
                        IO.write((byte)ratings.brazil);
                        IO.write((byte)ratings.fpb);
                        IO.write(ratings.reserved);
                        break;
                    case XeHeaderKeys.LAN_KEY:
                        IO.position = opt.pos;
                        IO.write(xgd3_media_id);
                        break;
                    case XeHeaderKeys.ALTERNATE_TITLE_IDS:
                        IO.position = opt.pos + 4;

                        foreach(byte[] id in alternative_title_ids)
                        {
                            IO.write(id);
                        }
                        break;
                }
                num++;
            }
        }
        public void rebuild_xex(string output, bool rebuild_pe)
        {
            FileIO outio = new FileIO(output, System.IO.FileMode.Create);
            byte[] hash_blank = new byte[20] {
                0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00
            };

            // Write out Header.
            outio.position = 0;
            outio.write(Encoding.ASCII.GetBytes("XEX2")); // XEX2
            outio.write(module_flags, Endian.High);
            outio.write((UInt32)0, Endian.High); // Leave blank for now.
            outio.write(reserved, Endian.High);
            outio.write((UInt32)(24 + (8 * opt_headers.Count)), Endian.High);
            outio.write((UInt32)opt_headers.Count, Endian.High);


            // Write out the certificate.
            #region Certificate
            outio.position = 24 + (8 * opt_headers.Count);
            outio.write(cert.header_size, Endian.High);
            outio.write(cert.image_size, Endian.High);
            outio.write(cert.rsa_sig);
            outio.write(cert.UnkLen, Endian.High);
            outio.write(cert.image_flags, Endian.High);
            outio.write(cert.load_address, Endian.High);
            outio.write(hash_blank); // Will hash later.
            outio.write(cert.import_table_count, Endian.High);
            outio.write(cert.import_table_digest);
            outio.write(cert.xgd2_media_id);
            outio.write(cert.seed_key);
            outio.write(cert.export_table_pos, Endian.High);
            outio.write(hash_blank); // Will hash later.
            outio.write(cert.game_regions, Endian.High);
            outio.write(cert.media_flags, Endian.High);
            #endregion

            // Write out Section Table.
            outio.position = (24 + (8 * opt_headers.Count)) + 0x180;
            outio.write((UInt32)sections.Count, Endian.High);

            for (int i = 0; i < sections.Count; i++)
            {
                outio.write(sections[i].value, Endian.High);
                outio.write(sections[i].digest);
            }

            // Output Headers.
            #region Out opt_headers
            // Write out opt headers.
            for (int i = 0; i < opt_headers.Count; i++)
            {
                switch ((uint)opt_headers[i].key)
                {
                    case (uint)XeHeaderKeys.RESOURCE_INFO:
                        #region Writeout Resources
                        // Set opt headers offset.
                        opt_headers[i].data = (uint)outio.position;

                        // Write out size + 4.
                        outio.write((UInt32)(16 * resources.Count) + 4, Endian.High);
                        
                        for(int x = 0; x < resources.Count; x++) 
                        {
                            string name = resources[x].name;
                            // Filepadding.
                            for (int p = name.Length; p < 8; p++) { name += "\0"; }
                            outio.write(Encoding.ASCII.GetBytes(name));
                            outio.write(resources[x].address, Endian.High);
                            outio.write(resources[x].size, Endian.High);
                        }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.FILE_FORMAT_INFO:
                        #region Writeout FileFormat
                        // Set opt headers offset.
                        opt_headers[i].data = (uint)outio.position;

                        // Write out BaseFileInfo.
                        outio.write(base_file_info_h.info_size, Endian.High);
                        outio.write((UInt16)base_file_info_h.enc_type, Endian.High);
                        outio.write((UInt16)base_file_info_h.comp_type, Endian.High);

                        // Now output compression header.
                        #region XeCompression
                        switch (base_file_info_h.comp_type)
                        {
                            case XeCompressionType.Raw:
                                // Write out RawBaseFileInfo.
                                outio.write(base_file_info_h.info_size, Endian.High);
                                outio.write(raw_file_info_h.info_size,Endian.High);
                                outio.write((UInt16)raw_file_info_h.enc_type, Endian.High);
                                outio.write((UInt16)raw_file_info_h.comp_type, Endian.High);

                                foreach (XeRawBaseFileBlock block in raw_file_info_h.block)
                                {
                                    outio.write(block.data_size, Endian.High);
                                    outio.write(block.zero_size, Endian.High);
                                }
                                
                                break;
                            case XeCompressionType.Compressed:
                                
                                break;
                            case XeCompressionType.DeltaCompressed:
                                break;
                        }
                        #endregion
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.DELTA_PATCH_DESCRIPTOR: break;
                    case (uint)XeHeaderKeys.BASE_REFERENCE: break;
                    case (uint)XeHeaderKeys.XGD3_MEDIA_KEY:
                        #region Writeout XGD3MediaID
                        // Set opt headers offset.
                        opt_headers[i].data = (uint)outio.position;

                        // Write out XGD3 Media ID.
                        outio.write(xgd3_media_id);
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.BOUNDING_PATH:
                        #region Writeout BoundingPath
                        // Set opt headers offset.
                        opt_headers[i].data = (uint)outio.position;

                        // Write out length.
                        outio.write((UInt32)bound_path.Length, Endian.High);

                        // Write out bounding path.
                        outio.write(Encoding.ASCII.GetBytes(bound_path));
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.DEVICE_ID:
                        #region Writeout DeviceID
                        // Set opt headers offset.
                        opt_headers[i].data = (uint)outio.position;

                        // Write out DeviceID (No length, header has length.
                        outio.write(device_id);
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.ORIGINAL_BASE_ADDRESS:
                        #region Writeout OriginalBaseAddress
                        // Set headers data.
                        opt_headers[i].data = orig_base_addr;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.ENTRY_POINT:
                        #region Writeout EntryPoint
                        // Set headers data.
                        opt_headers[i].data = exe_entry_point;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.IMAGE_BASE_ADDRESS:
                        #region Writeout ImageBaseAddress
                        // Set headers data.
                        opt_headers[i].data = img_base_addr;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.IMPORT_LIBRARIES:
                        #region Writeout ImportLibarys
                        long pos = outio.position;
                        opt_headers[i].data = (uint)pos;
                        string kernals = "";

                        foreach (XeImportLibary lib in import_libs)
                        {
                            kernals += (lib.name + "\0");
                        }
                        
                        outio.write((UInt32)0, Endian.High); // Write this later. (Header Size)
                        outio.write((UInt32)kernals.Length, Endian.High);
                        outio.write((UInt32)import_libs.Count, Endian.High);
                        outio.write(Encoding.ASCII.GetBytes(kernals));

                        UInt32 size = (UInt32)(kernals.Length + 12);
                        for(int x = 0; x < import_libs.Count; x++)
                        {
                            import_libs[x].write(outio);
                            size += (UInt32)(import_libs[x].data.Length + 4);
                        }

                        outio.position = pos;
                        outio.write(size, Endian.High); // Write size to start.
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.CHECKSUM_TIMESTAMP:
                        #region Writeout ChecksumTimestamp
                        // Set opt headers offset.
                        opt_headers[i].data = (uint)outio.position;

                        // Write out ChecksumTimestamp (No length, header has length.
                        outio.write(checksum_timestamp, Endian.High);
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.ENABLED_FOR_CALLCAP:
                        #region Writeout Callcap
                        // Set opt headers offset.
                        opt_headers[i].data = (uint)outio.position;

                        // Write out Callcap
                        outio.write(callcap_data);
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.ENABLED_FOR_FASTCAP: break;
                    case (uint)XeHeaderKeys.ORIGINAL_PE_NAME:
                        #region Writeout OriginalPEName
                        // Set opt headers offset.
                        opt_headers[i].data = (uint)outio.position;

                        // Write out length
                        outio.write((UInt32)orig_pe_name.Length, Endian.High);

                        // Writeout PE Original Name
                        outio.write(Encoding.ASCII.GetBytes(orig_pe_name));
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.STATIC_LIBRARIES:
                        opt_headers[i].data = (UInt32)outio.position;

                        // Write out size.
                        outio.write((UInt32)4 + (16 * static_libs.Count), Endian.High);

                        foreach(XeStaticLib lib in static_libs)
                        {
                            if(lib.name.Length < 8)
                            {
                                for (int x = lib.name.Length; x < 8; x++)
                                {
                                    lib.name += "\0";
                                }
                            }
                            else if(lib.name.Length > 8)
                            {
                                lib.name = lib.name.Substring(0, 8);
                            }
                            outio.write(Encoding.ASCII.GetBytes(lib.name));
                            outio.write(lib.major, Endian.High);
                            outio.write(lib.minor, Endian.High);
                            outio.write(lib.build, Endian.High);
                            outio.write(lib.qfe, Endian.High);
                        }
                        break;
                    case (uint)XeHeaderKeys.TLS_INFO:
                        #region Writeout TLSInfo
                        // Set headers position.
                        opt_headers[i].data = (UInt32)outio.position;
                       
                        // Writeout TLS Info.
                        outio.write(tls_info.slot_count, Endian.High);
                        outio.write(tls_info.raw_data_addr, Endian.High);
                        outio.write(tls_info.data_size, Endian.High);
                        outio.write(tls_info.raw_data_size, Endian.High);
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.DEFAULT_STACK_SIZE:
                        opt_headers[i].data = default_stack_size;
                        break;
                    case (uint)XeHeaderKeys.DEFAULT_FILESYSTEM_CACHE_SIZE: break;
                    case (uint)XeHeaderKeys.DEFAULT_HEAP_SIZE: break;
                    case (uint)XeHeaderKeys.PAGE_HEAP_SIZE_AND_FLAGS: break;
                    case (uint)XeHeaderKeys.SYSTEM_FLAGS:
                        #region Writeout SystemFlags
                        opt_headers[i].data = system_flags;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.UNKNOWN1:
                        #region Writeout Unknown1
                        opt_headers[i].data = Unknown_OPT_Data;
                        #endregion
                        break; // Found in dash.xex...
                    case (uint)XeHeaderKeys.EXECUTION_INFO:
                        #region Writeout ExecutionInfo
                        // Set headers position.
                        opt_headers[i].data = (uint)outio.position;

                        // Writeout Execution Info
                        outio.write(xeinfo.media_id, Endian.High);
                        outio.write(xeinfo.version, Endian.High);
                        outio.write(xeinfo.base_version, Endian.High);
                        outio.write(xeinfo.title_id, Endian.High);
                        outio.write(xeinfo.platform);
                        outio.write(xeinfo.executable_table);
                        outio.write(xeinfo.disc_number);
                        outio.write(xeinfo.disc_count);
                        outio.write(xeinfo.savegame_id, Endian.High);
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.TITLE_WORKSPACE_SIZE:
                        opt_headers[i].data = title_workspace_size;
                        break;
                    case (uint)XeHeaderKeys.GAME_RATINGS:
                        #region Writeout RatingData
                        // Set headers position.
                        opt_headers[i].data = (uint)outio.position;

                        // Writeout Rating Data.
                        outio.write((byte)ratings.esrb);
                        outio.write((byte)ratings.pegi);
                        outio.write((byte)ratings.pegifi);
                        outio.write((byte)ratings.pegipt);
                        outio.write((byte)ratings.bbfc);
                        outio.write((byte)ratings.cero);
                        outio.write((byte)ratings.usk);
                        outio.write((byte)ratings.oflcau);
                        outio.write((byte)ratings.oflcnz);
                        outio.write((byte)ratings.kmrb);
                        outio.write((byte)ratings.brazil);
                        outio.write((byte)ratings.fpb);
                        outio.write(ratings.reserved);
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.LAN_KEY:
                        #region Writeout LanKey
                        // Set headers position.
                        opt_headers[i].data = (UInt32)outio.position;

                        // Writeout Lan Key.
                        outio.write(lan_key);
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.XBOX360_LOGO:
                        #region Writeout Xbox360Logo
                        // Set headers position.
                        opt_headers[i].data = (UInt32)outio.position;

                        // Writeout Length.
                        outio.write((UInt32)xbox_360_logo.Length, Endian.High);

                        // Writeout logo.
                        outio.write(xbox_360_logo);
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.MULTIDISC_MEDIA_IDS: break;
                    case (uint)XeHeaderKeys.ALTERNATE_TITLE_IDS: break;
                    case (uint)XeHeaderKeys.ADDITIONAL_TITLE_MEMORY: break;
                    case (uint)XeHeaderKeys.EXPORTS_BY_NAME:
                        opt_headers[i].data = (UInt32)outio.position;
                        // Header contains the length.
                        outio.write(exports_named.raw_size, Endian.High);
                        outio.write(exports_named.num_exports, Endian.High);
                        break;
                }
            }
            #endregion

            // Output PE.
            UInt32 pe_offset = (UInt32)outio.position;

            if (rebuild_pe == true)
            {

                // Writeout the ImageDosHeader.
                outio.position = pe_offset;

                #region ImageDosHeader
                outio.write(img_dos_h.e_magic, Endian.Low);
                outio.write(img_dos_h.e_cblp, Endian.Low);
                outio.write(img_dos_h.e_cp, Endian.Low);
                outio.write(img_dos_h.e_crlc, Endian.Low);
                outio.write(img_dos_h.e_cparhdr, Endian.Low);
                outio.write(img_dos_h.e_minalloc, Endian.Low);
                outio.write(img_dos_h.e_maxalloc, Endian.Low);
                outio.write(img_dos_h.e_ss, Endian.Low);
                outio.write(img_dos_h.e_sp, Endian.Low);
                outio.write(img_dos_h.e_csum, Endian.Low);
                outio.write(img_dos_h.e_ip, Endian.Low);
                outio.write(img_dos_h.e_cs, Endian.Low);
                outio.write(img_dos_h.e_lfarlc, Endian.Low);
                outio.write(img_dos_h.e_ovno, Endian.Low);

                for (int i = 0; i < 4; i++)
                {
                    outio.write(img_dos_h.e_res[i], Endian.Low);
                }

                outio.write(img_dos_h.e_oemid, Endian.Low);
                outio.write(img_dos_h.e_oeminfo, Endian.Low);

                for (int i = 0; i < 10; i++)
                {
                    outio.write(img_dos_h.e_res2[i], Endian.Low);
                }
                outio.write(img_dos_h.e_lfanew, Endian.Low);
                outio.write(img_dos_h.e_rstub);
                #endregion
                #region ImageFileHeader
                outio.write(img_file_h.magic, Endian.Low);
                outio.write(img_file_h.Machine, Endian.Low);
                outio.write(img_file_h.NumberOfSections, Endian.Low);
                outio.write(img_file_h.TimeDateStamp, Endian.Low);
                outio.write(img_file_h.PointerToSymbolTable, Endian.Low);
                outio.write(img_file_h.NumberOfSymbols, Endian.Low);
                outio.write(img_file_h.SizeOfOptionalHeader, Endian.Low);
                outio.write(img_file_h.Characteristics, Endian.Low);
                #endregion
                #region ImageOptionalHeader
                outio.write(img_opt_h.Magic, Endian.Low);
                outio.write(img_opt_h.MajorLinkerVersion);
                outio.write(img_opt_h.MinorLinkerVersion);
                outio.write(img_opt_h.SizeOfCode, Endian.Low);
                outio.write(img_opt_h.SizeOfInitializedData, Endian.Low);
                outio.write(img_opt_h.SizeOfUninitializedData, Endian.Low);
                outio.write(img_opt_h.AddressOfEntryPoint, Endian.Low);
                outio.write(img_opt_h.BaseOfCode, Endian.Low);
                outio.write(img_opt_h.BaseOfData, Endian.Low);
                outio.write(img_opt_h.ImageBase, Endian.Low);
                outio.write(img_opt_h.SectionAlignment, Endian.Low);
                outio.write(img_opt_h.FileAlignment, Endian.Low);
                outio.write(img_opt_h.MajorOperatingSystemVersion, Endian.Low);
                outio.write(img_opt_h.MinorOperatingSystemVersion, Endian.Low);
                outio.write(img_opt_h.MajorImageVersion, Endian.Low);
                outio.write(img_opt_h.MinorImageVersion, Endian.Low);
                outio.write(img_opt_h.MajorSubsystemVersion, Endian.Low);
                outio.write(img_opt_h.MinorOperatingSystemVersion, Endian.Low);
                outio.write(img_opt_h.Reserved1, Endian.Low);
                outio.write(img_opt_h.SizeOfImage, Endian.Low);
                outio.write(img_opt_h.CheckSum, Endian.Low);
                outio.write(img_opt_h.Subsystem, Endian.Low);
                outio.write(img_opt_h.DllCharacteristics, Endian.Low);
                outio.write(img_opt_h.SizeOfStackReserve, Endian.Low);
                outio.write(img_opt_h.SizeOfStackCommit, Endian.Low);
                outio.write(img_opt_h.SizeOfHeapReserve, Endian.Low);
                outio.write(img_opt_h.SizeOfHeapCommit, Endian.Low);
                outio.write(img_opt_h.LoaderFlags, Endian.Low);
                outio.write(img_opt_h.NumberOfRvaAndSizes, Endian.Low);
                #endregion
                #region ImageSections
                outio.position = pe_offset + img_dos_h.e_lfanew + 248;

                for (int i = 0; i < img_file_h.NumberOfSections; i++)
                {
                    for(int x = img_sections[i].Name.Length; x < 8; x++)
                    {
                        img_sections[i].Name += "\0";
                    }
                    outio.write(Encoding.ASCII.GetBytes(img_sections[i].Name));
                    outio.write(img_sections[i].Misc, Endian.Low);
                    outio.write(img_sections[i].VirtualAddress, Endian.Low);
                    outio.write(img_sections[i].SizeOfRawData, Endian.Low);
                    outio.write(img_sections[i].RawDataPtr, Endian.Low);
                    outio.write(img_sections[i].RelocationsPtr, Endian.Low);
                    outio.write(img_sections[i].LineNumsPtr, Endian.Low);
                    outio.write(img_sections[i].NumRelocations, Endian.Low);
                    outio.write(img_sections[i].NUmLineNumbers, Endian.Low);
                    outio.write(img_sections[i].Characteristics, Endian.Low);

                    long pos = outio.position;

                    // read -> write section data.
                    IO.position = pe_data_offset + img_sections[i].RawDataPtr;
                    byte[] data = IO.read_bytes((int)img_sections[i].SizeOfRawData);

                    outio.position = pe_offset + img_sections[i].RawDataPtr;
                    outio.write(data);

                    // Go back to writing sections table.
                    outio.position = pos;
                }
                #endregion
            }
            else // Else, just copy the original.
            {
                outio.position = pe_offset;
                IO.position = pe_data_offset;
                int num = 0;

                switch (base_file_info_h.comp_type)
                {
                    case XeCompressionType.Raw:
                        num = (int)(IO.length - pe_data_offset); // Original length.
                        break;
                    case XeCompressionType.Zeroed:
                        num = (int)cert.image_size; // Zeroed length.
                        break;
                }

                byte[] data = IO.read_bytes(num);
                outio.write(data);
            }

            // Writeout PE Offset.
            outio.position = 8;
            outio.write(pe_offset, Endian.High);

            // Writeout Options Headers.
            outio.position = 24;

            foreach (XeOptHeader opt in opt_headers)
            {
                outio.write((UInt32)opt.key, Endian.High);
                outio.write(opt.data, Endian.High);
            }
            // Rehash.

            // Resign - Todo.

            // Close output.
            outio.close();
            outio = null;
        }

        public void extract_pe(string output_file, bool make_full_size = false)
        {
            // TODO Decrypt.

            // TODO Decompress.

            IO.position = pe_data_offset;

            byte[] exe = IO.read_bytes((int)(IO.length - pe_data_offset)); //(int)cert.image_size);

            System.IO.FileStream outio = new System.IO.FileStream(output_file, System.IO.FileMode.Create);
            if (make_full_size == true)
            {
                outio.Position = 0;

                for (uint i = 0; i < cert.image_size; i++)
                {
                    outio.WriteByte(0x00);
                }

                outio.Position = 0;
            }
            outio.Write(exe, 0, exe.Length);
            outio.Flush();
            outio.Close();
            outio = null;
            exe = null;
        }
        public void extract_resource(string output_file, XeResourceInfo res)
        {
            IO.position = pe_data_offset + (res.address - cert.load_address);

            byte[] data = IO.read_bytes((int)res.size);

            FileStream outio = new FileStream(output_file, FileMode.Create);

            outio.Position = 0;
            outio.Write(data, 0, data.Length);
            outio.Flush();
            outio.Close();
            outio = null;
            data = null;
        }
        public void extract_pe_section(string output_file, ImageSectionHeader sec)
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

        public void parse_image_sections()
        {
            /*if (has_img_idata_section == true)
            {
                pe_sec_idata = new SectionImportData(AppDomain.CurrentDomain.BaseDirectory + "/cache/idata.bin");
                pe_sec_idata.read();
            }*/
        }

        public static UInt32 get_version(byte major, byte minor, UInt16 build, byte qfe)
        {
            return (UInt32)(((major & 0xFFFFFFF0) << 28) | ((minor & 0xFFFFFFF0) << 24) | ((build & 0xFFFF0000) << 8) | (qfe & 0xFFFFFF00));
        }
    }
}

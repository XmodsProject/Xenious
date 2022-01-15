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
using Xbox360.IO;
using Xbox360.Crypto;

namespace Xbox360
{
    public class XenonExecutable
    {
        public FileIO IO;

        public bool is_xex {
            get { return (bool)(
                    // Thanks MATTRiCK and Emoose (Maybe Anthony too ?):)
                    (magic == "XEX2") || // Kernel Must Be >= 2.0.1861.0
                    (magic == "XEX1") || // Kernel Must Be >= 2.0.1838.0
                    (magic == "XEX%") || // Kernel Must Be >= 2.0.1746.0
                    (magic == "XEX-") || // Kernel Must Be >= 2.0.1640.0
                    (magic == "XEX?") || // Kernel Must Be >= 2.0.1529.0
                    (magic == "XEX0"));   // Kernel Must Be >= 2.0.1332.0
            }
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
        public string orig_import_kernels; // Holds original string with padding, this is used to until I figure out the padding.
        public byte[] orig_imports; // Holds original imports header.
        public byte[] orig_static_imps; // Holds original static libs header.
        public List<XeImportLibary> import_libs;
        public XeDeltaPatch delta_patch;
        public XeTLSInfo tls_info;
        public XeExportsByName exports_named;
        public XUIZHeader xuiz_h;
        public string bound_path = "";
        public string orig_pe_name = "";
        public UInt32 orig_base_addr = 0;
        public UInt32 exe_entry_point = 0;
        public UInt32 img_base_addr = 0; 
        public UInt32 system_flags = 0;
        public UInt32 Unknown_OPT_Data= 0;
        public UInt32 default_stack_size = 0;
        public UInt32 title_workspace_size = 0;
        public UInt32 default_fs_cache_size = 0;
        public UInt32 default_heap_size = 0;
        public byte[] checksum;
        public UInt32 timestamp = 0;
        public UInt32 callcap_start = 0;
        public UInt32 callcap_end = 0;
        public byte[] xgd3_media_id;
        public byte[] xbox_360_logo;
        public byte[] lan_key;
        public byte[] device_id;
        public byte[] base_file_format_header;
        public List<byte[]> alternative_title_ids;
        public List<byte[]> multidisc_media_ids;

        /* Encryption Context */
        Rijndael rjndl;

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
                cert.dencrypt_key = decrypt_session_key(false);
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
        public void parse_optional_headers()
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
                            throw new Exception("Failed Loading Optional Header (Resources).");
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
                            base_file_info_h.data = IO.read_bytes(base_file_info_h.info_size - 4);
                        }
                        catch
                        {
                            throw new Exception("Failed Loading Optional Header (File FOrmat Info).");
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
                            throw new Exception("Failed Loading Optional Header (Delta Patch Descriptor).");
                        }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.XGD3_MEDIA_KEY:
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            xgd3_media_id = IO.read_bytes((int)opt_headers[i].len);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (XGD3 Media ID)."); }
                        break;
                    case (uint)XeHeaderKeys.BOUNDING_PATH:
                        try
                        {
                            IO.position = (opt_headers[i].pos);
                            len = IO.read_uint32(Endian.High);
                            bound_path = IO.read_string((int)len);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (Bounding Path)."); }
                        break;
                    case (uint)XeHeaderKeys.DEVICE_ID:
                        #region DeviceID
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            device_id = IO.read_bytes((int)opt_headers[i].len);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (Device ID)."); }
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
                            orig_imports = IO.read_bytes((int)ils - 4);
                            IO.position -= ils - 4;
                            uint libs_len = IO.read_uint32(Endian.High);
                            uint num_libs = IO.read_uint32(Endian.High);
                            import_libs = new List<XeImportLibary>();
                            if (num_libs == 1)
                            {
                                orig_import_kernels = IO.read_string((int)libs_len);
                                XeImportLibary impl = new XeImportLibary();
                                impl.name = orig_import_kernels;
                                impl.read(IO);
                                import_libs.Add(impl);
                            }
                            else
                            {
                                orig_import_kernels = IO.read_string((int)libs_len);
                                string[] buf = orig_import_kernels.Split('\0');
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
                        catch { throw new Exception("Failed Loading Optional Header (Import Librarys)."); }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.CHECKSUM_TIMESTAMP:
                        #region ChecksumTimestamp
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            checksum = IO.read_bytes(4);
                            timestamp = IO.read_uint32(Endian.High);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (Checksum and Timestamp)."); }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.ENABLED_FOR_CALLCAP:
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            callcap_start = IO.read_uint32(Endian.High);
                            callcap_end = IO.read_uint32(Endian.High);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (Callcap)."); }
                        break;
                    case (uint)XeHeaderKeys.ORIGINAL_PE_NAME:
                        #region OriginalPEName
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            len = IO.read_uint32(Endian.High);
                            orig_pe_name = IO.read_string((int)len);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (Original Pe Name)."); }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.STATIC_LIBRARIES:
                        #region StaticLibs
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            uint num = (IO.read_uint32(Endian.High));
                            orig_static_imps = IO.read_bytes((int)num - 4);
                            IO.position -= (num - 4);

                            len = ((num - 4) / 16);
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
                        catch { throw new Exception("Failed Loading Optional Header (Static Librarys)."); }
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
                        catch { throw new Exception("Failed Loading Optional Header (TLS Info)."); }
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
                            xeinfo.version = IO.read_bytes(4);
                            xeinfo.base_version = IO.read_bytes(4);
                            xeinfo.title_id = IO.read_uint32(Endian.High);
                            xeinfo.platform = IO.read_byte();
                            xeinfo.executable_table = IO.read_byte();
                            xeinfo.disc_number = IO.read_byte();
                            xeinfo.disc_count = IO.read_byte();
                            xeinfo.savegame_id = IO.read_uint32(Endian.High);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (Execution info)."); }
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
                        catch { throw new Exception("Failed Loading Optional Header (Game Ratings)."); }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.LAN_KEY:
                        #region LanKey
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            lan_key = IO.read_bytes((int)opt_headers[i].len);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (Lan key)."); }
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
                        catch { throw new Exception("Failed Loading Optional Header (Xbox 360 Logo)."); }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.MULTIDISC_MEDIA_IDS:
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            uint size = IO.read_uint32(Endian.High);
                            multidisc_media_ids = new List<byte[]>();

                            for (uint x = 0; x < (size - 4) / 16; x++)
                            {
                                multidisc_media_ids.Add(IO.read_bytes(16));
                            }
                        }
                        catch
                        {
                            throw new Exception("Failed Loading Optional Header (MultiDisc media IDs).");
                        }
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
                        catch { throw new Exception("Failed Loading Optional Header (Alternate Title IDs)."); }
                        break;
                    case (uint)XeHeaderKeys.EXPORTS_BY_NAME:
                        try
                        {
                            IO.position = opt_headers[i].pos;
                            exports_named = new XeExportsByName();
                            exports_named.raw_size = IO.read_uint32(Endian.High);
                            exports_named.num_exports = IO.read_uint32(Endian.High);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (Exports By Name)."); }
                        break;

                    case (uint)XeHeaderKeys.BASE_REFERENCE:
                    case (uint)XeHeaderKeys.ENABLED_FOR_FASTCAP:
                    case (uint)XeHeaderKeys.ADDITIONAL_TITLE_MEMORY:
                    case (uint)XeHeaderKeys.PAGE_HEAP_SIZE_AND_FLAGS:
                    
                    default:
                        unk_headers.Add(opt_headers[i]);
                        break;
                }
            }
        }

        public bool has_header_key(XeHeaderKeys key)
        {
            foreach(XeOptHeader k in this.opt_headers)
            {
                if(key == k.key)
                {
                    return true;
                }
            }
            return false;
        }
        public void remove_header_key(XeHeaderKeys key)
        {
            foreach(XeOptHeader opt in opt_headers)
            {
                if(opt.key == key)
                {
                    opt_headers.Remove(opt);
                    break;
                }
            }
        }

        public byte[] decrypt_session_key(bool devkit = false)
        {
            if(devkit)
            {
                this.rjndl = new Rijndael(new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 128);
            }
            else
            {
                this.rjndl = new Rijndael(new byte[16] { 0x20, 0xB1, 0x85, 0xA5, 0x9D, 0x28, 0xFD, 0xC3, 0x40, 0x58, 0x3F, 0xBB, 0x08, 0x96, 0xBF, 0x91 }, 128);
            }
            byte[] data = this.rjndl.Decrypt(this.cert.seed_key);
            return data;

        }
        
        public byte[] decrypt_pe()
        {
            byte[] data;
            IO.position = pe_data_offset;
            data = IO.read_bytes((int)this.cert.image_size);

            this.rjndl = new Rijndael(this.decrypt_session_key(false), 128);

            data = this.rjndl.Decrypt(data);
            return data;
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
                        IO.write(checksum);
                        IO.write(timestamp, Endian.High);
                        break;
                    case XeHeaderKeys.ENABLED_FOR_CALLCAP:
                        IO.position = opt.pos;
                        IO.write(callcap_start, Endian.High);
                        IO.write(callcap_start, Endian.High);
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
                        IO.write(xeinfo.version);
                        IO.write(xeinfo.base_version);
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
                        IO.write(lan_key);
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
            long ptr = outio.position;

            for (int i = 0; i < opt_headers.Count; i++)
            {
                switch ((uint)opt_headers[i].key)
                {
                    case (uint)XeHeaderKeys.RESOURCE_INFO:
                        #region Writeout Resources
                        // Set opt headers offset.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;

                        // Write out size + 4.
                        outio.write((UInt32)(16 * resources.Count) + 4, Endian.High);

                        for (int x = 0; x < resources.Count; x++)
                        {
                            string name = resources[x].name;
                            // Filepadding.
                            for (int p = name.Length; p < 8; p++) { name += "\0"; }
                            outio.write(Encoding.ASCII.GetBytes(name));
                            outio.write(resources[x].address, Endian.High);
                            outio.write(resources[x].size, Endian.High);
                        }
                        ptr += ((16 * resources.Count) + 4);
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.FILE_FORMAT_INFO:
                        #region Writeout FileFormat
                        // Set opt headers offset.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;

                        // Write out BaseFileInfo.
                        outio.write(base_file_info_h.info_size, Endian.High);
                        outio.write(base_file_info_h.data);
                        ptr += base_file_info_h.info_size;

                        #endregion
                        break;
                    case (uint)XeHeaderKeys.DELTA_PATCH_DESCRIPTOR: break;
                    case (uint)XeHeaderKeys.BASE_REFERENCE: break;
                    case (uint)XeHeaderKeys.XGD3_MEDIA_KEY:
                        #region Writeout XGD3MediaID
                        // Set opt headers offset.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;
                        // Write out XGD3 Media ID.
                        outio.write(xgd3_media_id);

                        ptr += 16;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.BOUNDING_PATH:
                        #region Writeout BoundingPath
                        // Set opt headers offset.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;

                        // Write out length.
                        outio.write((UInt32)bound_path.Length, Endian.High);

                        // Write out bounding path.
                        outio.write(Encoding.ASCII.GetBytes(bound_path));
                        ptr += bound_path.Length + 4;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.DEVICE_ID:
                        #region Writeout DeviceID
                        // Set opt headers offset.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;

                        // Write out DeviceID (No length, header has length.
                        outio.write(device_id);
                        ptr += device_id.Length;
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
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;

                        outio.write((UInt32)orig_imports.Length + 4, Endian.High);
                        outio.write(orig_imports);
                        ptr += orig_imports.Length + 4;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.CHECKSUM_TIMESTAMP:
                        #region Writeout ChecksumTimestamp
                        // Set opt headers offset.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;

                        // Write out ChecksumTimestamp (No length, header has length.
                        outio.write(checksum);
                        outio.write(timestamp, Endian.High);
                        ptr += 8;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.ENABLED_FOR_CALLCAP:
                        #region Writeout Callcap
                        // Set opt headers offset.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;

                        // Write out Callcap
                        outio.write(callcap_start, Endian.High);
                        outio.write(callcap_end, Endian.High);
                        ptr += 8;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.ENABLED_FOR_FASTCAP: break;
                    case (uint)XeHeaderKeys.ORIGINAL_PE_NAME:
                        #region Writeout OriginalPEName
                        // Set opt headers offset.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;

                        // Write out length
                        outio.write((UInt32)orig_pe_name.Length, Endian.High);

                        // Writeout PE Original Name
                        outio.write(Encoding.ASCII.GetBytes(orig_pe_name));
                        ptr += orig_pe_name.Length + 4;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.STATIC_LIBRARIES:
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;

                        // Write out size.
                        outio.write((UInt32)(4 + orig_static_imps.Length), Endian.High);
                        outio.write(orig_static_imps);

                        ptr += (4 + orig_static_imps.Length);

                        break;
                    case (uint)XeHeaderKeys.TLS_INFO:
                        #region Writeout TLSInfo
                        // Set headers position.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;

                        // Writeout TLS Info.
                        outio.write(tls_info.slot_count, Endian.High);
                        outio.write(tls_info.raw_data_addr, Endian.High);
                        outio.write(tls_info.data_size, Endian.High);
                        outio.write(tls_info.raw_data_size, Endian.High);
                        ptr += 16;
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
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;

                        // Writeout Execution Info
                        outio.write(xeinfo.media_id, Endian.High);
                        outio.write(xeinfo.version);
                        outio.write(xeinfo.base_version);
                        outio.write(xeinfo.title_id, Endian.High);
                        outio.write(xeinfo.platform);
                        outio.write(xeinfo.executable_table);
                        outio.write(xeinfo.disc_number);
                        outio.write(xeinfo.disc_count);
                        outio.write(xeinfo.savegame_id, Endian.High);

                        ptr += 24;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.TITLE_WORKSPACE_SIZE:
                        opt_headers[i].data = title_workspace_size;
                        break;
                    case (uint)XeHeaderKeys.GAME_RATINGS:
                        #region Writeout RatingData
                        // Set headers position.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;

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
                        ptr += 64;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.LAN_KEY:
                        #region Writeout LanKey
                        // Set headers position.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;

                        // Writeout Lan Key.
                        outio.write(lan_key);
                        ptr += 16;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.XBOX360_LOGO:
                        #region Writeout Xbox360Logo
                        // Set headers position.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;

                        // Writeout Length.
                        outio.write((UInt32)xbox_360_logo.Length, Endian.High);

                        // Writeout logo.
                        outio.write(xbox_360_logo);
                        ptr += xbox_360_logo.Length + 4;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.MULTIDISC_MEDIA_IDS:
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;

                        // Writeout size.
                        outio.write((UInt32)(4 + (16 * multidisc_media_ids.Count)), Endian.High);

                        for (int x = 0; x < multidisc_media_ids.Count; x++)
                        {
                            outio.write(multidisc_media_ids[x]);
                        }
                        ptr += (4 + (16 * multidisc_media_ids.Count));
                        break;
                    case (uint)XeHeaderKeys.ALTERNATE_TITLE_IDS: break;
                    case (uint)XeHeaderKeys.ADDITIONAL_TITLE_MEMORY: break;
                    case (uint)XeHeaderKeys.EXPORTS_BY_NAME:
                        opt_headers[i].data = (UInt32)ptr;
                        outio.position = ptr;
                        // Header contains the length.
                        outio.write(exports_named.raw_size, Endian.High);
                        outio.write(exports_named.num_exports, Endian.High);
                        ptr += 8;
                        break;
                }
            }
            #endregion

            // Output PE.
            UInt32 pe_offset = (UInt32)outio.position;

            IO.position = pe_data_offset;
            int num = 0;

            num = (int)(IO.length - pe_data_offset); // Original length.
            int ptr2 = 0;
            int size = num;
            byte[] buf;

            // Write out RAW.
            while (size > 0)
            {
                outio.position = pe_data_offset + ptr2;
                if ((size - 4096) > 0)
                {
                    buf = IO.read_bytes(4096);
                    outio.write(buf);
                    size -= 4096;
                    ptr2 += 4096;
                }
                else
                {
                    buf = IO.read_bytes(size);
                    outio.write(buf);
                    size = 0;
                    ptr2 = 0;
                }
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
            IO.position = pe_data_offset;

            byte[] exe = new byte[(int)(IO.length - pe_data_offset)];

            if (base_file_info_h.comp_type == XeCompressionType.Compressed)
            {
                if (base_file_info_h.enc_type == XeEncryptionType.Encrypted)
                {
                    exe = this.decrypt_pe();
                }

                // Test For PE Header, is MZ exists then its just a compressed raw image.
                // else it is compressed with lzx.
                if(exe[0] != 0x4d && exe[1] != 0x5a)
                {
                    return;
                }
            }
            else if(base_file_info_h.comp_type == XeCompressionType.DeltaCompressed)
            {
                if (base_file_info_h.enc_type == XeEncryptionType.Encrypted)
                {
                    exe = this.decrypt_pe();
                }
            }
            else if(base_file_info_h.comp_type == XeCompressionType.Raw ||
                    base_file_info_h.comp_type == XeCompressionType.Zeroed)
            {
                if (base_file_info_h.enc_type == XeEncryptionType.Encrypted)
                {
                    exe = this.decrypt_pe();
                }
                else
                {
                    exe = IO.read_bytes((int)(IO.length - pe_data_offset));
                }
            }
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
    }
}

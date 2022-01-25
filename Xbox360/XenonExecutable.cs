/*
 * Reversed by [ Hect0r ] @ www.staticpi.net
 * With thanks to : http://www.free60.org/wiki/XEX
 * For inital stuff :)
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hect0rs.IO;
using System.IO;
using Xbox360.PE;
using Xbox360.XEX;
using Xbox360.XUIZ;
using Xbox360.Crypto;

namespace Xbox360
{
    public class XenonExecutable
    {
        public IOH IO;

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
        public XUIZ.Header xuiz_h;
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
            IO = new IOH(file, System.IO.FileMode.Open);
            IO.Position = 0;
        }
        public void read_header()
        {
            if (IO.IsOpen)
            {
                try {
                    magic = IO.ReadString(4);

                    if (is_xex)
                    {
                        module_flags = IO.ReadUInt32(Endian.High);
                        pe_data_offset = IO.ReadUInt32(Endian.High);
                        reserved = IO.ReadUInt32(Endian.High);
                        certificate_pos = IO.ReadUInt32(Endian.High);
                        opt_header_count = IO.ReadUInt32(Endian.High);

                        XeOptHeader bh;
                        opt_headers = new List<XeOptHeader>();
                        for (int i = 0; i < opt_header_count; i++)
                        {
                            bh = new XeOptHeader();
                            bh.key = (XeHeaderKeys)IO.ReadUInt32(Endian.High);
                            bh.data = IO.ReadUInt32(Endian.High);

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
                IO.Position = certificate_pos;
                cert = new XeCertificate();
                cert.header_size = IO.ReadUInt32(Endian.High);
                cert.image_size = IO.ReadUInt32(Endian.High);
                cert.rsa_sig = IO.ReadBytes(256);
                cert.UnkLen = IO.ReadUInt32(Endian.High);
                cert.image_flags = IO.ReadUInt32(Endian.High);
                cert.load_address = IO.ReadUInt32(Endian.High);
                cert.section_disgest = IO.ReadBytes(20);
                cert.import_table_count = IO.ReadUInt32(Endian.High);
                cert.import_table_digest = IO.ReadBytes(20);
                cert.xgd2_media_id = IO.ReadBytes(16);
                cert.seed_key = IO.ReadBytes(16);
                cert.dencrypt_key = decrypt_session_key(false);
                cert.export_table_pos = IO.ReadUInt32(Endian.High);
                cert.header_digest = IO.ReadBytes(20);
                cert.game_regions = IO.ReadUInt32(Endian.High);
                cert.media_flags = IO.ReadUInt32(Endian.High);
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
                IO.Position = certificate_pos + 0x180;
                uint num = IO.ReadUInt32(Endian.High);
                sections = new List<XeSection>();

                for (uint i = 0; i < num; i++)
                {
                    XeSection sec = new XeSection();
                    if (pe_data_offset <= 0x90000000) { sec.page_size = 64 * 1024; }
                    else { sec.page_size = 4 * 1024; }
                    sec.value = IO.ReadUInt32(Endian.High);
                    sec.digest = IO.ReadBytes(20);
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
                            IO.Position = (opt_headers[i].pos);
                            uint num = IO.ReadUInt32(Endian.High);
                            num = (num - 4) / 16;

                            XeResourceInfo res;
                            for (int x = 0; x < num; x++)
                            {
                                res = new XeResourceInfo();
                                res.name = IO.ReadString(8);
                                res.address = IO.ReadUInt32(Endian.High);
                                res.size = IO.ReadUInt32(Endian.High);
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
                            IO.Position = opt_headers[i].pos;
                            base_file_info_h = new XeBaseFileInfoHeader();
                            base_file_info_h.info_size = IO.ReadInt32(Endian.High);
                            base_file_info_h.data = IO.ReadBytes(base_file_info_h.info_size);// - 4);
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
                            IO.Position = opt_headers[i].pos;
                            delta_patch = new XeDeltaPatch();
                            delta_patch.size = IO.ReadUInt32(Endian.High);
                            delta_patch.target_version = IO.ReadUInt32(Endian.High);
                            delta_patch.source_version = IO.ReadUInt32(Endian.High);
                            delta_patch.hash_source = IO.ReadBytes(20);
                            delta_patch.key_source = IO.ReadBytes(16);
                            delta_patch.target_headers_size = IO.ReadUInt32(Endian.High);
                            delta_patch.headers_source_offset = IO.ReadUInt32(Endian.High);
                            delta_patch.headers_source_size = IO.ReadUInt32(Endian.High);
                            delta_patch.headers_target_offset = IO.ReadUInt32(Endian.High);
                            delta_patch.image_source_offset = IO.ReadUInt32(Endian.High);
                            delta_patch.image_source_size = IO.ReadUInt32(Endian.High);
                            delta_patch.image_target_offset = IO.ReadUInt32(Endian.High);

                            delta_patch.header_data = IO.ReadBytes((int)(delta_patch.size - 76));
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
                            IO.Position = opt_headers[i].pos;
                            xgd3_media_id = IO.ReadBytes((int)opt_headers[i].len);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (XGD3 Media ID)."); }
                        break;
                    case (uint)XeHeaderKeys.BOUNDING_PATH:
                        try
                        {
                            IO.Position = (opt_headers[i].pos);
                            len = IO.ReadUInt32(Endian.High);
                            bound_path = IO.ReadString((int)len - 4);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (Bounding Path)."); }
                        break;
                    case (uint)XeHeaderKeys.DEVICE_ID:
                        #region DeviceID
                        try
                        {
                            IO.Position = opt_headers[i].pos;
                            device_id = IO.ReadBytes((int)opt_headers[i].len);
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
                            IO.Position = opt_headers[i].pos;
                            uint ils = IO.ReadUInt32(Endian.High);
                            orig_imports = IO.ReadBytes((int)ils - 4);
                            IO.Position -= ils - 4;
                            uint libs_len = IO.ReadUInt32(Endian.High);
                            uint num_libs = IO.ReadUInt32(Endian.High);
                            import_libs = new List<XeImportLibary>();
                            if (num_libs == 1)
                            {
                                orig_import_kernels = IO.ReadString((int)libs_len - 4);
                                UInt32 Unknown = IO.ReadUInt32(Endian.High);
                                XeImportLibary impl = new XeImportLibary();
                                impl.name = orig_import_kernels;
                                impl.read(IO);
                                import_libs.Add(impl);
                            }
                            else
                            {
                                orig_import_kernels = IO.ReadString((int)libs_len - 4);
                                UInt32 Unknown = IO.ReadUInt32(Endian.High);
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
                            IO.Position = opt_headers[i].pos;
                            checksum = IO.ReadBytes(4);
                            timestamp = IO.ReadUInt32(Endian.High);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (Checksum and Timestamp)."); }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.ENABLED_FOR_CALLCAP:
                        try
                        {
                            IO.Position = opt_headers[i].pos;
                            callcap_start = IO.ReadUInt32(Endian.High);
                            callcap_end = IO.ReadUInt32(Endian.High);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (Callcap)."); }
                        break;
                    case (uint)XeHeaderKeys.ORIGINAL_PE_NAME:
                        #region OriginalPEName
                        try
                        {
                            IO.Position = opt_headers[i].pos;
                            len = IO.ReadUInt32(Endian.High);
                            orig_pe_name = IO.ReadString((int)len);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (Original Pe Name)."); }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.STATIC_LIBRARIES:
                        #region StaticLibs
                        try
                        {
                            IO.Position = opt_headers[i].pos;
                            uint num = (IO.ReadUInt32(Endian.High));
                            orig_static_imps = IO.ReadBytes((int)num - 4);
                            IO.Position -= (num - 4);

                            len = ((num - 4) / 16);
                            static_libs = new List<XeStaticLib>();

                            for (int x = 0; x < len; x++)
                            {
                                XeStaticLib sl = new XeStaticLib();
                                sl.name = IO.ReadString(8);
                                sl.major = IO.ReadUInt16(Endian.High);
                                sl.minor = IO.ReadUInt16(Endian.High);
                                sl.build = IO.ReadUInt16(Endian.High);
                                sl.qfe = IO.ReadUInt16(Endian.High);
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
                            IO.Position = opt_headers[i].pos;
                            tls_info = new XeTLSInfo();
                            tls_info.slot_count = IO.ReadUInt32(Endian.High);
                            tls_info.raw_data_addr = IO.ReadUInt32(Endian.High);
                            tls_info.data_size = IO.ReadUInt32(Endian.High);
                            tls_info.raw_data_size = IO.ReadUInt32(Endian.High);
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
                            
                            IO.Position = opt_headers[i].pos;
                            xeinfo = new XeExecutionInfo();
                            xeinfo.media_id = IO.ReadUInt32(Endian.High);
                            xeinfo.version = IO.ReadBytes(4);
                            xeinfo.base_version = IO.ReadBytes(4);
                            xeinfo.title_id = IO.ReadUInt32(Endian.High);
                            xeinfo.platform = IO.ReadByte();
                            xeinfo.executable_table = IO.ReadByte();
                            xeinfo.disc_number = IO.ReadByte();
                            xeinfo.disc_count = IO.ReadByte();
                            xeinfo.savegame_id = IO.ReadUInt32(Endian.High);
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
                            IO.Position = opt_headers[i].pos;
                            ratings.esrb = (XeRating_esrb)IO.ReadByte();
                            ratings.pegi = (XeRating_pegi)IO.ReadByte();
                            ratings.pegifi = (XeRating_pegi_fi)IO.ReadByte();
                            ratings.pegipt = (XeRating_pegi_pt)IO.ReadByte();
                            ratings.bbfc = (XeRating_bbfc)IO.ReadByte();
                            ratings.cero = (XeRating_cero)IO.ReadByte();
                            ratings.usk = (XeRating_usk)IO.ReadByte();
                            ratings.oflcau = (XeRating_oflc_au)IO.ReadByte();
                            ratings.oflcnz = (XeRating_oflc_nz)IO.ReadByte();
                            ratings.kmrb = (XeRating_kmrb)IO.ReadByte();
                            ratings.brazil = (XeRating_brazil)IO.ReadByte();
                            ratings.fpb = (XeRating_fpb)IO.ReadByte();
                            ratings.reserved = IO.ReadBytes(52);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (Game Ratings)."); }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.LAN_KEY:
                        #region LanKey
                        try
                        {
                            IO.Position = opt_headers[i].pos;
                            lan_key = IO.ReadBytes((int)opt_headers[i].len);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (Lan key)."); }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.XBOX360_LOGO:
                        #region Xbox360Logo
                        try
                        {
                            IO.Position = opt_headers[i].pos;
                            uint x360l_len = IO.ReadUInt32(Endian.High);
                            xbox_360_logo = IO.ReadBytes((int)x360l_len);
                        }
                        catch { throw new Exception("Failed Loading Optional Header (Xbox 360 Logo)."); }
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.MULTIDISC_MEDIA_IDS:
                        try
                        {
                            IO.Position = opt_headers[i].pos;
                            uint size = IO.ReadUInt32(Endian.High);
                            multidisc_media_ids = new List<byte[]>();

                            for (uint x = 0; x < (size - 4) / 16; x++)
                            {
                                multidisc_media_ids.Add(IO.ReadBytes(16));
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
                            IO.Position = opt_headers[i].pos;
                            uint len2 = IO.ReadUInt32(Endian.High);
                            alternative_title_ids = new List<byte[]>();
                            for (int x = 0; x < (int)(len2 - 4) / 4; x++)
                            {
                                alternative_title_ids.Add(IO.ReadBytes(4));
                            }
                        }
                        catch { throw new Exception("Failed Loading Optional Header (Alternate Title IDs)."); }
                        break;
                    case (uint)XeHeaderKeys.EXPORTS_BY_NAME:
                        try
                        {
                            IO.Position = opt_headers[i].pos;
                            exports_named = new XeExportsByName();
                            exports_named.raw_size = IO.ReadUInt32(Endian.High);
                            exports_named.num_exports = IO.ReadUInt32(Endian.High);
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

        public byte[] decrypt_session_key(bool devkit)
        {
            if(devkit == true)
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
        
        public byte[] decrypt_pe(bool devkit)
        {
            byte[] data;
            IO.Position = pe_data_offset;
            data = IO.ReadBytes((int)(IO.Length - pe_data_offset));

            this.rjndl = new Rijndael(this.decrypt_session_key(devkit), 128);

            data = this.rjndl.Decrypt(data);
            return data;
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
            IO.Position = 4;
            IO.Write(module_flags, Endian.High);

            // Save Certificate.
            IO.Position = certificate_pos;
            IO.Write(cert.header_size, Endian.High);
            IO.Write(cert.image_size, Endian.High);
            IO.Write(cert.rsa_sig);
            IO.Write(cert.UnkLen, Endian.High);
            IO.Write(cert.image_flags, Endian.High);
            IO.Write(cert.load_address, Endian.High);
            IO.Write(hash_blank); // Will hash later.
            IO.Write(cert.import_table_count, Endian.High);
            IO.Write(cert.import_table_digest);
            IO.Write(cert.xgd2_media_id);
            IO.Write(cert.seed_key);
            IO.Write(cert.export_table_pos, Endian.High);
            IO.Write(hash_blank); // Will hash later.
            IO.Write(cert.game_regions, Endian.High);
            IO.Write(cert.media_flags, Endian.High);

            // Writeout Optional Headers.
            int num = 0;
            foreach(XeOptHeader opt in opt_headers)
            {
                switch(opt.key)
                {
                    case XeHeaderKeys.ORIGINAL_PE_NAME:
                        IO.Position = opt.pos;
                        IO.Write((UInt32)orig_pe_name.Length, Endian.High);
                        IO.Write(Encoding.ASCII.GetBytes(orig_pe_name));
                        break;
                    case XeHeaderKeys.BOUNDING_PATH:
                        IO.Position = opt.pos;
                        IO.Write((UInt32)bound_path.Length, Endian.High);
                        IO.Write(Encoding.ASCII.GetBytes(bound_path));
                        break;
                    case XeHeaderKeys.XGD3_MEDIA_KEY:
                        IO.Position = opt.pos;
                        IO.Write(xgd3_media_id);
                        break;
                    case XeHeaderKeys.DEVICE_ID:
                        IO.Position = opt.pos;
                        IO.Write(xgd3_media_id);
                        break;
                    case XeHeaderKeys.ORIGINAL_BASE_ADDRESS:
                        IO.Position = 24 + ((8 * (num + 1))  - 4);
                        IO.Write(orig_base_addr, Endian.High);
                        break;
                    case XeHeaderKeys.ENTRY_POINT:
                        IO.Position = 24 + ((8 * (num + 1))  - 4);
                        IO.Write(exe_entry_point, Endian.High);
                        break;
                    case XeHeaderKeys.IMAGE_BASE_ADDRESS:
                        IO.Position = 24 + ((8 * (num + 1))  - 4);
                        IO.Write(img_base_addr, Endian.High);
                        break;
                    case XeHeaderKeys.CHECKSUM_TIMESTAMP:
                        IO.Position = opt.pos;
                        IO.Write(checksum);
                        IO.Write(timestamp, Endian.High);
                        break;
                    case XeHeaderKeys.ENABLED_FOR_CALLCAP:
                        IO.Position = opt.pos;
                        IO.Write(callcap_start, Endian.High);
                        IO.Write(callcap_start, Endian.High);
                        break;
                    case XeHeaderKeys.DEFAULT_STACK_SIZE:
                        IO.Position = 24 + ((8 * (num + 1))  - 4);
                        IO.Write(default_stack_size, Endian.High);
                        break;
                    case XeHeaderKeys.DEFAULT_FILESYSTEM_CACHE_SIZE:
                        IO.Position = 24 + ((8 * (num + 1))  - 4);
                        IO.Write(default_fs_cache_size, Endian.High);
                        break;
                    case XeHeaderKeys.DEFAULT_HEAP_SIZE:
                        IO.Position = 24 + ((8 * (num + 1))  - 4);
                        IO.Write(default_heap_size, Endian.High);
                        break;
                    case XeHeaderKeys.SYSTEM_FLAGS:
                        IO.Position = 24 + ((8 * (num + 1))  - 4);
                        IO.Write(system_flags, Endian.High);
                        break;
                    case XeHeaderKeys.UNKNOWN1:
                        IO.Position = 24 + ((8 * (num + 1))  - 4);
                        IO.Write(Unknown_OPT_Data, Endian.High);
                        break;
                    case XeHeaderKeys.EXECUTION_INFO:
                        IO.Position = opt.pos;
                        IO.Write(xeinfo.media_id, Endian.High);
                        IO.Write(xeinfo.version);
                        IO.Write(xeinfo.base_version);
                        IO.Write(xeinfo.title_id, Endian.High);
                        IO.Write(xeinfo.platform);
                        IO.Write(xeinfo.executable_table);
                        IO.Write(xeinfo.disc_number);
                        IO.Write(xeinfo.disc_count);
                        IO.Write(xeinfo.savegame_id, Endian.High);
                        break;
                    case XeHeaderKeys.TITLE_WORKSPACE_SIZE:
                        IO.Position = 24 + ((8 * (num + 1))  - 4);
                        IO.Write(title_workspace_size, Endian.High);
                        break;
                    case XeHeaderKeys.GAME_RATINGS:
                        IO.Position = opt.pos;
                        IO.Write((byte)ratings.esrb);
                        IO.Write((byte)ratings.pegi);
                        IO.Write((byte)ratings.pegifi);
                        IO.Write((byte)ratings.pegipt);
                        IO.Write((byte)ratings.bbfc);
                        IO.Write((byte)ratings.cero);
                        IO.Write((byte)ratings.usk);
                        IO.Write((byte)ratings.oflcau);
                        IO.Write((byte)ratings.oflcnz);
                        IO.Write((byte)ratings.kmrb);
                        IO.Write((byte)ratings.brazil);
                        IO.Write((byte)ratings.fpb);
                        IO.Write(ratings.reserved);
                        break;
                    case XeHeaderKeys.LAN_KEY:
                        IO.Position = opt.pos;
                        IO.Write(lan_key);
                        break;
                    case XeHeaderKeys.ALTERNATE_TITLE_IDS:
                        IO.Position = opt.pos + 4;

                        foreach(byte[] id in alternative_title_ids)
                        {
                            IO.Write(id);
                        }
                        break;
                }
                num++;
            }
        }
        public void rebuild_xex(string output, bool rebuild_pe)
        {
            IOH outio = new IOH(output, System.IO.FileMode.Create);
            byte[] hash_blank = new byte[20] {
                0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00
            };

            // Write out Header.
            outio.Position = 0;
            outio.Write(Encoding.ASCII.GetBytes("XEX2")); // XEX2
            outio.Write(module_flags, Endian.High);
            outio.Write((UInt32)0, Endian.High); // Leave blank for now.
            outio.Write(reserved, Endian.High);
            outio.Write((UInt32)(24 + (8 * opt_headers.Count)), Endian.High);
            outio.Write((UInt32)opt_headers.Count, Endian.High);


            // Write out the certificate.
            #region Certificate
            outio.Position = 24 + (8 * opt_headers.Count);
            outio.Write(cert.header_size, Endian.High);
            outio.Write(cert.image_size, Endian.High);
            outio.Write(cert.rsa_sig);
            outio.Write(cert.UnkLen, Endian.High);
            outio.Write(cert.image_flags, Endian.High);
            outio.Write(cert.load_address, Endian.High);
            outio.Write(hash_blank); // Will hash later.
            outio.Write(cert.import_table_count, Endian.High);
            outio.Write(cert.import_table_digest);
            outio.Write(cert.xgd2_media_id);
            outio.Write(cert.seed_key);
            outio.Write(cert.export_table_pos, Endian.High);
            outio.Write(hash_blank); // Will hash later.
            outio.Write(cert.game_regions, Endian.High);
            outio.Write(cert.media_flags, Endian.High);
            #endregion

            // Write out Section Table.
            outio.Position = (24 + (8 * opt_headers.Count)) + 0x180;
            outio.Write((UInt32)sections.Count, Endian.High);

            for (int i = 0; i < sections.Count; i++)
            {
                outio.Write(sections[i].value, Endian.High);
                outio.Write(sections[i].digest);
            }

            // Output Headers.
            #region Out opt_headers
            // Write out opt headers.
            long ptr = outio.Position;

            for (int i = 0; i < opt_headers.Count; i++)
            {
                switch ((uint)opt_headers[i].key)
                {
                    case (uint)XeHeaderKeys.RESOURCE_INFO:
                        #region Writeout Resources
                        // Set opt headers offset.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.Position = ptr;

                        // Write out size + 4.
                        outio.Write((UInt32)(16 * resources.Count) + 4, Endian.High);

                        for (int x = 0; x < resources.Count; x++)
                        {
                            string name = resources[x].name;
                            // Filepadding.
                            for (int p = name.Length; p < 8; p++) { name += "\0"; }
                            outio.Write(Encoding.ASCII.GetBytes(name));
                            outio.Write(resources[x].address, Endian.High);
                            outio.Write(resources[x].size, Endian.High);
                        }
                        ptr += ((16 * resources.Count) + 4);
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.FILE_FORMAT_INFO:
                        #region Writeout FileFormat
                        // Set opt headers offset.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.Position = ptr;

                        // Write out BaseFileInfo.
                        outio.Write(base_file_info_h.info_size, Endian.High);
                        outio.Write(base_file_info_h.data);
                        ptr += base_file_info_h.info_size;

                        #endregion
                        break;
                    case (uint)XeHeaderKeys.DELTA_PATCH_DESCRIPTOR: break;
                    case (uint)XeHeaderKeys.BASE_REFERENCE: break;
                    case (uint)XeHeaderKeys.XGD3_MEDIA_KEY:
                        #region Writeout XGD3MediaID
                        // Set opt headers offset.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.Position = ptr;
                        // Write out XGD3 Media ID.
                        outio.Write(xgd3_media_id);

                        ptr += 16;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.BOUNDING_PATH:
                        #region Writeout BoundingPath
                        // Set opt headers offset.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.Position = ptr;

                        // Write out length.
                        outio.Write((UInt32)bound_path.Length, Endian.High);

                        // Write out bounding path.
                        outio.Write(Encoding.ASCII.GetBytes(bound_path));
                        ptr += bound_path.Length + 4;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.DEVICE_ID:
                        #region Writeout DeviceID
                        // Set opt headers offset.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.Position = ptr;

                        // Write out DeviceID (No length, header has length.
                        outio.Write(device_id);
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
                        long pos = outio.Position;
                        opt_headers[i].data = (UInt32)ptr;
                        outio.Position = ptr;

                        outio.Write((UInt32)orig_imports.Length + 4, Endian.High);
                        outio.Write(orig_imports);
                        ptr += orig_imports.Length + 4;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.CHECKSUM_TIMESTAMP:
                        #region Writeout ChecksumTimestamp
                        // Set opt headers offset.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.Position = ptr;

                        // Write out ChecksumTimestamp (No length, header has length.
                        outio.Write(checksum);
                        outio.Write(timestamp, Endian.High);
                        ptr += 8;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.ENABLED_FOR_CALLCAP:
                        #region Writeout Callcap
                        // Set opt headers offset.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.Position = ptr;

                        // Write out Callcap
                        outio.Write(callcap_start, Endian.High);
                        outio.Write(callcap_end, Endian.High);
                        ptr += 8;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.ENABLED_FOR_FASTCAP: break;
                    case (uint)XeHeaderKeys.ORIGINAL_PE_NAME:
                        #region Writeout OriginalPEName
                        // Set opt headers offset.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.Position = ptr;

                        // Write out length
                        outio.Write((UInt32)orig_pe_name.Length, Endian.High);

                        // Writeout PE Original Name
                        outio.Write(Encoding.ASCII.GetBytes(orig_pe_name));
                        ptr += orig_pe_name.Length + 4;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.STATIC_LIBRARIES:
                        opt_headers[i].data = (UInt32)ptr;
                        outio.Position = ptr;

                        // Write out size.
                        outio.Write((UInt32)(4 + orig_static_imps.Length), Endian.High);
                        outio.Write(orig_static_imps);

                        ptr += (4 + orig_static_imps.Length);

                        break;
                    case (uint)XeHeaderKeys.TLS_INFO:
                        #region Writeout TLSInfo
                        // Set headers position.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.Position = ptr;

                        // Writeout TLS Info.
                        outio.Write(tls_info.slot_count, Endian.High);
                        outio.Write(tls_info.raw_data_addr, Endian.High);
                        outio.Write(tls_info.data_size, Endian.High);
                        outio.Write(tls_info.raw_data_size, Endian.High);
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
                        outio.Position = ptr;

                        // Writeout Execution Info
                        outio.Write(xeinfo.media_id, Endian.High);
                        outio.Write(xeinfo.version);
                        outio.Write(xeinfo.base_version);
                        outio.Write(xeinfo.title_id, Endian.High);
                        outio.Write(xeinfo.platform);
                        outio.Write(xeinfo.executable_table);
                        outio.Write(xeinfo.disc_number);
                        outio.Write(xeinfo.disc_count);
                        outio.Write(xeinfo.savegame_id, Endian.High);

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
                        outio.Position = ptr;

                        // Writeout Rating Data.
                        outio.Write((byte)ratings.esrb);
                        outio.Write((byte)ratings.pegi);
                        outio.Write((byte)ratings.pegifi);
                        outio.Write((byte)ratings.pegipt);
                        outio.Write((byte)ratings.bbfc);
                        outio.Write((byte)ratings.cero);
                        outio.Write((byte)ratings.usk);
                        outio.Write((byte)ratings.oflcau);
                        outio.Write((byte)ratings.oflcnz);
                        outio.Write((byte)ratings.kmrb);
                        outio.Write((byte)ratings.brazil);
                        outio.Write((byte)ratings.fpb);
                        outio.Write(ratings.reserved);
                        ptr += 64;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.LAN_KEY:
                        #region Writeout LanKey
                        // Set headers position.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.Position = ptr;

                        // Writeout Lan Key.
                        outio.Write(lan_key);
                        ptr += 16;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.XBOX360_LOGO:
                        #region Writeout Xbox360Logo
                        // Set headers position.
                        opt_headers[i].data = (UInt32)ptr;
                        outio.Position = ptr;

                        // Writeout Length.
                        outio.Write((UInt32)xbox_360_logo.Length, Endian.High);

                        // Writeout logo.
                        outio.Write(xbox_360_logo);
                        ptr += xbox_360_logo.Length + 4;
                        #endregion
                        break;
                    case (uint)XeHeaderKeys.MULTIDISC_MEDIA_IDS:
                        opt_headers[i].data = (UInt32)ptr;
                        outio.Position = ptr;

                        // Writeout size.
                        outio.Write((UInt32)(4 + (16 * multidisc_media_ids.Count)), Endian.High);

                        for (int x = 0; x < multidisc_media_ids.Count; x++)
                        {
                            outio.Write(multidisc_media_ids[x]);
                        }
                        ptr += (4 + (16 * multidisc_media_ids.Count));
                        break;
                    case (uint)XeHeaderKeys.ALTERNATE_TITLE_IDS: break;
                    case (uint)XeHeaderKeys.ADDITIONAL_TITLE_MEMORY: break;
                    case (uint)XeHeaderKeys.EXPORTS_BY_NAME:
                        opt_headers[i].data = (UInt32)ptr;
                        outio.Position = ptr;
                        // Header contains the length.
                        outio.Write(exports_named.raw_size, Endian.High);
                        outio.Write(exports_named.num_exports, Endian.High);
                        ptr += 8;
                        break;
                }
            }
            #endregion

            // Output PE.
            UInt32 pe_offset = (UInt32)outio.Position;

            IO.Position = pe_data_offset;
            int num = 0;

            num = (int)(IO.Length - pe_data_offset); // Original length.
            int ptr2 = 0;
            int size = num;
            byte[] buf;

            // Write out RAW.
            while (size > 0)
            {
                outio.Position = pe_data_offset + ptr2;
                if ((size - 4096) > 0)
                {
                    buf = IO.ReadBytes(4096);
                    outio.Write(buf);
                    size -= 4096;
                    ptr2 += 4096;
                }
                else
                {
                    buf = IO.ReadBytes(size);
                    outio.Write(buf);
                    size = 0;
                    ptr2 = 0;
                }
            }
            // Writeout PE Offset.
            outio.Position = 8;
            outio.Write(pe_offset, Endian.High);

            // Writeout Options Headers.
            outio.Position = 24;

            foreach (XeOptHeader opt in opt_headers)
            {
                outio.Write((UInt32)opt.key, Endian.High);
                outio.Write(opt.data, Endian.High);
            }
            // Rehash.

            // Resign - Todo.

            // Close output.
            outio.close();
            outio = null;
        }

        public void extract_pe(string output_file, bool SkipHashCheck = false, bool make_full_size = false, bool devkit = false)
        {
            // We need to be at the end of the meta header.
            IO.Position = pe_data_offset;

            byte[] ExeData = null;
            byte[] exe_compressed = null;
            int act_size_comp = 0;

            // If compressed only.
            if (base_file_info_h.comp_type == XeCompressionType.Compressed)
            {
                byte[] CompExeData;
                if (base_file_info_h.enc_type == XeEncryptionType.Encrypted)
                {
                    CompExeData = this.decrypt_pe(devkit);
                }
                else
                {
                    CompExeData = IO.ReadBytes((int)(IO.Length - pe_data_offset));
                }

                IOH CIO = new IOH(AppDomain.CurrentDomain.BaseDirectory + "/cache/comp_pe.bin", FileMode.Create);
                CIO.Position = 0;
                CIO.Write(CompExeData);
                CIO.Position = 0;
                CompExeData = null;

                // Declare our output buffer.
                exe_compressed = new byte[(int)CIO.Length];

                // Our sha1 hash computer for block verification of the compressed data.
                System.Security.Cryptography.SHA1Managed s1 = new System.Security.Cryptography.SHA1Managed();

                // Basically NextBlockSize + NextBlockHash then data.
                byte[] CompDataBlock = null; // we dclare this in the loop below, until we each the end, each block of data could be a larger size.




                ulong i = pe_data_offset;
                int BlockPos = 0;
                ushort ChunkCount;
                uint NextBlockSize = this.base_file_info_h.compressed_base_file_info.block.block_size;
                byte[] NextBlockHash = this.base_file_info_h.compressed_base_file_info.block.hash;
                int nBlockIdx = 0;
                int CompBufIndex = 0;
                byte[] Buf = new byte[4]
                {
                    0,0,0,0
                };

                while (i < (ulong)CIO.Length)
                {
                    BlockPos = 0;

                    // Read block
                    CompDataBlock = CIO.ReadBytes((int)NextBlockSize);

                    // Check if we can skip or be less strict.
                    if (SkipHashCheck == false)
                    {
                        // Now compute a hash of this block data.
                        byte[] bh = s1.ComputeHash(CompDataBlock);

                        string[] results = new string[2]
                        {
                            BitConverter.ToString(bh).ToLower().Replace("-", ""),
                            BitConverter.ToString(NextBlockHash).ToLower().Replace("-", ""),
                        };
                        // Check the first block of data hash with the first compresed block data in BaseFileInfoHeader.
                        if (results[0] != results[1])
                        {
                            throw new Exception(String.Format("Compressed data hash of n block in basefileinfo/compressednextblock does not match computed hash of block data. [n={0}]... Try setting 'Relaxed'.", nBlockIdx));
                        }
                    }

                    // Increment Stuff.
                    nBlockIdx++;
                    i += (ulong)NextBlockSize;
                    Buf = new byte[4] { 0, 0, 0, 0 };
                    // Copy over nextblocksize from current block.
                    Array.Copy(CompDataBlock, 0, Buf, 0, 4);

                    if (BitConverter.IsLittleEndian) { Array.Reverse(Buf); }

                    // Change for next round.
                    NextBlockSize = BitConverter.ToUInt32(Buf, 0);
                    Array.Copy(CompDataBlock, 4, NextBlockHash, 0, 20);

                    // Now incrememnt BlockPos as we need it.
                    BlockPos += 24;


                    // Now the magic chunk parsing starts.
                    while (true)
                    {
                        // Reset chunk count.
                        ChunkCount = 0;

                        // Get CHunk count (Little endian)
                        Buf = new byte[2] { 0, 0 };
                        Array.Copy(CompDataBlock, BlockPos, Buf, 0, 2);
                        ChunkCount = (ushort)(Buf[0] << 8 | Buf[1]);
                        BlockPos += 2;

                        if (ChunkCount == 0)
                        {
                            break;
                        }

                        // Check for invalid or if using the wrong decryption key.
                        if (ChunkCount > CompDataBlock.Length)
                        {
                            throw new Exception("Cannot read compressed image, likely using the wrong mode try switch mode, if that doesent work then the image is invalid.");
                        }

                        try
                        {
                            Array.Copy(CompDataBlock, BlockPos, exe_compressed, CompBufIndex, ChunkCount);
                        }
                        catch 
                        {
                            throw new Exception("Exception occured the error usually means its invalid chunck, try devkit keys or retail (depends on which is set, basically try the other key set, if it still crashes then its a fucked up compressed image).");
                        }
                        CompBufIndex += ChunkCount;
                        act_size_comp += ChunkCount;
                        BlockPos += ChunkCount;
                    }
                }
                CIO.close();
                CIO = null;
                // Remove padding.
                byte[] temp = new byte[act_size_comp];
                Array.Copy(exe_compressed, 0, temp, 0, act_size_comp);
                exe_compressed = temp;
                temp = null;

                ExeData = new byte[(int)this.cert.image_size];
                try
                {
                    int x = Compression.LZX.Decompress(exe_compressed, exe_compressed.Length, ExeData, (int)this.cert.image_size, this.base_file_info_h.compressed_base_file_info.compression_window);

                    if (x != 0)
                    {
                        throw new Exception("Could not decompress image... Likely broken xex.");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (base_file_info_h.comp_type == XeCompressionType.Zeroed ||base_file_info_h.comp_type == XeCompressionType.Raw)
            {
                if (base_file_info_h.enc_type == XeEncryptionType.Encrypted)
                {
                    ExeData = this.decrypt_pe(devkit);
                }
                else
                {
                    ExeData = IO.ReadBytes((int)(this.cert.image_size));
                }

                // We should really add zero data, todo in later release. 
            }

            IOH outio = new IOH(output_file, FileMode.Create);
            outio.Position = 0;
            outio.Write(ExeData);
            outio.close();
            outio = null;
        }
        public void extract_resource(string output_file, XeResourceInfo res)
        {
            IO.Position = pe_data_offset + (res.address - cert.load_address);

            byte[] data = IO.ReadBytes((int)res.size);

            FileStream outio = new FileStream(output_file, FileMode.Create);

            outio.Position = 0;
            outio.Write(data, 0, data.Length);
            outio.Flush();
            outio.Close();
            outio = null;
            data = null;
        }
        public void extract_resource_from_pe(IOH inputPE, string output_file, XeResourceInfo res)
        {
            inputPE.Position = (res.address - cert.load_address);

            byte[] data = inputPE.ReadBytes((int)res.size);

            FileStream outio = new FileStream(output_file.Replace("\0", ""), FileMode.Create);

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

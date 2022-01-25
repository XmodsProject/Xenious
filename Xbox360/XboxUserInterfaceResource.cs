using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbox360
{
    public class XboxUserInterfaceResource
    {
        public Hect0rs.IO.IOH IO;
        public XUIZ.Header Header;
        public List<XUIZ.ContentEntry> Entrys;

        private int EndOfHeader = 0;
        private long StartPos = 0;

        public XboxUserInterfaceResource(string InputFile)
        {
            this.IO = new Hect0rs.IO.IOH(InputFile, System.IO.FileMode.Open);
            this.Parse();
        }

        public XboxUserInterfaceResource(Hect0rs.IO.IOH InputIO)
        {
            this.IO = InputIO;
            this.Parse();
        }
        public void SetStartPos(long Start)
        {
            if (Start < 0 || Start > this.IO.Length)
            {
                throw new Exception("Couldent Set Starting Position of the XUIZ IO Handle.");
            }

            this.StartPos = Start;
        }

        public bool IsValid
        {
            get
            {
                if (this.Header.FileEnd != IO.Length)
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsXUIZ
        {
            get { return (bool)(this.Header.Magic == "XUIZ" && this.IsValid); }
        }

        public void Parse()
        {
            // Start at zero.
            this.IO.Position = StartPos;

            // Read Header.
            this.Header = new XUIZ.Header();
            this.Header.Magic = IO.ReadString(4);
            this.Header.Flags = IO.ReadUInt32(Hect0rs.IO.Endian.High);
            this.Header.FileEnd = IO.ReadUInt32(Hect0rs.IO.Endian.High);
            this.Header.Unknown1 = IO.ReadUInt32(Hect0rs.IO.Endian.High);
            this.Header.FileSystemHeaderLen = IO.ReadUInt32(Hect0rs.IO.Endian.High);
            this.Header.EmbeddedFileCount = IO.ReadUInt16(Hect0rs.IO.Endian.High);

            // Check is XUIZ.
            if (!IsXUIZ)
            {
                throw new Exception("File is not a Xbox User Interfaces Resource Package.");
            }

            // Read Embedded Content FS Entrys (8 + 1 + EntryNameLength)
            IO.Position = StartPos + 22;

            // Pointer.
            Xbox360.XUIZ.ContentEntry ent = null;

            // Reset FileSystemEntrys.
            this.Entrys = new List<XUIZ.ContentEntry>();
            this.EndOfHeader = (int)StartPos + 22;

            for (int i = 0; i < this.Header.EmbeddedFileCount; i++)
            {
                ent = new XUIZ.ContentEntry();
                ent.FileLength = IO.ReadUInt32(Hect0rs.IO.Endian.High);
                ent.Address = IO.ReadUInt32(Hect0rs.IO.Endian.High);
                byte len = IO.ReadByte();
                EndOfHeader += 9;
                if((this.Header.Flags & 0x2) == (UInt32)0x2)
                {
                    ent.Name = IO.ReadString(len);
                    EndOfHeader += ((int)StartPos + len);
                }
                else
                {
                    ent.Name = IO.ReadUnicodeStringFixedLength(len * 2, Hect0rs.IO.Endian.High);
                    EndOfHeader += (int)StartPos + (len * 2);
                }
                
                
                this.Entrys.Add(ent);
                ent = null;
            }

            // QuickCHeck to see if end of header is actually greater than our header suggests.
            if(this.EndOfHeader > this.Header.FileSystemHeaderLen)
            {
                this.Header.FileSystemHeaderLen = (UInt32)this.EndOfHeader;
            }
            return;
        }

        public void ExtractEntry(string OutputFile, XUIZ.ContentEntry Entry)
        {
            if (Entry == null) { throw new Exception("Cannot extract a empty or null XUIZContentEntry."); }

            this.IO.Position = this.StartPos + this.Header.FileSystemHeaderLen + Entry.Address;
            byte[] buf = IO.ReadBytes((int)Entry.FileLength);

            // Setup a output filewriter.
            Hect0rs.IO.IOH OIO = new Hect0rs.IO.IOH(OutputFile, System.IO.FileMode.Create);

            OIO.Position = 0;
            OIO.Write(buf);

            buf = null;
            OIO.close();
            OIO = null;
        }
        public byte[] ExtractEntry(XUIZ.ContentEntry Entry)
        {
            if (Entry == null) { throw new Exception("Cannot extract a empty or null XUIZContentEntry."); }

            this.IO.Position = this.StartPos + this.Header.FileSystemHeaderLen + Entry.Address;
            return  IO.ReadBytes((int)Entry.FileLength);
        }

        public string GetDebugSourceFile(XUIZ.ContentEntry Entry)
        {
            if((this.Header.Unknown1 & 1) == 0)
            {
                return "Unknown";
            }
            if (Entry == null) { throw new Exception("Cannot extract a empty or null XUIZContentEntry."); }

            this.IO.Position = this.StartPos + this.Header.FileSystemHeaderLen + Entry.Address + Entry.FileLength;
           
            IO.Position += 8;
            ushort len = IO.ReadUInt16(Hect0rs.IO.Endian.High);
            return IO.ReadString(len);
        }
    }
}

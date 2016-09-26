/*
 * Reversed by [ Hect0r ] @ www.staticpi.net
 * With thanks to : http://www.free60.org/wiki/XEX
 * For inital stuff :)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360.XEX
{
    // ESRB (Entertainment Software Rating Board)
    public enum XeRating_esrb : uint
    {
        eC = 0x00,
        E = 0x02,
        E10 = 0x04,
        T = 0x06,
        M = 0x08,
        AO = 0x0E,
        UNRATED = 0xFF,
    }
    // PEGI (Pan European Game Information)
    public enum XeRating_pegi : uint
    {
        ThreePLUS = 0,
        SevenPLUS = 4,
        TwelvePLUS = 9,
        SixteenPLUS = 13,
        EighteenPLUS = 14,
        UNRATED = 0xFF,
    }
    // PEGI (Pan European Game Information) - Finland
    public enum XeRating_pegi_fi : uint
    {
        ThreePLUS = 0,
        SevenPLUS = 4,
        ElevenPLUS = 8,
        FifteenPLUS = 12,
        EighteenPLUS = 14,
        UNRATED = 0xFF,
    }
    // PEGI (Pan European Game Information) - Portugal
    public enum XeRating_pegi_pt : uint
    {
        FourPLUS = 1,
        SixPLUS = 3,
        TwelvePLUS = 9,
        SixteenPLUS = 13,
        EighteenPLUS = 14,
        UNRATED = 0xFF,
    }
    // BBFC (British Board of Film Classification) - UK/Ireland
    public enum XeRating_bbfc : uint
    {
        UNIVERSAL = 1,
        PG = 5,
        ThreePLUS = 0,
        SevenPLUS = 4,
        TwelvePLUS = 9,
        FifteenPLUS = 12,
        SixteenPLUS = 13,
        EighteenPLUS = 14,
        UNRATED = 0xFF,
    }
    // CERO (Computer Entertainment Rating Organization)
    public enum XeRating_cero : uint
    {
        A = 0,
        B = 2,
        C = 4,
        D = 6,
        Z = 8,
        UNRATED = 0xFF,
    }
    // USK (Unterhaltungssoftware SelbstKontrolle)
    public enum XeRating_usk : uint
    {
        ALL = 0,
        SixPLUS = 2,
        TwelvePLUS = 4,
        SixteenPLUS = 6,
        EighteenPLUS = 8,
        UNRATED = 0xFF,
    }
    // OFLC (Office of Film and Literature Classification) - Australia
    public enum XeRating_oflc_au : uint
    {
        G = 0,
        PG = 2,
        M = 4,
        MA15_PLUS = 6,
        UNRATED = 0xFF,
    }
    // OFLC (Office of Film and Literature Classification) - New Zealand
    public enum XeRating_oflc_nz : uint
    {
        G = 0,
        PG = 2,
        M = 4,
        MA15_PLUS = 6,
        UNRATED = 0xFF,
    }
    // KMRB (Korea Media Rating Board)
    public enum XeRating_kmrb : uint
    {
        ALL = 0,
        TwelvePLUS = 2,
        FifteenPLUS = 4,
        EighteenPLUS = 6,
        UNRATED = 0xFF,
    }
    // Brazil
    public enum XeRating_brazil : uint
    {
        ALL = 0,
        TwelvePLUS = 2,
        ForteenPLUS = 4,
        SixteenPLUS = 5,
        EighteenPLUS = 8,
        Unknown = 96,
        UNRATED = 0xFF,
    }
    // FPB (Film and Publication Board)
    public enum XeRating_fpb : uint
    {
        ALL = 0,
        PG = 6,
        TenPLUS = 7,
        ThirteenPLUS = 10,
        SixteenPLUS = 13,
        EighteenPLUS = 14,
        UNRATED = 0xFF,
    }
    public struct XeGame_Ratings
    {
        public XeRating_esrb esrb;
        public XeRating_pegi pegi;
        public XeRating_pegi_fi pegifi;
        public XeRating_pegi_pt pegipt;
        public XeRating_bbfc bbfc;
        public XeRating_cero cero;
        public XeRating_usk usk;
        public XeRating_oflc_au oflcau;
        public XeRating_oflc_nz oflcnz;
        public XeRating_kmrb kmrb;
        public XeRating_brazil brazil;
        public XeRating_fpb fpb;
        public byte[] reserved;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbox360.STFS
{
    public enum XeContentType : Int32
    {
        ArcadeTitle = 0xD0000,
        AvatarItem = 0x9000,
        CacheFile = 0x40000,
        CommunityGame = 0x2000000,
        GameDemo = 0x80000,
        GamerPicture = 0x20000,
        GameTitle = 0xA0000,
        GameTrailer = 0xC0000,
        GameVideo = 0x400000,
        InstalledGame = 0x4000,
        Installer = 0xB0000,
        IPTVPauseBuffer = 0x2000,
        LicenseStore = 0xF0000,
        MarketplaceContent = 0x2,
        Movie = 0x100000,
        MusicVideo = 0x300000,
        PodcastVideo = 0x500000,
        Profile = 0x10000,
        Publisher = 0x3,
        SavedGame = 0x1,
        StorageDownload = 0x50000,
        Theme = 0x30000,
        TV = 0x200000,
        Video = 0x90000,
        ViralVideo = 0x600000,
        XboxDownload = 0x70000,
        XboxOriginalGame = 0x5000,
        XboxSavedGame = 0x60000,
        Xbox360Title = 0x1000,
        XboxTitle = 0x5000,
        XNA = 0xE0000
    }
}

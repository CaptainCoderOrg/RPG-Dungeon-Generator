using System.IO;
namespace CaptainCoder.Dungeoneering
{

    public static class Corridors
    {
        static Corridors()
        {
            s_Path = "Assets/Resources/Grid/Corridors";
            s_Straight = File.ReadAllText($"{s_Path}/Straight.txt");
            s_CornerTL = File.ReadAllText($"{s_Path}/Corner.txt");
            s_CornerTR = s_CornerTL.Rotate90();
            s_CornerBR = s_CornerTR.Rotate90();
            s_CornerBL = s_CornerBR.Rotate90();
            s_TeeSouth = File.ReadAllText($"{s_Path}/Tee.txt");
            s_TeeWest = s_TeeSouth.Rotate90();
            s_TeeNorth = s_TeeWest.Rotate90();
            s_TeeEast = s_TeeNorth.Rotate90();
        }
        private readonly static string s_Path;
        private readonly static string s_Straight;
        private readonly static string s_CornerTL, s_CornerTR, s_CornerBR, s_CornerBL;
        private readonly static string s_TeeSouth, s_TeeWest, s_TeeNorth, s_TeeEast;
        private readonly static MapBuilderReader s_Reader = new();
        public static MapBuilder EastWest => s_Reader.FromString(s_Straight);
        public static MapBuilder NorthSouth => s_Reader.FromString(s_Straight.Rotate90());
        public static MapBuilder Cross => s_Reader.FromFile($"{s_Path}/Cross.txt");
        public static MapBuilder CornerTL => s_Reader.FromString(s_CornerTL);
        public static MapBuilder CornerTR => s_Reader.FromString(s_CornerTR);
        public static MapBuilder CornerBR => s_Reader.FromString(s_CornerBR);
        public static MapBuilder CornerBL => s_Reader.FromString(s_CornerBL);
        public static MapBuilder TeeSouth => s_Reader.FromString(s_TeeSouth);
        public static MapBuilder TeeNorth => s_Reader.FromString(s_TeeNorth);
        public static MapBuilder TeeEast => s_Reader.FromString(s_TeeEast);
        public static MapBuilder TeeWest => s_Reader.FromString(s_TeeWest);
        public static MapBuilder[] All => new[] {
            Cross,
            EastWest,
            NorthSouth,
            CornerTL,
            CornerTR,
            CornerBR,
            CornerBL,
            TeeNorth,
            TeeEast,
            TeeSouth,
            TeeWest
        };
    }
}
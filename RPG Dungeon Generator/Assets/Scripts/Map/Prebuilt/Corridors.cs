
namespace CaptainCoder.Dungeoneering
{

    public static class Corridors
    {
        static Corridors()
        {
            string[] mapData = {
                "+-----+",
                "$. . .$",
                "+-----+",
            };
            s_EastWest = string.Join("\n", mapData);

            mapData = new string[]{
                  "+$+",
                  "|.|",
                  "| |",
                  "|.|",
                  "| |",
                  "|.|",
                  "+$+",
            };
            s_NorthSouth = string.Join("\n", mapData);
            mapData = new string[]{
                "  +$+  ",
                "  |.|  ",
                "--+ +--",
                "$. . .$",
                "--+ +--",
                "  |.|  ",
                "  +$+  ",
            };
            s_Cross = string.Join("\n", mapData);

            mapData = new string[]{
                "+---+",
                "|. .$",
                "| +-+",
                "|.|  ",
                "+$+  ",
            };
            s_Corner = string.Join("\n", mapData);
        }

        private static MapBuilderReader s_Reader = new();
        private readonly static string s_EastWest;
        private readonly static string s_NorthSouth;
        private readonly static string s_Cross;
        private readonly static string s_Corner;        
        // +-----+
        // $. . .$
        // +-----+    
        public static MapBuilder EastWest => s_Reader.FromString(s_EastWest);

        // +$+
        // |.|
        // | |
        // |.|
        // | |
        // |.|
        // +$+
        public static MapBuilder NorthSouth => s_Reader.FromString(s_NorthSouth);

        //   +$+
        //   |.|
        // --+ +--
        // $. . .$
        // --+ +--
        //   |.|
        //   +$+
        public static MapBuilder Cross => s_Reader.FromString(s_Cross);

        // +---+
        // |. .$
        // | +-+
        // |.|
        // +$+
        public static MapBuilder Corner => s_Reader.FromString(s_Corner);

        public static MapBuilder[] All => new[] { Cross, Corner, EastWest, NorthSouth };

    }

}
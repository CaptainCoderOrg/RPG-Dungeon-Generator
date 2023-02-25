
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
            
        }
        private readonly static string s_Path = "Assets/Resources/Grid/Corridors";
        private readonly static MapBuilderReader s_Reader = new();
        private readonly static string s_EastWest;
        private readonly static string s_NorthSouth;      
        // +-----+
        // $. . .$
        // +-----+    
        public static MapBuilder EastWest => s_Reader.FromString(s_EastWest);
        public static MapBuilder Straight => s_Reader.FromFile($"{s_Path}/Straight.txt");

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
        public static MapBuilder Cross => s_Reader.FromFile($"{s_Path}/Cross.txt");

        // +---+
        // |. .$
        // | +-+
        // |.|
        // +$+
        public static MapBuilder Corner => s_Reader.FromFile($"{s_Path}/Corner.txt");
        public static MapBuilder Tee => s_Reader.FromFile($"{s_Path}/Tee.txt");

        public static MapBuilder[] All => new[] { Cross, Straight, Tee, Corner };
    }
}
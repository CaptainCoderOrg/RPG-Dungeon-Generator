
namespace CaptainCoder.Dungeoneering
{
    public static class Rooms
    {
        static Rooms()
        {
            string[] mapData = {
                "+$--+",
                "$. .|",
                "|   |",
                "|. .$",
                "+--$+"
            };
            s_Room2x2 = string.Join("\n", mapData);

            mapData = new string[]{
                "+--$----+",
                "|. . . .|",
                "         ",
                "$. . . .|",
                "         ",
                "|. . . .$",
                "         ",
                "|. . . .|",
                "+----$--+",
            };
            s_Room4x4 = string.Join("\n", mapData);
            mapData = new string[]{
                "+----$-----$----+",
                "|. . . . . . . .|",
                "                 ",
                "$. . . . . . . .|",
                "                 ",
                "|. . . . . . . .$",
                "                 ",
                "|. . . . . . . .|",
                "+----$-----$----+",
            };
            s_Room8x4 = string.Join("\n", mapData);
        }

        private static string s_Path = "Assets/Resources/Grid/Rooms";
        private static MapBuilderReader s_Reader = new();
        private readonly static string s_Room2x2;
        private readonly static string s_Room4x4;
        private readonly static string s_Room8x4;          
        public static MapBuilder Room2x2 => s_Reader.FromString(s_Room2x2);
        public static MapBuilder Room4x4 => s_Reader.FromString(s_Room4x4);
        public static MapBuilder Room8x4 => s_Reader.FromString(s_Room8x4);
        public static MapBuilder Pillar3x3 => s_Reader.FromFile($"{s_Path}/3x3WithPillar.txt");
        public static MapBuilder URoom => s_Reader.FromFile($"{s_Path}/URoom.txt");
        public static MapBuilder[] All => new[] { Room2x2, Room4x4, Room8x4, Pillar3x3, URoom };
    }
}
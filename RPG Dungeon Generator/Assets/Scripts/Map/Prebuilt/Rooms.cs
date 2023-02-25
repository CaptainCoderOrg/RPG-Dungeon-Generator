
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

        private static MapBuilderReader s_Reader = new();
        private readonly static string s_Room2x2;
        private readonly static string s_Room4x4;
        private readonly static string s_Room8x4;          
        public static MapBuilder Room2x2 => s_Reader.FromString(s_Room2x2);
        public static MapBuilder Room4x4 => s_Reader.FromString(s_Room4x4);
        public static MapBuilder Room8x4 => s_Reader.FromString(s_Room8x4);
        public static MapBuilder[] All => new[] { Room2x2, Room4x4, Room8x4 };
    }
}
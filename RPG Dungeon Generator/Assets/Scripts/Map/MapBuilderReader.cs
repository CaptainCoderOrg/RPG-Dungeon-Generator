using System.Collections.Generic;
using System.Linq;

namespace CaptainCoder.Dungeoneering
{

    public class MapBuilderReader
    {

        private readonly HashSet<char> _wallChars = "|-+".ToHashSet();
        private readonly HashSet<char> _connectionChars = "$".ToHashSet();
        private readonly HashSet<char> _floorChars = ".".ToHashSet();

        public MapBuilderReader() : this("|-+", "$", ".") { }

        public MapBuilderReader(string walls, string connections, string floors)
        {
            _wallChars = walls.ToHashSet();
            _connectionChars = connections.ToHashSet();
            _floorChars = floors.ToHashSet();
        }

        public MapBuilder FromString(string data)
        {
            string[] mapData = data.Split("\n");
            MapBuilder builder = new();
            for (int r = 0; r < (mapData.Length - 1) / 2; r++)
            {
                string row = mapData[r * 2 + 1];
                for (int c = 0; c < (row.Length - 1) / 2; c++)
                {
                    char ch = LookupFloor(mapData, r, c);
                    if (ch == ' ') { continue; }
                    // TODO: Actually parse in the floor character
                    if (!_floorChars.Contains(ch)) { throw new System.ArgumentException($"Found illegal floor character at ({r}, {c}): '{ch}'"); }
                    Position pos = new(c, r);
                    builder.AddFloor(pos);
                    AddWallsAndConnectionPoints(pos, mapData, builder);
                }
            }
            return builder;
        }

        //   0 1 2 3 
        //  +----$--+
        //0 |. . . .|
        //  |       |
        //1 |. . . .$
        //  |       |
        //2 $. . . .|
        //  |       |
        //3 |. . . .|
        //  +--$----+
        // 0, 0 => 0, -1
        private static char LookupFloor(string[] mapData, int r, int c) => mapData[r * 2 + 1][c * 2 + 1];
        private static char LookupWall(string[] mapData, Position p, Facing facing)
        {
            int r = (p.Row * 2 + 1);
            int c = (p.Col * 2 + 1);
            (int offR, int offC) = facing switch
            {
                Facing.North => (-1, 0),
                Facing.South => (1, 0),
                Facing.East => (0, 1),
                Facing.West => (0, -1),
                _ => throw new System.ArgumentException($"Invalid facing {facing}"),
            };

            return mapData[r + offR][c + offC];
        }

        private void AddWallsAndConnectionPoints(Position pos, string[] mapData, MapBuilder builder)
        {
            foreach (Facing facing in FacingExtensions.All)
            {
                char neighborCh = LookupWall(mapData, pos, facing); //mapData[ToASCIIPosition(neighbor.Row)][ToASCIIPosition(neighbor.Col)];
                if (_wallChars.Contains(neighborCh))
                {
                    builder.AddWall(pos, facing);
                }
                else if (_connectionChars.Contains(neighborCh))
                {
                    builder.AddConnectionPoint(new ConnectionPoint(pos, facing));
                }
            }
        }

    }

}
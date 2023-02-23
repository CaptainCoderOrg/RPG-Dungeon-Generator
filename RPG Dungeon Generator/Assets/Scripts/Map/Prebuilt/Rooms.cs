
namespace CaptainCoder.Dungeoneering
{
    public static class Rooms
    {
            /*
             +$--+
             $. .|
             |   |
             |. .$
             +--$+
            */
            public static MapBuilder Room2x2 => new MapBuilder()
                .AddFloor(0, 0)
                .AddFloor(0, 1)
                .AddFloor(1, 0)
                .AddFloor(1, 1)
                .AddWalls(new Position(0, 0), Facing.North, Facing.West)
                .AddWalls(new Position(1, 0), Facing.North, Facing.East)
                .AddWalls(new Position(1, 1), Facing.South)
                .AddWalls(new Position(0, 1), Facing.South, Facing.West)
                .AddConnectionPoint(new ConnectionPoint(new Position(0, 0), Facing.West))
                .AddConnectionPoint(new ConnectionPoint(new Position(0, 0), Facing.North))
                .AddConnectionPoint(new ConnectionPoint(new Position(1, 1), Facing.East))                
                .AddConnectionPoint(new ConnectionPoint(new Position(1, 1), Facing.South))
                ;
    }
}
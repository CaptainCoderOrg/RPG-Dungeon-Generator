
namespace CaptainCoder.Dungeoneering
{
    public static class Rooms
    {

        //  +$--+
        //  $. .|
        //  |   |
        //  |. .$
        //  +--$+            
        public static MapBuilder Room2x2 => new MapBuilder()
            .AddFloor(0, 0)
            .AddFloor(0, 1)
            .AddFloor(1, 0)
            .AddFloor(1, 1)
            .AddWalls(new Position(1, 0), Facing.North, Facing.East)
            .AddWalls(new Position(0, 1), Facing.South, Facing.West)
            .AddConnectionPoint(new ConnectionPoint(new Position(0, 0), Facing.West))
            .AddConnectionPoint(new ConnectionPoint(new Position(0, 0), Facing.North))
            .AddConnectionPoint(new ConnectionPoint(new Position(1, 1), Facing.East))
            .AddConnectionPoint(new ConnectionPoint(new Position(1, 1), Facing.South))
            ;

        /*
       +--$----+ 
       |. . . .|

       $. . . .|

       |. . . .$

       |. . . .|
        ----$--+
        */

        public static MapBuilder Room4x4 => new MapBuilder()
            .AddFloor(0, 0).AddFloor(0, 1).AddFloor(0, 2).AddFloor(0, 3)
            .AddFloor(1, 0).AddFloor(1, 1).AddFloor(1, 2).AddFloor(1, 3)
            .AddFloor(2, 0).AddFloor(2, 1).AddFloor(2, 2).AddFloor(2, 3)
            .AddFloor(3, 0).AddFloor(3, 1).AddFloor(3, 2).AddFloor(3, 3)
            .AddWalls(new Position(0, 0), Facing.North, Facing.West)
            .AddWalls(new Position(2, 0), Facing.North)
            .AddWalls(new Position(3, 0), Facing.North, Facing.East)
            .AddWalls(new Position(3, 1), Facing.East)
            .AddWalls(new Position(3, 3), Facing.East, Facing.South)
            .AddWalls(new Position(1, 3), Facing.South)
            .AddWalls(new Position(0, 3), Facing.South, Facing.West)
            .AddWalls(new Position(0, 2), Facing.West)
            .AddConnectionPoint(new ConnectionPoint(new Position(1, 0), Facing.North))
            .AddConnectionPoint(new ConnectionPoint(new Position(0, 1), Facing.West))
            .AddConnectionPoint(new ConnectionPoint(new Position(3, 2), Facing.East))
            .AddConnectionPoint(new ConnectionPoint(new Position(2, 3), Facing.South))
            ;

        /*
       +----$-----$----+ 
       |. . . . . . . .|

       $. . . . . . . .|

       |. . . . . . . .$

       |. . . . . . . .|
        ----$-----$----+
        */

        public static MapBuilder Room8x4 => new MapBuilder()
            .AddFloor(0, 0).AddFloor(1, 0).AddFloor(2, 0).AddFloor(3, 0).AddFloor(4, 0).AddFloor(5, 0).AddFloor(6, 0).AddFloor(7, 0)
            .AddFloor(0, 1).AddFloor(1, 1).AddFloor(2, 1).AddFloor(3, 1).AddFloor(4, 1).AddFloor(5, 1).AddFloor(6, 1).AddFloor(7, 1)
            .AddFloor(0, 2).AddFloor(1, 2).AddFloor(2, 2).AddFloor(3, 2).AddFloor(4, 2).AddFloor(5, 2).AddFloor(6, 2).AddFloor(7, 2)
            .AddFloor(0, 3).AddFloor(1, 3).AddFloor(2, 3).AddFloor(3, 3).AddFloor(4, 3).AddFloor(5, 3).AddFloor(6, 3).AddFloor(7, 3)
            .AddWalls(new Position(0, 0), Facing.North, Facing.West)
            .AddWalls(new Position(1, 0), Facing.North)
            .AddWalls(new Position(3, 0), Facing.North)
            .AddWalls(new Position(4, 0), Facing.North)
            .AddWalls(new Position(6, 0), Facing.North)
            .AddWalls(new Position(7, 0), Facing.North, Facing.East)

            .AddWalls(new Position(7, 1), Facing.East)
            .AddWalls(new Position(0, 2), Facing.West)
            
            .AddWalls(new Position(0, 3), Facing.South, Facing.West)
            .AddWalls(new Position(1, 3), Facing.South)
            .AddWalls(new Position(3, 3), Facing.South)
            .AddWalls(new Position(4, 3), Facing.South)
            .AddWalls(new Position(6, 3), Facing.South)
            .AddWalls(new Position(7, 3), Facing.South, Facing.East)

            .AddConnectionPoint(new ConnectionPoint(new Position(2, 0), Facing.North))
            .AddConnectionPoint(new ConnectionPoint(new Position(5, 0), Facing.North))
            .AddConnectionPoint(new ConnectionPoint(new Position(0, 1), Facing.West))
            .AddConnectionPoint(new ConnectionPoint(new Position(7, 2), Facing.East))
            .AddConnectionPoint(new ConnectionPoint(new Position(2, 3), Facing.South))
            .AddConnectionPoint(new ConnectionPoint(new Position(5, 3), Facing.South));

        public static MapBuilder[] All => new[] { Room2x2, Room4x4, Room8x4 };
    }
}
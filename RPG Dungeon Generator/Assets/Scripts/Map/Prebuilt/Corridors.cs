
namespace CaptainCoder.Dungeoneering
{

    public static class Corridors
    {
        
        // +-----+
        // $. . .$
        // +-----+    
        public static MapBuilder EastWest => new MapBuilder()
            .AddFloor(0, 0)
            .AddWalls(new Position(0, 0), Facing.North, Facing.South)
            .AddFloor(1, 0)
            .AddWalls(new Position(1, 0), Facing.North, Facing.South)
            .AddFloor(2, 0)
            .AddWalls(new Position(2, 0), Facing.North, Facing.South)
            .AddConnectionPoint(new ConnectionPoint(new Position(0, 0), Facing.West))
            .AddConnectionPoint(new ConnectionPoint(new Position(2, 0), Facing.East));

        // +$+
        // |.|
        // | |
        // |.|
        // | |
        // |.|
        // +$+
        public static MapBuilder NorthSouth => new MapBuilder()
            .AddFloor(0, 0)
            .AddWalls(new Position(0, 0), Facing.East, Facing.West)
            .AddFloor(0, 1)
            .AddWalls(new Position(1, 0), Facing.East, Facing.West)
            .AddFloor(0, 2)
            .AddWalls(new Position(2, 0), Facing.East, Facing.West)
            .AddConnectionPoint(new ConnectionPoint(new Position(0, 0), Facing.North))
            .AddConnectionPoint(new ConnectionPoint(new Position(0, 2), Facing.South));
    }

}
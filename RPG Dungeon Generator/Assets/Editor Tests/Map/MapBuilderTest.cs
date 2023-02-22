using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

namespace CaptainCoder.Dungeoneering
{
    public class MapBuilderTest
    {
        [Test, Timeout(5000)]
        public void MapBuilderTestMergeAtWithStraightHall()
        {
            MapBuilder roomBuilder = new();

            /*
             +---+
             |. .|
             |   |
             |. .$
             +---+
            */
            roomBuilder
                .AddFloor(0, 0)
                .AddFloor(0, 1)
                .AddFloor(1, 0)
                .AddFloor(1, 1)
                .AddWalls(new Position(0, 0), Facing.North, Facing.West)
                .AddWalls(new Position(1, 0), Facing.North, Facing.East)
                .AddWalls(new Position(1, 1), Facing.South)
                .AddWalls(new Position(0, 1), Facing.South, Facing.West);

            ConnectionPoint roomConnection = new ConnectionPoint(new Position(1, 1), Facing.East);

            /*
            +-----+
            $. . .
            +-----+
            */
            MapBuilder corridorBuilder = new();
            corridorBuilder
                .AddFloor(0, 0)
                .AddWalls(new Position(0, 0), Facing.North, Facing.South)
                .AddFloor(1, 0)
                .AddWalls(new Position(1, 0), Facing.North, Facing.South)
                .AddFloor(2, 0)
                .AddWalls(new Position(2, 0), Facing.North, Facing.South);

            ConnectionPoint corridorConnection = new ConnectionPoint(new Position(0, 0), Facing.West);
            roomBuilder.MergeAt(roomConnection, corridorBuilder, corridorConnection);

            /*
             +---+
             |. .|
             |   +-----+
             |. . . . .
             +---------+
             */

            Assert.AreEqual(0, roomBuilder.UnconnectedPoints.Count);
            IMap builtRoom = roomBuilder.Build();
            Assert.AreEqual(7, builtRoom.Tiles.Count());
            

            Dictionary<Position, HashSet<Facing>> expected = new();
            expected[new(0, 0)] = new() { Facing.North, Facing.West };
            expected[new(1, 0)] = new() { Facing.North, Facing.East };
            expected[new(0, 1)] = new() { Facing.South, Facing.West };
            expected[new(1, 1)] = new() { Facing.South };
            expected[new(2, 1)] = new() { Facing.North, Facing.South };
            expected[new(3, 1)] = new() { Facing.North, Facing.South };
            expected[new(4, 1)] = new() { Facing.North, Facing.South };

            foreach ((Position p, HashSet<Facing> walls) in expected)
            {
                Assert.True(builtRoom.TileAt(p).IsPassable);
                Assert.AreEqual(walls, builtRoom.TileAt(p).Walls);
            }

        }

        [Test, Timeout(5000)]
        public void MapBuilderTest2x2Room()
        {
            MapBuilder mapBuilder = new MapBuilder();
            mapBuilder
                    .AddFloor(0, 0)
                    .AddFloor(0, 1)
                    .AddFloor(1, 0)
                    .AddFloor(1, 1)
                    .AddWalls(new Position(0, 0), Facing.North, Facing.West)
                    .AddWalls(new Position(1, 0), Facing.North, Facing.East)
                    .AddWalls(new Position(1, 1), Facing.South, Facing.East)
                    .AddWalls(new Position(0, 1), Facing.South, Facing.West);

            IMap map = mapBuilder.Build();
            ITile topLeft = map.TileAt(new Position(0, 0));
            Assert.True(topLeft.IsPassable);
            Assert.True(topLeft.Walls.Contains(Facing.North));
            Assert.True(topLeft.Walls.Contains(Facing.West));
            Assert.False(topLeft.Walls.Contains(Facing.East));
            Assert.False(topLeft.Walls.Contains(Facing.South));

            ITile topRight = map.TileAt(new Position(1, 0));
            Assert.True(topRight.IsPassable);
            Assert.True(topRight.Walls.Contains(Facing.North));
            Assert.True(topRight.Walls.Contains(Facing.East));
            Assert.False(topRight.Walls.Contains(Facing.South));
            Assert.False(topRight.Walls.Contains(Facing.West));

            ITile bottomRight = map.TileAt(new Position(1, 1));
            Assert.True(bottomRight.IsPassable);
            Assert.True(bottomRight.Walls.Contains(Facing.South));
            Assert.True(bottomRight.Walls.Contains(Facing.East));
            Assert.False(bottomRight.Walls.Contains(Facing.North));
            Assert.False(bottomRight.Walls.Contains(Facing.West));

            ITile bottomLeft = map.TileAt(new Position(0, 1));
            Assert.True(bottomLeft.IsPassable);
            Assert.True(bottomLeft.Walls.Contains(Facing.South));
            Assert.True(bottomLeft.Walls.Contains(Facing.West));
            Assert.False(bottomLeft.Walls.Contains(Facing.North));
            Assert.False(bottomLeft.Walls.Contains(Facing.East));

            ITile outOfBounds = map.TileAt(new Position(-1, 0));
            Assert.False(outOfBounds.IsPassable);
            Assert.True(outOfBounds.Walls.Contains(Facing.East));

            (Position tl, Position br) = map.TileBounds;
            Assert.AreEqual(tl, new Position(0, 0));
            Assert.AreEqual(br, new Position(1, 1));
        }
    }
}
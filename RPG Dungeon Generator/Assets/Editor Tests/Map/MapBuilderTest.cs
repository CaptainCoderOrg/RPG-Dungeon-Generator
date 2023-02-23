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

        [Test, Timeout(5000)]
        public void MapBuilderTestMergeAndBuildAtShortHall()
        {
            MapBuilder roomBuilder = new();

            /*
             +---+
             |. .|
             |   |
             |. .$
             +---+
            */
            ConnectionPoint roomConnection = new ConnectionPoint(new Position(1, 1), Facing.East);
            roomBuilder
                .AddFloor(0, 0)
                .AddFloor(0, 1)
                .AddFloor(1, 0)
                .AddFloor(1, 1)
                .AddWalls(new Position(0, 0), Facing.North, Facing.West)
                .AddWalls(new Position(1, 0), Facing.North, Facing.East)
                .AddWalls(new Position(1, 1), Facing.South)
                .AddWalls(new Position(0, 1), Facing.South, Facing.West)
                .AddConnectionPoint(roomConnection);
            Assert.AreEqual(1, roomBuilder.UnconnectedPoints.Count);


            /*
            +-----+
            $. . .$
            +-----+
            */
            MapBuilder corridorBuilder = new();
            ConnectionPoint corridorConnection = new(new Position(0, 0), Facing.West);
            ConnectionPoint corridorConnection2 = new(new Position(2, 0), Facing.East);
            corridorBuilder
                .AddFloor(0, 0)
                .AddWalls(new Position(0, 0), Facing.North, Facing.South)
                .AddFloor(1, 0)
                .AddWalls(new Position(1, 0), Facing.North, Facing.South)
                .AddFloor(2, 0)
                .AddWalls(new Position(2, 0), Facing.North, Facing.South)
                .AddConnectionPoint(corridorConnection)
                .AddConnectionPoint(corridorConnection2);
            Assert.AreEqual(2, corridorBuilder.UnconnectedPoints.Count);

            roomBuilder.MergeAt(roomConnection, corridorBuilder, corridorConnection);
            Assert.AreEqual(1, roomBuilder.UnconnectedPoints.Count);
            Assert.AreEqual(new ConnectionPoint(new Position(4, 1), Facing.East), roomBuilder.UnconnectedPoints[0]);

            /*
             +---+
             |. .|
             |   +-----+
             |. . . . .$
             +---------+
            */
            IMap map = roomBuilder.Build();

            string actual = map.ToASCII();
            string[] rows = {
                " - -       ",
                "|. .|      ",
                "     - - - ",
                "|. . . . .|",
                " - - - - - "
            };
            string expected = string.Join("\n", rows);
            Assert.AreEqual(expected, actual);
        }

        [Test, Timeout(5000)]
        public void MapBuilderTestMergeAndBuildWith2xShortHall()
        {
            MapBuilder roomBuilder = new();

            /*
             +---+
             |. .|
             |   |
             |. .$
             +---+
            */
            ConnectionPoint roomConnection = new ConnectionPoint(new Position(1, 1), Facing.East);
            roomBuilder
                .AddFloor(0, 0)
                .AddFloor(0, 1)
                .AddFloor(1, 0)
                .AddFloor(1, 1)
                .AddWalls(new Position(0, 0), Facing.North, Facing.West)
                .AddWalls(new Position(1, 0), Facing.North, Facing.East)
                .AddWalls(new Position(1, 1), Facing.South)
                .AddWalls(new Position(0, 1), Facing.South, Facing.West)
                .AddConnectionPoint(roomConnection);
            Assert.AreEqual(1, roomBuilder.UnconnectedPoints.Count);


            /*
            +-----+
            $. . .$
            +-----+
            */
            MapBuilder corridorBuilder = new();
            ConnectionPoint corridorConnection = new(new Position(0, 0), Facing.West);
            ConnectionPoint corridorConnection2 = new(new Position(2, 0), Facing.East);
            corridorBuilder
                .AddFloor(0, 0)
                .AddWalls(new Position(0, 0), Facing.North, Facing.South)
                .AddFloor(1, 0)
                .AddWalls(new Position(1, 0), Facing.North, Facing.South)
                .AddFloor(2, 0)
                .AddWalls(new Position(2, 0), Facing.North, Facing.South)
                .AddConnectionPoint(corridorConnection)
                .AddConnectionPoint(corridorConnection2);
            Assert.AreEqual(2, corridorBuilder.UnconnectedPoints.Count);

            /*
             +---+
             |. .|
             |   +-----+
             |. . . . .$
             +---------+
            */
            roomBuilder.MergeAt(roomConnection, corridorBuilder, corridorConnection);
            Assert.AreEqual(1, roomBuilder.UnconnectedPoints.Count);
            Assert.AreEqual(new ConnectionPoint(new Position(4, 1), Facing.East), roomBuilder.UnconnectedPoints[0]);

            /*
             +---+
             |. .|
             |   +-----------+
             |. . . . . . . .$
             +---------------+
            */
            roomBuilder.MergeAt(new ConnectionPoint(new Position(4, 1), Facing.East), corridorBuilder, corridorConnection);
            Assert.AreEqual(1, roomBuilder.UnconnectedPoints.Count);
            Assert.AreEqual(new ConnectionPoint(new Position(7, 1), Facing.East), roomBuilder.UnconnectedPoints[0]);


            /*
             +---+
             |. .|
             |   +-----------+
             |. . . . . . . .|
             +---------------+
            */
            IMap map = roomBuilder.Build();

            string actual = map.ToASCII();
            string[] rows = {
                " - -             ",
                "|. .|            ",
                "     - - - - - - ",
                "|. . . . . . . .|",
                " - - - - - - - - "
            };
            string expected = string.Join("\n", rows);
            Assert.AreEqual(expected, actual);
        }

        [Test, Timeout(5000)]
        public void MapBuilderTestBuildWithShortHall()
        {
            MapBuilder roomBuilder = new();

            /*
             +---+
             |. .|
             |   |- - -
             |. . . . .$
             +---+- - -
            */
            ConnectionPoint roomConnection = new(new Position(4, 1), Facing.East);
            roomBuilder
                .AddFloor(0, 0)
                .AddFloor(1, 0)
                .AddFloor(0, 1)
                .AddFloor(1, 1)
                .AddFloor(2, 1)
                .AddFloor(3, 1)
                .AddFloor(4, 1)
                .AddWalls(new Position(0, 0), Facing.North, Facing.West)
                .AddWalls(new Position(1, 0), Facing.North, Facing.East)
                .AddWalls(new Position(1, 1), Facing.South)
                .AddWalls(new Position(0, 1), Facing.South, Facing.West)
                .AddWalls(new Position(1, 1), Facing.South)
                .AddWalls(new Position(2, 1), Facing.South, Facing.North)
                .AddWalls(new Position(3, 1), Facing.South, Facing.North)
                .AddWalls(new Position(4, 1), Facing.South, Facing.North, Facing.East)
                .AddConnectionPoint(roomConnection);



            /*
             +---+
             |. .|
             |   +-----+
             |. . . . .|
             +---------+
            */
            IMap map = roomBuilder.Build();

            string actual = map.ToASCII();
            string[] rows = {
                " - -       ",
                "|. .|      ",
                "     - - - ",
                "|. . . . .|",
                " - - - - - "
            };
            string expected = string.Join("\n", rows);
            Assert.AreEqual(expected, actual);
        }

        [Test, Timeout(5000)]
        public void TestCanMergeAt()
        {
            /*
             +$--+
             $. .|
             |   |
             |. .$
             +--$+
            */
            MapBuilder roomBuilder = Rooms.Room2x2;

            ConnectionPoint north = new(new Position(0, 0), Facing.North);
            ConnectionPoint west = new(new Position(0, 0), Facing.West);
            ConnectionPoint south = new(new Position(1, 1), Facing.South);
            ConnectionPoint east = new(new Position(1, 1), Facing.East);

            // +-----+
            // $. . .$
            // +-----+   
            ConnectionPoint corridorWest = new(new Position(0, 0), Facing.West);
            ConnectionPoint corridorEast = new(new Position(2, 0), Facing.East);

            Assert.False(roomBuilder.CanMergeAt(north, Corridors.EastWest, corridorEast));
            Assert.False(roomBuilder.CanMergeAt(north, Corridors.EastWest, corridorWest));
            Assert.False(roomBuilder.CanMergeAt(south, Corridors.EastWest, corridorEast));
            Assert.False(roomBuilder.CanMergeAt(south, Corridors.EastWest, corridorWest));

            Assert.True(roomBuilder.CanMergeAt(east, Corridors.EastWest, corridorWest));
            Assert.False(roomBuilder.CanMergeAt(east, Corridors.EastWest, corridorEast));

            Assert.True(roomBuilder.CanMergeAt(west, Corridors.EastWest, corridorEast));
            Assert.False(roomBuilder.CanMergeAt(west, Corridors.EastWest, corridorWest));

            // +$+
            // |.|
            // | |
            // |.|
            // | |
            // |.|
            // +$+ 
            ConnectionPoint corridorNorth = new(new Position(0, 0), Facing.North);
            ConnectionPoint corridorSouth = new(new Position(0, 2), Facing.South);

            Assert.True(roomBuilder.CanMergeAt(north, Corridors.NorthSouth, corridorSouth));
            Assert.False(roomBuilder.CanMergeAt(north, Corridors.NorthSouth, corridorNorth));
            Assert.True(roomBuilder.CanMergeAt(south, Corridors.NorthSouth, corridorNorth));
            Assert.False(roomBuilder.CanMergeAt(south, Corridors.NorthSouth, corridorSouth));

            Assert.False(roomBuilder.CanMergeAt(east, Corridors.NorthSouth, corridorNorth));
            Assert.False(roomBuilder.CanMergeAt(east, Corridors.NorthSouth, corridorSouth));
            Assert.False(roomBuilder.CanMergeAt(west, Corridors.NorthSouth, corridorNorth));
            Assert.False(roomBuilder.CanMergeAt(west, Corridors.NorthSouth, corridorSouth));
        }
    }
}
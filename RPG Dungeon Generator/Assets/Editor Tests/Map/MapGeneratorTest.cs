using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

namespace CaptainCoder.Dungeoneering
{
    public class MapGeneratorTest
    {
        [Test, Timeout(5000)]
        public void MapGeneratorTestSimpleGenerate()
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
                .AddWalls(new Position(0, 1), Facing.South, Facing.West)
                .AddConnectionPoint(new ConnectionPoint(new Position(1, 1), Facing.East));

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
                .AddWalls(new Position(2, 0), Facing.North, Facing.South)
                .AddConnectionPoint(new ConnectionPoint(new Position(0, 0), Facing.West));

            List<MapBuilder> corridorOptions = new() { corridorBuilder };
            MapGenerator generator = new(roomBuilder, corridorOptions);
            IMap builtRoom = generator.Generate();

            /*
             +---+
             |. .|
             |   +-----+
             |. . . . .
             +---------+
             */
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
        public void MapGeneratorTestSimpleGenerateWithAdditionalConnections()
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
                .AddWalls(new Position(0, 1), Facing.South, Facing.West)
                .AddConnectionPoint(new ConnectionPoint(new Position(1, 1), Facing.East));

            /*
            +-----+
            $. . .$
            +-----+
            */
            MapBuilder corridorBuilder = new();
            corridorBuilder
                .AddFloor(0, 0)
                .AddWalls(new Position(0, 0), Facing.North, Facing.South)
                .AddFloor(1, 0)
                .AddWalls(new Position(1, 0), Facing.North, Facing.South)
                .AddFloor(2, 0)
                .AddWalls(new Position(2, 0), Facing.North, Facing.South)
                .AddConnectionPoint(new ConnectionPoint(new Position(0, 0), Facing.West))
                .AddConnectionPoint(new ConnectionPoint(new Position(2, 0), Facing.East));

            List<MapBuilder> corridorOptions = new() { corridorBuilder };
            MapGenerator generator = new(roomBuilder, corridorOptions);
            Assert.AreEqual(1, roomBuilder.UnconnectedPoints.Count());
            Assert.AreEqual(2, corridorBuilder.UnconnectedPoints.Count());
            /*
             +---+
             |. .|
             |   +-----+
             |. . . . .$
             +---------+
             */
            Assert.True(generator.GenerateStep());
            Assert.AreEqual(1, roomBuilder.UnconnectedPoints.Count());
            Assert.AreEqual(2, corridorBuilder.UnconnectedPoints.Count());
            var expectedPoint = new ConnectionPoint(new Position(4, 1), Facing.East);
            Assert.AreEqual(expectedPoint, roomBuilder.UnconnectedPoints[0]);

            /*
             +---+
             |. .|
             |   +------------
             |. . . . . . . .$
             +----------------
             */
            Assert.True(generator.GenerateStep());
            Assert.AreEqual(1, roomBuilder.UnconnectedPoints.Count());
            Assert.AreEqual(2, corridorBuilder.UnconnectedPoints.Count());
            expectedPoint = new ConnectionPoint(new Position(7, 1), Facing.East);
            Assert.AreEqual(expectedPoint, roomBuilder.UnconnectedPoints[0]);


            /*
            +---+
            |. .|
            |   +-----------------+
            |. . . . . . . . . . .$
            +---------------------+
            */
            Assert.True(generator.GenerateStep());
            Assert.AreEqual(1, roomBuilder.UnconnectedPoints.Count());
            Assert.AreEqual(2, corridorBuilder.UnconnectedPoints.Count());
            expectedPoint = new ConnectionPoint(new Position(10, 1), Facing.East);
            Assert.AreEqual(expectedPoint, roomBuilder.UnconnectedPoints[0]);

            IMap builtRoom = roomBuilder.Build();
            Assert.AreEqual(13, builtRoom.Tiles.Count());

            Dictionary<Position, HashSet<Facing>> expected = new();
            expected[new(0, 0)] = new() { Facing.North, Facing.West };
            expected[new(1, 0)] = new() { Facing.North, Facing.East };
            expected[new(0, 1)] = new() { Facing.South, Facing.West };
            expected[new(1, 1)] = new() { Facing.South };
            expected[new(2, 1)] = new() { Facing.North, Facing.South };
            expected[new(3, 1)] = new() { Facing.North, Facing.South };
            expected[new(4, 1)] = new() { Facing.North, Facing.South };
            expected[new(5, 1)] = new() { Facing.North, Facing.South };
            expected[new(6, 1)] = new() { Facing.North, Facing.South };
            expected[new(7, 1)] = new() { Facing.North, Facing.South };
            expected[new(8, 1)] = new() { Facing.North, Facing.South };
            expected[new(9, 1)] = new() { Facing.North, Facing.South };
            expected[new(10, 1)] = new() { Facing.North, Facing.South, Facing.East };

            foreach ((Position p, HashSet<Facing> walls) in expected)
            {
                Assert.True(builtRoom.TileAt(p).IsPassable);
                Assert.AreEqual(walls, builtRoom.TileAt(p).Walls, 
                $"Walls did not match at {p}. Expected {string.Join(", ", walls)} but was {string.Join(", ", builtRoom.TileAt(p).Walls)}");
            }
        }
    }
}
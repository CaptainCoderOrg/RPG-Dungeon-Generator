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
        public void MapGeneratorTestGenerate()
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

            MapGenerator generator = new ();
            generator._builder = roomBuilder;
            generator._corridorOptions.Add(corridorBuilder);
            IMap map = generator.Generate();
            Assert.True(false);
        }
    }
}
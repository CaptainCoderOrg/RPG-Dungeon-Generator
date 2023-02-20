using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CaptainCoder.Dungeoneering
{
    public class MapBuilderTest
    {

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
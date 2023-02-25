using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

namespace CaptainCoder.Dungeoneering
{
    public class MapBuilderReaderTest
    {
        [Test, Timeout(5000)]
        public void TestRead2x2Room()
        {
            //  +$--+
            //  $. .|
            //  |   |
            //  |. .$
            //  +--$+            
            string[] mapData = {
                "+$--+",
                "$. .|",
                "|   |",
                "|. .$",
                "+--$+"
            };

            string map = string.Join("\n", mapData);
            
            MapBuilderReader reader = new ("+-|", "$", ".");
            MapBuilder builder = reader.FromString(map);

            Assert.AreEqual(4, builder.Floors.Count);
            Assert.True(builder.Floors.Contains(new Position(0, 0)));
            Assert.True(builder.Floors.Contains(new Position(1, 0)));
            Assert.True(builder.Floors.Contains(new Position(1, 0)));
            Assert.True(builder.Floors.Contains(new Position(1, 1)));

            Assert.AreEqual(0, builder.WallsAt(new Position(0,0)).Count);
            Assert.AreEqual(0, builder.WallsAt(new Position(1,1)).Count);
            Assert.True(builder.WallsAt(new Position(1,0)).SetEquals(new []{Facing.North, Facing.East}));
            Assert.True(builder.WallsAt(new Position(0,1)).SetEquals(new []{Facing.South, Facing.West}));

            Assert.AreEqual(4, builder.UnconnectedPoints.Count);
            HashSet<ConnectionPoint> expected = new HashSet<ConnectionPoint>(){
                new ConnectionPoint(0, 0, Facing.North),
                new ConnectionPoint(0, 0, Facing.West),
                new ConnectionPoint(1, 1, Facing.East),
                new ConnectionPoint(1, 1, Facing.South),                
            }.ToHashSet();
            Assert.True(expected.SetEquals(builder.UnconnectedPoints));
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

namespace CaptainCoder.Dungeoneering
{
    public class SimpleGeneratorTest
    {
        [Test, Timeout(5000)]
        public void TestSimpleConstructor()
        {
            MapBuilder eastWest = Corridors.EastWest;
            MapBuilder northSouth = Corridors.NorthSouth;
            MapBuilder cross = Corridors.Cross;
            List<(MapBuilder builder, float weight)> options = new()
            {
                (eastWest, 1),
                (northSouth, 1)
            };
            SimpleGeneratorTable table = new(options);
            IEnumerable<(MapBuilder builder, float probability)> result = table.Table;
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(0.5f, table.ProbabilityOf(eastWest), 0.001);
            Assert.AreEqual(0.5f, table.ProbabilityOf(northSouth), 0.001);
            Assert.AreEqual(0, table.ProbabilityOf(cross));
        }

        [Test, Timeout(5000)]
        public void TestComplexConstructor()
        {
            MapBuilder eastWest = Corridors.EastWest;
            MapBuilder northSouth = Corridors.NorthSouth;
            MapBuilder cross = Corridors.Cross;
            List<(MapBuilder builder, float weight)> options = new()
            {
                (eastWest, 1),
                (northSouth, 1),
                (cross, 2),
            };
            SimpleGeneratorTable table = new(options);
            IEnumerable<(MapBuilder builder, float probability)> result = table.Table;
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(0.25f, table.ProbabilityOf(eastWest), 0.001);
            Assert.AreEqual(0.25f, table.ProbabilityOf(northSouth), 0.001);
            Assert.AreEqual(0.5f, table.ProbabilityOf(cross), 0.001);
        }

        [Test, Timeout(5000)]
        public void TestSimpleNext()
        {
            MapBuilder eastWest = Corridors.EastWest;
            eastWest.Name = "EastWest";
            MapBuilder northSouth = Corridors.NorthSouth;
            northSouth.Name = "NorthSouth";
            MapBuilder cross = Corridors.Cross;
            List<(MapBuilder builder, float weight)> options = new()
            {
                // +-----+
                // $. . .$
                // +-----+
                (eastWest, 1),
                // +$+
                // |.|
                // | |
                // |.|
                // | |
                // |.|
                // +$+
                (northSouth, 1),
            };
            SimpleGeneratorTable table = new(options);
            MapBuilder simpleRoom = Rooms.Room2x2;
            


            // "+$--+",
            // "$. .|",
            // "|   |",
            // "|. .$",
            // "+--$+"
            
            void RunTrials(ConnectionPoint onSimpleRoom, ConnectionPoint onExpected, MapBuilder expectedBuilder)
            {
                for (int i = 0; i < 100; i++)
                {
                    Assert.True(table.Next(simpleRoom, onSimpleRoom, out GeneratorResult result));
                    Assert.AreEqual(simpleRoom, result.MainMap);
                    Assert.AreEqual(onSimpleRoom, result.OnMainMap);
                    Assert.AreEqual(expectedBuilder, result.ExtensionMap);
                    Assert.AreEqual(onExpected, result.OnExtension);
                }
            }
            ConnectionPoint westOnSimpleRoom = new (0, 0, Facing.West);
            RunTrials(westOnSimpleRoom, new (2, 0, Facing.East), eastWest);
            ConnectionPoint eastOnSimpleRoom = new (1, 1, Facing.East);
            RunTrials(eastOnSimpleRoom, new (0, 0, Facing.West), eastWest);
            ConnectionPoint northOnSimpleRoom = new (0, 0, Facing.North);
            RunTrials(northOnSimpleRoom, new (0, 2, Facing.South), northSouth);
            ConnectionPoint southOnSimpleRoom = new (1, 1, Facing.South);
            RunTrials(southOnSimpleRoom, new (0, 0, Facing.North), northSouth);
        }
    }
}

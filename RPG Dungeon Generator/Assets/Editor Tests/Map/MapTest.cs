using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

namespace CaptainCoder.Dungeoneering
{
    public class MapTest
    {
        [Test, Timeout(5000)]
        public void Test1x1RoomToASCII()
        {
            (Position, ITile)[] tiles = 
            {
                (new Position(0, 0), new MutableTile(new []{Facing.North, Facing.East, Facing.South, Facing.West}))
            };
            Map map = new (tiles);
            string[] roomArray = 
            {
                " - ",
                "|.|",
                " - ",
            };
            
            string expected = string.Join("\n", roomArray);
            string actual = map.ToASCII();
            Assert.AreEqual(expected, actual);

        }

        [Test, Timeout(5000)]
        public void Test2x2RoomToASCII()
        {
            (Position, ITile)[] tiles = 
            {
                (new Position(0, 0), new MutableTile(new []{Facing.North, Facing.West})),
                (new Position(1, 0), new MutableTile(new []{Facing.North, Facing.East})),
                (new Position(0, 1), new MutableTile(new []{Facing.South, Facing.West})),
                (new Position(1, 1), new MutableTile(new []{Facing.East, Facing.South}))

            };
            Map map = new (tiles);
            string[] roomArray = 
            {
                " - - ",
                "|. .|",
                "     ",
                "|. .|",
                " - - ",
            };
            
            string expected = string.Join("\n", roomArray);
            string actual = map.ToASCII();
            Assert.AreEqual(expected, actual);
        }

        [Test, Timeout(5000)]
        public void TestRoomWithHallToASCII()
        {
            (Position, ITile)[] tiles = 
            {
                (new Position(0, 0), new MutableTile(new []{Facing.North, Facing.West})),
                (new Position(1, 0), new MutableTile(new []{Facing.North, Facing.East})),
                (new Position(0, 1), new MutableTile(new []{Facing.South, Facing.West})),
                (new Position(1, 1), new MutableTile(new []{Facing.South})),
                (new Position(2, 1), new MutableTile(new []{Facing.North, Facing.South})),
                (new Position(3, 1), new MutableTile(new []{Facing.North, Facing.South})),
                (new Position(4, 1), new MutableTile(new []{Facing.North, Facing.South, Facing.East})),
            };
            Map map = new (tiles);
            string[] roomArray = 
            {
                " - -       ",
                "|. .|      ",
                "     - - - ",
                "|. . . . .|",
                " - - - - - ",
            };
            
            string expected = string.Join("\n", roomArray);
            string actual = map.ToASCII();
            Assert.AreEqual(expected, actual);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

namespace CaptainCoder.Dungeoneering
{
    public class ArrayExtensionTests
    {
        [Test, Timeout(5000)]
        public void TestRotate90()
        {
            //  +$--+
            //  $. .|
            //  |   |
            //  |. .$
            //  +--$+            
            char[,] input = {
                {'A', 'B', 'C', 'D'},
                {'E', 'F', 'G', 'H'},
                {'I', 'J', 'K', 'L'},
            };

            char[,] rotated = {
                {'I', 'E', 'A'},
                {'J', 'F', 'B'},
                {'K', 'G', 'C'},
                {'L', 'H', 'D'},
            };

            char[,] actual = input.Rotate90();

            for (int r = 0; r < rotated.GetLength(0); r++)
            {
                for (int c = 0; c < rotated.GetLength(1); c++)
                {
                    Assert.AreEqual(rotated[r, c], actual[r, c]);
                }
            }
        }

        [Test, Timeout(5000)]
        public void TestRotateCorner()
        {
            // +---+
            // $. .$
            // | --+
            // |.|
            // +$+       
            string[] dataInput =
            {
                "#####",
                "#. .$",
                "# ###",
                "#.#",
                "#$#",      
            };

            string corner = string.Join("\n", dataInput);
            string actual = corner.Rotate90();
            
            dataInput = new string[]{
                "#####",
                "$. .#",
                "### #",
                "  #.#",
                "  #$#",
            };
            string expected = string.Join("\n", dataInput);
            Assert.AreEqual(expected, actual);

            actual = actual.Rotate90();
            dataInput = new string[]{
                "  #$#",
                "  #.#",
                "### #",
                "$. .#",
                "#####"
            };
            expected = string.Join("\n", dataInput);
            Assert.AreEqual(expected, actual);

            actual = actual.Rotate90();
            dataInput = new string[]{
                "#$#  ",
                "#.#  ",
                "# ###",
                "#. .$",
                "#####",
            };
            expected = string.Join("\n", dataInput);
            Assert.AreEqual(expected, actual);
        }
    }
}
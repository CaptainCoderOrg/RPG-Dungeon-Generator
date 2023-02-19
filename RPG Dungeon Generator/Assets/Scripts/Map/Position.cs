using System;
using System.Collections.Generic;

namespace CaptainCoder.Dungeoneering
{
    public struct Position
    {
        public readonly int X;
        public readonly int Y;
        
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position North => new (X, Y + 1);
        public Position East => new (X + 1, Y);
        public Position South => new (X, Y - 1);
        public Position West => new (X - 1, Y);

        public Position Neighbor(Facing facing)
        {
            return facing switch 
            {
                Facing.North => North,
                Facing.South => South,
                Facing.East => East,
                Facing.West => West,
                _ => throw new System.InvalidOperationException($"Unknown facing detected {facing}")
            };
        }

        /// <summary>
        /// Returns an IEnumarble containing the neighboring positions as well as
        /// the direction of each neighbor.
        /// </summary>
        public IEnumerable<(Position, Facing)> Neighbors
        {
            get
            {
                yield return (new Position(X, Y + 1), Facing.North);
                yield return (new Position(X + 1, Y), Facing.East);
                yield return (new Position(X, Y - 1), Facing.South);
                yield return (new Position(X - 1, Y), Facing.West);
            }
        } 

        public override string ToString() => $"Position ({X}, {Y})";

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   X == position.X &&
                   Y == position.Y;
        }

        public override int GetHashCode() => HashCode.Combine(X, Y);
    }
}
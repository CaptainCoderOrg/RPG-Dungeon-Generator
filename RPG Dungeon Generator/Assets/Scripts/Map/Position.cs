using System;
using System.Collections.Generic;

namespace CaptainCoder.Dungeoneering
{
    public struct Position
    {
        public int X;
        public int Y;
        
        public Position((int x, int y) pair) : this (pair.x, pair.y) {}

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position North => new (X, Y - 1);
        public Position East => new (X + 1, Y);
        public Position South => new (X, Y + 1);
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

        public static (Position, Position) FindBounds(IEnumerable<Position> positions)
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            foreach (Position p in positions)
            {
                minX = Math.Min(minX, p.X);
                maxX = Math.Max(maxX, p.X);
                minY = Math.Min(minY, p.Y);
                maxY = Math.Max(maxY, p.Y);
            }
            return (new Position(minX, minY), new Position(maxX, maxY));
        }

        public static bool InBounds(Position p, (Position, Position) bounds)
        {
            (Position topLeft, Position bottomRight) = bounds;
            return !(p.X < topLeft.X || p.X > bottomRight.X || p.Y < topLeft.Y || p.Y > bottomRight.Y);
        }

        public static Position operator+ (Position a, Position b) => new (a.X + b.X, a.Y + b.Y);
        public static Position operator- (Position a, Position b) => new (a.X - b.X, a.Y - b.Y);

        
    }
}
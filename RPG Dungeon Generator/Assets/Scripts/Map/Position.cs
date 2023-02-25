using System;
using System.Collections.Generic;

namespace CaptainCoder.Dungeoneering
{
    public struct Position
    {
        public int Col;
        public int Row;
        
        public Position((int col, int row) pair) : this (pair.col, pair.row) {}

        public Position(int col, int row)
        {
            Col = col;
            Row = row;
        }

        public Position North => new (Col, Row - 1);
        public Position East => new (Col + 1, Row);
        public Position South => new (Col, Row + 1);
        public Position West => new (Col - 1, Row);

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
                yield return (new Position(Col, Row + 1), Facing.North);
                yield return (new Position(Col + 1, Row), Facing.East);
                yield return (new Position(Col, Row - 1), Facing.South);
                yield return (new Position(Col - 1, Row), Facing.West);
            }
        } 

        public override string ToString() => $"Position ({Col}, {Row})";

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   Col == position.Col &&
                   Row == position.Row;
        }

        public override int GetHashCode() => HashCode.Combine(Col, Row);

        public static (Position, Position) FindBounds(IEnumerable<Position> positions)
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            foreach (Position p in positions)
            {
                minX = Math.Min(minX, p.Col);
                maxX = Math.Max(maxX, p.Col);
                minY = Math.Min(minY, p.Row);
                maxY = Math.Max(maxY, p.Row);
            }
            return (new Position(minX, minY), new Position(maxX, maxY));
        }

        public static bool InBounds(Position p, (Position, Position) bounds)
        {
            (Position topLeft, Position bottomRight) = bounds;
            return !(p.Col < topLeft.Col || p.Col > bottomRight.Col || p.Row < topLeft.Row || p.Row > bottomRight.Row);
        }

        public static Position operator+ (Position a, Position b) => new (a.Col + b.Col, a.Row + b.Row);
        public static Position operator- (Position a, Position b) => new (a.Col - b.Col, a.Row - b.Row);

        
    }
}
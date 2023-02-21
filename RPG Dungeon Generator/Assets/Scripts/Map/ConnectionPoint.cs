using System;
using System.Collections.Generic;

namespace CaptainCoder.Dungeoneering
{
    public struct ConnectionPoint
    {
        public readonly Position Position;
        public readonly Facing Direction;

        public ConnectionPoint(Position position, Facing direction)
        {
            Position = position;
            Direction = direction;
        }

        public override bool Equals(object obj)
        {
            return obj is ConnectionPoint point &&
                   EqualityComparer<Position>.Default.Equals(Position, point.Position) &&
                   Direction == point.Direction;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Direction);
        }
    }
}
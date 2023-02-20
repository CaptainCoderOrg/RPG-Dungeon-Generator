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
    }
}
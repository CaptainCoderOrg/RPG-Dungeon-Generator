namespace CaptainCoder.Dungeoneering
{
    public enum Facing
    {
        North, East, South, West
    }

    public static class FacingExtensions
    {
        public static Facing RotateClockwise(this Facing facing) => (Facing)(((int)facing + 1) % 4);
        public static Facing RotateCounterClockwise(this Facing facing) => (Facing)(((int)facing + 3) % 4);
        public static Facing Rotate180(this Facing facing) => (Facing)(((int)facing + 2) % 4);

        public static Position ToPositionOffset(this Facing facing)
        {
            return facing switch
            {
                Facing.East => new (1, 0),
                Facing.West => new (-1, 0),
                Facing.North => new (0, -1),
                Facing.South => new (0, 1),
                _ => throw new System.ArgumentException($"Cannot find position offset for {facing}."),
            };
        }
    }
}
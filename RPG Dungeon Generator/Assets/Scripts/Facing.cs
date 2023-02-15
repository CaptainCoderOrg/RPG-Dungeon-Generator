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
    }
}
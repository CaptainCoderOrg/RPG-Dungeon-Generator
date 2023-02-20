namespace CaptainCoder.Dungeoneering
{
    public interface IMap
    {
        public ITile TileAt(Position p);

        /// <summary>
        /// Returns the bounds of positions that could potentially have tiles.
        /// Any position outside of these bounds is guaranteed to not be passable.
        /// </summary>
        public (Position topLeft, Position bottomRight) TileBounds { get; }
    }
}
using System.Collections.Generic;

namespace CaptainCoder.Dungeoneering
{
    public interface IMap
    {
        public ITile TileAt(Position p);

        public IEnumerable<(Position, ITile)> Tiles { get; }

        /// <summary>
        /// Returns the bounds of positions that could potentially have tiles.
        /// Any position outside of these bounds is guaranteed to not be passable.
        /// </summary>
        public (Position topLeft, Position bottomRight) TileBounds { get; }
    }
}
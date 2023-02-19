using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CaptainCoder.Dungeoneering
{
    public class Map : IMap
    {
        private readonly Dictionary<Position, ITile> _tiles;

        public Map(IEnumerable<(Position, ITile)> tiles)
        {
            _tiles = new Dictionary<Position, ITile>();
            foreach((Position p, ITile t) in tiles)
            {
                Debug.Assert(!_tiles.ContainsKey(p), $"Duplicate position detected {p}");
                _tiles[p] = t;
            }
        }

        public ITile TileAt(Position p)
        {
            if (_tiles.TryGetValue(p, out ITile tile))
            {
                return tile;
            }
            HashSet<Facing> walls = new();
            foreach ((Position pos, Facing dir) in p.Neighbors)
            {
                if (!CheckWall(pos, dir.Rotate180())) { continue; }
                walls.Add(dir);
            }
            ITile noTile = new NoTile(walls);
            return noTile;
        }

        private bool CheckWall(Position toCheck, Facing wallDirection)
        {
            if (!_tiles.TryGetValue(toCheck, out ITile tile)) { return false; }
            return tile.Walls.Contains(wallDirection);
        }

        public (Position topLeft, Position bottomRight) TileBounds()
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            foreach(Position p in _tiles.Keys)
            {
                minX = Math.Min(minX, p.X);
                maxX = Math.Max(maxX, p.X);
                minY = Math.Min(minY, p.Y);
                maxY = Math.Max(maxY, p.Y);
            }
            return (new Position(minX, maxY), new Position(maxX, minY));
        }
    }
}
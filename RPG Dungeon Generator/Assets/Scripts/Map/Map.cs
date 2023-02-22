using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CaptainCoder.Dungeoneering
{
    public class Map : IMap
    {
        private readonly Dictionary<Position, ITile> _tiles;
        public (Position topLeft, Position bottomRight) TileBounds { get; }

        public IEnumerable<(Position, ITile)> Tiles
        {
            get
            {
                foreach (KeyValuePair<Position, ITile> pair in _tiles)
                {
                    yield return (pair.Key, pair.Value);
                }
            }
        }

        public Map(IEnumerable<(Position, ITile)> tiles)
        {
            _tiles = new Dictionary<Position, ITile>();
            foreach ((Position p, ITile t) in tiles)
            {
                Debug.Assert(!_tiles.ContainsKey(p), $"Duplicate position detected {p}");
                _tiles[p] = t;
            }
            TileBounds = FindBounds();
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

        private (Position, Position) FindBounds()
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            foreach (Position p in _tiles.Keys)
            {
                minX = Math.Min(minX, p.X);
                maxX = Math.Max(maxX, p.X);
                minY = Math.Min(minY, p.Y);
                maxY = Math.Max(maxY, p.Y);
            }
            return (new Position(minX, minY), new Position(maxX, maxY));
        }

        public string ToASCII()
        {
            int width = TileBounds.bottomRight.X - TileBounds.topLeft.X + 1;
            int height = TileBounds.bottomRight.Y - TileBounds.topLeft.Y + 1;
            char[,] output = new char[height*2 + 1, width*2 + 1];
            
            foreach ((Position pos, ITile tile) in _tiles)
            {
                int row = pos.Y * 2 + 1;
                int col = pos.X * 2 + 1;
                
                if (tile.IsPassable)
                {
                    output[row, col] = '.';
                }
                foreach(Facing facing in tile.Walls)
                {
                    (char wall, int colOff, int rowOff) = facing switch {
                        Facing.North => ('-', 0, -1),
                        Facing.South => ('-', 0, 1),
                        Facing.East => ('|', 1, 0),
                        Facing.West => ('|', -1, 0),
                        _ => throw new InvalidOperationException($"Could not turn {facing} into char."), 
                    };
                    output[row + rowOff, col + colOff] = wall;
                }
            }
            StringBuilder sb = new ();
            for (int row = 0; row < output.GetLength(0); row++)
            {
                for (int col = 0; col < output.GetLength(1); col++)
                {
                    char ch = output[row, col] == default ? ' ' : output[row, col];
                    sb.Append(ch);
                }
                if (row < output.GetLength(0) - 1) { sb.Append('\n'); }
            }
            return sb.ToString();
        }
    }
}
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace CaptainCoder.Dungeoneering
{
    public class MapBuilder
    {
        private readonly Dictionary<Position, MutableTile> _tiles = new ();
        private readonly Dictionary<Position, HashSet<Facing>> _walls = new ();

        public MapBuilder AddWalls(Position position, params Facing[] facing) => AddWalls(position, facing.ToList());
        public MapBuilder AddWalls(Position position, IEnumerable<Facing> facings)
        {
            foreach (Facing facing in facings) 
            {
                AddWall(position, facing);
            }
            return this;
        }

        public MapBuilder AddWall(Position pos, Facing facing)
        {
            if (!_walls.TryGetValue(pos, out HashSet<Facing> wallsAtPosition))
            {
                wallsAtPosition = new HashSet<Facing>();
                _walls[pos] = wallsAtPosition;
            }
            wallsAtPosition.Add(facing);
            
            Position neighbor = pos.Neighbor(facing);
            AddNeighborWall(neighbor, facing.Rotate180());
            
            return this;
        }

        private void AddNeighborWall(Position pos, Facing facing)
        {
            if (!_walls.ContainsKey(pos))
            {
                AddWall(pos, facing);
            }
            else
            {
                _walls[pos].Add(facing);
            }
        }

        
        private HashSet<Facing> NeighborWalls(Position p)
        {
            HashSet<Facing> walls = new();
            foreach ((Position pos, Facing dir) in p.Neighbors)
            {
                if (!CheckWall(pos, dir.Rotate180())) { continue; }
                walls.Add(dir);
            }
            return walls;
        }


        // TODO: Code smell, this is almost identical to Map.CheckWall
        private bool CheckWall(Position toCheck, Facing wallDirection)
        {
            if (!_walls.TryGetValue(toCheck, out HashSet<Facing> walls)) { return false; }
            return walls.Contains(wallDirection);
        }

        public MapBuilder AddFloors(IEnumerable<Position> positions)
        {
            foreach (Position p in positions)
            {
                AddFloor(p);
            }
            return this;
        }

        public MapBuilder AddFloor(int x, int y) => AddFloor(new Position(x, y));

        public MapBuilder AddFloor(Position position)
        {
            Debug.Assert(!_tiles.ContainsKey(position), $"Floor already exists at position {position}.");
            _tiles[position] = new ();
            return this;
        }


        public IMap Build()
        {
            List<(Position, ITile)> tiles = new ();
            foreach (Position pos in _tiles.Keys)
            {
                MutableTile tile = _tiles[pos];
                HashSet<Facing> walls = _walls[pos];
                tile._walls = walls;
                tiles.Add((pos, tile));
            }
            return new Map(tiles);
        }
    }

    class MutableTile : ITile
    {
        internal HashSet<Facing> _walls;

        public bool IsPassable => true;

        public HashSet<Facing> Walls => _walls.ToHashSet();
    }
}
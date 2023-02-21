using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;
namespace CaptainCoder.Dungeoneering
{
    public class MapBuilder
    {
        private readonly Dictionary<Position, MutableTile> _tiles = new();
        private readonly Dictionary<Position, HashSet<Facing>> _walls = new();
        // TODO: Priority Queue would be more efficient for _unconnectedPoints
        // Property of Enqueue and Dequeue
        // When you Enqueue (randomly specify the priority)
        // Priority Queue is a fancy BinarySearchTree (actually a Heap data structure)
        // O(logn) - for adding/removing to a priority queue
        private readonly List<ConnectionPoint> _unconnectedPoints = new();
        private readonly HashSet<ConnectionPoint> _connectionPoints = new();
        public bool HasConnectionPoint => _unconnectedPoints.Count > 0;
        public Random RNG { get; set; } = new Random();
        public List<ConnectionPoint> UnconnectedPoints => _unconnectedPoints.ToList();

        public bool TryFindConnectionPoint(Facing dir, out ConnectionPoint connectAt)
        {
            connectAt = default;
            var filtered = _unconnectedPoints
                .Where(f => f.Direction == dir);
            if (filtered.Count() == 0) { return false; }

            connectAt = filtered
                .OrderBy((f) => RNG.Next())
                .First();
            return true;
        }

        public MapBuilder AddConnectionPoint(ConnectionPoint point)
        {
            // TODO: Point should be on edge of map?
            if (_connectionPoints.Contains(point)) { return this; }
            _connectionPoints.Add(point);
            _unconnectedPoints.Add(point);
            return this;
        }

        /// <summary>
        /// Attempts to remove a Random <see cref="ConnectionPoint"/>. If succesful, returns true
        /// and <paramref name="removed"/> is set to the removed <see cref="ConnectionPoint"/>.
        /// Othewise, returns false and the value of <paramref name="removed"/> is undefined.
        /// </summary>
        public bool TryRemoveRandomConnectionPoint(out ConnectionPoint removed)
        {
            if (!HasConnectionPoint)
            {
                removed = default;
                return false;
            }
            // TODO: Relatively inefficient design here (if speed becomes an issue consider priority queue)
            int ix = RNG.Next(0, _unconnectedPoints.Count);
            removed = _unconnectedPoints[ix];
            _unconnectedPoints.RemoveAt(ix);
            return true;
        }

        public MapBuilder MergeAt(ConnectionPoint onBuilder, MapBuilder toMerge, ConnectionPoint onMap)
        {
            
            // TODO: Check for conflicts in Map Merge
            Position connectionPosition = onBuilder.Position.Neighbor(onBuilder.Direction);
            Position offset = new(connectionPosition.X + onMap.Position.X, connectionPosition.Y + onMap.Position.Y);
            MergeFloors(toMerge, offset);
            MergeWalls(toMerge, offset);
            MergeConnectionPoints(toMerge, offset, onMap);
            // TODO: Consider ensuring toMerge contains onMap
            return this;
        }

        private void MergeConnectionPoints(MapBuilder toMerge, Position offset, ConnectionPoint onMap)
        {
            foreach (ConnectionPoint point in toMerge._unconnectedPoints)
            {
                if (point.Equals(onMap)) { continue; }
                Position newPos = point.Position;
                newPos.X += offset.X;
                newPos.Y += offset.Y;
                ConnectionPoint newPoint = new(newPos, point.Direction);
                AddConnectionPoint(newPoint);
            }
        }

        private void MergeWalls(MapBuilder toMerge, Position offset)
        {
            foreach ((Position pos, HashSet<Facing> walls) in toMerge._walls)
            {
                Position newPos = pos;
                newPos.X += offset.X;
                newPos.Y += offset.Y;
                AddWalls(newPos, walls);
            }
        }

        private void MergeFloors(MapBuilder toMerge, Position offset)
        {
            foreach ((Position pos, MutableTile tile) in toMerge._tiles)
            {
                Position newPos = pos;
                newPos.X += offset.X;
                newPos.Y += offset.Y;

                //TODO Check for conflicts
                _tiles[newPos] = tile;
            }
        }

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
            _tiles[position] = new();
            return this;
        }

        public IMap Build()
        {
            foreach (ConnectionPoint connectionPoint in _unconnectedPoints)
            {
                AddWall(connectionPoint.Position, connectionPoint.Direction);
            }
            List<(Position, ITile)> tiles = new();
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
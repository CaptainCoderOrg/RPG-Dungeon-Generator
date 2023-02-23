using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CaptainCoder.Dungeoneering
{
    public class MapGenerator
    {
        public Random RNG { get; set; } = new();
        private List<MapBuilder> _roomOptions = new();
        private List<MapBuilder> _corridorOptions = new();
        private MapBuilder _builder = new();

        public MapGenerator(MapBuilder startingBuilder, List<MapBuilder> corridorOptions)
        {
            Debug.Assert(startingBuilder != null);
            Debug.Assert(corridorOptions != null);
            _builder = startingBuilder;
            _corridorOptions = new(corridorOptions);
        }

        public bool GenerateStep()
        {
            if (_builder.TryRemoveRandomConnectionPoint(out ConnectionPoint toConnect))
            {
                var corridor = _corridorOptions
                    .Where(c => _builder.CanMergeAt(toConnect, c))
                    .OrderBy(_ => RNG.Next())
                    .FirstOrDefault();
                if (corridor == default)
                {
                    _builder.AddWall(toConnect.Position, toConnect.Direction);
                }
                else
                {
                    _builder.Merge(toConnect, corridor);
                }
                return true;
            }
            return false;
        }

        public IMap Generate(int steps = 100)
        {
            while (steps-- > 0 && GenerateStep()) ;
            return _builder.Build();
        }

    }
}
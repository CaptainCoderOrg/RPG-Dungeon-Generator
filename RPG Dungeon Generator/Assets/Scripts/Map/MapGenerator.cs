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

        public MapGenerator(MapBuilder startingBuilder, IEnumerable<MapBuilder> corridorOptions) : this(startingBuilder, corridorOptions, new MapBuilder[]{}) {}

        public MapGenerator(MapBuilder startingBuilder, IEnumerable<MapBuilder> corridorOptions, IEnumerable<MapBuilder> roomOptions)
        {
            Debug.Assert(startingBuilder != null);
            Debug.Assert(corridorOptions != null);
            _builder = startingBuilder;
            _corridorOptions = new(corridorOptions);
            _roomOptions = new List<MapBuilder>(roomOptions);
        }

        public bool GenerateStep()
        {
            if (_builder.TryRemoveRandomConnectionPoint(out ConnectionPoint toConnect))
            {
                double chance = RNG.NextDouble();

                List<MapBuilder> options = _roomOptions;
                if (chance < .8)
                {
                    options = _corridorOptions;
                }

                var addition = options
                    .Where(c => _builder.CanMergeAt(toConnect, c))
                    .OrderBy(_ => RNG.Next())
                    .FirstOrDefault();
                if (addition == default)
                {
                    _builder.AddWall(toConnect.Position, toConnect.Direction);
                }
                else
                {
                    _builder.Merge(toConnect, addition);
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
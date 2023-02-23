using System.Diagnostics;
using System.Collections.Generic;
using System;

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
                int ix = RNG.Next(0, _corridorOptions.Count);
                MapBuilder corridor = _corridorOptions[ix];
                if (corridor.TryFindConnectionPoint(toConnect.Direction.Rotate180(), out ConnectionPoint connectAt))
                {
                    _builder.MergeAt(toConnect, corridor, connectAt);
                }
                else
                {
                    // TODO: Do something more intelligent
                    _builder.AddWall(toConnect.Position, toConnect.Direction);
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
using System.Collections.Generic;
using System;

namespace CaptainCoder.Dungeoneering
{
    public class MapGenerator
    {
        public Random RNG { get; set; } = new ();
        public List<MapBuilder> _roomOptions = new ();
        public List<MapBuilder> _corridorOptions = new();
        public MapBuilder _builder = new ();
        
        public IMap Generate()
        {
            while (_builder.TryRemoveRandomConnectionPoint(out ConnectionPoint toConnect))
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
            }
            return _builder.Build();
        }

    }
}
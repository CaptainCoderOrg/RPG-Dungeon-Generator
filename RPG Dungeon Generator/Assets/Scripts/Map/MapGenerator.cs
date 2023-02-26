using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Linq;

namespace CaptainCoder.Dungeoneering
{
    public class MapGenerator
    {
        public Random RNG { get; set; } = new();
        private readonly IGeneratorTable _table;
        private MapBuilder _builder = new();

        public MapGenerator(MapBuilder startingBuilder, IGeneratorTable table)  
        {
            _builder = startingBuilder;
            _table = table;
        }

        public bool GenerateStep()
        {
            if (_builder.TryRemoveRandomConnectionPoint(out ConnectionPoint toConnect))
            {
                if(_table.Next(_builder, toConnect, out GeneratorResult result))
                {
                    _builder.MergeAt(result.OnMainMap, result.ExtensionMap, result.OnExtension);
                }
                else
                {
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

using System.Collections.Generic;
using System;
using System.Linq;

namespace CaptainCoder.Dungeoneering
{
    public class SimpleGeneratorTable : IGeneratorTable
    {
        public static Random RNG { get; set; } = new Random();
        private readonly List<(MapBuilder builder, float lowerRange)> _lookupTable = new();
        private readonly List<(MapBuilder builder, float probability)> _probTable = new();
        public IEnumerable<(MapBuilder builder, float probability)> Table => _probTable.ToList();

        public SimpleGeneratorTable(IEnumerable<(MapBuilder builder, float weight)> options)
        {
            float totalWeights = options.Sum(pair => pair.weight);
            float lowerRange = 0;
            foreach ((MapBuilder builder, float weight) in options)
            {
                float probability = weight / totalWeights;
                lowerRange += probability;
                _lookupTable.Add((builder, lowerRange));
                _probTable.Add((builder, probability));
            }
        }

        public float ProbabilityOf(MapBuilder toCheck)
        {
            foreach((MapBuilder builder, float probability) in _probTable)
            {
                if (toCheck == builder)
                {
                    return probability;
                }
            }
            return 0;
        }

        public bool Next(MapBuilder toExtend, ConnectionPoint mergePoint, out GeneratorResult result) => Next(RNG, toExtend, mergePoint, out result);

        public bool Next(Random generator, MapBuilder toExtend, ConnectionPoint mergePoint, out GeneratorResult result)
        {
            result = null;
            var possibleOptions = _probTable.Where(entry => toExtend.CanMergeAt(mergePoint, entry.builder));
            if (possibleOptions.Count() == 0) { return false; }
            SimpleGeneratorTable simplified = new (possibleOptions);
            MapBuilder extension = simplified.Find((float)generator.NextDouble());
            ConnectionPoint onExtension = extension.FindConnectionPoint(mergePoint.Direction.Rotate180());
            result = new GeneratorResult(toExtend, mergePoint, extension, onExtension);
            return true;
        }

        private MapBuilder Find(float probability)
        {
            foreach ((MapBuilder builder, float chance) in _lookupTable)
            {
                if (probability < chance)
                {
                    return builder;
                }
            }
            return _lookupTable.Last().builder;
        }
    }
}
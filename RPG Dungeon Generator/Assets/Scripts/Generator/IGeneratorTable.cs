using System.Diagnostics;
namespace CaptainCoder.Dungeoneering
{
    public interface IGeneratorTable
    {
        public bool Next(MapBuilder toExtend, ConnectionPoint mergePoint, out GeneratorResult result);
    }

    public class GeneratorResult
    {
        public MapBuilder MainMap { get; }
        public ConnectionPoint OnMainMap { get; }
        public MapBuilder ExtensionMap { get; }
        public ConnectionPoint OnExtension { get; }

        public GeneratorResult(MapBuilder main, ConnectionPoint onMain, MapBuilder extension, ConnectionPoint onExtension)
        {
            Debug.Assert(main != null);
            Debug.Assert(extension != null);
            MainMap = main;
            OnMainMap = onMain;
            ExtensionMap = extension;
            OnExtension = onExtension;
        }
    }

}
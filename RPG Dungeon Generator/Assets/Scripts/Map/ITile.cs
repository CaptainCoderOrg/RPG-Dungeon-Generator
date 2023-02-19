using System.Collections.Generic;

namespace CaptainCoder.Dungeoneering
{
    public interface ITile
    {
        public bool IsPassable { get; }
        public HashSet<Facing> Walls { get; }
    }
}
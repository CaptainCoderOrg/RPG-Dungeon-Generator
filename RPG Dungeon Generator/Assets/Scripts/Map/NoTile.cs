using System.Collections.Generic;
using System.Linq;

namespace CaptainCoder.Dungeoneering
{
    public class NoTile : ITile
    {
        public NoTile(HashSet<Facing> walls)
        {
            Walls = walls.ToHashSet();
        }

        public bool IsPassable => false;
        public HashSet<Facing> Walls { get; }
    }
}
using UnityEngine;
using System.Collections.Generic;

namespace CaptainCoder.Dungeoneering
{
    public class GridCellDatabase : MonoBehaviour
    {
        [SerializeField]
        private List<Entry> _cellEntries;
        private Dictionary<char, GameObject> _cellEntryDict;
        private Dictionary<char, GameObject> CellEntryDict
        {
            get
            {
                if (_cellEntryDict == null)
                {
                    _cellEntryDict = new Dictionary<char, GameObject>();
                    foreach (Entry e in _cellEntries)
                    {
                        Debug.Assert(!_cellEntryDict.ContainsKey(e.Key), $"Entry List contains duplicate entry for character '{e.Key}'");
                        _cellEntryDict[e.Key] = e.Value;
                    }
                }
                return _cellEntryDict;
            }
        }

        
        [SerializeField]
        private List<Entry> _wallEntries;
        private Dictionary<char, GameObject> _wallEntryDict;
        private Dictionary<char, GameObject> WallEntryDict
        {
            get
            {
                if (_wallEntryDict == null)
                {
                    _wallEntryDict = new Dictionary<char, GameObject>();
                    foreach (Entry e in _wallEntries)
                    {
                        Debug.Assert(!_wallEntryDict.ContainsKey(e.Key), $"Entry List contains duplicate entry for character '{e.Key}'");
                        _wallEntryDict[e.Key] = e.Value;
                    }
                }
                return _wallEntryDict;
            }
        }

        public GameObject InstantiateTile(char ch, Transform parent = null) => Instantiate(CellEntryDict[ch], parent);
        public GameObject InstantiateWall(char ch, Transform parent = null) => Instantiate(WallEntryDict[ch], parent);
    
        [System.Serializable]
        public class Entry
        {
            [field: SerializeField]
            public char Key { get; private set; }
            [field: SerializeField]
            public GameObject Value { get; private set; } 
        }
    }
}
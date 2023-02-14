using UnityEngine;
using System.Collections.Generic;

namespace CaptainCoder.Dungeoneering
{
    public class GridCellDatabase : MonoBehaviour
    {
        [SerializeField]
        private List<Entry> _entries;
        private Dictionary<char, GameObject> _entryDict;
        private Dictionary<char, GameObject> EntryDict
        {
            get
            {
                if (_entryDict == null)
                {
                    _entryDict = new Dictionary<char, GameObject>();
                    foreach (Entry e in _entries)
                    {
                        Debug.Assert(!_entryDict.ContainsKey(e.Key), $"Entry List contains duplicate entry for character '{e.Key}'");
                        _entryDict[e.Key] = e.Value;
                    }
                }
                return _entryDict;
            }
        }
        public GameObject Instantiate(char ch, Transform parent = null) => Instantiate(EntryDict[ch], parent);
    
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
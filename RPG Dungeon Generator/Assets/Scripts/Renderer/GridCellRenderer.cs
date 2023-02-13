using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaptainCoder.Dungeoneering
{
    public enum CellType
    {
        Floor,
        Wall
    }

    public class GridCellRenderer : MonoBehaviour
    {
        [field: SerializeField]
        private CellType _typeOfCell;
        public CellType TypeOfCell 
        { 
            get => _typeOfCell; 
            set
            {
                _typeOfCell = value;
                Wall.gameObject.SetActive(_typeOfCell == CellType.Wall);
            }
        }

        private WallRenderer _wall;
        public WallRenderer Wall
        {
            get 
            {
                if (_wall == null)
                {
                    _wall = GetComponentInChildren<WallRenderer>();
                    Debug.Assert(_wall != null);
                }
                return _wall;
            }
        }
        private FloorRenderer _floor;
        public FloorRenderer Floor
        {
            get 
            {
                if (_floor == null)
                {
                    _floor = GetComponentInChildren<FloorRenderer>();
                    Debug.Assert(_floor != null);
                }
                return _floor;
            }
        }
    }
}
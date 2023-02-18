using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaptainCoder.Dungeoneering
{
    public class WallTileEndDetector : MonoBehaviour
    {
        private static int s_NextID = 0;
        private int? _id;
        private int ID 
        {
            get 
            {
                if (_id == null)
                {
                    _id = s_NextID;
                    s_NextID++;
                }
                return _id.Value;
            }
        }
        public void DetectOtherEnds()
        { 
            Physics.SyncTransforms();
            Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.one * 2f);
            List<GameObject> toDestroy = new ();
            foreach(Collider c in colliders)
            {
                WallTileEndDetector other = c.GetComponent<WallTileEndDetector>();
                if (other == null || other == this) { continue; } 
                WallTileEndDetector loser = other.ID < this.ID ? other : this;
                toDestroy.Add(loser.gameObject);                
            }
            foreach (GameObject go in toDestroy)
            {
                DestroyImmediate(go);
            }
        }

        public void Start()
        {
            DetectOtherEnds();
        }
    }
}
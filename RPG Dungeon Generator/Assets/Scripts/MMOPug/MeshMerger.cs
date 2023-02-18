using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace MMOPug
{

    public class MeshMerger : MonoBehaviour
    {

        [SerializeField] private string[] tagToMerge; // The tag to identify objects to merge
        [SerializeField] private Material[] mergedMaterial; // The material to apply to the merged mesh

        public void Start() => MergeMesh();

        public void MergeMesh()
        {
            for (int tag = 0; tag < tagToMerge.Length; tag++)
            {
                // Find all objects with the specified tag
                GameObject[] objectsToMerge = GameObject.FindGameObjectsWithTag(tagToMerge[tag]);

                // Create a new mesh to merge the objects into
                Mesh mergedMesh = new Mesh();

                // Combine the meshes of all objects into the merged mesh
                List<CombineInstance> combines = new ();
                // CombineInstance[] combine = new CombineInstance[objectsToMerge.Length];
                for (int i = 0; i < objectsToMerge.Length; i++)
                {
                    MeshFilter[] filters = objectsToMerge[i].GetComponentsInChildren<MeshFilter>();
                    foreach(MeshFilter filter in filters)
                    {
                        CombineInstance combine = default;
                        combine.mesh = filter.mesh;
                        combine.transform = filter.gameObject.transform.localToWorldMatrix;
                        combines.Add(combine);
                    }
                    // combines[i].mesh = objectsToMerge[i].GetComponent<MeshFilter>().mesh;
                    // combines[i].transform = objectsToMerge[i].transform.localToWorldMatrix;
                }
                mergedMesh.CombineMeshes(combines.ToArray());

                // Create a new game object to hold the merged mesh
                GameObject mergedObject = new GameObject(tagToMerge + "_Merged");
                mergedObject.AddComponent<MeshFilter>().mesh = mergedMesh;
                mergedObject.AddComponent<MeshRenderer>().material = mergedMaterial[tag];
                mergedObject.AddComponent<MeshCollider>();

                // Remove the original objects
                for (int i = 0; i < objectsToMerge.Length; i++)
                {
                    Destroy(objectsToMerge[i]);
                }
            }

        }
    }
}
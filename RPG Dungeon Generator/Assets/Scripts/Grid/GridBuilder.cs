using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.ProBuilder.Shapes;

namespace CaptainCoder.Dungeoneering
{
    public class GridBuilder
    {
        private GridCellDatabase _database;

        public GridBuilder(GridCellDatabase database)
        {
            _database = database;
        }

        public void Build(string grid, Transform container)
        {
            string[] rows = grid.Split("\n").Select(s => s.TrimEnd()).ToArray();
            GameObject floorContainer = new("Floors");
            floorContainer.transform.parent = container;
            BuildFloors(rows, floorContainer.transform);
            GameObject wallContainer = new("Walls");
            wallContainer.transform.parent = container;
            wallContainer.transform.localPosition = new Vector3(-Grid.CellSize + 1, 0, -0.5f);
            BuildWalls(rows, wallContainer.transform);
        }

        private void BuildWalls(string[] rows, Transform container)
        {
            List<GameObject> allWalls = new();
            HashSet<(int, int)> wallEnds = new();
            for (int r = 0; r < rows.Length; r++)
            {
                // On even rows, we care about the odd columns
                // On odd rows, we care about the even columns
                int startColumn = r % 2 == 0 ? 1 : 0;
                bool isNorthSouth = r % 2 == 1;
                for (int c = startColumn; c < rows[r].Length; c += 2)
                {
                    char ch = rows[r][c];
                    if (ch == ' ') { continue; }
                    GameObject obj = _database.InstantiateWall(ch, isNorthSouth, container);
                    allWalls.Add(obj);

                    float row = (r * .5f);
                    float col = (c * .5f);
                    obj.name = $"({row}, {col}) - {obj.name}";
                    obj.transform.localPosition = new Vector3(row * Grid.CellSize, 0, col * Grid.CellSize);

                    WallTileEndDetector[] ends = obj.GetComponentsInChildren<WallTileEndDetector>();
                    foreach (WallTileEndDetector end in ends)
                    {
                        end.DetectOtherEnds();
                    }
                }
            }
            // GameObject merged = MergeMeshes(allWalls, _database.WallMaterial);
            // merged.transform.parent = container;
        }

        private void BuildFloors(string[] rows, Transform container)
        {
            for (int r = 0; r < (rows.Length - 1) / 2; r++)
            {
                string row = rows[r * 2 + 1];
                for (int c = 0; c < (row.Length - 1) / 2; c++)
                {
                    char ch = row[c * 2 + 1];
                    if (ch == ' ') { continue; }
                    GameObject obj = _database.InstantiateTile(ch, container);
                    obj.name = $"({r}, {c}) - {obj.name}";
                    obj.transform.localPosition = new Vector3(r * Grid.CellSize, 0, c * Grid.CellSize);
                }
            }
        }

        public static GameObject MergeMeshes(List<GameObject> objectsToMerge, Material material)
        {
            // Create a new mesh to merge the objects into
            Mesh mergedMesh = new Mesh();
            List<CombineInstance> combines = new();
            foreach (GameObject toMerge in objectsToMerge) //int i = 0; i < objectsToMerge.Count; i++)
            {
                MeshFilter[] filters = toMerge.GetComponentsInChildren<MeshFilter>();
                foreach (MeshFilter filter in filters)
                {
                    CombineInstance combine = default;
                    combine.mesh = filter.sharedMesh;
                    combine.transform = filter.gameObject.transform.localToWorldMatrix;
                    combines.Add(combine);
                }
            }
            mergedMesh.CombineMeshes(combines.ToArray());

            // Create a new game object to hold the merged mesh
            GameObject mergedObject = new("mesh_Merged");
            mergedObject.AddComponent<MeshFilter>().mesh = mergedMesh;
            mergedObject.AddComponent<MeshRenderer>().material = material;
            mergedObject.AddComponent<MeshCollider>();

            // Remove the original objects
            foreach (GameObject go in objectsToMerge)
            {
                GameObject.DestroyImmediate(go);
            }
            return mergedObject;
        }
    }
}
using UnityEngine;

namespace CaptainCoder.Dungeoneering
{
    public class GridBuilder
    {
        const int CellSize = 5;
        private GridCellDatabase _database;        

        public GridBuilder(GridCellDatabase database)
        {
            _database = database;
        }

        public void Build(string grid, Transform container)
        {
            string[] rows = grid.Split();
            for (int r = 0; r < rows.Length; r++)
            {
                for (int c = 0; c < rows[r].Length; c++)
                {
                    char ch = rows[r][c];
                    GameObject obj = _database.Instantiate(ch, container);
                    obj.name = $"({r}, {c}) - {obj.name}";
                    obj.transform.localPosition = new Vector3(r * CellSize, 0, c * CellSize);
                }
            }
        }
    }
}
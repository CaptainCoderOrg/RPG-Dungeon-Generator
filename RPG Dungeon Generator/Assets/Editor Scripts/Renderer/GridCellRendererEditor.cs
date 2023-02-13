using UnityEngine;
using UnityEditor;

namespace CaptainCoder.Dungeoneering
{

    [CustomEditor(typeof(GridCellRenderer))]
    public class GridCellRendererEditor : Editor
    {

        private GridCellRenderer _cell;

        void OnEnable()
        {
            _cell = (GridCellRenderer)target;
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            _cell.TypeOfCell = (CellType)EditorGUILayout.EnumPopup("Type", _cell.TypeOfCell);
            // EditorGUILayout.LabelField("Name", "Test");
            // if (_characterInspector.Character == null)
            // {
            //     EditorGUILayout.LabelField("Character Data", "Character data not initialized");
            // }
            // else
            // {
            //     RenderCharacter(_characterInspector.Character);
            // }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
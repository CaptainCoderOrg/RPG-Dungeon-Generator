using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace CaptainCoder.Dungeoneering
{
    public class GridEditor : EditorWindow
    {
        [MenuItem("Dungeoneering Kit/Grid Editor")]
        public static void ShowExample()
        {
            GridEditor wnd = GetWindow<GridEditor>();
            wnd.titleContent = new GUIContent("GridEditor");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
            // EditorGUILayout.EnumPopup("Type", CellType.Wall);
            // Import UXML
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor Scripts/GridEditor/GridEditor.uxml");
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            VisualElement refContainer = root.Q<VisualElement>("ObjectRefContainer");
            ObjectField gridTarget = new("Target Transform") { objectType = typeof(Transform) };
            ObjectField gridCellDatabase = new("Grid Cell Database") { objectType = typeof(GridCellDatabase) };
            refContainer.Add(gridTarget);
            refContainer.Add(gridCellDatabase);

            var mapData = rootVisualElement.Q<TextField>("MapData");
            var button = rootVisualElement.Q<Button>("AddToScene");

            
            
            // button.clicked += () => ShowValue(gridCellDatabase);
            button.clicked += () => AddToScene((GridCellDatabase)gridCellDatabase.value, mapData.text, (Transform)gridTarget.value);
        }

        private void ShowValue(ObjectField gridCellDatabase)
        {
            Debug.Log(gridCellDatabase.value);
        }

        private void AddToScene(GridCellDatabase database, string grid, Transform container)
        {
            GameObject parent = new ("Generated Grid");
            parent.transform.parent = container;
            GridBuilder builder = new (database);
            builder.Build(grid, parent.transform);
        }
    }
}
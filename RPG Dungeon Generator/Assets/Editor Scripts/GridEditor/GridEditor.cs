using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

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

            TextField mapData = rootVisualElement.Q<TextField>("MapData");
            var addToSceneButton = rootVisualElement.Q<Button>("AddToScene");

            // button.clicked += () => ShowValue(gridCellDatabase);
            addToSceneButton.clicked += () => AddToScene((GridCellDatabase)gridCellDatabase.value, mapData.text, (Transform)gridTarget.value);

            var generationSteps = rootVisualElement.Q<SliderInt>("GenerationSteps");
            var generateButton = rootVisualElement.Q<Button>("Generate");
            generateButton.clicked += () => GenerateGrid(mapData, generationSteps);
        }

        private void GenerateGrid(TextField mapOutput, SliderInt generationSteps)
        {
            List<(MapBuilder options, float weight)> options = new ()
            {
                // (Rooms.Room2x2, 1f),
                // (Rooms.Room4x4, 3f),
                // (Rooms.Room8x4, 1f),
                (Rooms.URoom, 10f),
                (Rooms.Pillar3x3, 10f),
                (Corridors.EastWest, 1f),
                (Corridors.NorthSouth, 1f),
                (Corridors.Cross, 3f),
                (Corridors.CornerTL, 5f),
                (Corridors.CornerTR, 5f),
                (Corridors.CornerBL, 5f),
                (Corridors.CornerBR, 5f),
                (Corridors.TeeEast, 3f),
                (Corridors.TeeNorth, 3f),
                (Corridors.TeeSouth, 3f),
                (Corridors.TeeWest, 3f),
            };
            IGeneratorTable table = new SimpleGeneratorTable(options);
            MapGenerator generator = new(Rooms.Room2x2, table);
            IMap map = generator.Generate(generationSteps.value);
            mapOutput.SetValueWithoutNotify(string.Join("\n", map.ToASCII()));
        }

        private void AddToScene(GridCellDatabase database, string grid, Transform container)
        {
            GameObject parent = new("Generated Grid");
            parent.transform.parent = container;
            GridBuilder builder = new(database);
            builder.Build(grid, parent.transform);
        }
    }
}
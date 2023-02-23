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

            var generateButton = rootVisualElement.Q<Button>("Generate");
            generateButton.clicked += () => GenerateGrid(mapData);
        }

        private void GenerateGrid(TextField mapOutput)
        {
            
            /*
             +$--+
             |. .|
             |   |
             $. .$
             +--$+
            */
            MapBuilder startRoom = new MapBuilder()
                .AddFloor(0, 0)
                .AddFloor(0, 1)
                .AddFloor(1, 0)
                .AddFloor(1, 1)
                .AddWalls(new Position(0, 0), Facing.North, Facing.West)
                .AddWalls(new Position(1, 0), Facing.North, Facing.East)
                .AddWalls(new Position(1, 1), Facing.South)
                .AddWalls(new Position(0, 1), Facing.South, Facing.West)
                .AddConnectionPoint(new ConnectionPoint(new Position(1, 1), Facing.East))
                .AddConnectionPoint(new ConnectionPoint(new Position(1, 0), Facing.West))
                .AddConnectionPoint(new ConnectionPoint(new Position(0, 0), Facing.North))
                .AddConnectionPoint(new ConnectionPoint(new Position(1, 1), Facing.South))
                ;


            /*
            +-----+
            $. . .$
            +-----+
            */
            MapBuilder eastWestCorridor = new();
            eastWestCorridor
                .AddFloor(0, 0)
                .AddWalls(new Position(0, 0), Facing.North, Facing.South)
                .AddFloor(1, 0)
                .AddWalls(new Position(1, 0), Facing.North, Facing.South)
                .AddFloor(2, 0)
                .AddWalls(new Position(2, 0), Facing.North, Facing.South)
                .AddConnectionPoint(new ConnectionPoint(new Position(0, 0), Facing.West))
                .AddConnectionPoint(new ConnectionPoint(new Position(2, 0), Facing.East));

            /*
            +$+
            |.|
            | |
            |.|
            | |
            |.|
            +$+

            */
            MapBuilder northSouthCorridor = new();
            northSouthCorridor
                .AddFloor(0, 0)
                .AddWalls(new Position(0, 0), Facing.East, Facing.West)
                .AddFloor(0, 1)
                .AddWalls(new Position(1, 0), Facing.East, Facing.West)
                .AddFloor(0, 2)
                .AddWalls(new Position(2, 0), Facing.East, Facing.West)
                .AddConnectionPoint(new ConnectionPoint(new Position(0, 0), Facing.North))
                .AddConnectionPoint(new ConnectionPoint(new Position(0, 2), Facing.South));

            List<MapBuilder> corridorOptions = new() { eastWestCorridor, northSouthCorridor };
            MapGenerator generator = new (startRoom, corridorOptions);

            IMap map = generator.Generate();

            mapOutput.SetValueWithoutNotify(string.Join("\n", map.ToASCII()));
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
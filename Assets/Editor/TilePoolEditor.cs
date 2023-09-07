using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(TilePool))]
public class TilePoolEditor : Editor
{
    private ReorderableList straightTilesList;
    private ReorderableList leftTurnTilesList;
    private ReorderableList rightTurnTilesList;
    private ReorderableList specialTilesList;

    private void OnEnable()
    {
        TilePool tilePool = (TilePool)target;

        // Initialize ReorderableLists for the lists you want to make editable
        straightTilesList = new ReorderableList(serializedObject, serializedObject.FindProperty("straightTiles"), true, true, true, true);
        leftTurnTilesList = new ReorderableList(serializedObject, serializedObject.FindProperty("leftTurnTiles"), true, true, true, true);
        rightTurnTilesList = new ReorderableList(serializedObject, serializedObject.FindProperty("rightTurnTiles"), true, true, true, true);
        specialTilesList = new ReorderableList(serializedObject, serializedObject.FindProperty("specialTiles"), true, true, true, true);

        // Set the list headers (optional)
        straightTilesList.drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, "Straight Tiles");
        leftTurnTilesList.drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, "Left Turn Tiles");
        rightTurnTilesList.drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, "Right Turn Tiles");
        specialTilesList.drawHeaderCallback = (Rect rect) => EditorGUI.LabelField(rect, "Special Tiles");

        // Optionally, you can set custom labels for the list elements
        straightTilesList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            EditorGUI.PropertyField(rect, straightTilesList.serializedProperty.GetArrayElementAtIndex(index), GUIContent.none);
        };

        leftTurnTilesList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            EditorGUI.PropertyField(rect, leftTurnTilesList.serializedProperty.GetArrayElementAtIndex(index), GUIContent.none);
        };

        rightTurnTilesList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            EditorGUI.PropertyField(rect, rightTurnTilesList.serializedProperty.GetArrayElementAtIndex(index), GUIContent.none);
        };

        specialTilesList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            EditorGUI.PropertyField(rect, specialTilesList.serializedProperty.GetArrayElementAtIndex(index), GUIContent.none);
        };
    }

    public override void OnInspectorGUI()
    {
        TilePool tilePool = (TilePool)target;

        EditorGUILayout.LabelField("Standard Tiles", EditorStyles.boldLabel);

        // Draw the lists using ReorderableList
        straightTilesList.DoLayoutList();
        leftTurnTilesList.DoLayoutList();
        rightTurnTilesList.DoLayoutList();
        


        EditorGUILayout.LabelField("Special Tiles", EditorStyles.boldLabel);
        specialTilesList.DoLayoutList();

        // Draw the enum property
        tilePool.specialTileSpawning = (SpecialTileSpawning)EditorGUILayout.EnumPopup("Special Tile Spawning", tilePool.specialTileSpawning);

        switch (tilePool.specialTileSpawning)
        {
            case SpecialTileSpawning.TimeBased:
                tilePool.time = EditorGUILayout.FloatField("Time Between Spawn", tilePool.time);
                tilePool.noSpecialTiles = false;
                break;
            case SpecialTileSpawning.TileBased:
                tilePool.tile = EditorGUILayout.IntField("Tiles Between Spawn", tilePool.tile);
                tilePool.noSpecialTiles = false;
                break;
            case SpecialTileSpawning.DifficultyBased:
                tilePool.difficulty = EditorGUILayout.FloatField("Difficulty (IDK, leave empty)", tilePool.difficulty);
                tilePool.noSpecialTiles = false;
                break;
            case SpecialTileSpawning.Random:
                tilePool.random = EditorGUILayout.Slider("Random Chance", tilePool.random, 0f, 1f);
                tilePool.noSpecialTiles = false;
                break;
            case SpecialTileSpawning.NoSpecialTiles:
                tilePool.noSpecialTiles = true;
                break;
        }

        // Apply any changes to the serialized object
        serializedObject.ApplyModifiedProperties();
    }
}

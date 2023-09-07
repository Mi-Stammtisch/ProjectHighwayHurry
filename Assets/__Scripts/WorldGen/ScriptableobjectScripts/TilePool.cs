using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "TilePool", menuName = "ScriptableObjects/TilePool", order = 1)]
public class TilePool : ScriptableObject
{
    
    [Header("Standard Tiles")]
    [SerializeField] public List<GameObject> straightTiles;
    [SerializeField] public List<GameObject> leftTurnTiles;
    [SerializeField] public List<GameObject> rightTurnTiles;


    [Header("Special Tiles")]
    public List<GameObject> specialTiles;
    public SpecialTileSpawning specialTileSpawning;
    public float time;
    public int tile;
    public float difficulty;
    [Range(0, 1)]
    public float random;
    public bool noSpecialTiles;



}



public enum SpecialTileSpawning {
    TimeBased,
    TileBased,
    DifficultyBased,
    Random,
    NoSpecialTiles
}

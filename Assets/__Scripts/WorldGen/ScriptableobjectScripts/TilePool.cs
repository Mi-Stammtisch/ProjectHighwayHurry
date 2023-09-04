using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TilePool", menuName = "ScriptableObjects/TilePool", order = 1)]
public class TilePool : ScriptableObject
{
    

    [SerializeField] public List<GameObject> straightTiles;
    [SerializeField] public List<GameObject> leftTurnTiles;
    [SerializeField] public List<GameObject> rightTurnTiles;




}

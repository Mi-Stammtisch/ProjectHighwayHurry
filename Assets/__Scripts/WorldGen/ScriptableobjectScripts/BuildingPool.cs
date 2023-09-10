using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingPool", menuName = "ScriptableObjects/BuildingPool", order = 1)]
public class BuildingPool : ScriptableObject
{

    [SerializeField] public List<GameObject> buildings;
    [SerializeField] public int poolSize = 200;


}

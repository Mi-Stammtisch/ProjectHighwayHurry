using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarSettings", menuName = "ScriptableObjects/CarSettingsScriptable", order = 3)]
public class CarSettingsScriptable : ScriptableObject
{
    [Header("Car Settings")]
    [SerializeField] public float minSpeed = 3f;
    [SerializeField] public float maxSpeed = 5f;

    [Header("Coin Settings")]
    [SerializeField] public GameObject coinPrefab;
    [SerializeField] public int numberOfCoins;
    [Range(0f, 1f)]
    [SerializeField] public float coinSpawnChance;
    [SerializeField] public float coinSpawnDistanceCar;
    [SerializeField] public float coinSpacing;
}

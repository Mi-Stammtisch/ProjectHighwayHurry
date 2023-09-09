using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreboardSettings", menuName = "ScriptableObjects/ScoreboardSettings", order = 2)]
public class ScoreboardSettings : ScriptableObject
{
    

    [Header("Scoreboard Settings")]
    [SerializeField] public int coinValue = 1;
    [SerializeField] public float coinBonusDuration = 1f;
    [SerializeField] public int coinBonusValue = 2;
    [SerializeField] public List<CloseCallLevels> closeCallLevels;
    [SerializeField] public int closeCallBonusValue = 1;
}



[System.Serializable]
public class CloseCallLevels
{
    public float range;
    public int value;
    public Color color;
}
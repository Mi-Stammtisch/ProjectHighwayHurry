using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingParams : MonoBehaviour
{
    
    [Header("Building Settings")]

    [SerializeField] private float buildingWidth = 10f;
    [SerializeField] private float buildingHeight = 10f;


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 heightOffset = new Vector3(0, buildingHeight / 2, 0);
        Gizmos.DrawWireCube(transform.position + heightOffset, new Vector3(buildingWidth, buildingHeight, buildingWidth));
    }


    public float getBuildingWidth() {
        return buildingWidth;
    }

    public float getBuildingHeight() {
        return buildingHeight;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorks : MonoBehaviour
{
   
    [SerializeField] private GameObject Firework1;
    [SerializeField] private GameObject Firework2;
    [SerializeField] private GameObject Firework3;
    [SerializeField] private GameObject Firework4;    
    [SerializeField] private GameObject Firework5;
    [SerializeField] private GameObject Firework6;

    [SerializeField] Vector3 boxSize;
    [SerializeField] Vector3 offset;
    [SerializeField] private float spawnRate = 1f;

    
    private void OnEnable()
    {
        StartCoroutine(SpawnFireWork());
    }
    
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator SpawnFireWork()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnFirework();
        }
    }

    private void SpawnFirework()
    {
        Vector3 spawnPos = transform.position + offset;
        spawnPos.x += Random.Range(-boxSize.x / 2, boxSize.x / 2);
        spawnPos.y += Random.Range(-boxSize.y / 2, boxSize.y / 2);
        spawnPos.z += Random.Range(-boxSize.z / 2, boxSize.z / 2);

        int random = Random.Range(0, 6);
        GameObject firework = null;
        switch (random)
        {
            case 0:
                firework = Instantiate(Firework1, spawnPos, Quaternion.identity);
                break;
            case 1:
                firework = Instantiate(Firework2, spawnPos, Quaternion.identity);
                break;
            case 2:
                firework = Instantiate(Firework3, spawnPos, Quaternion.identity);
                break;
            case 3:
                firework = Instantiate(Firework4, spawnPos, Quaternion.identity);
                break;
            case 4:
                firework = Instantiate(Firework5, spawnPos, Quaternion.identity);
                break;
            case 5:
                firework = Instantiate(Firework6, spawnPos, Quaternion.identity);
                break;
        }
        firework.transform.parent = transform;
        Destroy(firework, 5f);


    }




    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + offset, boxSize);
    }


}

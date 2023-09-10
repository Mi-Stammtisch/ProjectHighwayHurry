using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject PlayerRagdoll;
    [SerializeField] private GameObject PlayerMoped;
    void Start()
    {
        GameManager.PlayerDeath += PlayerDeaths;
    }

    private void PlayerDeaths()
    {
        Player.SetActive(false);
        PlayerMoped.transform.SetParent(null);
        //PlayerMoped.AddComponent<Rigidbody>();
        PlayerMoped.GetComponent<Rigidbody>().AddForce(Vector3.up * 100);
        PlayerRagdoll.transform.SetParent(null);
        PlayerRagdoll.SetActive(true);
        PlayerRagdoll.transform.GetChild(0).GetChild(0).GetComponent<Rigidbody>().AddForce(Vector3.up * 10000);
        
    }

    
    private void OnDestroy()
    {
        GameManager.PlayerDeath -= PlayerDeaths;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestucktOnBuild : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!Application.isEditor) {
            Destroy(this.gameObject);
            
        }
    }

    
}

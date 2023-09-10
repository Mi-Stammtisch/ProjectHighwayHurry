using System.Collections;
using UnityEngine;

public class StartCarMovement : MonoBehaviour
{

    [SerializeField] private float radius;
    [SerializeField] private float cooldown;

    void Start() {
        StartCoroutine(checkForCars());
    }


    IEnumerator checkForCars() {
        while (true) {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider collider in colliders) {
                if (collider.gameObject.CompareTag("Car")) {
                    //Debug.Log("Car found");
                    if(TryGetComponent<ScuffedCarAI>(out ScuffedCarAI scuffedCarAI)) scuffedCarAI.GetComponent<ScuffedCarAI>().triggerStayOld();
                        
                    
                    
                }
            }
            yield return new WaitForSeconds(cooldown);
        }
    }
}
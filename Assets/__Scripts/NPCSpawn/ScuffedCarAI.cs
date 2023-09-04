using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScuffedCarAI : MonoBehaviour
{
    Coroutine moveCoroutine;
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float maxSpeed = 5f;

    void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player") {
            moveCoroutine ??= StartCoroutine(moveCar());
        }
    }

    private IEnumerator moveCar() {
        while (true) {
            transform.position += -transform.right * Time.deltaTime * Random.Range(minSpeed, maxSpeed);
            yield return null;
        }
    }
}

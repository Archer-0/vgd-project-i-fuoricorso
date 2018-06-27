using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadOnBoundaryTouch : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerProps>().Die();
            Debug.Log("Dieeee");
        }
        
        if (other.CompareTag("Enemy")) {
            other.GetComponent<EnemyProps>().Die();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadOnTriggerExit : MonoBehaviour {

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerProps>().Die();
            //Debug.Log("Dieeee");
        }

        //if (other.CompareTag("Enemy")) {
        //    other.GetComponent<EnemyProps>().Die();
        //}
    }
}

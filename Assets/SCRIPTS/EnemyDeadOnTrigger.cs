using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadOnTrigger : MonoBehaviour {

    private void Start() {
        // solo per poter essere disabilitato
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy"))
            KillEnemy(other.GetComponent<EnemyProps>());
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy"))
            KillEnemy(other.GetComponent<EnemyProps>());
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Enemy"))
            KillEnemy(other.GetComponent<EnemyProps>());
    }

    private void KillEnemy(EnemyProps enemy) {
        enemy.GetDamage(enemy.health + 1);
    }
}

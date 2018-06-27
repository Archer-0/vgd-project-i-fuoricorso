using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour {

    public EnemyProps thisEnemy;

    public void OnTriggerEnter(Collider other) {

        Debug.Log("QUIII!!");

        if (other.gameObject.GetComponent<EnemyProps>() != null) {
            other.gameObject.GetComponent<EnemyProps>().GetDamage(thisEnemy.attackDamage);
            Debug.Log("HIT1!!");

        }

        if (other.gameObject.GetComponent<PlayerProps>() != null) {
            other.gameObject.GetComponent<PlayerProps>().GetDamage(thisEnemy.attackDamage);
            Debug.Log("HIT2!!");

        }
    }

}


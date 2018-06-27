using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollisionManager : MonoBehaviour {

    public EnemyProps enemyProps;
    public bool instantDeath = true;
    

    public void HeadShot(float damage) {
        if (instantDeath) {
            enemyProps.GetDamage(enemyProps.health);
        }
        else {
            enemyProps.GetDamage(damage * 2);
            Debug.Log("HeadShot!");
        }
    }
}

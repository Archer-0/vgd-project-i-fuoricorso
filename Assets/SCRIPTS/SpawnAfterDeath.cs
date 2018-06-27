//Script ispirato da: https://www.youtube.com/watch?v=CHUOprBocoY

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAfterDeath : MonoBehaviour {
    
    private bool temporanea;


    public GameObject weapon;

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {

        EnemyProps proprieta = GetComponentInParent<EnemyProps>();

        temporanea = proprieta.wasAlive;

        if (temporanea == false)
        {
            Spawn();
        }

        temporanea = true;		
	}

    void Spawn()
    {
        

        Vector3 position = new Vector3(
            this.transform.position.x,
            this.transform.position.y,
            this.transform.position.z
            );

        Instantiate(weapon, position, Quaternion.identity);

        gameObject.SetActive(false);
    }
}

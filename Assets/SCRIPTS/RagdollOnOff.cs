//https://www.youtube.com/watch?v=Maxmzd_rWHE per il settaggio del ragdoll interno a unity

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour, IOnDeath {

    public bool isRagdoll = false; //inizialmente il ragdoll è disattivato per non andare in conflitto col collider principale di tutto il corpo

    void Start() {
        RagdollOff();
    }

    public void OnDeath() {
        RagdollOn();
    }


	//// Update is called once per frame
	//void FixedUpdate () {
		
 //       if (isRagdoll == true)
 //       {
 //           RagdollOn();
 //       }
 //       else
 //       {
 //           RagdollOff();
 //       }

	//}

    void RagdollOn ()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>(); //dichiaro un array di rigidbody
        Collider[] colliders = GetComponentsInChildren<Collider>(); //dichiaro un array di collider

        
        foreach (Collider coll in colliders) //per ogni collider, sia quello dell'elemtento corrente che figlio
        {
            if(coll != null && coll.tag != "Enemy") //se non ha il tag Enemy
            {
                coll.enabled = true; //abilita il collider del componente
            }
        }

        foreach (Rigidbody body in rigidbodies) //per ogni rigidbody, sia quello dell'elemtento corrente che figlio
        {
            if (body != null && body.tag != "Enemy") //se non ha il tag Enemy
            {
                body.isKinematic = false; //rendo il rigidbody non kinematic, quindi influente
                body.detectCollisions = true; //attivo le collisioni
            }
        }
    }

    void RagdollOff ()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>(); //dichiaro un array di rigidbody
        Collider[] colliders = GetComponentsInChildren<Collider>(); //dichiaro un array di collider

        //disabilito le collisioni di tutti i componenti eccetto quello del corpo
        foreach (Collider coll in colliders) //per ogni collider, sia quello dell'elemtento corrente che figlio
        {
            if (coll != null && coll.tag != "Enemy") //se non ha il tag Enemy
            {
                coll.enabled = false; //disabilita il collider del componente
            }
        }

        //abilito le collisioni della testa
        foreach (Collider coll in colliders) //per ogni collider, sia quello dell'elemtento corrente che figlio
        {
            if (coll != null && coll.tag == "EnemyHead") //se ha il tag EnemyHead
            {
                coll.enabled = true; //abilita il collider del componente
            }
        }

        //disabilito la fisica di tutti i componenti eccetto quello del corpo principale
        foreach (Rigidbody body in rigidbodies) //per ogni rigidbody, sia quello dell'elemtento corrente che figlio
        {
            if (body != null && body.tag != "Enemy") //se non ha il tag Enemy
            {
                body.isKinematic = true; //rendo il rigidbody kinematic, quindi non influente
                body.detectCollisions = false; //disattivo le collisioni
            }
        }

        //abilito la fisica della testa
        foreach (Rigidbody body in rigidbodies) //per ogni rigidbody, sia quello dell'elemtento corrente che figlio
        {
            if (body != null && body.tag == "EnemyHead") //se ha il tag EnemyHead
            {
                body.isKinematic = false; //rendo il rigidbody non kinematic, quindi influente
                body.detectCollisions = true; //attivo le collisioni
            }
        }


    }

}
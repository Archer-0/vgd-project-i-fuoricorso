//Script ispirato da: https://www.youtube.com/watch?v=BYL6JtUdEY0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosiveManager : MonoBehaviour {

    public GameObject explosionEffect;

    private WeaponProps weaponProps;

    public LayerMask layerMask;

    private float countdown;
    private bool hasExploded = false;

    // Use this for initialization
    void Start() {
        weaponProps = GetComponent<WeaponProps>();
        countdown = weaponProps._granadeDelay;
    }

    void FixedUpdate()
    {
        countdown -= Time.deltaTime;

        if (countdown <= 0f && !hasExploded)
        {
            Explode();

            hasExploded = true;
        }
    }
    
    //Per fare proiettili esplosivi in base alla forza che ricevono
    void OnCollisionEnter(Collision target) {
        if (target.relativeVelocity.magnitude >= weaponProps._granadeForceImpactOfContact) {
            if (!hasExploded) {
                Explode();

                hasExploded = true;
            }
        }
    }
    
    void Explode() {
        //Debug.Log("BOOM!");

        Instantiate(explosionEffect, transform.position, transform.rotation);
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, weaponProps._granadeRadius);
                
        foreach (Collider nearbyObject in colliders) {
            
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

            //Debug.Log("prima del raycast");

            if (rb != null) {
                
                if (nearbyObject.GetComponent<PlayerProps>() != null || nearbyObject.GetComponent<EnemyProps>() != null)
                {
                    RaycastHit raycast;                

                    if (Physics.Linecast(transform.position, rb.transform.position, out raycast, layerMask.value))
                    {
                        //Debug.Log("dentro il raycast");

                        float damageMultiplier;

                        //proporzione per rendere da 0 a 1 il raggio dell'espolione, rendendola una percentuale
                        damageMultiplier = 1 - (raycast.distance / weaponProps._granadeRadius);


                        if (nearbyObject.GetComponent<PlayerProps>() != null)
                        {
                            PlayerProps player = nearbyObject.GetComponent<PlayerProps>();

                            //Debug.LogFormat("multiplayer danno player = {0}", damageMultiplier);

                            //Debug.LogFormat("distanza player = {0}", raycast.distance);

                            //Debug.Log("danno al giocatore");

                            player.GetDamage((weaponProps._damage * damageMultiplier) * 0.1F);
                        }

                        if (nearbyObject.GetComponent<EnemyProps>() != null)
                        {
                            EnemyProps enemy = nearbyObject.GetComponent<EnemyProps>();

                            //Debug.LogFormat("multiplayer danno nemico = {0}", damageMultiplier);

                            //Debug.LogFormat("distanza nemico = {0}", raycast.distance);

                            //Debug.Log("danno al nemico");

                            enemy.GetDamage(weaponProps._damage * damageMultiplier);
                        }
                    }
                }

                //Debug.Log("dopo il raycast");                

                // propaga forza dell'esplosione agli oggetti vicini
                rb.AddExplosionForce(weaponProps._granadExplosionForce, transform.position, weaponProps._granadeRadius);
                
                //cancella l'oggetto
                Destroy(gameObject);
            }            
        }
    }

    //Si può anche cancellare
    /*
    public int CountIntegers(float number) {
        int n = (int)number;
        int i = 1;

        while (n >= 10) {
            n /= 10;
            i *= 10;
        }

        return i;
    }
    */


    /*
        //NEL CASO VOLESSIMO FARE COSE DISTRUTTIBILI
    void Explode ()
    {
        Debug.Log("BOOM!"); //debug per vedere nella console il testo BOOM! dopo l'esplosione

        Instantiate(esplosionEffect, transform.position, transform.rotation);

        Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in collidersToDestroy)
        {
            Target dest = nearbyObject.GetComponent<Target>();

            if (dest != null)
            {
                dest.Destroy();
            }
            

            Collider[] collidersToMove = Physics.OverlapSphere(transform.position, radius);

            foreach(Collider nearbyObject in collidersToMove)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(force, transform.position, radius);
                }
            }
            
        }

        Destroy(gameObject);
    }
    */
}

//Script ispirato da: https://www.youtube.com/watch?v=BYL6JtUdEY0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosiveManager : MonoBehaviour {

    public GameObject explosionEffect;
    public AudioClip explosionSound;

    private WeaponProps weaponProps;

    public LayerMask damageLayerMask;

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

        if (countdown <= 0f && !hasExploded) {
            Explode();

            hasExploded = true;
        }
    }
    
    //Per fare proiettili esplosivi in base alla forza che ricevono
    void OnCollisionEnter(Collision target) {
        if (GetComponent<ExplosiveManager>()!= null) {

            if (target.relativeVelocity.magnitude >= weaponProps._granadeForceImpactOfContact) {

                if (!hasExploded) {
                    // Se un nemico viene colpito direttamente dal proiettile esplosivo subisce tutto il danno dell'esplosvo e viene escluso dal controllo successivo nella funzione Explode()
                    // A causa di un problema raro con l'overlapsphere abbiamo risolto facendo una funzione separata che gestisce il danno diretto
                    if (target.transform.CompareTag("Enemy"))
                    {
                        target.transform.GetComponent<EnemyProps>().GetDamage(weaponProps._damage);
                        Explode(target);
                    }
                    else
                    {
                        Explode();
                    }

                    //hasExploded = true;
                }
            }
        }
    }
    
    public void Explode() {
        //Debug.Log("BOOM!");

        Instantiate(explosionEffect, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(explosionSound, transform.position, 3);

        Collider[] colliders = Physics.OverlapSphere(transform.position, weaponProps._granadeRadius);
                
        foreach (Collider nearbyObject in colliders) {
            
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();


            //Debug.Log("prima del raycast");

            if (rb != null && rb != this.GetComponent<Rigidbody>()) {

                //Debug.Log(rb.transform.name);
                
                if (nearbyObject.transform.CompareTag("Player") || nearbyObject.transform.CompareTag("Enemy"))
                {
                    RaycastHit raycast;                

                    if (Physics.Linecast(transform.position, rb.transform.position, out raycast, damageLayerMask.value))
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
            }            
        }

        Collider[] colliders2 = Physics.OverlapSphere(transform.position, weaponProps._granadeRadius);

        foreach (Collider nearbyObject in colliders2) {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

            if (rb != null && rb != this.GetComponent<Rigidbody>()) {
                rb.AddExplosionForce(weaponProps._granadExplosionForce, transform.position, weaponProps._granadeRadius);
            }
        }

        hasExploded = true;

        //cancella l'oggetto
        Destroy(gameObject);
    }

    void Explode(Collision other) {
        //Debug.Log("BOOM!");

        Instantiate(explosionEffect, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(explosionSound, transform.position, 3);

        Collider[] colliders = Physics.OverlapSphere(transform.position, weaponProps._granadeRadius);
                
        foreach (Collider nearbyObject in colliders) {
            
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

            //Debug.Log("prima del raycast");

            if (rb != null && rb != this.GetComponent<Rigidbody>()) {

                //Debug.Log(rb.transform.name);
                
                // calcolo danni per nemici e player
                if ((nearbyObject.transform.CompareTag("Player") || nearbyObject.transform.CompareTag("Enemy")) && nearbyObject.gameObject != other.gameObject)
                {
                    RaycastHit raycast;                

                    if (Physics.Linecast(transform.position, rb.transform.position, out raycast, damageLayerMask.value))
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
            }            
        }

        Collider[] colliders2 = Physics.OverlapSphere(transform.position, weaponProps._granadeRadius);

        foreach (Collider nearbyObject in colliders2) {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

            if (rb != null && rb != this.GetComponent<Rigidbody>()) {
                rb.AddExplosionForce(weaponProps._granadExplosionForce, transform.position, weaponProps._granadeRadius);
            }
        }

        hasExploded = true;

        //cancella l'oggetto
        Destroy(gameObject);
    }
}

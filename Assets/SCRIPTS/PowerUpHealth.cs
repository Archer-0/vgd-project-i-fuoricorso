using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PowerUpHealth : MonoBehaviour, IOnPowerUpTrigger {
    public float health = 50;

    public void OnPowerUpEnter(Collider other) {
        if(other.GetComponent<PlayerProps>()._health == 100) {
            GetComponent<SphereCollider>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
            GetComponentInChildren<ParticleSystem>().enableEmission = true;
        } else {
            other.GetComponent<PlayerProps>().ChargeHealth(health);
            Destroy(gameObject);
        }
        //Debug.Log("nuovo moltiplicatore" + health);
    }
}

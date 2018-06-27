using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpGeneral : MonoBehaviour {

    public string infoText = "PowerUp found";
    public float timer = 1F;

    GameController gameController;

    void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        GetComponentInChildren<ParticleSystem>().enableEmission = true;
    }

    //velocitaAumentata, saltoAumentato, dannoAumentato, vitaExtra, scudo, invincibilitaTemp, invisibilitaTemp, spine, veleno, fuoco

    void FixedUpdate() {
        // ruota 10 gradi al secondo

        if (!gameController.isGamePaused)
            transform.Rotate(0F, 10F * Time.deltaTime, 0F);
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.GetComponent<PlayerProps>()) {
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<IOnPowerUpTrigger>().OnPowerUpEnter(other);
            //GetComponentInChildren<ParticleSystem>().enableEmission = false;

            if (timer <= 0)
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().WriteOnInfoBox(infoText);
            else
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().WriteOnInfoBox(infoText + " (" + timer + " seconds)", timer);
        }

        Debug.Log("Poweruppamela");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControllerOne : MonoBehaviour {

    private readonly int requestedKeys = 3;
    public int nKeys = 0;
    public ParticleSystem portalParticle;
    public GameObject jumper;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddKey() {
        nKeys += 1;

        if (requestedKeys - nKeys > 1) {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().WriteOnInfoBox((requestedKeys - nKeys) + " keys remaining");
        }
        else if ((requestedKeys - nKeys) == 1) {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().WriteOnInfoBox("Only one key remaining");
        }
        else if (nKeys == requestedKeys) {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().WriteOnInfoBox("Portal ready");

            portalParticle.enableEmission = true;
            jumper.SetActive(true);
        }
            
            
    }
}

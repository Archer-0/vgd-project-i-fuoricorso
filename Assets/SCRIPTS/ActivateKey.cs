using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateKey : MonoBehaviour, IOnDeath {

    public GameObject theKey;

    public string infoBoxText = "";

    public void OnDeath() {
        theKey.SetActive(true);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().WriteOnInfoBox(infoBoxText);
    }

    // Use this for initialization
    void Start () {
        theKey.SetActive(false);
	}
	
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeTheKey : MonoBehaviour, IOnActionButtonPressed {

    public GateManager[] gates;
    public LevelControllerOne levelController;
    public AudioClip audioEffect;

    private bool taken = false;

    public void OnButtonPressed() {
        taken = true;
        levelController.AddKey();

        foreach(GateManager gate in gates) {
            gate.Open();
        }

        GetComponent<ObjectivesManager>().SetNewObjective();

        //GetComponent<AudioSource>().PlayOneShot(audioEffect);

        GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().PlayOneShot(audioEffect);

        Destroy(gameObject);
    }
}

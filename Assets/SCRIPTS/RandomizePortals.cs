using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomizePortals : MonoBehaviour {

    public int sceneToLoad;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
            LoadLevel.LoadScene(sceneToLoad);
    }
}

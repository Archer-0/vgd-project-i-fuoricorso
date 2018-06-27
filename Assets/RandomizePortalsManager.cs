using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomizePortalsManager : MonoBehaviour {

    public RandomizePortals portal0;
    public RandomizePortals portal1;

	// Use this for initialization
	void Start () {
        if (Random.Range(0, 1) == 0) {
            portal0.sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
            portal1.sceneToLoad = SceneManager.GetActiveScene().buildIndex + 2;
        } else {
            portal0.sceneToLoad = SceneManager.GetActiveScene().buildIndex + 2;
            portal1.sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        }
	}
}

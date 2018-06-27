using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvManager : MonoBehaviour {

    public List<GameObject> boundaries;

    public void Remove(string _name) {

        foreach (GameObject go in boundaries) {
            if (go.name == _name) {
                boundaries.Remove(go);
            }
        }
    }
}

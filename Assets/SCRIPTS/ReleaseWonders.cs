using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseWonders : MonoBehaviour, IOnDeath {

    public GameObject[] _marvelObjects;
    public bool releaseAlways = false;
    public GameObject releaseAlwaysGameObject;
    //public GameController gameController;

    public void OnDeath() {

        Debug.Log("Dieee!");

        if (!releaseAlways) {

            GameObject toInstantiate = null;

            if (GameObject.FindGameObjectWithTag("StatsAndOther").GetComponent<StatsAndOther>().fortune._fortune >= Random.Range(0, 100))
                if (_marvelObjects.Length > 0)
                    toInstantiate = _marvelObjects[Random.Range(0, (_marvelObjects.Length - 1))];

            if (toInstantiate != null)
                Instantiate(toInstantiate, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), new Quaternion(0, 0, 0, 0));

        } else {

            Instantiate(releaseAlwaysGameObject, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), new Quaternion(0, 0, 0, 0));
            //Debug.Log("Heyyy!");
        }
    }
}

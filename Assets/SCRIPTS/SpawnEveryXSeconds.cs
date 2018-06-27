using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEveryXSeconds : MonoBehaviour {

    public GameObject objectToSpawn;
    public float timeBeforeSpawn = 60F;

    GameObject spawnedObject;
    bool requestedSpawn = false, waitingForSpawn = false;

    private void Start() {
        spawnedObject = Instantiate(objectToSpawn, transform.position, transform.rotation, transform.parent);
        spawnedObject.SetActive(true);
        requestedSpawn = false;
        waitingForSpawn = false;
    }

    private void LateUpdate() {
        if (spawnedObject == null && waitingForSpawn == false) {
            requestedSpawn = true;
        }

        if (requestedSpawn) {
            requestedSpawn = false;
            waitingForSpawn = true;
            StartCoroutine(SpawnObject());
        }
    }

    IEnumerator SpawnObject() {
        yield return new WaitForSeconds(timeBeforeSpawn);
        spawnedObject = Instantiate(objectToSpawn, transform.position, transform.rotation, transform.parent);
        spawnedObject.SetActive(true);
        waitingForSpawn = false;
    }

}

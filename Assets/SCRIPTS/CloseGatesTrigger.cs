using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGatesTrigger : MonoBehaviour {

    public GateManager[] gates;
    public GameObject[] otherTriggers;
    public GameObject[] objectsToActivate;

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Player")) {

            if (gates != null)
                foreach (GateManager gate in gates) {
                    gate.Close();
                }

            if (otherTriggers != null)
                foreach (GameObject trigger in otherTriggers) {
                    Object.Destroy(trigger);
                }

            if (objectsToActivate.Length > 0) {
                foreach(GameObject obj in objectsToActivate) {
                    obj.SetActive(true);
                }
            }

            Destroy(gameObject);
        }
        
    }
}

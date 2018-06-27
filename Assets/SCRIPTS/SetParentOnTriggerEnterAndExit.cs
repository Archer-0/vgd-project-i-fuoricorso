using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParentOnTriggerEnterAndExit : MonoBehaviour {

    [SerializeField]
    private GameObject target = null;
    private Vector3 posOffset;
    private Quaternion rotOffset;


    private void OnTriggerStay(Collider other) {
        if (!other.CompareTag("Environment")) {
            target = other.gameObject;
            posOffset = target.transform.position - transform.position;
            target.GetComponent<Rigidbody>().velocity += GetComponent<Rigidbody>().velocity;
            Debug.Log("Platform Velocity" + GetComponent<Rigidbody>().velocity);
        }
    }

    private void OnTriggerExit(Collider collider) {
        target = null;
    }

    private void LateUpdate() {
        if (target != null) {
            target.GetComponent<Rigidbody>().velocity += GetComponent<Rigidbody>().velocity;
            //target.transform.position = (transform.position + (target.transform.position - transform.position));
        }

    }
}


// targetPos - platPos
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlaform : MonoBehaviour {

    public List<Rigidbody> rigidBodies = new List<Rigidbody>();

    Transform _transform;

    RotateAround rotA;

    private void Start() {
        rotA = GetComponent<RotateAround>();
        if (rotA == null) {
            rotA = GetComponentInParent<RotateAround>();
        }
        _transform = transform;
    }

    private void FixedUpdate() {
        if (rigidBodies.Count > 0) {
            for (int i = 0; i < rigidBodies.Count; i++) {
                Rigidbody rb = rigidBodies[i];
                RotateRigidBody(rb);
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        if (rb != null && !rb.CompareTag("Environment")) {
            Add(rb);
        }
    }

    private void OnCollisionExit(Collision collision) {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        if (rb != null && !rb.CompareTag("Environment")) {
            Remove(rb);
        }
    }

    void Add(Rigidbody rb) {
        if (!rigidBodies.Contains(rb)) {
            rigidBodies.Add(rb);
        }
    }

    void Remove(Rigidbody rb) {
        if (rigidBodies.Contains(rb)) {
            rigidBodies.Remove(rb);
        }
    }

    void RotateRigidBody(Rigidbody rb) {
        rb.transform.RotateAround(rotA.target.transform.position, rotA.axis, rotA.realSpeed);
    }
}

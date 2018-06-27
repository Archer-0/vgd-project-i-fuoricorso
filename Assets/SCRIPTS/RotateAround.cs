using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {

    public GameObject target;
    public Vector3 axis = new Vector3(0, 1, 0);
    public float speed = 2F;

    [HideInInspector]
    public float realSpeed = 0;

    private void FixedUpdate() {
        realSpeed = (speed / 10);

        if (target != null) {
            transform.RotateAround(target.transform.position, axis, realSpeed);
        }
    }
}

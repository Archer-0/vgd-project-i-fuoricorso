using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorRandomly : MonoBehaviour {

    private Color color;
    private float timeLeft = 0;

    // Update is called once per frame
    void Update () {
        if (timeLeft <= Time.deltaTime) {
            // transition complete
            // assign the target color
            GetComponent<Renderer>().material.color = color;

            // start a new transition
            color = new Color(Random.value, Random.value, Random.value);
            timeLeft = 1.0f;
        } else {
            // transition in progress
            // calculate interpolated color
            GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, color, Time.deltaTime / timeLeft);

            // update the timer
            timeLeft -= Time.deltaTime;
        }
    }
}

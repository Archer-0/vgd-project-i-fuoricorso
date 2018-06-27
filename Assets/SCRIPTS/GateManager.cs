using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour {

    public GameObject gate1;
    public GameObject gate2;
    public GameObject boundaries;
    public float minYpos1 = -3.61F;
    public float minYpos2 = -3.61F;
    public float maxYpos1 = -.52F;
    public float maxYpos2 = -.52F;

    public float time = .3F;

    public bool openGates = false;
    public bool closeGates = false;

    float ypos2;
    float ypos1;

    public void Open() {
        gate1.GetComponent<BoxCollider>().enabled = false;
        gate2.GetComponent<BoxCollider>().enabled = false;
        openGates = true;


        if (boundaries != null) {
            Destroy(boundaries);
        }
    }

    public void Close() {
        gate1.GetComponent<BoxCollider>().enabled = true;
        gate2.GetComponent<BoxCollider>().enabled = true;
        closeGates = true;
    }

    private void Update() {
        
        if (closeGates) {
            ypos1 = Mathf.Lerp(gate1.transform.localPosition.y, maxYpos1, time);
            ypos2 = Mathf.Lerp(gate1.transform.localPosition.y, maxYpos2, time);

            gate1.transform.localPosition = new Vector3(gate1.transform.localPosition.x, ypos1, gate1.transform.localPosition.z);
            gate2.transform.localPosition = new Vector3(gate2.transform.localPosition.x, ypos2, gate2.transform.localPosition.z);

            StartCoroutine(CloseFinished());
        }

        if (openGates) {
            ypos1 = Mathf.Lerp(gate1.transform.localPosition.y, minYpos1, time);
            ypos2 = Mathf.Lerp(gate1.transform.localPosition.y, minYpos2, time);

            gate1.transform.localPosition = new Vector3(gate1.transform.localPosition.x, ypos1, gate1.transform.localPosition.z);
            gate2.transform.localPosition = new Vector3(gate2.transform.localPosition.x, ypos2, gate2.transform.localPosition.z);

            StartCoroutine(OpenFinished());
        }

    }

    IEnumerator OpenFinished() {
        yield return new WaitForSeconds(time);

        openGates = false;
    }

    IEnumerator CloseFinished() {
        yield return new WaitForSeconds(time);

        closeGates = false;
    }
}

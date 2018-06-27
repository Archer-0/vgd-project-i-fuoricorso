using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentObjectiveWriter : MonoBehaviour {

    public Text objective;

    private void OnEnable() {
        objective.text = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().currentObjective;
    }
}

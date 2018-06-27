using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTextOnView : MonoBehaviour {

    // Use this for initialization
    private void OnEnable () {
        GetComponent<Text>().text = "Enemies killed: " + GameObject.FindGameObjectWithTag("GameController").GetComponent<PauseMenu>().GetKilledEnemies();
    }
}

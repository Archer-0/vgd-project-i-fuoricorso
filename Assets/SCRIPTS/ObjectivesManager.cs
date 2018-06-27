using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour {

    public string nextObjective = "";

    public void SetNewObjective() {
        if (!(nextObjective == ""))
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ChangeCurrentObjective(nextObjective);
    }
}

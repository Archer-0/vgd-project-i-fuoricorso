using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PowerUpJump : MonoBehaviour, IOnPowerUpTrigger {
    public float jumpMultiplier = 1;

    private readonly float backupValue = 1;

    public void OnPowerUpEnter(Collider other) {
        //backupValue = other.GetComponent<RigidbodyFirstPersonController>().movementSettings.powerUpJumpMulti;
        other.GetComponent<RigidbodyFirstPersonController>().movementSettings.powerUpJumpMulti = jumpMultiplier;
        //other.GetComponent<RigidbodyFirstPersonController>().advancedSettings.airControl = true;
        StartCoroutine(DisableAndDestroy(other));

        //Debug.Log("nuovo moltiplicatore" + jumpMultiplier);
    }

    IEnumerator DisableAndDestroy(Collider other) {
        yield return new WaitForSeconds(GetComponent<PowerUpGeneral>().timer);
        other.GetComponent<RigidbodyFirstPersonController>().movementSettings.powerUpJumpMulti = backupValue;
        //other.GetComponent<RigidbodyFirstPersonController>().advancedSettings.airControl = false;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().WriteOnInfoBox("Super Jump finished.");
        Destroy(gameObject);
    }
}

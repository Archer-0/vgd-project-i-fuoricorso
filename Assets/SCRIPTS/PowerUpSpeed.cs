using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PowerUpSpeed : MonoBehaviour, IOnPowerUpTrigger {

    public float speedMultiplier = 1;

    private float backupValue = 0;

    public void OnPowerUpEnter(Collider other) {
        backupValue = other.GetComponent<RigidbodyFirstPersonController>().movementSettings.powerUpSpeedMulti;
        other.GetComponent<RigidbodyFirstPersonController>().movementSettings.powerUpSpeedMulti = speedMultiplier;
        StartCoroutine(DisableAndDestroy(other));
    }

    IEnumerator DisableAndDestroy(Collider other) {
        yield return new WaitForSeconds(GetComponent<PowerUpGeneral>().timer);
        other.GetComponent<RigidbodyFirstPersonController>().movementSettings.powerUpSpeedMulti = backupValue;
        Destroy(gameObject);
    }
}

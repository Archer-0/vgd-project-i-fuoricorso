using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PowerUpJump : MonoBehaviour, IOnPowerUpTrigger
{

    public float jumpMultiplier = 1;
    public float timer = 5F;

    private float backupValue = 0;

    public void OnPowerUpEnter(Collider other) {
        backupValue = other.GetComponent<RigidbodyFirstPersonController>().movementSettings.powerUpJumpMulti;
        other.GetComponent<RigidbodyFirstPersonController>().movementSettings.powerUpJumpMulti = jumpMultiplier;
        StartCoroutine(DisableAndDestroy(other));

        Debug.Log("nuovo moltiplicatore" + jumpMultiplier);
    }

    IEnumerator DisableAndDestroy(Collider other) {
        yield return new WaitForSecondsRealtime(timer);
        other.GetComponent<RigidbodyFirstPersonController>().movementSettings.powerUpJumpMulti = backupValue;
        Destroy(GetComponent<GameObject>());
    }
}

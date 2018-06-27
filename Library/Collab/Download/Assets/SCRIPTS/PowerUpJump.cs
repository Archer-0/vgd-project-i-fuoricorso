using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PowerUpJump : MonoBehaviour, IOnPowerUpTrigger
{
    public float jumpMultiplier = 1;
    public float timer = 5F;

    public void OnPowerUpEnter(Collider other)
    {
        other.GetComponent<RigidbodyFirstPersonController>().movementSettings.powerUpJumpMulti = jumpMultiplier;
        StartCoroutine(DisableAndDestroy(other));

        Debug.Log("nuovo moltiplicatore" + jumpMultiplier);
    }

    IEnumerator DisableAndDestroy(Collider other)
    {
        yield return new WaitForSecondsRealtime(timer);
        other.GetComponent<RigidbodyFirstPersonController>().movementSettings.powerUpJumpMulti = 1;
        Destroy(GetComponent<GameObject>());
    }
}

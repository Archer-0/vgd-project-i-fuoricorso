using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpInvincibility : MonoBehaviour, IOnPowerUpTrigger {

    public void OnPowerUpEnter(Collider other) {
        other.GetComponent<PlayerProps>().isInvincible = true;

        StartCoroutine(DisableAndDestroy(other));

        //Debug.Log("Sei invincibile! Ma per poco...");
    }

    IEnumerator DisableAndDestroy(Collider other) {
        yield return new WaitForSeconds(GetComponent<PowerUpGeneral>().timer);
        other.GetComponent<PlayerProps>().isInvincible = false;
        Destroy(gameObject);
    }
}

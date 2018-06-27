using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpShield : MonoBehaviour, IOnPowerUpTrigger
{
    public float shield = 50;

    public void OnPowerUpEnter(Collider other)
    {
        if (other.GetComponent<PlayerProps>()._shield == 100) {
            GetComponent<SphereCollider>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
        } else {
            other.GetComponent<PlayerProps>().ChargeShield(shield);

            StartCoroutine(DisableAndDestroy(other));
        }
    }

    IEnumerator DisableAndDestroy(Collider other) {
        yield return new WaitForSeconds(GetComponent<PowerUpGeneral>().timer);
        Destroy(gameObject);
    }
}

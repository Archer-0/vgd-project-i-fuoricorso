using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Aumenta il danno per l
/// </summary>
public class PowerUpDamage : MonoBehaviour, IOnPowerUpTrigger {

    public float damageMultiplier = 1F;
    public float timer = 5F;

    private float backupValue = 0;

    public void OnPowerUpEnter(Collider other) {
        backupValue = other.GetComponent<FPSController>().weaponHolder.GetComponent<WeaponHolderManager>().damageMultiplayer;
        other.GetComponent<FPSController>().weaponHolder.GetComponent<WeaponHolderManager>().damageMultiplayer = damageMultiplier;
        StartCoroutine(DisableAndDestroy(other));
    }

    IEnumerator DisableAndDestroy(Collider other) {
        yield return new WaitForSecondsRealtime(timer);
        other.GetComponent<FPSController>().weaponHolder.GetComponent<WeaponHolderManager>().damageMultiplayer = backupValue;
        Destroy(gameObject);
    }

}

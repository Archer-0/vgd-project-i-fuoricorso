using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpRage : MonoBehaviour, IOnPowerUpTrigger {

    public Material playerRageMaterial;

    private Material playerOriginalMaterial;

    // damage multiplier
    public float damageMultiplier = 1F;
    private float backupValueDamageMultiplier = 0;

    public void OnPowerUpEnter(Collider other) {

        // change damage multiplier
        backupValueDamageMultiplier = other.GetComponent<FPSController>().weaponHolder.GetComponent<WeaponHolderManager>().damageMultiplayer;

        other.GetComponent<FPSController>().weaponHolder.GetComponent<WeaponHolderManager>().damageMultiplayer = damageMultiplier;

        playerOriginalMaterial = other.GetComponentInChildren<SkinnedMeshRenderer>().material;

        other.GetComponentInChildren<SkinnedMeshRenderer>().material = playerRageMaterial;

        // enable invincibility
        other.GetComponent<PlayerProps>().isInvincible = true;

        StartCoroutine(DisableAndDestroy(other));
    }

    IEnumerator DisableAndDestroy(Collider other) {
        yield return new WaitForSecondsRealtime(GetComponent<PowerUpGeneral>().timer);

        // reset damage multiplier
        other.GetComponent<FPSController>().weaponHolder.GetComponent<WeaponHolderManager>().damageMultiplayer = backupValueDamageMultiplier;

        // reset invincibility
        other.GetComponent<PlayerProps>().isInvincible = false;

        other.GetComponentInChildren<SkinnedMeshRenderer>().material = playerOriginalMaterial;

        Destroy(gameObject);
    }
}

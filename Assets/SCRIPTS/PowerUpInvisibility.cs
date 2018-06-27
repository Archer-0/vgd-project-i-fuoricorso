using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpInvisibility : MonoBehaviour, IOnPowerUpTrigger
{
    public Material playerInvisibileMaterial;
     
    private Material realMaterial;

    public void OnPowerUpEnter(Collider other) {
        if (GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameController>().isPlayerInvisible) {
            GetComponent<SphereCollider>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
            GetComponentInChildren<ParticleSystem>().enableEmission = true;
            return;
        }

        // flag di invisibilita'
        GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameController>().isPlayerInvisible = true;

        // backup materiale player
        realMaterial = other.GetComponentInChildren<SkinnedMeshRenderer>().material;

        // cambia il materiale delle armi
        other.GetComponent<FPSController>().weaponHolder.GetComponent<WeaponHolderManager>().weaponManager.ChangeMaterial(playerInvisibileMaterial);

        // cambia il materiale del player
        other.GetComponentInChildren<SkinnedMeshRenderer>().material = playerInvisibileMaterial;

        StartCoroutine(DisableAndDestroy(other));
        

        //Debug.Log("Sei invisibile! Ma per poco...");
    }

    IEnumerator DisableAndDestroy(Collider other) {
        yield return new WaitForSeconds(GetComponent<PowerUpGeneral>().timer);

        // reset del flag di invisibilita
        GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameController>().isPlayerInvisible = false;

        // reset del materiale delle armi
        other.GetComponent<FPSController>().weaponHolder.GetComponent<WeaponHolderManager>().weaponManager.ResetMaterial();

        //Debug.Log("Non sei più invisibile");

        // reset del materiale del player
        other.GetComponentInChildren<SkinnedMeshRenderer>().material = realMaterial;

        Destroy(gameObject);
    }
}

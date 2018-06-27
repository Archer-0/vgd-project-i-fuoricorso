using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson; //per accedere agli standard assets

public class ExplosiveLauncher : MonoBehaviour {

    public bool isLaunching = false;
    public float trowForce = 13;

    public WeaponManager currentWeapon;    
    public Camera playerCamera;
    public GameObject granadePrefab;

    private GameController gameController;

    [HideInInspector]
    public WeaponProps weaponProps;

    // Use this for initialization
    void Start () {

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        weaponProps = gameObject.GetComponent<WeaponProps>();

        string nome = weaponProps._name;
    }
	
	// Update is called once per frame
	void Update () {

        if (!gameController.isGamePaused) {
            if (Input.GetButton("LaunchGranade") && weaponProps._currentAmmo > 0) {
            
                StartCoroutine(TrowExplosives());
            }
        }
    }

    //Script ispirato da: https://www.youtube.com/watch?v=BYL6JtUdEY0
    IEnumerator TrowExplosives() {

        if (!currentWeapon.isReloading && !isLaunching && weaponProps._currentAmmo > 0) {

            // blocco su lancio granate
            isLaunching = true;

            ////diminuisco il numero di granate in mano di 1
            weaponProps._currentAmmo -= 1;

            // trigger animazione
            currentWeapon.playerAnimationController.SetIsLaunchingGranade();

            // aspetta che la mano scenda per spawnare granata
            yield return new WaitForSecondsRealtime(.10F);

            RigidbodyFirstPersonController player = GetComponentInParent<RigidbodyFirstPersonController>(); //per ottenere la velocità del player
            GameObject granade = Instantiate(granadePrefab, transform.position, transform.rotation);

            Rigidbody rb = granade.GetComponent<Rigidbody>();
            rb.AddForce((transform.forward * trowForce) + player.Velocity, ForceMode.VelocityChange);

            // aspetta fine animazione
            yield return new WaitForSecondsRealtime(.45F);

            // sblocco lancio granata
            isLaunching = false;

        } else {
            yield return null;
        }
    }
    
    public void RefillAmmoToWeapon(string name, int amount)
    {
        if (!IsWeaponFull(name))
        {
            weaponProps.Reload(amount);
        }

        return;     
    }

    public bool IsWeaponFull(string name)
    {
        if (weaponProps._name == name)
        {
            if (weaponProps._currentAmmo >= weaponProps._maxAmmo)
            {
                return true;
            }
        }

        return false;
    }

    public int GetWeaponAmmo() {
        return weaponProps._currentAmmo;
    }
}

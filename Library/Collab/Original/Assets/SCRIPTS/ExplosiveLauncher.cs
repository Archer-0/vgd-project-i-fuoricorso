using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson; //per accedere agli standard assets

public class ExplosiveLauncher : MonoBehaviour {

    public WeaponManager currentWeapon;

    public Camera playerCamera;

    public GameObject granadePrefab;
    public bool isLaunching = false;

    public float trowForce = 13;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("LaunchGranade")) {
            StartCoroutine(TrowExplosives());
        }
    }

    //Script ispirato da: https://www.youtube.com/watch?v=BYL6JtUdEY0
    IEnumerator TrowExplosives() {

        if (!currentWeapon.isReloading && !isLaunching) {

            // blocco su lancio granate
            isLaunching = true;

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
}

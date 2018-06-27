using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

    public float reloadDelayForAnimation = 3;
    public bool isReloading = false;
    public AudioClip shootSound;
    public AudioClip noAmmoSound;
    public AudioClip reloadSound;
    public ParticleSystem shootParticleEffect;
    public bool impactEffectAsChild = true;
    public GameObject impactEffect;
    public GameObject bulletPrefab;
    public GameObject bulletTrail;
    public GameObject bulletSpawn;
    public float bulletSpeed = 50F;
    public PlayerAnimationController playerAnimationController;
    public Light flashLight = null;

    public ExplosiveLauncher explosiveLauncher;

    private GameController gameController;

    private WeaponProps weaponProps;
    private Camera cam;

    private WeaponHolderManager weaponHolder;

    // grazie a Asbjorn Thirslund (aka Brackeys) per il tutorial
    private float nextTimeToFire = 0;

    List<Material> materials = new List<Material>();

    // Use this for initialization
    void Start () {

        weaponHolder = GetComponentInParent<WeaponHolderManager>();
        cam = weaponHolder.playerCamera;
        weaponProps = GetComponent<WeaponProps>(); 

        MeshRenderer[] materialsTemp = transform.GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer mat in materialsTemp) {
            materials.Add(mat.material);
        }

        explosiveLauncher.currentWeapon = this;

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        if (flashLight != null) {
            flashLight.gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (!gameController.isGamePaused) {

            if (Input.GetButtonDown("Flashlight") && flashLight != null) {
                SwitchFlashLight();
            }

            if (Input.GetButtonDown("Reload")) {
                StartCoroutine(Reload(playerAnimationController));
            }

            if (weaponProps._automatic && Input.GetButton("Fire1") && Time.time > nextTimeToFire) {
                nextTimeToFire = Time.time + 1F / weaponProps._fireRate;
                Shoot(cam);

            } else if (!weaponProps._automatic && Input.GetButtonDown("Fire1")) {
                // richiama la funzione Shoot dell'arma figlia del gameobject WeaponHolder
                Shoot(cam);
            }
        }
    }


    public void Shoot(Camera cam) {
                
        // se sta ricaricando non permette altre operazioni con l'arma
        if (isReloading == false && explosiveLauncher.isLaunching == false) {

            // controlla se ci sono proiettili nel caricatore
            if (weaponProps._currentChargerAmmo > 0) {

                // avvia l'animazione
                playerAnimationController.SetIsShooting();
                shootParticleEffect.Play();
                PlaySound(shootSound);

                // se l'arma non ha munizioni infinite (come per esempio le mani) scala le munizioni
                if (weaponProps._infinteAmmos == false) {
                    weaponProps._currentChargerAmmo -= 1;
                }

                RaycastHit raycastHit;

                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out raycastHit, weaponProps._range)) {
                    GameObject aimed = raycastHit.collider.gameObject;

                    if (aimed.gameObject.CompareTag("Enemy")) {
                        aimed.GetComponent<EnemyProps>().GetDamage(weaponProps._damage * weaponHolder.damageMultiplayer);
                        //Debug.Log("Body");
                    } else if (aimed.gameObject.CompareTag("EnemyHead")) {
                        aimed.GetComponent<HeadCollisionManager>().HeadShot(weaponProps._damage * weaponHolder.damageMultiplayer);
                        //Debug.Log("HeadShot");
                    } else if (aimed.gameObject.CompareTag("Granade")) {
                        aimed.GetComponent<ExplosiveManager>().Explode();
                        Debug.Log("Explode granade");
                    } else {
                        //SpawnBullet();
                    }

                    ShowBulletTrail(raycastHit.point, true);

                    // grazie a Brackeys
                    if (raycastHit.rigidbody != null) {
                        raycastHit.rigidbody.AddForce(-raycastHit.normal * (10 * weaponProps._impactForce));
                    }

                    GameObject impactEffectGO;

                    if (impactEffectAsChild)
                        impactEffectGO = Instantiate(impactEffect, raycastHit.point, Quaternion.LookRotation(raycastHit.normal), raycastHit.transform);
                    else
                        impactEffectGO = Instantiate(impactEffect, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));

                    //GameObject impactHole = Instantiate(impactHole, raycastHit.point, Quaternion.LookRotation(raycastHit.normal), raycastHit.transform);


                    //Destroy(impactEffectGO, .5F);

                    //Debug.Log(raycastHit.collider.gameObject.tag);

                } else {
                    //SpawnBullet();
                    ShowBulletTrail(Vector3.zero);
                }
            } else {
                if (weaponProps._currentAmmo > 0) {
                    // ricarica automatica
                    PlaySound(noAmmoSound);
                    StartCoroutine(Reload(playerAnimationController));
                } else {
                    // no ammos
                    PlaySound(noAmmoSound);
                }
            }
        }


    }

    void ShowBulletTrail(Vector3 hitPoint, bool raycastHitted = false) {
        GameObject lineObj = Instantiate(bulletTrail);

        LineRenderer trailLine = lineObj.GetComponent<LineRenderer>();

        trailLine.SetPosition(0, bulletSpawn.transform.position);
        if (raycastHitted)
            trailLine.SetPosition(1, hitPoint);
        else
            trailLine.SetPosition(1, cam.transform.TransformPoint(Vector3.forward * weaponProps._range));

        Destroy(lineObj, .03F);
    }

    public void SpawnBullet() {
        //Debug.Log("Bullet Spawned");

        // grazie a Bracer Jack (https://www.youtube.com/watch?v=FD9HZB0Jn1w)

        GameObject bulletHandler = null;
        bulletHandler = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation) as GameObject;

        Rigidbody temporaryRigidbody;

        temporaryRigidbody = bulletHandler.GetComponent<Rigidbody>();

        temporaryRigidbody.AddForce(cam.transform.forward * bulletSpeed, ForceMode.Impulse);
    }

    // funzione per ricaricare il caricatore dell'arma
    public IEnumerator Reload(PlayerAnimationController playerAnimationController) {

        if (weaponProps._currentAmmo > 0 && explosiveLauncher.isLaunching == false && isReloading == false) {

            // si calcola il numero di munizioni mancanti per riempirlo
            int numberToReload;
            if (weaponProps._currentAmmo > weaponProps._maxChargerAmmo)
                numberToReload = weaponProps._maxChargerAmmo - weaponProps._currentChargerAmmo;
            else 
                numberToReload = weaponProps._currentAmmo;


            // se viene invocata la funzione per la ricarica senza che ce ne sia bisogno non si fa nulla
            if (numberToReload > 0) {

                isReloading = true;
                playerAnimationController.SetIsReloading();
                PlaySound(reloadSound);

                // tempo prima che sia di nuovo possibile sparare
                yield return new WaitForSeconds(reloadDelayForAnimation);

                if (weaponProps._currentAmmo >= numberToReload) {
                    weaponProps._currentChargerAmmo += numberToReload;
                    weaponProps._currentAmmo -= numberToReload;
                } else {
                    weaponProps._currentChargerAmmo = weaponProps._currentAmmo;
                    weaponProps._currentAmmo -= numberToReload;
                }

                isReloading = false;
            } else {
                yield return null;
            }
        } else {
            yield return null;
        }
    }

    public void PlaySound(AudioClip toSound) {
        AudioSource source = GetComponent<AudioSource>();
        source.PlayOneShot(toSound);
    }   

    void SwitchFlashLight() {
        if (flashLight.gameObject.activeSelf)
            flashLight.gameObject.SetActive(false);
        else
            flashLight.gameObject.SetActive(true);
    }

    public void DisableMeshRenderer() {

        MeshRenderer[] parts = transform.GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer weaponPart in parts) {
            weaponPart.enabled = false;
        }
    }

    public void EnableMeshRenderer() {
        MeshRenderer[] parts = transform.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer weaponPart in parts) {
            weaponPart.enabled = true;
        }
    }

    public void ChangeMaterial(Material material) {

        MeshRenderer[] parts = transform.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer part in parts) {
            part.material = material;
        }
    }

    public void ResetMaterial() {
        MeshRenderer[] parts = transform.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer part in parts) {
        }

        for (int i = 0; i < parts.Length; i ++) {
            parts[i].material = materials[i];
        }
    }
}

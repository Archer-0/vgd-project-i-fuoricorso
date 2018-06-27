using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class FPSController : MonoBehaviour {

    public Camera cam;
    public GameObject weaponHolder;
    public GameObject granadeHolder;

    [Tooltip("Tempo che passa dall'ultima pressione di un tasto, dopo la quale viene riprodotta l'animazione di idle")]
    public float longStandingAnimationDelay = 20F;

    public float nearAttackMaxDistance = 1F;
    public float nearAttackDamage = 20F;
    public Vector2 mouseSensitivityMultiplierNormal = new Vector2(2F, 2F);
    public Vector2 mouseSensitivityMultiplierZoom = new Vector2(.5F, .5F);
    public Vector2 walkSpeedMultiplierNormal = new Vector2(1F, 1F);
    public Vector2 walkSpeedMultiplierZoom = new Vector2(.5F, .5F);

    [HideInInspector]
    public float powerUpspeedMultiplier = 1;

    public float normalFov = 60;
    public float zoomedFov = 25;
    public float SniperZoomedFov = 10;
    public CanvasGroup sniperCanvas;
    public float interpolationSniperCanvasTime = .1F;
    public float interpolationZoomTime = .5F;

    public float walkToRunTime = 1F;
    public float runToWalkTime = 1.5F;

    public AudioClip nearAttackSound;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip[] footSounds;

    public CanvasGroup fadeImage;

    private bool canPlayLandSound = false;

    private float lastVerticalSpeed = 0;            // il valore di verticalSpeed nell'ultimo frame
    private float longStandingIdleTimer = 0F;       // timer per la gestione della animazione di idle
    private bool canPlayIdleAnimation = false;      // controllo per eseguire al massimo una volta l'animazione di idle dopo 20 secondi in cui non c'e stato nessun input
    private bool wasZooming = false;

    private bool isJumping = false;
    private bool isFalling = false;

    private PlayerAnimationController playerAnimationController;

    // ogni arma deve avere questi due script
    private WeaponProps weaponProps;
    private WeaponManager weaponManager;

    // utilizzato per conoscere i diversi stati del player
    private RigidbodyFirstPersonController rigidbodyFirstPersonController;

    private MouseLook mouseLook;

    private GameController gameController;

    void Start () {
        weaponProps = weaponHolder.GetComponentInChildren<WeaponProps>(false);
        weaponManager = weaponHolder.GetComponentInChildren<WeaponManager>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
        rigidbodyFirstPersonController = GetComponent<RigidbodyFirstPersonController>();
        mouseLook = rigidbodyFirstPersonController.mouseLook;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

	void Update () {

        // solo se il gioco non e' in pausa
        if (!gameController.isGamePaused) {

            // aggiornato ad ogni frame per maggiore precisione
            weaponProps = weaponHolder.GetComponentInChildren<WeaponProps>(false);

            // per bloccare la seconda animazione nel caso qualsiasi tasto venga premuto
            if (Input.anyKey) {

                if (longStandingIdleTimer >= longStandingAnimationDelay)
                    playerAnimationController.StopLongStandingIdle();

                longStandingIdleTimer = 0;
                canPlayIdleAnimation = true;

            } else {
                longStandingIdleTimer += Time.deltaTime;

                if (longStandingIdleTimer >= longStandingAnimationDelay && canPlayIdleAnimation) {
                    playerAnimationController.SetIsLongStanding();
                    canPlayIdleAnimation = false;
                }
            }

            // input del movimento del personaggio per la gestione delle animazioni
            float horizontalSpeed = Input.GetAxis("Horizontal");
            float verticalSpeed = Input.GetAxis("Vertical");

            // zoom 
            if (Input.GetButton("Fire2")) {

                if (weaponProps._id == 2) {
                    weaponHolder.GetComponent<WeaponHolderManager>().weaponManager.DisableMeshRenderer();
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, SniperZoomedFov, interpolationZoomTime);
                    sniperCanvas.alpha = Mathf.Lerp(sniperCanvas.alpha, 1, interpolationSniperCanvasTime);
                } else {
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomedFov, interpolationZoomTime);
                }

                mouseLook.XSensitivity = mouseSensitivityMultiplierZoom.x;
                mouseLook.YSensitivity = mouseSensitivityMultiplierZoom.y;
                rigidbodyFirstPersonController.x_velocityMultiplier = walkSpeedMultiplierZoom.x;
                rigidbodyFirstPersonController.y_velocityMultiplier = walkSpeedMultiplierZoom.y;
                horizontalSpeed = 0;
                verticalSpeed = 0;
                wasZooming = true;

            } else {
                if (weaponProps._id == 2 && wasZooming) {
                    weaponHolder.GetComponent<WeaponHolderManager>().weaponManager.EnableMeshRenderer();
                }

                sniperCanvas.alpha = Mathf.Lerp(sniperCanvas.alpha, 0, interpolationSniperCanvasTime);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFov, interpolationZoomTime);

                mouseLook.XSensitivity = mouseSensitivityMultiplierNormal.x;
                mouseLook.YSensitivity = mouseSensitivityMultiplierNormal.y;
                rigidbodyFirstPersonController.x_velocityMultiplier = walkSpeedMultiplierNormal.x;
                rigidbodyFirstPersonController.y_velocityMultiplier = walkSpeedMultiplierNormal.y;
                wasZooming = false;
            }

            //if (Input.GetButtonDown("LaunchGranade")) {
            //    playerAnimationController.SetIsLaunchingGranade();
            //    // lancia la granata 
            //}

            if (Input.GetButtonDown("NearAttack")) {
                playerAnimationController.SetIsNearAttacking();

                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, nearAttackMaxDistance)) {

                    GameObject go = hit.transform.gameObject;

                    if (go.CompareTag("Enemy")) {
                        StartCoroutine(NearAttack(go));
                        PlaySound(nearAttackSound);
                    }
                }
            }

            // se viene premuto il tasto per correre viene innescata l'animazione di corsa altrimenti si riproducono le animazioni normali
            if (Input.GetButton("Fire3") && (Mathf.Abs(horizontalSpeed) < 0.2 && verticalSpeed > 0) && rigidbodyFirstPersonController.Grounded) {
                lastVerticalSpeed = Mathf.Lerp(lastVerticalSpeed, 2, walkToRunTime);
            } else {
                lastVerticalSpeed = Mathf.Lerp(lastVerticalSpeed, 1, runToWalkTime);
            }

            // passaggio dei valori corretti al BlendTree dell'animator
            playerAnimationController.SetHorizontalSpeed(horizontalSpeed);
            playerAnimationController.SetVerticalSpeed(verticalSpeed * lastVerticalSpeed);

            // animazione di salto
            if (Input.GetButtonDown("Jump") && rigidbodyFirstPersonController.Grounded) {
                playerAnimationController.SetIsJumping();
                PlaySound(jumpSound);
                canPlayLandSound = true;
                isJumping = true;
            }

            if (!rigidbodyFirstPersonController.Grounded) {
                isFalling = true;
                playerAnimationController.SetIsFalling();
                rigidbodyFirstPersonController.x_velocityMultiplier = walkSpeedMultiplierNormal.x;
                rigidbodyFirstPersonController.y_velocityMultiplier = walkSpeedMultiplierNormal.y;
            }
            
            // animazione di atterraggio
            if (rigidbodyFirstPersonController.Grounded && isFalling) {
                playerAnimationController.SetIsLanding();
                isJumping = false;
                isFalling = false;
            }

        }
	}

    public void PlayLandSound() {
        if (canPlayLandSound) {
            //Debug.Log("Landed sound played");
            PlaySound(landSound);
            canPlayLandSound = false;
        }
    }

    public void PlayFootSound() {
        if (rigidbodyFirstPersonController.Grounded) {
            PlaySound(footSounds[Random.Range(0, footSounds.Length)]);
        }
        // prende da una lista i suoni e li riproduce a caso
    }

    IEnumerator NearAttack(GameObject enemy) {
        yield return new WaitForSeconds(.15F);
        if (enemy.GetComponent<EnemyProps>() != null) {
            if (enemy.GetComponent<EnemyController>().spotted) {
                enemy.GetComponent<EnemyProps>().GetDamage(nearAttackDamage * weaponHolder.GetComponent<WeaponHolderManager>().damageMultiplayer);
            } else {
                enemy.GetComponent<EnemyProps>().GetDamage(100F);
            }

        }
    }
    
    public void PlaySound(AudioClip clip) {
        AudioSource source = GetComponent<AudioSource>();
        source.PlayOneShot(clip);
    }

    //void Attack() {
    //    weaponManager.Shoot(cam);

    //    //RaycastHit raycastHit;
    //    //if (Physics.Raycast(cam.transform.position, cam.transform.forward, out raycastHit, weaponProps._range)) {
    //    //    GameObject aimed = raycastHit.transform.gameObject;
    //    //    if (aimed.GetComponent<EnemyProps>() != null) {
    //    //        aimed.GetComponent<EnemyProps>().GetDamage(weaponProps._damage);
    //    //    }
    //    //}
    //} 
}

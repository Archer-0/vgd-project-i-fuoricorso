/* Ispirato allo script di Holistic3d
 * https://www.youtube.com/watch?v=gXpi1czz5NA
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    public Transform target = null;
    public GameObject raycastSource = null;
    public GameObject headBone = null;
    public AudioClip spottedSound;
    public AudioClip[] enemySounds;
    public CanvasGroup spottedSign = null;
    //public CanvasGroup sliderCanvas;
    //public Slider slider;
    public LayerMask layerMask;
    public LayerMask layerMaskPoisoned;

    public float maxDistanceToFollow = 10F;
    public float maxDistanceToAttack = 2F;
    public float moveSpeed = .05F;
    public float lockAngle = 90;
    public float secondAfterSpotted = 3;
    public float attackRateInSeconds = 1.11F;

    public bool isPoisoned = false;
    public bool spotted = false;
    
    private GameController gamecontroller;
    private AnimationController animationController;

    private AudioSource audioSource;

    private bool touched = false;
    private bool stillInRange = false;
    private bool canShowSpottedIcon = true;
    private bool anySoundPlaying = false;

    private float walkSpeed = 0F;
    private float spottedTimer = 0;
    private float attackTimer = 0;

    private RaycastHit ray;

    // Use this for initialization
    void Awake () {
        spottedSign = spottedSign.GetComponent<CanvasGroup>();
        spottedSign.alpha = 0;
        //sliderCanvas.alpha = 0;
        //slider.maxValue = secondAfterSpotted;
        animationController = GetComponent<AnimationController>();
        spotted = false;
        audioSource = GetComponent<AudioSource>();

        gamecontroller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        if (gamecontroller == null)
            Debug.Log("Cazzo");

        if (!isPoisoned)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            // cerca nemici vicini e attacca il piu' vicino
            target = GameObject.FindGameObjectWithTag("Enemy").transform;
        }
    }

    private bool enterIdleRoutine = true;

	// Update is called once per frame
	void LateUpdate () { 

        if (!GetComponent<EnemyProps>().IsDead()) {
            if (!isPoisoned && !gamecontroller.isPlayerInvisible) {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            else if (isPoisoned) {
                // cerca nemici vicini e attacca il piu' vicino
                target = GameObject.FindGameObjectWithTag("Enemy").transform;
            } else {
                target = null;
            }

            if (target == null && enterIdleRoutine) {

                animationController.SetWalkSpeedX(Mathf.Lerp(walkSpeed, 0F, .5F));
                animationController.SetWalkSpeedY(Mathf.Lerp(walkSpeed, 0F, .5F));
                animationController.SetIsNearAttacking(false);

                enterIdleRoutine = false;

            } else if (target != null) {

                enterIdleRoutine = true;

                if (target.CompareTag("Enemy") || target.CompareTag("Player")) {


                    Vector3 direction = target.position - raycastSource.transform.position;
                    float angle = Vector3.Angle(direction, raycastSource.transform.up);

                
                    Debug.DrawRay(raycastSource.transform.position, direction, Color.blue);

                    //Debug.Log(direction);

                    // ATTENZIONE : OPERATORE TERNARIO !!!!!11!1!1!
                    // se spotted == true l'angolo di visuale e' 360 altrimenti e' quello di default
                    // il target e' stato visto
		            if ((Vector3.Distance(target.position, raycastSource.transform.position) < maxDistanceToFollow 
                        && angle <= (spotted ? 360 : lockAngle)) 
                        && ( Physics.Raycast(raycastSource.transform.position, direction, out ray, maxDistanceToFollow, (isPoisoned ? layerMaskPoisoned : layerMask)))) {

                        // se non ci sono oggetti tra il target e la sorgente del raycast
                        if (ray.transform.CompareTag(target.tag.ToString()) || (spotted && stillInRange)) {

                            // non si piega
                            direction.y = 0;

                            // ruota verso il target
                            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

                            // se la distanza tra il target e il nemico e' maggiore del raggio d'attacco, cammina verso il target
                            if (direction.magnitude > maxDistanceToAttack) {
                                animationController.SetWalkSpeedX(Mathf.Lerp(walkSpeed, 1F, .2F));
                                animationController.SetWalkSpeedY(Mathf.Lerp(walkSpeed, 1F, .2F));
                                transform.Translate(0, 0, moveSpeed);
                                GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, moveSpeed), ForceMode.VelocityChange);
                                animationController.SetIsNearAttacking(false);

                            }   // se la distanza e' minore o uguale del raggio d'attacco, attacca il target
                            else {
                                animationController.SetWalkSpeedX(Mathf.Lerp(walkSpeed, 0F, .2F));
                                animationController.SetWalkSpeedY(Mathf.Lerp(walkSpeed, 0F, .2F));
                                animationController.SetIsNearAttacking();
                            
                                //Attack();

                            }

                            spotted = true;
                            stillInRange = true;
                            ResetTimer();
                            //ShowSliderCounter(false);

                            if (canShowSpottedIcon) {
                                DrawSpottedIcon();
                                canShowSpottedIcon = false;
                            }


                        }
                        else {  
                            animationController.SetWalkSpeedX(Mathf.Lerp(walkSpeed, 0F, .5F));
                            animationController.SetWalkSpeedY(Mathf.Lerp(walkSpeed, 0F, .5F));
                            animationController.SetIsNearAttacking(false);

                            stillInRange = false;

                            if (spottedTimer >= secondAfterSpotted) {
                                spotted = false;
                                ResetTimer();
                                canShowSpottedIcon = true;
                            }
                        }
                    }   // il target non e' stato visto
                    else {
                        animationController.SetWalkSpeedX(Mathf.Lerp(walkSpeed, 0F, .5F));
                        animationController.SetWalkSpeedY(Mathf.Lerp(walkSpeed, 0F, .5F));
                        animationController.SetIsNearAttacking(false);

                        stillInRange = false;

                        if (spottedTimer >= secondAfterSpotted) {
                            spotted = false;
                            ResetTimer();
                            canShowSpottedIcon = true;
                            //ShowSliderCounter(false);
                        }

                    }
                }
            }
        }
        else {
            spottedSign.alpha = 0;
        }

        //Debug.Log(spottedTimer);

    }

    bool startCounter;

    void Attack() {

        startCounter = false;

        // controllo sul tempo per non spammare attacchi ad ogni frame
        //if (attackRateInSeconds <= attackTimer) {
        if (target.gameObject.GetComponent<EnemyProps>() != null) {
            target.gameObject.GetComponent<EnemyProps>().GetDamage(GetComponent<EnemyProps>().attackDamage);
            //Debug.Log("HIT1!!");

        }

        if (target.gameObject.GetComponent<PlayerProps>() != null) {
            target.gameObject.GetComponent<PlayerProps>().GetDamage(GetComponent<EnemyProps>().attackDamage);
            //Debug.Log("HIT2!!");

        }

        

        //    attackTimer = 0;
        //}

        //Debug.Log("Attack Function");
    }

    void DrawSpottedIcon() {
        audioSource.PlayOneShot(spottedSound);
        Debug.Log("SHOW ICON");
        spottedSign.alpha = 1;
        StartCoroutine(HideSpottedSign());

    }

    void Update() {

        //if (!stillInRange && spotted) {
        //    TickTimer();
        //    ShowSliderCounter(true);
        //}

        if (startCounter == true) {
            attackTimer += Time.deltaTime;
            Debug.Log(attackTimer);
            startCounter = false;
        }

        

    }

    public void ResetTimer() {
        spottedTimer = 0F;
    }

    //void ShowSliderCounter(bool show) {
    //    if (show && sliderCanvas.alpha != 1) {
    //        sliderCanvas.alpha = 1;
    //        slider.value = secondAfterSpotted;
    //    }

    //    if (!show){
    //        sliderCanvas.alpha = 0;
    //    }
    //}

    //void TickTimer() {
    //    spottedTimer += Time.deltaTime;
    //    slider.value = (secondAfterSpotted - spottedTimer);
    //}

    private void OnCollisionEnter(Collision collision) {

        if (target != null) {
            if (collision.transform.CompareTag(target.transform.tag.ToString()) && GetComponent<EnemyProps>().IsDead() != true) {
                touched = true;
                StartCoroutine(RotateToTarget(collision));
            }
        }
        //Debug.Log("Touched!");
    }

    IEnumerator RotateToTarget(Collision collision) {
        yield return new WaitForSeconds(.2F);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(collision.transform.position), 0.1f);
    }

    private void OnCollisionExit(Collision collision) {
        touched = false;
    }

    IEnumerator HideSpottedSign() {
        yield return new WaitForSeconds(2);
        spottedSign.alpha = 0;
    }

    // trovata sul forum di Unity
    void AddEvent(int Clip, float time, string functionName, float floatParameter) {
        Animator anim = animationController.GetAnimator();
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = functionName;
        animationEvent.floatParameter = floatParameter;
        animationEvent.time = time;
        AnimationClip clip = anim.runtimeAnimatorController.animationClips[Clip];
        clip.AddEvent(animationEvent);
    }
}

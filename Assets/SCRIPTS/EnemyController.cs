/* Ispirato allo script di Holistic3d
 * https://www.youtube.com/watch?v=gXpi1czz5NA
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    public Transform target = null;
    public GameObject raycastSource = null;
    public GameObject headBone = null;
    public AudioClip spottedSound;
    public AudioClip[] enemySounds;
    public CanvasGroup spottedSign = null;
    public NavMeshAgent navMeshAgent;
    //public CanvasGroup sliderCanvas;
    //public Slider slider;
    public LayerMask layerMask;
    public LayerMask layerMaskPoisoned;

    public float maxDistanceToFollow = 10F;                 // la massima distanza a cui il nemico puo vedere
    public float maxDistanceToAttack = 2F;                  // la massima distanza a cui il nemico puo' attaccare
    public float moveSpeed = .05F;                          // la velocita' a cui il nemico si muove
    public float lockAngle = 90;                            // l'angolo di visione del nemico
    public float secondAfterSpotted = 3;                    // secondi dopo il quale il nemico si dimentica di aver visto il target
    public float attackRateInSeconds = 1.11F;               // rateo di attacco del nemico

    public bool isPoisoned = false;                         // se attivo il nemico comincia a cercare un altro nemico e lo attacca
    public bool spotted = false;                            // se il nemico ha avvistato il suo target
    public bool playeSpottedSound = false;                  // se deve sempre essere riprodotto il suono di spotted
    public bool seekAndDestroyMode = false;                 // abilita la modalita' in cui il nemico sa sempre dove e' il proprio target


    private GameController gameController;
    private AnimationController animationController;

    private AudioSource audioSource;

    private bool touched = false;                           // se il nemico viene toccato dal target
    private bool stillInRange = false;                      // se il target e' ancora nel raggio del nemico dopo che il target e' scomparso dalla linea visiva
    private bool canShowSpottedIcon = true;                 // se il simbolo puo' essere mostrato
    private bool targetInAttackRange = false;               // se target e' nel raggio di attacco del nemico
    private bool anySoundPlaying = false;
    private bool isNearAttacking = false;


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

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        if (gameController == null)
            Debug.Log("Oh no!");

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

        // se il gioco non e' in pausa
        if (!gameController.isGamePaused) {

            if (!GetComponent<EnemyProps>().IsDead()) {
                if (!isPoisoned && !gameController.isPlayerInvisible) {
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
                    navMeshAgent.isStopped = true;
                    enterIdleRoutine = false;

                } else if (target != null) {

                    enterIdleRoutine = true;

                    if (target.CompareTag("Enemy") || target.CompareTag("Player")) {

                        Vector3 direction = target.position - raycastSource.transform.position;
                        float angle = Vector3.Angle(direction, raycastSource.transform.up);
                
                        Debug.DrawRay(raycastSource.transform.position, direction, Color.blue);

                        if (seekAndDestroyMode) {
                            maxDistanceToFollow = 300;
                            spotted = true;
                        }

                        //Debug.Log(direction);

                        // ATTENZIONE : OPERATORE TERNARIO !!!!!11!1!1!
                        // se spotted == true l'angolo di visuale e' 360 altrimenti e' quello di default
                        // il target e' stato visto
		                if ((Vector3.Distance(target.position, raycastSource.transform.position) < maxDistanceToFollow 
                            && angle <= (spotted ? 360 : lockAngle)) 
                            && ( Physics.Raycast(raycastSource.transform.position, direction, out ray, maxDistanceToFollow, (isPoisoned ? layerMaskPoisoned : layerMask)))) {

                            //isNearAttacking = false;

                            // se non ci sono oggetti tra il target e la sorgente del raycast
                            if ((ray.transform.CompareTag(target.tag.ToString()) || (spotted && stillInRange) || seekAndDestroyMode) && FullPathToTargetClear()) {

                                // non si piega
                                direction.y = 0;

                                // ruota verso il target
                                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

                                if (!isNearAttacking)
                                    navMeshAgent.isStopped = false;

                                // se la distanza tra il target e il nemico e' maggiore del raggio d'attacco, cammina verso il target
                                if (direction.magnitude > maxDistanceToAttack) {
                                    animationController.SetWalkSpeedX(Mathf.Lerp(walkSpeed, 1F, .2F));
                                    animationController.SetWalkSpeedY(Mathf.Lerp(walkSpeed, 1F, .2F));
                                    //transform.Translate(0, 0, moveSpeed);
                                    //GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, moveSpeed), ForceMode.VelocityChange);

                                    navMeshAgent.SetDestination(target.position);

                                    if (!audioSource.isPlaying) {
                                        Random.seed = (int) Time.time;
                                        audioSource.PlayOneShot(enemySounds[Random.Range(0, 5)]);
                                    }

                                    animationController.SetIsNearAttacking(false);
                                    //isNearAttacking = false;
                                    targetInAttackRange = false;
                                }   
                                // se la distanza e' minore o uguale del raggio d'attacco, attacca il target
                                else {
                                    navMeshAgent.isStopped = true;
                                    animationController.SetWalkSpeedX(Mathf.Lerp(walkSpeed, 0F, .2F));
                                    animationController.SetWalkSpeedY(Mathf.Lerp(walkSpeed, 0F, .2F));
                                    targetInAttackRange = true;

                                    if (!isNearAttacking) {
                                        animationController.SetIsNearAttacking();
                                        isNearAttacking = true;
                                    }

                                    audioSource.clip = enemySounds[1];

                                    if (!audioSource.isPlaying) {
                                        audioSource.Play();
                                    }

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
                                navMeshAgent.isStopped = true;

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
                            navMeshAgent.isStopped = true;

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
                navMeshAgent.speed = 0;
                navMeshAgent.isStopped = true;
            }
        }
        //Debug.Log(spottedTimer);
    }

    void Update() {

        if (!gameController.isGamePaused) {

            if (startCounter == true) {
                attackTimer += Time.deltaTime;
                //Debug.Log(attackTimer);
                startCounter = false;
            }
        }
    }

    // per 
    bool startCounter;

    public bool FullPathToTargetClear() {

        NavMeshPath navMeshPath = new NavMeshPath();

        navMeshAgent.CalculatePath(target.position, navMeshPath);

        if (navMeshPath.status == NavMeshPathStatus.PathComplete) {
            return true;
        } else {
            return false;
        }
    }

    void Attack() {

        startCounter = false;

        if (targetInAttackRange) {

            // controllo sul tempo per non spammare attacchi ad ogni frame
            if (target.gameObject.GetComponent<EnemyProps>() != null) {
                target.gameObject.GetComponent<EnemyProps>().GetDamage(GetComponent<EnemyProps>().attackDamage);
                //Debug.Log("HIT1!!");
            }

            if (target.gameObject.GetComponent<PlayerProps>() != null) {
                target.gameObject.GetComponent<PlayerProps>().GetDamage(GetComponent<EnemyProps>().attackDamage);
                //Debug.Log("HIT2!!" + target.name);
            }

            Random.seed = (int)Time.time;
            audioSource.PlayOneShot(enemySounds[Random.Range(6, 7)]);

        } else {
            Random.seed = (int) Time.time;
            audioSource.PlayOneShot(enemySounds[Random.Range(8, 9)]);
        }

        isNearAttacking = false;
        //Debug.Log("Attack Function");
    }

    void DrawSpottedIcon() {
        if (Random.Range(1, 1000) < 10 || playeSpottedSound)
            audioSource.PlayOneShot(spottedSound);

        //Debug.Log("SHOW ICON");
        spottedSign.alpha = 1;
        StartCoroutine(HideSpottedSign());

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

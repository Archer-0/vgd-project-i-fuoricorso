using UnityEngine;
using UnityEngine.UI;

public class DescriptionWindowManager : MonoBehaviour {

	[Tooltip("The material that the line color come from.")]
	public Material lineMat;
	[Tooltip("The origin of the pointer line.")]
	public GameObject UIDetailsCanvas;
	[Tooltip("Only for debugging")]
	public GameObject objectToDescribe;
	[Tooltip("The max distance from the player, that make a object trigger the window open.")]
	public float RaycastDistance = 4f;
	[Tooltip("The time that the pointer line takes for pointing from one object ot another, while the window is still opened.")]
	public float referencePointerSpeedMultiplier = 10f;
	[Tooltip ("The time that the description window will stay on, after the pointed object goes null.")]
	public float objectReferenceTimeout = 3f;	// tempo prima che il puntatore al vecchio oggetto venga cancellato

    public CanvasGroup textWindow = null;
    public Text buttonToPressText = null;

    public WeaponHolderManager weaponHolderManager;
    public ExplosiveLauncher granadeHolderManager;

	private float timeLeft = 0;						// tempo rimanente per la cancellazione del vecchio oggetto
	private Text titleText;	
	private Text descriptionText;
	private Canvas descriptionWindow;				
	protected RaycastHit hit;
	private Vector3 pointerObj = Vector3.zero;		// riferimento all'oggetto puntato
	private Animator anim;							// animator controller della finestra di descrizione

	void Start () {
		descriptionWindow = UIDetailsCanvas.GetComponentInParent<Canvas> ();
		Text[] texts = descriptionWindow.GetComponentsInChildren<Text> ();

		titleText = texts [0];
		descriptionText = texts [1];
		ResetObjPointer ();
		anim = UIDetailsCanvas.GetComponentInParent<Animator> ();

		// importante per non far partire l'animazione di apertura finestra all'avvio
		descriptionWindow.gameObject.SetActive (false);
	}

	void FixedUpdate () {

		if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, RaycastDistance)) {

			if (hit.transform.tag != null && (hit.transform.CompareTag ("DetailedObject") || hit.transform.CompareTag ("DetailedWeapon"))
                || hit.transform.GetComponent<ObjectProps>() != null || hit.transform.GetComponent<WeaponProps>() != null
                || hit.collider.transform.tag != null && (hit.transform.CompareTag("DetailedObject") || hit.collider.transform.CompareTag("DetailedWeapon"))) {


                objectToDescribe = hit.transform.gameObject;

            //    for (int i = 0; i < hit.transform.childCount; i++) {
            //        if (hit.transform.GetChild(i).CompareTag("DetailedObject") || hit.transform.GetChild(i).CompareTag("DetailedWeapon")) {
            //        }
            //    }

            //    if (objectToDescribe == null) {
            //        return;
            //    }

                if (anim.gameObject.activeSelf)
    				anim.SetBool ("closeWindow", false);

				descriptionWindow.gameObject.SetActive (true);

				//Debug.Log ("popUp");

				WriteDescription();
				DrawConnectingLines();

				timeLeft = objectReferenceTimeout;
				//Debug.Log ("Raycasted Object: " + objectToDescribe.name);

			} else {
				if (timeLeft <= 0) {
					DisableWindow ();
				}

                textWindow.alpha = 0;
            }
		} else {
			if (timeLeft <= 0) {
				DisableWindow ();
			}

            textWindow.alpha = 0;
		}

		if (timeLeft > 0 && objectToDescribe != null) {
			pointerObj = Vector3.Lerp (pointerObj, objectToDescribe.transform.position, Time.fixedDeltaTime * referencePointerSpeedMultiplier);
		}

		timeLeft -= Time.deltaTime;

		if (timeLeft <= 0) {
			ResetObjPointer ();
		}

		//Debug.DrawRay (this.transform.position, this.transform.forward * 100f, Color.green);
	}

	void DrawConnectingLines() {
		if (UIDetailsCanvas && objectToDescribe) {
			Vector3 mainPointPos = UIDetailsCanvas.transform.position;
			Vector3 pointPos = objectToDescribe.transform.position;

			GL.Begin (GL.LINES);
			lineMat.SetPass (0);
			GL.Color (new Color(lineMat.color.r, lineMat.color.g, lineMat.color.b));
			GL.Vertex3 (mainPointPos.x, mainPointPos.y, mainPointPos.z);
			GL.Vertex3 (pointerObj.x, pointerObj.y, pointerObj.z);
			GL.End ();
		}
	}

    /*
     * Questo script gestisce la descrizione degli oggetti nella finestra nella parte in alto a sinistra dello schermo
     * Vengono gestite anche le interazioni con gli oggetti integibili e le armi.
     * Quando si interagisce con le armi la logica di tale operazione viene gestita in parte qui e in parte nello script WeaponHolderManager
     * Quando si interagisce con gli oggetti essi vengono gestiti tramite un'interfaccia che comunichera' l'evento agli script che sono in ascolto 
     */
	void WriteDescription () {

        // quando un oggetto nella scena ha questo tag, il sistema lo vede come un'arma equipaggiabile
        // le informazioni dell'arma che vengono scritte nella finestra delle informazioni vengono prese direttamente dallo script che presente nell'oggetto
		if (objectToDescribe.CompareTag("DetailedWeapon") || objectToDescribe.GetComponent<WeaponProps>() != null
            || objectToDescribe.transform.GetComponent<WeaponProps>() != null) {

            WeaponProps weapon = objectToDescribe.GetComponent<WeaponProps>();

            // impostazione delle informazioni dell'oggetto sulla finestra
            // l'oggetto deve avere per forza un componente WeaponProps per essere descritto
            if (objectToDescribe.GetComponent<WeaponProps>() != null) {
                weapon = objectToDescribe.GetComponent<WeaponProps>();
                titleText.text = weapon._name;
				descriptionText.text = weapon._description 
                    + "\n\nAmmo: " + weapon._currentAmmo + "/" + weapon._maxAmmo
                    + "\nDamage: " + weapon._damage;
			} else {
                // in caso non ci siano informazioni sull'arma
                SetDefaultDescriptionInformations();
            }

            // gestione dell'interazione con l'arma
           
            ManageWeaponInteraction(weapon);

		} else if (objectToDescribe.CompareTag("DetailedObject") || objectToDescribe.GetComponent<ObjectProps>() != null) {
            ObjectProps obj = objectToDescribe.GetComponent<ObjectProps>();

            // impostazione delle informazioni dell'oggetto sulla finestra
            // l'oggetto deve avere per forza un componente ObjectProps per essere descritto
            if (objectToDescribe.GetComponent<ObjectProps>() != null) {
                obj = objectToDescribe.GetComponent<ObjectProps>();
                titleText.text = obj._name;
				descriptionText.text = obj._description;
			} else {
                // in caso non ci siano informazioni sull'oggetto
                SetDefaultDescriptionInformations();
            }

            // gestione dell'interazione con gli oggetti interagibili
            ManageObjectInteraction(obj);

		}
	}

	public void DisableWindow() {
        if (anim.gameObject.activeSelf) {
    		anim.SetBool ("closeWindow", true);
            textWindow.alpha = 0;
        }
        if (objectToDescribe != null) {
    		objectToDescribe = null;
        }
	}

    // gestione dell'interazione con l'arma
    private void ManageWeaponInteraction(WeaponProps weapon) {

        if (weapon._name == "Granade") {
            if (!granadeHolderManager.IsWeaponFull(weapon._name)) {
                textWindow.alpha = 1;
                buttonToPressText.text = "Press \"E\" button to pick ammo";

                if (Input.GetButtonDown("Interact")) {
                    granadeHolderManager.RefillAmmoToWeapon(weapon._name, weapon._currentChargerAmmo);
                    Destroy(objectToDescribe);
                    DisableWindow();
                    //Debug.Log("Premuto il tasto e 1");
                }
            }
        }
        else {
            if (weaponHolderManager.IsUnlocked(weapon._name) && !weaponHolderManager.IsWeaponFull(weapon._name)) {
                textWindow.alpha = 1;
                buttonToPressText.text = "Press \"E\" button to pick ammo";

                if (Input.GetButtonDown("Interact")) {
                    weaponHolderManager.RefillAmmoToWeapon(weapon._name, weapon._currentChargerAmmo);
                    Destroy(objectToDescribe);
                    DisableWindow();
                    //Debug.Log("Premuto il tasto e 1");
                }

            }
            else if (!weaponHolderManager.IsUnlocked(weapon._name)) {
                textWindow.alpha = 1;
                buttonToPressText.text = "Press \"E\" button to grab the weapon";

                if (Input.GetButtonDown("Interact")) {
                    weaponHolderManager.UnlockWeapon(weapon._name);
                    Destroy(objectToDescribe);
                    DisableWindow();
                    //Debug.Log("Premuto il tasto e 2");
                }
            }
        }        
    }

    // gestione dell'interazione con gli oggetti
    private void ManageObjectInteraction(ObjectProps obj) {
        if (obj != null) {
            if (obj._interactable) {
                textWindow.alpha = 1;
                buttonToPressText.text = obj._InteractText;

                // se l'oggetto e' interactable lo script che deve gestire l'interazione viene avvisato grazie all'interrfaccia implementata
                IOnActionButtonPressed onButton = objectToDescribe.GetComponent<IOnActionButtonPressed>();

                if (Input.GetButtonDown("Interact")) {
                    if (onButton != null) {
                        onButton.OnButtonPressed();
                        textWindow.alpha = 0;
                    }
                }
            }
        }   
    }

    // testo di default della finestra didescrizione
    private void SetDefaultDescriptionInformations() {
        titleText.text = "<No informations available>";
        descriptionText.text = "<No descritpion available for this object>";
    }

	void OnPostRender() {
		DrawConnectingLines ();
	}

	void ResetObjPointer () {
		pointerObj = Vector3.Lerp (pointerObj, UIDetailsCanvas.transform.position, referencePointerSpeedMultiplier);
	}

/*
	void OnDrawGizmos() {
		DrawConnectingLines ();
	}
*/
}

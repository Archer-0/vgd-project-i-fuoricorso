/*
 * Questo e' stato migliorato grazie ad alcuni utenti del forum di Unity. Putroppo non si ricordano gli username di essi perche' non si riesce3 a ritrovare il thread esatto
 */

using UnityEngine;

/// <summary>
/// Animate user interface on camera rotate.
/// Uses the rotation on the X and Y axes.
/// </summary>
public class AnimateUIOnCameraRotate : MonoBehaviour {

	public float rotationSpeedMultiplier = 2f;
	public Vector2 range = new Vector2 (2.5f, 2.5f);

	private GameObject UI;
	private float mouseSpeedX;
	private float mouseSpeedY;
	private Quaternion defaultRotation ;    // initial rotation (0, 0, 0, 0)
    private GameController gameController;
    //private bool wasGamePaused = false;

    private Vector2 tiltRotation = Vector2.zero;
    private Vector2 tiltRotBackup;

	void Start () {
		UI = transform.GetChild (0).gameObject;
		//Debug.Log (UI.name);
		defaultRotation = UI.transform.localRotation;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Update () {
		/* Da utilizzare nel menu principale per maggior feedback
		float halfWidth = Screen.width / 2;
		float halfHeight = Screen.height / 2;

		float x = Mathf.Clamp ((Input.mousePosition.x - halfWidth) / halfWidth, -1f, 1f);
		float y = Mathf.Clamp ((Input.mousePosition.y - halfHeight) / halfHeight, -1f, 1f);

		// calcola la nuova rotazione dipendentemente dala velocita' del mouse. Lerp interpola linearmente tra due valori A e B basndosi su un tempo T
		// in questo caso come valore A viene passato il risultato stesso.
		tiltRotation = Vector2.Lerp (tiltRotation, new Vector2 (-mouseSpeedX, -mouseSpeedY), Time.deltaTime * rotationSpeedMultiplier);

		// ruota la UI all'interno di un range moltiplicandolo per la posizione originaria utilizzata come punto di riferimento.
		UI.transform.localRotation = defaultRotation * Quaternion.Euler (-tiltRotation.y * range.y, tiltRotation.x * range.x, 0f);
		*/

        if (!gameController.isGamePaused) {
		    GetMouseAxesSpeed ();
		    SetNewRotation ();

            //tiltRotBackup = tiltRotation;
        }
        //else {
        //     wasGamePaused = true;
        //}

        //if (wasGamePaused == true) {
        //    wasGamePaused = false;
        //    defaultRotation = new Quaternion(0, 0, 0, 1.0F);
        //    tiltRotation = tiltRotBackup;
        //}

	}

	void GetMouseAxesSpeed () {
		
		mouseSpeedX = Mathf.Clamp((Input.GetAxis ("Mouse X") / Time.unscaledDeltaTime), -1f, 1f);
		//Debug.Log ("MouseSpeedX: " + mouseSpeedX);

		mouseSpeedY = Mathf.Clamp((Input.GetAxis ("Mouse Y") / Time.unscaledDeltaTime), -1f, 1f);
		//Debug.Log ("MouseSpeedY: " + mouseSpeedY);
	}

	/// <summary>
	/// Sets the new rotation depending on the mouse speed.
	/// </summary>
	void SetNewRotation () {
        //tiltRotation = new Vector2(0.0F, 0.0F);
		// calcola la nuova rotazione dipendentemente dala velocita' del mouse. Lerp interpola linearmente tra due valori A e B basandosi su un tempo T,
		// in questo caso come valore A viene passato il risultato stesso e come valore B un Vector2 con il valore della velocita' del mouse nei due assi
		// rimappati in un range tra -1 e 1.
    	tiltRotation = Vector2.Lerp (tiltRotation, new Vector2 (-mouseSpeedX, -mouseSpeedY), Time.unscaledDeltaTime * rotationSpeedMultiplier);


        //Debug.Log("deltatime" + Time.deltaTime);

		// ruota la UI all'interno di un range moltiplicandolo per la posizione originaria utilizzata come punto di riferimento.
		UI.transform.localRotation = defaultRotation * Quaternion.Euler (-tiltRotation.y * range.y, tiltRotation.x * range.x, 0f);

        //Debug.Log("defaultrot: " + defaultRotation);
        //Debug.Log("tiltrotY: " + tiltRotation.y);
        //Debug.Log("tiltrotX: " + tiltRotation.x);
        //Debug.Log("range: " + range.x + " " + range.y);
	}
}
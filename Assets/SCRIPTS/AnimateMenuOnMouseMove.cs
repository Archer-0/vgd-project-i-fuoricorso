using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class AnimateMenuOnMouseMove : MonoBehaviour {

    public float rotationSpeedMultiplier = 2f;
    public Vector2 range = new Vector2(2.5f, 2.5f);

    public GameObject UIRoot;

    private float mouseSpeedX;
    private float mouseSpeedY;
    private Quaternion defaultRotation; // initial rotation (0, 0, 0, 0)

    private Vector2 tiltRotation = Vector2.zero;

    void Start () {
        defaultRotation = UIRoot.transform.localRotation;
    }
	
	void Update () {
        // Da utilizzare nel menu principale per maggior feedback
		float halfWidth = Screen.width / 2;
		float halfHeight = Screen.height / 2;

		float x = Mathf.Clamp ((Input.mousePosition.x - halfWidth) / halfWidth, -1f, 1f);
		float y = Mathf.Clamp ((Input.mousePosition.y - halfHeight) / halfHeight, -1f, 1f);

		// calcola la nuova rotazione dipendentemente dala velocita' del mouse. Lerp interpola linearmente tra due valori A e B basndosi su un tempo T
		// in questo caso come valore A viene passato il risultato stesso.
		tiltRotation = Vector2.Lerp (tiltRotation, new Vector2 (-mouseSpeedX, -mouseSpeedY), Time.unscaledDeltaTime * rotationSpeedMultiplier);

		// ruota la UI all'interno di un range moltiplicandolo per la posizione originaria utilizzata come punto di riferimento.
		UIRoot.transform.localRotation = defaultRotation * Quaternion.Euler (-tiltRotation.y * range.y, tiltRotation.x * range.x, 0f);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour {

	public int selectedWeapon = 0;
	public GameObject[] WeaponsList;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		SelectWeapon();
	}
	
	// Update is called once per frame
	void Update () {
		rb = GetComponentInChildren<Rigidbody> ();
		if (rb != null) {
			GetComponentInChildren <Rigidbody>().detectCollisions = false;

		}
	}
		
	void SelectWeapon () {
		//int i = 0;
		foreach (GameObject weapon in WeaponsList) {
			//Debug.Log (i++);
		}
	}

	void disableRigidbody() {
		rb.detectCollisions = false;
		rb.useGravity = false;
	}
}

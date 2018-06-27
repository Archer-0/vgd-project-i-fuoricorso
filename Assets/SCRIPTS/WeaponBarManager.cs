using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBarManager : MonoBehaviour {

	public GameObject weaponHolder;
	public GameObject weaponAmmoWindow;
	public Text ammoText;
    public CanvasGroup weaponCanvasGroup;

	private WeaponProps weaponProps;


	void Start () {
		DisableWindow ();
        weaponCanvasGroup.alpha = 0;
	}

	void Update () {
		//Debug.Log ("WeaponHolder childs: " + weaponHolder.transform.childCount);

		//if (weaponHolder.transform.childCount > 1) {
		//	Debug.Log ("[WeaponBarManager.class] - WARNING: More than one weapon equipped.");
		//}
			
		weaponProps = weaponHolder.GetComponentInChildren<WeaponProps> ();

		if (weaponProps != null) {
			ActivateWindow ();
			UpdateAmmoText ();

		} else {
			DisableWindow ();
		}
	}

	void UpdateAmmoText () {
		ammoText.text = weaponProps._currentChargerAmmo + " / " + weaponProps._currentAmmo;
	}

	void DisableWindow () {
        if (weaponCanvasGroup.alpha > 0)
            weaponCanvasGroup.alpha -= .5F * Time.deltaTime;

		//weaponAmmoWindow.SetActive (false);
	}

	void ActivateWindow () {
        if (weaponCanvasGroup.alpha < 1)
            weaponCanvasGroup.alpha += .9F * Time.deltaTime;

        //if (!weaponAmmoWindow.activeSelf)
			//weaponAmmoWindow.SetActive (true);
	}
}

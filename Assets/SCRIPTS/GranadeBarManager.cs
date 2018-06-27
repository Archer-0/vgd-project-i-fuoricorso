using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GranadeBarManager : MonoBehaviour {

    public Text granadeAmmoText;
    public WeaponProps granadeWeapon;

    private CanvasGroup canvasGroup;

	void Start () {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
	}
	
	void Update () {

        if (granadeWeapon._currentAmmo > 0) {
            if (canvasGroup.alpha < 1) {
                canvasGroup.alpha += .5F * Time.deltaTime;
            }

            UpdateGranadeText();

        } else {
            if (canvasGroup.alpha > 0) {
                canvasGroup.alpha -= .8F * Time.deltaTime;
            }
        }
	}

    void UpdateGranadeText() {
        granadeAmmoText.text = "" + granadeWeapon._currentAmmo;
    }
}

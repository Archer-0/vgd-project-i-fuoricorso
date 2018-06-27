using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarManager : MonoBehaviour {

	public GameObject player;
	public GameObject red;
	public GameObject green;
	public Text lifePercent;

	private float maxPlayerHealth = 100;
	private float health;
	private Image redImg;
	private Image greenImg;
	private Color alphaGreen;
	private Color alphaRed;
    private Image mainImage;
    //private bool isBlinking = false;

    // Use this for initialization
    void Start () {
		redImg = red.GetComponent<Image> ();
		greenImg = green.GetComponent<Image> ();
	    mainImage = GetComponent<Image> ();
		mainImage.color = new Color (0,0,0,0);
	}
	
	// Update is called once per frame
	void Update () {

		health = player.GetComponent<PlayerProps> ()._health;

		CalculateAlphas();

		UpdateHealthText();

		SetImgsColor();

    }

	void UpdateHealthText () {
		lifePercent.text = String.Format("{0:F2}", health); //prende solo le prime 2 cifre dopo la virgola
	}

	void SetImgsColor () {
		redImg.color = alphaRed;
		greenImg.color = alphaGreen;
	}

	void CalculateAlphas () {
		alphaGreen = greenImg.color;
		alphaRed = redImg.color;

		alphaGreen.a = (health * 0.9f) / 90;
		//Debug.Log ("aplhaGreen: " + alphaGreen.a);

		alphaRed.a = ((maxPlayerHealth - health) * 0.9f) / 90;
        //Debug.Log ("alphaRed: " + alphaRed.a);
    }
}

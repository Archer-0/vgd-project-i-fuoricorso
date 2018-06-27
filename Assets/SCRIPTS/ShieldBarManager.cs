using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBarManager : MonoBehaviour {

    public GameObject player;
    public GameObject red;
    public GameObject green;
    public Text shieldPercent;

    private float maxPlayerShield = 100;
    private float shield;
    private Image redImg;
    private Image greenImg;
    private Color alphaGreen;
    private Color alphaRed;
    private Image mainImage;
    private CanvasGroup canvasGroup;
    //private bool isBlinking = false;

    // Use this for initialization
    void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
        redImg = red.GetComponent<Image>();
        greenImg = green.GetComponent<Image>();
        mainImage = GetComponent<Image>();
        mainImage.color = new Color(0, 0, 0, 0);
        canvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update() {

        shield = player.GetComponent<PlayerProps>()._shield;

        if (shield > 0) {
            if (canvasGroup.alpha < 1)
                canvasGroup.alpha += .4F * Time.deltaTime;

            CalculateAlphas();
            SetImgsColor();
            UpdateShieldText();
        } else {
            if (canvasGroup.alpha > 0)
                canvasGroup.alpha -= .8F * Time.deltaTime;
        }
    }


    void UpdateShieldText() {
        if (shield == 100)
            shieldPercent.text = "" + shield; //prende solo le prime 2 cifre dopo la virgola
        else
            shieldPercent.text = String.Format("{0:F1}", shield); //prende solo le prime 2 cifre dopo la virgola
    }

    void SetImgsColor() {
        redImg.color = alphaRed;
        greenImg.color = alphaGreen;
    }

    void CalculateAlphas() {
        alphaGreen = greenImg.color;
        alphaRed = redImg.color;

        alphaGreen.a = (shield * 0.9f) / 90;
        //Debug.Log ("aplhaGreen: " + alphaGreen.a);

        alphaRed.a = ((maxPlayerShield - shield) * 0.9f) / 90;
        //Debug.Log ("alphaRed: " + alphaRed.a);
    }
}

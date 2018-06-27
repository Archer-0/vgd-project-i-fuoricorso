using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFadeLayerOnStart : MonoBehaviour {

    //public GameObject fadeLayer;

    public float speed = 1F;

    public CanvasGroup fadeImg;

	// Use this for initialization
	void Start () {
        fadeImg.alpha = 1;
        StartCoroutine(FadeIn());
        //fadeLayer.SetActive(true);
	}

    IEnumerator FadeIn() {
        while (fadeImg.alpha > 0) {
            fadeImg.alpha -= speed * Time.deltaTime;
            yield return null;
        }
    }
}

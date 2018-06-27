using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelTrigger : MonoBehaviour {

    public float fadeTime = 1F;

    CanvasGroup fadeImg;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            //other.GetComponent<FPSController>().fadeImage.SetActive(true);
            fadeImg = other.GetComponent<FPSController>().fadeImage;

            GetComponent<SaveWeaponsPermanently>().SaveWeapons(other.gameObject);
            StartCoroutine(FadeOut());

        }
    }

    IEnumerator FadeOut() {

        //Debug.Log("Ciaooo");

        while(fadeImg.alpha < 1) {

            Debug.Log(fadeImg.alpha);
            fadeImg.alpha += fadeTime * Time.deltaTime;
            //fadeImg.alpha = Mathf.Lerp(0, 1, fadeTime);

            yield return null;
        }

        InitLoadNextLEvel();
    }

    void InitLoadNextLEvel() {
        Debug.Log("Loading");
        LoadLevel.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

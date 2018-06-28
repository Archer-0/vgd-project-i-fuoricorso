using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelWindowManager : MonoBehaviour {
    public CanvasGroup window;
    public CanvasGroup noLevelsText;
    public CanvasGroup backButton;
    public CanvasGroup creditsButton;
    public CanvasGroup[] levels;

    public float alphaSpeedWindow = 3;
    public float alphaSpeedLevels = 6;


    private void Start() {
        StartCoroutine(ShowLevelsWindow());
    }

    IEnumerator ShowLevelsWindow() {
        foreach(CanvasGroup level in levels) {
            level.alpha = 0;
        }

        while(window.alpha < 1) {
            window.alpha += alphaSpeedWindow * Time.deltaTime;
            yield return null;
        }

        while (creditsButton.alpha < 1) {
            creditsButton.alpha += alphaSpeedLevels * Time.deltaTime;
            yield return null;
        }

        while (backButton.alpha < 1) {
            backButton.alpha += alphaSpeedLevels * Time.deltaTime;
            yield return null;
        }

        //if (PlayerPrefs.GetInt("isGameFinished") == 1) {

        foreach(CanvasGroup level in levels) {
            level.gameObject.SetActive(true);
            while (level.alpha < 1) {
                level.alpha += alphaSpeedLevels * Time.deltaTime;
                yield return null;
            }
        }

        //} else {
        //    foreach (CanvasGroup level in levels) {
        //        level.gameObject.SetActive(false);
        //    }

        //    while (noLevelsText.alpha < 1) {
        //        noLevelsText.alpha += alphaSpeedLevels * Time.deltaTime;
        //        yield return null;
        //    }
        //}
    }

    public void ChoseLevel(int levelIndex) {
        LoadLevel.LoadScene(levelIndex);
    }

    public void CloseWindow() {
        StartCoroutine(CloseWindowRout());
    }

    IEnumerator CloseWindowRout() {
        while (window.alpha < 0) {
            window.alpha -= alphaSpeedWindow * Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}

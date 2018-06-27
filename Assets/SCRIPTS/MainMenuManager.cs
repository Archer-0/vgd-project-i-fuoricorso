using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public GameObject loadLevelWindow;

    public GameObject mainSection;
    public GameObject optionsSection;
    public GameObject guideWindow;

    public GameObject testLevel;

    string[] cheat;
    int index;

    private void Start() {
        cheat = new string[] { "c", "u", "l", "o" };
        index = 0;
    }

    public void NewGame() {
        LoadLevel.LoadScene(3);
    }

    public void LoadGame() {
        loadLevelWindow.SetActive(true);
    }

    public void ShowOptions() {
        mainSection.SetActive(false);
        optionsSection.SetActive(true);
    }

    public void HideOptions() {
        optionsSection.SetActive(false);
        mainSection.SetActive(true);
    }

    public void ShowGuide() {
        guideWindow.SetActive(true);
    }

    public void HideGuide() {
        guideWindow.SetActive(false);
    }

    private void Update() {

        if (Input.anyKeyDown) {
            if (Input.GetKeyDown(cheat[index])) {
                Debug.Log(cheat[index]);
                index++;
            }
            else {
                index = 0;
            }
        }

        if (index == cheat.Length) {
            testLevel.SetActive(true);
            index = 0;
        }

    }

}

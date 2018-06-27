/*
 * Questo script regola il gioco e contiene tutte le variabili pubbliche accessibili da tutti gli script all'interno del gioco
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityStandardAssets.Characters.FirstPerson;


public class GameController : MonoBehaviour {
        
    [Serializable]
    public class PlayerSettings {
        
    }

    public int soundTrackIndex = 0;

    public float defaulTimeBeforeDisappear = 5F;
    public bool isPlayerInvisible = false;
    public bool isGamePaused = false;

    public GameObject _player;
    public GameObject infoTextBlock;
    public GameObject deathScreen;

    public CanvasGroup currentObjectiveWindow;
    public Text currentObjectiveText;

    public String currentObjective = "Just play";

    private Text infoText;

    private float infoTimer = 0;

    public AudioController audioController;

    public int enemyCount = 0;
    public int enemyCountMax = 80;

    void Awake() {
        Time.timeScale = 1;
        //audioController = GameObject.FindGameObjectWithTag("StatsAndOther").GetComponent<AudioController>();
    }

    private void Start() {
        infoText = infoTextBlock.transform.GetChild(0).GetComponent<Text>();
        //deathScreen.SetActive(false);
        audioController.PlayAmbientTrack(soundTrackIndex, true);
        ChangeCurrentObjective(currentObjective);

        if (SceneManager.GetActiveScene().buildIndex == 6) {
            GameObject.FindGameObjectWithTag("StatsAndOther").GetComponent<StatsAndOther>().UnlockAllWeapons();
            GameObject.FindGameObjectWithTag("StatsAndOther").GetComponent<StatsAndOther>().hiddenLevelReached = true;
            GameObject.FindGameObjectWithTag("StatsAndOther").GetComponent<StatsAndOther>().fortune.SetDefaultFortune();
        }
    }

    void Update() {
        if (infoTimer >= 0) {
            infoTimer -= Time.fixedDeltaTime;
        }

        if (deathScreen.activeSelf) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (infoTextBlock != null) {
            if (infoTimer < 0 && infoTextBlock.activeSelf) {
                CloseInfoBox();
            }
        }

        if (Input.GetButton("ShowCurrentObjective") && !isGamePaused) {
            currentObjectiveWindow.alpha = Mathf.Lerp(currentObjectiveWindow.alpha, 1, .5F);
        }
        else {
            currentObjectiveWindow.alpha = Mathf.Lerp(currentObjectiveWindow.alpha, 0, .2F);
        }
    }

    public void WriteOnInfoBox(String text, float timeBeforeDisappear) {
        if (!infoTextBlock.activeSelf) {
            infoTextBlock.SetActive(true);
            infoTimer = timeBeforeDisappear;
            infoTextBlock.GetComponent<Animator>().SetBool("closeWindow", false);
            infoText.text = text;

        } else {
            infoTimer = timeBeforeDisappear;
            infoText.text = text;
        }
    }

    public void PlayerDead() {
        // se la scena non e' l'ultima
        if (SceneManager.GetActiveScene().buildIndex < 6) {
            _player.transform.position = new Vector3(0, -1000, 0);

            audioController.PlayPlayerDeadSound();

            ShowDeathScreen();

            GetComponent<PauseMenu>().PlayerIsDead();
        } // altrimenti carica i credits
        else {
            LoadLevel.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    IEnumerator ReloadScene() {
        yield return new WaitForSeconds(5F);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void WriteOnInfoBox(String text) {
        if (!infoTextBlock.activeSelf) {
            infoTextBlock.SetActive(true);
            infoTimer = defaulTimeBeforeDisappear;
            infoTextBlock.GetComponent<Animator>().SetBool("closeWindow", false);
            infoText.text = text;

        } else {
            infoTimer = defaulTimeBeforeDisappear;
            infoText.text = text;
        }
    }

    public void ChangeCurrentObjective(string objectiveText) {
        currentObjectiveText.text = objectiveText;
    }

    private void CloseInfoBox() {
        infoTextBlock.GetComponent<Animator>().SetBool("closeWindow", true);
    }

    private void ShowDeathScreen() {
        deathScreen.SetActive(true);
    }
}

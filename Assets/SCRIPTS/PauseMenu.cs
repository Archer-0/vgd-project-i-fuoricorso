using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseMenu : MonoBehaviour {

    public static bool gameIsPaused = false;

    // gameobject root del menu di pausa
    public GameObject pauseMenuUI;
    
    // sezioni del menu di pausa
    public GameObject mainSection;
    public GameObject optionsSection;

    public Text killsText;
    public Text currentObjective;

    private bool isLocked = true; //blocca al centro il cursore
    private bool isHide = true; //nasconde il cursore
    private GameController gameController;

    void Start() {
        gameController = GetComponent<GameController>();

        CursorIsLocked(isLocked); //blocca il cursore al centro dello schermo
        HideCursor(isHide); //nasconde il cursore

        pauseMenuUI.SetActive(false);
    }

    void Update() {

        // workaround per problemi con il rigidbodyFirstPlayerController di unity
        CursorIsLocked(isLocked);
        HideCursor(isHide);

        //Se si preme il tasto ESC o P la prima volta il gioco va in pausa altrementi riprende
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) {
            if (gameIsPaused) {
                Resume(); //se il gioco era in pausa, lo fa riprendere
            } else {
                Pause(); //se il gioco non era in pausa, lo mette in pausa
            }
        }
    }

    public void Resume() {
        isLocked = true;
        isHide = true;

        //mirino.SetActive(true); //fa ricomparire il mirino     

        if (gameController.isGamePaused == true) {
            gameController.isGamePaused = false;
        }

        gameController.isGamePaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause() {
        isLocked = false;
        isHide = false;

        //mirino.SetActive(false); //nasconde il mirino

        if (gameController.isGamePaused == false) {
            gameController.isGamePaused = true;
        }

        gameController.isGamePaused = true;
        pauseMenuUI.SetActive(true);

        mainSection.SetActive(true);
        optionsSection.SetActive(false);

        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void PlayerIsDead() {
        isLocked = false;
        isHide = false;

        //mirino.SetActive(false); //nasconde il mirino

        if (gameController.isGamePaused == false) {
            gameController.isGamePaused = true;
        }

        gameController.isGamePaused = true;
        pauseMenuUI.SetActive(false);

        mainSection.SetActive(false);
        optionsSection.SetActive(false);

        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    #region Options

    public void ShowOptionsMenu() {
        mainSection.SetActive(false);
        optionsSection.SetActive(true);
    }

    public void HideOptionsMenu() {
        optionsSection.SetActive(false);
        mainSection.SetActive(true);
    }

    #endregion

    public void LoadMainMenu() {
        Time.timeScale = 1f;

        Debug.Log("Loading menu...");
        LoadLevel.LoadScene(0);
        //SceneManager.LoadScene(0);
    }

    
    //public void QuitGame()
    //{
    //    Debug.Log("Quitting game...");
    //    Application.Quit();
    //}

    //blocca il cursore al centro dello schermo
    public void CursorIsLocked(bool locked) {
        if (locked == true) {
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    //nasconde il cursore
    public void HideCursor(bool hide) {
        if (hide == true) {
            Cursor.visible = false;
        } else {
            Cursor.visible = true;
        }
    }

    public int GetKilledEnemies() {
        return GameObject.FindGameObjectWithTag("StatsAndOther").GetComponent<StatsAndOther>().enemyKilled;
    }
}
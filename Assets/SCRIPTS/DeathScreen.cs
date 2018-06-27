using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour {

    public GameObject pauseMenuCanvas;
    public GameObject DeathSceneCanvas;

    public void RetryLevel() {
        Time.timeScale = 1;
        ResetCanvas();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GotoMainMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    private void ResetCanvas() {
        pauseMenuCanvas.SetActive(false);
        DeathSceneCanvas.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour {

    public void DoQuit() {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

}

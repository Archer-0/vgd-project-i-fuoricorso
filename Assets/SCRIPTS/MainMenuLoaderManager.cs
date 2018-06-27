using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MainMenuLoaderManager : MonoBehaviour {

    public void StartLevelLoad(int levelIndex) {
        LoadLevel.LoadScene(levelIndex);
    }
    
    public void StartLevelLoad(String levelName) {
        LoadLevel.LoadScene(SceneManager.GetSceneByName(levelName).buildIndex);
    }
}

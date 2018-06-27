using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelLoader : MonoBehaviour {

    [Header("Graphics Setup")]
    public Slider progressBar;
    public Text progressPercent;
    public Text levelTitle;
    public Text hint;

    [Header("Loading Setting")]
    public LoadSceneMode loadSceneMode = LoadSceneMode.Single;
    public ThreadPriority loadThreadPriority;

    [Header("Timing")]
    public float secondsBetweenHints = 10;


    public Animator hintAnimator;

    public static int sceneToLoad = -1;

    static String loaderScene = "LoaderScreen";

    Scene currScene;
    AsyncOperation operation;

    public static void LoadLevel(int sceneIndex) {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        sceneToLoad = sceneIndex;
        SceneManager.LoadScene(loaderScene);
    }

    private void Start() {
        if (sceneToLoad < 0) {
            Debug.Log("SceneToLoad index less than 0");
            return;
        }
        currScene = SceneManager.GetActiveScene();
        InitLoad();
    }

    // operazioni da eseguire appena si avvia la scena
    void InitLoad() {

        SetLevelTitle();

        StartCoroutine(AsyncLoad(sceneToLoad));
    }


    /*
    public void LoadLevel(String sceneName) {
        this.sceneIndex = SceneManager.GetSceneByName(sceneName).buildIndex;
        InitLoad();
    }
    */

    // gestione barra di caricamento
    private IEnumerator AsyncLoad(int sceneIndex) {

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

        // avvia routine per gestione dei consigli
        StartCoroutine(HintTextManager(new string[1]));

        while (!asyncOperation.isDone) {
            yield return null;

            float progress = Mathf.Clamp01(asyncOperation.progress / .9f);

            progressBar.value = progress;
            progressPercent.text = progress.ToString();

            Debug.Log("[" + this.name + "] - Loading progress: " + progress);

        }

        if(loadSceneMode == LoadSceneMode.Additive) {
            SceneManager.UnloadSceneAsync(currScene.name);
        }
        else {
            operation.allowSceneActivation = true;
        }

        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    IEnumerator HintTextManager(String[] hintsList) {

        String[] hints = null;
        hints[0] = "This is a hint";
        hints[1] = "This is a hint too";
        hints[2] = "Just another hint";

        int i = 0;

        while(true) {
            this.hint.text = hints[i];
            i = (i + 1) % hints.Length;

            yield return new WaitForSeconds(secondsBetweenHints);
        }
    }

    void SetHintText(String actualHint) {
        hintAnimator.SetBool("newText", true);
        this.hint.text = actualHint;
        hintAnimator.SetBool("newText", false);
    }

    void SetLevelTitle() {
        levelTitle.text = "Debug - " + SceneManager.GetSceneByBuildIndex(sceneToLoad).name;
    }
}
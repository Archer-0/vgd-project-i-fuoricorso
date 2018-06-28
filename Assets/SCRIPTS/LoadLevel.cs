using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadLevel : MonoBehaviour {

    [Header("Loading Graphics")]
    public Text title;
    public Text hintsField;
    public Slider progressBar;
    public Text textPercent;
    public Image fadeOverlay;
    public CanvasGroup pressAnyKeyText;

    [Header("Timing Settings")]
    public float waitAfterLoadEnd = .50f;
    public float fadeDuration = .50f;

    [Header("Loading Settings")]
    public LoadSceneMode loadSceneMode = LoadSceneMode.Single;
    public ThreadPriority loadThreadPriority;

    [Header("Others")]
    [Tooltip("If loading scene mode is addictive, plink here the camera audio listener, to avoid multiple active audop listeners.")]
    public AudioListener audioListener;

    AsyncOperation operation;
    Scene currentScene;

    public static int sceneToLoad = -1; // inizializzato a -1 perche' puo' esistere una scena con build index == -1
    static String loadingSceneName = "LoaderScreen";

    public string[] levelNames;
    [TextArea(2, 5)]
    public string[] hints;

    // e' la funzione per l'attivazione della schermata di caricamento
    public static void LoadScene(int levelIndex) {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        sceneToLoad = levelIndex;
        SceneManager.LoadScene(loadingSceneName);
    }

    void Start() {
        if (sceneToLoad < 0) {
            Debug.Log("LoadLevel - no scene index found");
            return;
        }

        // disattivate di default per poter avviare l'animazione all'attivazione
        if (title != null)
            title.gameObject.SetActive(false);
        if (hintsField != null)
            hintsField.gameObject.SetActive(false);
        if (progressBar != null)
            progressBar.gameObject.SetActive(false);
        if (fadeOverlay != null)
            fadeOverlay.gameObject.SetActive(true);

        currentScene = SceneManager.GetActiveScene();

        if (title != null && hintsField != null && progressBar != null && fadeOverlay != null)
            StartCoroutine(AsyncLoad(sceneToLoad));

    }

    private IEnumerator AsyncLoad(int levelIndex) {
        Debug.Log("LoadLEvel - Starting Loading...");

        ShowLoadingGraphics();

        yield return null;

        FadeIn();
        yield return new WaitForSeconds(fadeDuration);

        StartOperation(levelIndex);

        while(IsLoadingDone() == false) {
            //yield return null;
            UpdateProgressBar(operation.progress);

            Debug.Log(operation.progress);
        }


        if (loadSceneMode == LoadSceneMode.Additive && audioListener != null)
            audioListener.enabled = false;

        UpdateProgressBar(1);

        //yield return new WaitForSeconds(waitAfterLoadEnd);

        bool anyKeyPressed = false;

        while(!anyKeyPressed) {
            if (Input.anyKey)
                anyKeyPressed = true;

            pressAnyKeyText.alpha = Mathf.PingPong(Time.time, 1);

            yield return null;
        }

        FadeOut();

        yield return new WaitForSeconds(fadeDuration);

        // se la modalita' di caricamento di una nuova scena e' additiva, disattivo la scena di caricamento altrimaneti
        // permetto l'attivazione dela scena appena caricata
        if (loadSceneMode == LoadSceneMode.Additive)
            SceneManager.UnloadSceneAsync(currentScene.name);
        else
            operation.allowSceneActivation = true;

        Debug.Log("LoadLevel - Loading Finished.");
    }

    private void StartOperation(int levelIndex) {
        Application.backgroundLoadingPriority = loadThreadPriority;
        operation = SceneManager.LoadSceneAsync(levelIndex, loadSceneMode);

        if (loadSceneMode == LoadSceneMode.Single)
            operation.allowSceneActivation = false;
    }

    private bool IsLoadingDone() {
        return (loadSceneMode == LoadSceneMode.Additive && operation.isDone) ||
            (loadSceneMode == LoadSceneMode.Single && operation.progress >= 0.9f);
    }

    private void ShowLoadingGraphics() {
        // attivazione degli oggetti avvio della animazione predefinita
        title.gameObject.SetActive(true);
        hintsField.gameObject.SetActive(true);
        progressBar.gameObject.SetActive(true);

        UpdateProgressBar(.0f);
        SetTextsFields("", "");
    }

    void UpdateProgressBar(float value) {
        float progress = Mathf.Clamp01(value / .9f);
        progressBar.value = progress;
        textPercent.text = String.Format("{0:F1}", progress * 100) + " %";
    }

    void SetTextsFields(string titleText, string hintText) {

        title.text = "Last Edge - " + levelNames[sceneToLoad];
        
        // i consigli saranno poi presi a caso da una lista e aggiornati ogni tot secondi
        hintsField.text = hints[UnityEngine.Random.Range(0, hints.Length)];
    }

    void FadeIn() {
        fadeOverlay.CrossFadeAlpha(0, fadeDuration, true);
    }

    void FadeOut() {
        fadeOverlay.CrossFadeAlpha(1, fadeDuration, true);
    }
}


/*
 * Grazie a Brackeys per i tutorial sulle opzioni
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class SettingsMenu : MonoBehaviour {

    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Toggle fullScreenToggle;
    public Toggle postProcessingToggle;
    public Slider masterVolumeSlider;
    public Slider effectsVolumeSlider;
    public Slider ambientVolumeSlider;
    public GameObject guideWindow;
    public AudioController audioController;

    private Resolution[] resolutions;


    private void Start() {
        //audioController = GameObject.FindGameObjectWithTag("StatsAndOther").GetComponent<AudioController>();
        InitCurrentOptions();
    }

    private void InitCurrentOptions() {
        // inizializza lista risoluzioni
        InitResolutionDropdown();
        // inizializza lista qualita'
        InitQualityDropdown();
        // inizializz toggle fullscreen
        fullScreenToggle.isOn = Screen.fullScreen;
        // inizializza toggle postprocessing
        postProcessingToggle.isOn = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<PostProcessingBehaviour>().enabled;
        // inizializza slider volume
        InitVolumeSliders();
    }

    private void InitVolumeSliders() {
        masterVolumeSlider.value = audioController.GetMasterVolume();
        effectsVolumeSlider.value = audioController.GetEffectsVolume();
        ambientVolumeSlider.value = audioController.GetAmbientVolume();
    }

    private void InitQualityDropdown() {
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }

    private void InitResolutionDropdown() {

        // ottiene tutte le risoluzioni da unity
        resolutions = Screen.resolutions;

        List<string> resOptions = new List<string>();

        // index per menu dropdown
        int currentResIndex = 0;

        // passa tutte le risoluzioni in una lista di stringhe
        for (int i = 0; i < resolutions.Length; i ++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resOptions.Add(option);

            // se la risoluzione e' quella attuale salva l'index nella variabile
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
                currentResIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();

        // aggiunge le risoluzioni disponibili al menu dropdown
        resolutionDropdown.AddOptions(resOptions);

        // inizializza il valore index del menu e aggiorna
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.height, resolution.height, Screen.fullScreen);
    }

    public void SetMasterVolume(float volume) {
        audioController.SetMasterVolume(volume, true);
    }

    public void SetEffectsVolume(float volume) {
        audioController.SetEffectsVolume(volume, true);
    }

    public void SetAmbientVolume(float volume) {
        audioController.SetAmbientVolume(volume, true);
    }

    public void SetQuality(int qualityIndex) {
        // ci abbiamo provato
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen (bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    public void SetPostProcessingEffects(bool enabled) {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<PostProcessingBehaviour>().enabled = enabled;
    }

    public void ShowGuide() {
        guideWindow.SetActive(true);
    }

    public void HideGuide() {
        guideWindow.SetActive(false);
    }
}

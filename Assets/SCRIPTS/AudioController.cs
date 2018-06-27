using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour {

    public AudioMixer mixer;
    public AudioMixerGroup master;
    public AudioMixerGroup effects;
    public AudioMixerGroup ambient;

    // impostati dall'utente
    [Range(-80, 0)]
    public float masterVolume = 0;
    [Range(-80, 0)]
    public float effectsVolume = 0;
    [Range(-80, 0)]
    public float ambientVolume = 0;

    public float playPauseVolumeTime = .5F;

    public AudioClip deathSound;
    public AudioClip[] soundTracks;

    public AudioSource audioSource;

    private void Start() {
        //masterVolume = GetMasterVolume();
        //effectsVolume = GetEffectsVolume();
        //ambientVolume = GetAmbientVolume();
        SetMasterVolume(-80);
        StartCoroutine(FadeInMaster());

    }

    public void PlayPlayerDeadSound() {
        StartCoroutine(FadeOutAmbient());
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = effects;
        source.PlayOneShot(deathSound);
    }

    public void PlayAmbientTrack(int index, bool loop = false) {
        StartCoroutine(FadeInAmbient());

        Debug.Log("InPlay: " + soundTracks[index].name);

        if (loop)
            audioSource.loop = loop;

        audioSource.clip = soundTracks[index];
        audioSource.Play();
    }

    public void SlowDownPitch(float interpolationTime) {
        StartCoroutine(PitchVariation(interpolationTime));
    }

    public void ResetPitch(float interpolationTime) {
        StartCoroutine(PitchReset(interpolationTime));
    }

    IEnumerator PitchVariation(float interpolationTime) {
        
        while (GetEffectsPitch() > .5F) {
            SetEffectsPitch(Mathf.Lerp(GetEffectsPitch(), .5F, interpolationTime));
            SetAmbientPitch(Mathf.Lerp(GetAmbientPitch(), .5F, interpolationTime));
            yield return null;
        }
    }

    IEnumerator PitchReset(float interpolationTime) {

        while (GetEffectsPitch() < 1) {
            SetEffectsPitch(Mathf.Lerp(GetEffectsPitch(), 1, interpolationTime));
            SetAmbientPitch(Mathf.Lerp(GetAmbientPitch(), 1, interpolationTime));
            yield return null;
        }
    }

    IEnumerator FadeInAmbient() {
        SetAmbientVolume(-80);
        while (GetAmbientVolume() < ambientVolume) {
            //SetAmbientVolume(Mathf.Lerp(GetAmbientVolume(), ambientVolume, playPauseVolumeTime));
            SetAmbientVolume(GetAmbientVolume() + playPauseVolumeTime * Time.unscaledDeltaTime);
            //print(GetAmbientVolume());
            yield return null;
        }

        SetAmbientVolume(ambientVolume);
    }

    IEnumerator FadeOutAmbient() {
        while (GetAmbientVolume() > -80) {
            //SetAmbientVolume(Mathf.Lerp(GetAmbientVolume(), -80, playPauseVolumeTime));
            SetAmbientVolume(GetAmbientVolume() - playPauseVolumeTime * Time.unscaledDeltaTime);
            print(GetAmbientVolume());
            yield return null;
        }

        SetAmbientVolume(-80);
    }

    IEnumerator FadeInMaster() {
        while (GetMasterVolume() < masterVolume) {
            SetMasterVolume(Mathf.Lerp(GetMasterVolume(), masterVolume, .09F));
            yield return null;
        }
    }



    #region Setter&Getter

    public float GetMasterVolume() {
        float volume = 0;
        mixer.GetFloat("MasterVolume", out volume);

        return volume;
    }

    public float GetEffectsVolume() {
        float volume = 0;
        mixer.GetFloat("EffectsVolume", out volume);

        return volume;
    }

    public float GetAmbientVolume() {
        float volume = 0;
        mixer.GetFloat("AmbientVolume", out volume);

        return volume;
    }

    public float GetMasterPitch() {
        float pitch = 0;
        mixer.GetFloat("MasterPitch", out pitch);

        return pitch;
    }

    public float GetEffectsPitch() {
        float pitch = 0;
        mixer.GetFloat("EffectsPitch", out pitch);

        return pitch;
    }

    public float GetAmbientPitch() {
        float pitch = 0;
        mixer.GetFloat("AmbientPitch", out pitch);

        return pitch;
    }

    public float GetMenuEffectsPitch() {
        float pitch = 0;
        mixer.GetFloat("MenuEffectsPitch", out pitch);

        return pitch;
    }

    // byUser e' usato per tenere una copia di backup del valore se esso e' stato modificato dall'utente
    public void SetMasterVolume(float volume, bool byUser = false) {
        mixer.SetFloat("MasterVolume", volume);
        if (byUser)
            masterVolume = volume;
    }

    public void SetEffectsVolume(float volume, bool byUser = false) {
        mixer.SetFloat("EffectsVolume", volume);
        if (byUser)
            effectsVolume = volume;
    }

    public void SetAmbientVolume(float volume, bool byUser = false) {
        mixer.SetFloat("AmbientVolume", volume);
        if (byUser)
            ambientVolume = volume;
    }

    public void SetMasterPitch(float pitch) {
        mixer.SetFloat("MasterPitch", pitch);
    }

    public void SetEffectsPitch(float pitch) {
        mixer.SetFloat("EffectsPitch", pitch);
    }

    public void SetAmbientPitch(float pitch) {
        mixer.SetFloat("AmbientPitch", pitch);
    }

    public void SetMenuEffectsPitch(float pitch) {
        mixer.SetFloat("MenuEffectsPitch", pitch);
    }

    #endregion
}

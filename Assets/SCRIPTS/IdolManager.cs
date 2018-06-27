using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdolManager : MonoBehaviour, IOnActionButtonPressed {

    public Light spot;
    public float spotAngle;
    public float spotInterpolationTime;
    public Light point;
    public float pointIntensity;
    public float pointInterpolationTime;

    public AudioClip audioEffect;

    [HideInInspector]
    public bool completed = false;

    private void OnEnable() {

    }

    private void Start() {
        StartCoroutine(SwitchOnSpot());
        //StartCoroutine(SwitchOffPoint());
    }

    public void EnableIdol() {
        StartCoroutine(SwitchOnPoint());
        GetComponent<ObjectProps>()._interactable = true;
    }

    public void DisableIdol() {
        GetComponent<ObjectProps>()._interactable = false;
        StartCoroutine(SwitchOffPoint());
        StartCoroutine(SwitchOffSpot());
    }

    IEnumerator SwitchOnSpot() {
        spot.enabled = true;
        spot.spotAngle = 0;

        while(spot.spotAngle <= spotAngle) {
            //spot.spotAngle = Mathf.Lerp(spot.spotAngle, spotAngle, spotInterpolationTime);
            spot.spotAngle += spotInterpolationTime * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator SwitchOffSpot() {
        while (spot.spotAngle >= 0) {
            //spot.spotAngle = Mathf.Lerp(spot.spotAngle, 0, spotInterpolationTime);
            spot.spotAngle -= spotInterpolationTime * Time.deltaTime;
            yield return null;
        }

        //spot.enabled = false;
    }

    IEnumerator SwitchOnPoint() {
        point.intensity = 0;
        while (point.intensity <= pointIntensity) {
            //point.intensity = Mathf.Lerp(point.intensity, pointIntensity, pointInterpolationTime);
            point.intensity += pointInterpolationTime * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator SwitchOffPoint() {
        while (point.intensity >= 0) {
            //point.intensity = Mathf.Lerp(point.intensity, 0, pointInterpolationTime);
            point.intensity -= pointInterpolationTime * Time.deltaTime;
            yield return null;
        }

        //point.flare = null;
    }

    public void OnButtonPressed() {
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelControllerThree>().OnIdolInteracted(this);
        completed = true;
        GetComponent<AudioSource>().PlayOneShot(audioEffect);
        DisableIdol();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSlowTime : MonoBehaviour, IOnPowerUpTrigger {
    public float timeScaleValue = 0.5F;

    public void OnPowerUpEnter(Collider other) {        
        StartCoroutine(DisableAndDestroy(other));

        Time.timeScale = timeScaleValue;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>().SlowDownPitch(.1F);

        //Debug.Log("Funge?");
    }

    IEnumerator DisableAndDestroy(Collider other) {
        yield return new WaitForSeconds(GetComponent<PowerUpGeneral>().timer);

        //Debug.Log("Funge");

        Time.timeScale = 1F;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>().ResetPitch(.9F);

        Destroy(gameObject);
    }
}

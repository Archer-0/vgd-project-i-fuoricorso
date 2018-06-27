using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnFx : MonoBehaviour {

    public AudioClip buttonHoverSound;
    public AudioClip buttonClickSound;

    public void OnButtonHover() {
        GetComponent<AudioSource>().PlayOneShot(buttonHoverSound);
    }

    public void OnButtonClick() {
        GetComponent<AudioSource>().PlayOneShot(buttonClickSound);
    }
}

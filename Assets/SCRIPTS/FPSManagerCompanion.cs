using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSManagerCompanion : MonoBehaviour {

    public FPSController FPSController;

    public void PlayLandSound() {
        FPSController.PlayLandSound();
    }

    public void PlayFootSound() {
        FPSController.PlayFootSound();
    }
}

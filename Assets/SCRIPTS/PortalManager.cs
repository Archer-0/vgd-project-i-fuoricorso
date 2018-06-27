using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour {

    public ParticleSystem portalParticleEffects;
    public GameObject portalJumper;

    private void Start() {
        portalParticleEffects.enableEmission = false;
        portalJumper.SetActive(false);
    }

    public void EnablePortal() {
        portalJumper.SetActive(true);
        portalParticleEffects.enableEmission = true;
    }
}

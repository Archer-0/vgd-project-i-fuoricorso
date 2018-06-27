using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoSomeMagic : MonoBehaviour, IOnActionButtonPressed {

    public ParticleSystem particle;
    public AudioClip audioclip;

    public void OnButtonPressed() {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().WriteOnInfoBox("Wow, it's magic", 5F);
        Debug.Log(GameObject.FindGameObjectWithTag("Player").name);

        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SkinnedMeshRenderer>().material = GetComponent<Renderer>().material;
        GameObject.FindGameObjectWithTag("StatsAndOther").GetComponent<StatsAndOther>().fortune.ChangeFortunePermanently(50F);

        AudioSource.PlayClipAtPoint(audioclip, transform.position, 2);
        Destroy(gameObject);
        Instantiate(particle, transform.position, transform.rotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour {

    public float secondsBeforeDestroy = 5F;

	void Start () {
        StartCoroutine(DestroyAfter());
	}

    IEnumerator DestroyAfter() {
        yield return new WaitForSeconds(secondsBeforeDestroy);
        Destroy(this.gameObject);
    }
}

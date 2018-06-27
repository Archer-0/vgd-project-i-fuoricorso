using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PortalElevator : MonoBehaviour {

    [Range(0.1F, 1)]
    public float speed = .5F;

    Transform player;

    private void OnCollisionEnter(Collision other) {

        Debug.Log("Elevator actrivated");

        if (other.transform.CompareTag("Player")) {
            other.transform.GetComponent<FPSController>().powerUpspeedMultiplier = 0;
            other.transform.GetComponent<RigidbodyFirstPersonController>().isGamePaused = true;
            other.transform.GetComponent<RigidbodyFirstPersonController>().freeLook = true;

            player = other.transform;

            StartCoroutine(Elevator());

            //InvokeRepeating("Elevator", 0, .01F);

            //other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, elevationForce, 0), ForceMode.VelocityChange);
        }

        if (other.transform.CompareTag("Enemy")) {
            other.transform.GetComponent<EnemyProps>().Die();
        }   
    }
    
    IEnumerator Elevator() {

        while (true) {
            if (Time.timeScale != 0)
                player.position = new Vector3(player.position.x, Mathf.Lerp(player.position.y, player.position.y + 1, speed), player.position.z);
            yield return null;
        }
    }

}

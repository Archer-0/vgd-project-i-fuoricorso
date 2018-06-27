using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoss : MonoBehaviour {

    public float damagePerSecond;
    public float moveVelocity;

    private GameController gameController;

    private Transform player;

	// Use this for initialization
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        player = gameController._player.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 movePos = new Vector3(Mathf.Lerp(transform.position.x, player.position.x, moveVelocity / 1000),
            transform.position.y, 
            Mathf.Lerp(transform.position.z, player.position.z, moveVelocity / 1000));

        transform.position = movePos;
	}

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerProps>().GetDamage(damagePerSecond * Time.deltaTime);
        }
    }
}

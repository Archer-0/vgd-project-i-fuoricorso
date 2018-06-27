//ispirato da: https://www.youtube.com/watch?v=CHV1ymlw-P8&t=303s

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class HumanoidEnemyController : MonoBehaviour {

    public GameObject player;

    public NavMeshAgent agent;

    public ThirdPersonCharacter character;

    public GameObject raycastOrigin;

    private RaycastHit hit;

	// Use this for initialization
	void Start () {

        agent.updateRotation = false;
	}
	
	// Update is called once per frame
	void Update () {

        Physics.Raycast(raycastOrigin.transform.position, player.transform.position, out hit);

        Vector3 direction = player.transform.position - raycastOrigin.transform.position;
        
        Debug.Log("aaa " + direction.magnitude);

        Debug.DrawRay(raycastOrigin.transform.position, direction, Color.red);

        if(direction.magnitude < 10F)
        {
            agent.SetDestination(player.transform.position);

            if (agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity, false, false);
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
        }
        else
        {
            character.Move(Vector3.zero, false, false);          
        }







        //if (agent.remainingDistance > 10F)
        //{
        //    character.Move(Vector3.zero, false, false);
        //}
        //else
        //{
        //    agent.SetDestination(player.transform.position);

        //    if (agent.remainingDistance > agent.stoppingDistance)
        //    {
        //        character.Move(agent.desiredVelocity, false, false);
        //    }
        //    else
        //    {
        //        character.Move(Vector3.zero, false, false);
        //    }
        //}
	}
}

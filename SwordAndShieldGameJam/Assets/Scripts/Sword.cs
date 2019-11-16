using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    PlayerControl playerScript;

    private void Start()
    {
        playerScript = GetComponentInParent<PlayerControl>();

    }


    private void OnTriggerEnter(Collider other)
    {
        string colliderTag = other.gameObject.tag;

        switch (colliderTag)
        {
            case "Player":
                //check if regular or charged attack with bool
                if (playerScript.isCharging)
                {
                    //apply big dam, instant kill
                }
                else //normal attack
                {
                    //apply damage
                }

                break;

            case "Shield":
                // check if regular or charged attack with bool
                if (playerScript.isCharging)
                {
                    //bounce back mechanic
                }
                else //normal attack
                {
                    //blocked, nothin happens?
                }
                break;

            default:
                Debug.Log("Collided with something not in switch statemetn");
                break;
        }
    }

    private void Update()
    {
        
    }

}

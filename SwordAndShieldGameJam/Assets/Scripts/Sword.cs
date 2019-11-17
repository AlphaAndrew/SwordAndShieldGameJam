using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Sword : MonoBehaviour
{
    PlayerControl playerScript;
    public float normalDamage;
    public float chargedDamage;

    private void Start()
    {

            playerScript = this.GetComponentInParent<PlayerControl>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        Debug.Log(other.gameObject.tag.ToString());
        if (other.gameObject.tag != null)
        {
            Debug.Log("hit something");

            string colliderTag = other.gameObject.tag;
            switch (colliderTag)
            {
                case "Player":
                    //check if regular or charged attack with bool
                    if (playerScript.isCharging)
                    {
                        other.gameObject.GetComponentInParent<PlayerControl>().ApplyDamage(chargedDamage);
                    }
                    else //normal attack
                    {
                        other.gameObject.GetComponentInParent<PlayerControl>().ApplyDamage(normalDamage);
                    }

                    break;

                case "Shield":
                    Debug.Log("hit Sheild");
                    // check if regular or charged attack with bool
                    if (playerScript.isCharging)
                    {
                        //player boucnes back
                        playerScript.hitSomeone = true;
                        //target bounces back
                        other.gameObject.GetComponentInParent<PlayerControl>().isBouncing = true;
                    }
                    else //normal attack
                    {
                        //blocked, nothin happens?
                    }
                    break;
                case "Obstacle":
                    // check if regular or charged attack with bool
                    if (playerScript.isCharging)
                    {
                     
                        playerScript.hitSomeone = true;
                    }
                    else //normal attack
                    {
                        //blocked, nothin happens?
                    }
                    break;

                default:
                    Debug.Log("Sword collided with " + other.gameObject.tag + " no action takes place");
                    break;
            }
        }

    }

    private void Update()
    {
        
    }

}

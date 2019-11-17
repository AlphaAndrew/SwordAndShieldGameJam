using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Player control script
/// Includes score, health, movement, attack, and block methods
/// </summary>
public class PlayerControl : NetworkBehaviour
{
    //Player
    private Rigidbody playerRB;
    private GameObject player;
    private string playerTeam;
    public float playerScore;
    //Health
    public float health;
    private float currentHealth;
    //Speed
    public float playerBaseSpeed;
    public float playerShieldSpeed;
    private float playerSpeed;
    //Shield
    public GameObject shield;
    public GameObject sideShieldPos;
    public GameObject frontShieldPos;
    //Charge
    public float chargeMultiplier;
    public float chargeLimit;
    public bool isCharging = false;
    public float chargeSpeed = 1f;
    private float chargeTimer;
    //Lerp
    private Vector3 lerpStartPos;
    private Vector3 lerpEndPos;
    private float lerpDistance;
    private float lerpStartTime;
    private float lerpDuration;
    //Bounce
    public bool hitSomeone = false;
    public bool isBouncing = false;
    public float bounceMultiplier;

    public Camera camera;

    //Coroutine accumulatePoints;
    //see capture point "Do Battle" 

    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            
            playerRB = GetComponent<Rigidbody>();

            player = this.gameObject;
            playerSpeed = playerBaseSpeed;
            


            playerTeam = "Red";
            return;
        }
        camera.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            if (currentHealth <= 0)
            {
                //Death
            }
            MovementControls();
            //Normal Attack
            //Charge Attack
            if (Input.GetMouseButton(0))
            {
                if (chargeTimer < chargeLimit)
                {
                    chargeTimer += Time.deltaTime;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //Attack
                //ChargePrep();
                //isCharging = true;
                playerRB.AddRelativeForce(Vector3.forward *(chargeMultiplier*chargeTimer), ForceMode.Impulse);
                chargeTimer = 0;
            }
            //if (isCharging)
            //{
            //    ChargeAttack();
            //}
            if (isBouncing)
            {
                BounceBack();
            }
            if (Input.GetMouseButton(1))
            {
                //Shield Up
                ShieldUp();
            }
            else if(Input.GetMouseButtonUp(1))
            {
                ShieldDown();
            }
        }        
    }

    //Shield to front of player
    public void ShieldUp()
    {
        shield.transform.SetPositionAndRotation(frontShieldPos.transform.position, frontShieldPos.transform.rotation);
        playerSpeed = playerShieldSpeed;
    }
    //Shield back down to side of player
    public void ShieldDown()
    {
        shield.transform.SetPositionAndRotation(sideShieldPos.transform.position, sideShieldPos.transform.rotation);
        playerSpeed = playerBaseSpeed;
    }

    //Charge Attack Variables
    public void ChargePrep()
    {
        lerpStartPos = player.transform.position;
        lerpEndPos = player.transform.position + transform.forward * (chargeMultiplier * chargeTimer);
        isCharging = true;
        lerpStartTime = Time.time;
        lerpDistance = Vector3.Distance(player.transform.position, player.transform.position + transform.forward * (chargeMultiplier * chargeTimer));
    }

    //Charge Attack Implementation
    public void ChargeAttack()
    {
        //Attack
        if (lerpDuration < 1 && !hitSomeone)
        {
            lerpDuration = (Time.time - lerpStartTime) * chargeSpeed / lerpDistance;
            player.transform.position = Vector3.Lerp(lerpStartPos, lerpEndPos, lerpDuration);
        }
        else if (hitSomeone)
        {
            Debug.Log("Hit");
            lerpStartPos = player.transform.position;
            lerpEndPos = player.transform.position + -transform.forward * bounceMultiplier;
            isBouncing = true;
            lerpStartTime = Time.time;
            lerpDistance = Vector3.Distance(player.transform.position, player.transform.position + -transform.forward * bounceMultiplier);
            isCharging = false;
            hitSomeone = false;
        }
        else
        {
            isCharging = false;
            lerpDuration = 0;
        }
    }
    public void MovementControls()
    {
        if (Input.GetKey(KeyCode.D))
        {
            //playerRB.velocity = transform.right * playerSpeed;
            player.transform.position += transform.right * playerSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            //playerRB.velocity = -transform.right * playerSpeed;
            player.transform.position += -transform.right * playerSpeed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            //playerRB.velocity = -transform.right * playerSpeed;
            player.transform.position += transform.forward * playerSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            //playerRB.velocity = -transform.right * playerSpeed;
            player.transform.position += -transform.forward * playerSpeed;
        }

        //float x = Input.GetAxis("Mouse Y");
        float y = -Input.GetAxis("Mouse X");
        player.transform.Rotate(0, -y, 0);
        //Camera.main.transform.Rotate(-x, 0, 0);
    }
    public void BounceBack()
    {
        //Bounce
        if (lerpDuration < 1 && !hitSomeone)
        {
            lerpDuration = (Time.time - lerpStartTime) * chargeSpeed / lerpDistance;
            player.transform.position = Vector3.Lerp(lerpStartPos, lerpEndPos, lerpDuration);
        }
        else
        {
            isBouncing = false;
            lerpDuration = 0;
        }
    }
    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;
    }
    public void Death()
    {
        //Respawn
    }

    public string GetTeam()
    {
        return playerTeam;
    }
}

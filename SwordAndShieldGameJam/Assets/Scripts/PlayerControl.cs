using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour
{
    public float health;
    private float currentHealth;
   
    private Rigidbody playerRB;
    private GameObject player;
    public GameObject shield;
    public GameObject sideShieldPos;
    public GameObject frontShieldPos;
    private float playerSpeed;
    public float playerBaseSpeed;
    public float playerShieldSpeed;
    public float chargeMultiplier;
    private float chargeTimer;
    public float chargeLimit;
    public bool isCharging = false;
    public bool hitSomeone = false;
    private Vector3 chargeStartTrans;
    private Vector3 chargeEndTrans;
    private float chargeStartTime;
    private float chargeDistance;
    public float chargeSpeed = 1f;
    private float chargeDuration;
    //public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        //playerRB = GetComponent<Rigidbody>();
        player = this.gameObject;
        playerSpeed = playerBaseSpeed;
    }

    // Update is called once per frame
    void Update()
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
                Debug.Log("Release");
                //Attack
                ChargePrep();
                //isCharging = true;
                chargeTimer = 0;
            }
            if (isCharging)
            {
                ChargeAttack();
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
        chargeStartTrans = player.transform.position;
        chargeEndTrans = player.transform.position + transform.forward * (chargeMultiplier * chargeTimer);
        isCharging = true;
        chargeStartTime = Time.time;
        chargeDistance = Vector3.Distance(player.transform.position, player.transform.position + transform.forward * (chargeMultiplier * chargeTimer));
    }

    //Charge Attack Implementation
    public void ChargeAttack()
    {
        //Attack
        if (chargeDuration < 1 && !hitSomeone)
        {
            chargeDuration = (Time.time - chargeStartTime) * chargeSpeed / chargeDistance;
            player.transform.position = Vector3.Lerp(chargeStartTrans, chargeEndTrans, chargeDuration);
        }
        else if (hitSomeone)
        {
            BounceBack();
            isCharging = false;
        }
        else
        {
            isCharging = false;
            chargeDuration = 0;
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

    }
    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;
    }
    public void Death()
    {
        //Respawn
    }
}

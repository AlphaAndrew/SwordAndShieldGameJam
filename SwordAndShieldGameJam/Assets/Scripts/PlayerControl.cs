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
    [SyncVar]
    private GameObject player;
    [SyncVar]
    public GameObject playerBody;
    [SyncVar]
    public int playerNum = 0;
    [SyncVar]
    public string playerTeam;
    [SyncVar]
    public float playerScore;
    //Health
    [SyncVar]
    public float health;
    [SyncVar]
    private float currentHealth;
    //Speed
    public float playerBaseSpeed;
    public float playerShieldSpeed;
    private float playerSpeed;
    //Shield
    [SyncVar]
    public GameObject shield;
    [SyncVar]
    public GameObject sideShieldPos;
    [SyncVar]
    public GameObject frontShieldPos;
    //Charge
    public float chargeMultiplier;
    public float chargeMinLimit;
    public float chargeMaxLimit;
    [SyncVar]
    public bool isCharging = false;
    public float chargeSpeed = 1f;
    [SyncVar]
    private float chargeTimer;
    //Lerp
    [SyncVar]
    private Vector3 lerpStartPos;
    [SyncVar]
    private Vector3 lerpEndPos;
    [SyncVar]
    private float lerpDistance;
    [SyncVar]
    private float lerpStartTime;
    [SyncVar]
    private float lerpDuration;
    //Bounce
    [SyncVar]
    public bool hitSomeone = false;
    [SyncVar]
    public bool isBouncing = false;
    public float bounceMultiplier;
    public Camera camera;
    private Renderer rend;
    public bool cantMove;
    public GameObject[] spawnPoints;
    public float deathTime;
    // Start is called before the first frame update
    void Start()
    {

        if (playerNum == 0)
        {
            playerTeam = "Red";
            
            playerNum++;
        }
        else if (playerNum == 1)
        {
            playerTeam = "Blue";
            playerNum++;
        }
        
        if (isLocalPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CmdVariableSync();
            return;
        }
        camera.enabled = false;
    }
    [Command]
    public void CmdVariableSync()
    {
        rend = GetComponentInChildren<Renderer>();
        if (playerTeam == "Red")
        {
            rend.material.color = Color.red;
        }
        playerRB = GetComponentInChildren<Rigidbody>();

        player = this.gameObject;
        playerSpeed = playerBaseSpeed;
        //playerBody = GameObject.FindGameObjectWithTag("Player");
        playerTeam = "Red";
        shield.GetComponent<BoxCollider>().enabled = false;
        return;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (currentHealth <= 0)
        {
            StartCoroutine("Death");
        }

        MovementControls();

        //Normal Attack
        //Charge Attack
        if (Input.GetMouseButton(0))
        {
            cantMove = true;
            if (chargeTimer < chargeMaxLimit)
            {
                chargeTimer += Time.deltaTime;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            cantMove = false;
            if (chargeTimer < chargeMinLimit) { return; }
            //Attack
            ChargePrep();
            //isCharging = true;
            //playerRB.AddRelativeForce(Vector3.forward *(chargeMultiplier*chargeTimer), ForceMode.Impulse);
            chargeTimer = 0;
        }
        if (isCharging)
        {
            ChargeAttack();
        }
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

    //Shield to front of player
    public void ShieldUp()
    {
        shield.GetComponent<BoxCollider>().enabled = true;
        shield.transform.SetPositionAndRotation(frontShieldPos.transform.position, frontShieldPos.transform.rotation);
        playerSpeed = playerShieldSpeed;
    }
    //Shield back down to side of player
    public void ShieldDown()
    {
        shield.GetComponent<BoxCollider>().enabled = false;
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
            BouncePrep();
        }
        else
        {
            isCharging = false;
            lerpDuration = 0;
        }
    }
    public void BouncePrep()
    {
        Debug.Log("BouncePrep");
        lerpStartPos = player.transform.position;
        lerpEndPos = player.transform.position + -transform.forward * bounceMultiplier;        
        lerpStartTime = Time.time;
        lerpDistance = Vector3.Distance(player.transform.position, player.transform.position + -transform.forward * bounceMultiplier);
        isBouncing = true;
        isCharging = false;
        hitSomeone = false;
    }
    public void BounceBack()
    {
        Debug.Log("Bounce");
        //Bounce
        if (lerpDuration < 1)
        {

            lerpDuration = (Time.time - lerpStartTime) * chargeSpeed / lerpDistance;
            player.transform.position = Vector3.Lerp(lerpStartPos, lerpEndPos, lerpDuration);
            Debug.Log("start: " + lerpStartPos + " End: " + lerpEndPos);
        }
        else
        {
            Debug.Log("Bouncing false");
            isBouncing = false;
            lerpDuration = 0;
        }
    }

    public void MovementControls()
    {
        Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //if (m_Input.sqrMagnitude > 1)
        //{
        //    m_Input.Normalize();
        //}

        //if (playerRB.velocity.magnitude < 10f)
        //{
        //    playerRB.AddRelativeForce(m_Input * playerSpeed, ForceMode.VelocityChange);
        //}
        if (!cantMove)
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
        }
        //float x = Input.GetAxis("Mouse Y");
        float y = -Input.GetAxis("Mouse X");
        player.transform.Rotate(0, -y, 0);
        //Camera.main.transform.Rotate(-x, 0, 0);
    }

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;
    }
    /// <summary>
    /// Add points to the local player
    /// </summary>
    /// <param name="value"></param>
    public void IncrimentPoints(float value)
    {
        playerScore += value;
    }
    public IEnumerator Death()
    {
        cantMove = true;
        currentHealth = health;
        playerBody.SetActive(false);
        int rand = Random.Range(0, spawnPoints.Length);
        this.gameObject.transform.position = spawnPoints[rand].transform.position;
        yield return new WaitForSeconds(deathTime);
        playerBody.SetActive(true);
        cantMove = false;
    }

    public string GetTeam()
    {
        return playerTeam;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
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
    public string playerTeam;
    [SyncVar]
    public float playerScore;
    ////Health
    //[SyncVar]
    //public float health;
    //[SyncVar]
    //private float currentHealth = 10;
    //Speed
    public float playerBaseSpeed;
    public float playerShieldSpeed;
    private float playerSpeed;
    //Shield
    [SyncVar]
    public GameObject shield;
    public Material shieldMat;
    [SyncVar]
    public GameObject sword;
    public Material swordMat;
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
    public Renderer rend;
    public bool cantMove;
    public GameObject[] spawnPoints;
    public float deathTime;
    public GameObject canvas;
    public GameObject winImage;
    public GameObject loseImage;
    public Animator anim;

    // Sound variables
    public Coroutine footstepSounds;
    private AudioSource audioSource;
    public AudioClip footSteps;
    public AudioClip rocketSound;
    public bool isWalking;
    private bool isFootStepsPlaying;

    // Start is called before the first frame update
    void Start()
    {

        if (isLocalPlayer)
        {
            audioSource = GetComponent<AudioSource>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (isServer)
            {
                VariableSync();
            }
            else
            {
                CmdVariableSync();
            }           
            return;
        }
        camera.enabled = false;
    }
    [Command]
    public void CmdVariableSync()
    {
        //currentHealth = health;
        rend = GetComponentInChildren<Renderer>();
        playerRB = GetComponentInChildren<Rigidbody>();
        anim = this.gameObject.GetComponentInChildren<Animator>();
        player = this.gameObject;
        playerSpeed = playerBaseSpeed;
        //playerBody = GameObject.FindGameObjectWithTag("Player");
        shield.GetComponent<BoxCollider>().enabled = false;
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPos");
        return;
    }
    public void VariableSync()
    {
        //currentHealth = health;
        rend = GetComponentInChildren<Renderer>();
        playerRB = GetComponentInChildren<Rigidbody>();
        anim = this.gameObject.GetComponentInChildren<Animator>();
        player = this.gameObject;
        playerSpeed = playerBaseSpeed;
        //playerBody = GameObject.FindGameObjectWithTag("Player");
        shield.GetComponent<BoxCollider>().enabled = false;
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPos");
        return;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerTeam == "Red")
        {
            Debug.Log(player + "Red");
            rend.material.color = Color.red;
            sword.GetComponent<Renderer>().material = swordMat;
            shield.GetComponent<Renderer>().material = shieldMat;
        }
        if (!isLocalPlayer)
        {
            return;
        }

        //if (currentHealth <= 0)
        //{
        //    StartCoroutine("Death");
        //}

        MovementControlsPhysics();

        //Normal Attack
        //Charge Attack
        if (Input.GetMouseButton(0))
        {
            cantMove = true;
            if (chargeTimer < chargeMaxLimit)
            {
                chargeTimer += Time.deltaTime;
            }
            anim.SetBool("chargeAnimBool", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            cantMove = false;
            if (chargeTimer < chargeMinLimit) { return; }
            //sound
            //audioSource.clip = rocketSound;
            //audioSource.Play();
            //Attack
            //ChargePrep();
            isCharging = true;
            playerRB.AddRelativeForce(Vector3.forward *(chargeMultiplier*chargeTimer), ForceMode.Impulse);
            chargeTimer = 0;
        }
        if (isCharging)
        {
            //ChargeAttack();
            
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
            anim.SetBool("chargeAnimBool", false);
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
        anim.SetBool("chargeAnimBool", false);
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
    IEnumerator FootstepSounds()
    {
       
        if (isWalking)
        {
            isFootStepsPlaying = true;
            audioSource.clip = footSteps;
            audioSource.Play();
           
            yield return new WaitForSeconds(2f);
            isFootStepsPlaying = false;

        }
        yield return null;

    }

    /// <summary>
    /// Handles input and movement using physics
    /// </summary>
    public void MovementControlsPhysics()
    {
        Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // moves the camera as the mouse moves
        float y = -Input.GetAxis("Mouse X");
        player.transform.Rotate(0, -y, 0);

        // When the player veloctity is within moveable range, ends charge
        if (playerRB.velocity.magnitude <= 11f)
        {
            if (isCharging)
            {
                anim.SetBool("chargeAnimBool", false);
                isCharging = false;
            }
        }

        if (!cantMove)
        {
            // triggers all this other stuff when there is input
            if (m_Input.sqrMagnitude > 1)
            {
                // make sure that you cannot move faster than max speed by holding down two directions
                m_Input.Normalize();

                // checks to see if movement is faster than maximum speed
                if (playerRB.velocity.magnitude < 10f)
                {
                    playerRB.AddRelativeForce(m_Input, ForceMode.VelocityChange);
                }

                // sound and animations triggered here. 
                if (!isCharging)
                {
                    isWalking = true;
                    if (!isFootStepsPlaying)
                    {
                        footstepSounds = StartCoroutine(FootstepSounds());
                    }
                    anim.SetBool("isWalking", true);
                }
            }

            // if moving slower than threshold, goes back to idle animation
            else if (playerRB.velocity.magnitude <= 0.25f)
            {
                anim.SetBool("isWalking", false);
            }

            
        }
        
    }

    /// <summary>
    /// Handles input and movement using transform
    /// good for moving through walls
    /// </summary>
    public void MovementControlsTransform()
    {

        if (!cantMove)
        {
            if (Input.GetKey(KeyCode.D))
            {
                isWalking = true;
                if (!isFootStepsPlaying)
                {
                    footstepSounds = StartCoroutine(FootstepSounds());
                }

                //playerRB.velocity = transform.right * playerSpeed;
                player.transform.position += transform.right * playerSpeed;
                anim.SetBool("isWalking", true);
            }

            if (Input.GetKey(KeyCode.A))
            {
                isWalking = true;
                if (!isFootStepsPlaying)
                {
                    footstepSounds = StartCoroutine(FootstepSounds());
                }

                //playerRB.velocity = -transform.right * playerSpeed;
                player.transform.position += -transform.right * playerSpeed;
                anim.SetBool("isWalking", true);
            }
            if (Input.GetKey(KeyCode.W))
            {
                isWalking = true;
                if (!isFootStepsPlaying)
                {
                    footstepSounds = StartCoroutine(FootstepSounds());
                }

                //playerRB.velocity = -transform.right * playerSpeed;
                player.transform.position += transform.forward * playerSpeed;
                anim.SetBool("isWalking", true);
            }
            if (Input.GetKey(KeyCode.S))
            {
                isWalking = true;
                if (!isFootStepsPlaying)
                {
                    footstepSounds = StartCoroutine(FootstepSounds());
                }

                //playerRB.velocity = -transform.right * playerSpeed;
                player.transform.position += -transform.forward * playerSpeed;
                anim.SetBool("isWalking", true);
            }
            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
            {
                anim.SetBool("isWalking", false);
            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
            {
                StopCoroutine(FootstepSounds());
                isWalking = false;
                isFootStepsPlaying = false;
                audioSource.clip = null;
            }
        }
        float x = Input.GetAxis("Mouse Y");

        // moves the camera as the mouse moves
        float y = -Input.GetAxis("Mouse X");
        player.transform.Rotate(0, -y, 0);
    }

    /// <summary>
    /// Called by other functions to apply a force
    /// </summary>
    /// <param name="direction">Vector3 for direction to apply the force in, relative to local</param>
    /// <param name="strength">forcce multiplier</param>
    public void ApplyForce(Vector3 direction, float strength)
    {
        playerRB.AddRelativeForce(direction * strength, ForceMode.Impulse);
    }

    /// <summary>
    /// Called by other fucntions to apply damage to the player
    /// </summary>
    /// <param name="damage">damage to be dealt</param>
    public void ApplyDamage(float damage)
    {
        //currentHealth -= damage;
    }

    /// <summary>
    /// Add points to the local player
    /// </summary>
    /// <param name="value"></param>
    public void IncrimentPoints(float value)
    {
        playerScore += value;
    }

    //public IEnumerator Death()
    //{
    //    cantMove = true;
    //    currentHealth = health;
    //    playerBody.SetActive(false);
    //    int rand = Random.Range(0, spawnPoints.Length -1);
    //    this.gameObject.transform.position = spawnPoints[rand].transform.position;
    //    yield return new WaitForSeconds(deathTime);
    //    playerBody.SetActive(true);
    //    cantMove = false;
    //}

    public string GetTeam()
    {
        return playerTeam;
    }

    public void Win()
    {
        cantMove = true;
        canvas.SetActive(true);
        winImage.SetActive(true);
    }

    public void Lose()
    {
        cantMove = true;
        canvas.SetActive(true);
        loseImage.SetActive(true);
    }
}

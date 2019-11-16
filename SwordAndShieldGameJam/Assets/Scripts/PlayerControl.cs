using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    private Rigidbody playerRB;
    private GameObject player;
    public float playerSpeed;
    public float chargeMultiplier;
    private float chargeTimer;
    public float chargeLimit;
    private bool isCharging = false;

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
    }

    // Update is called once per frame
    void Update()
    {
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
        else if(Input.GetMouseButtonUp(0)) {
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
    }

    public void ChargePrep()
    {
        chargeStartTrans = player.transform.position;
        chargeEndTrans = player.transform.position + transform.forward * (chargeMultiplier * chargeTimer);
        isCharging = true;
        chargeStartTime = Time.time;
        chargeDistance = Vector3.Distance(player.transform.position, player.transform.position + transform.forward * (chargeMultiplier * chargeTimer));
    }
    public void ChargeAttack()
    {
        //Attack
        if (chargeDuration < 1)
        {
            chargeDuration = (Time.time - chargeStartTime) * chargeSpeed / chargeDistance;
            player.transform.position = Vector3.Lerp(chargeStartTrans, chargeEndTrans, chargeDuration);
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
}

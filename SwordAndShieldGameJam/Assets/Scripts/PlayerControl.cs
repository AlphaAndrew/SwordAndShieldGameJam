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
            ChargeAttack();
            chargeTimer = 0;
        }
    }

    public void ChargeAttack()
    {

        player.transform.position += transform.forward * (chargeMultiplier * chargeTimer); //* (chargeTimer/120);
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

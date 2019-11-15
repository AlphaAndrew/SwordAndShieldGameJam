using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    private Rigidbody playerRB;
    private GameObject player;
    public float playerSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //playerRB = GetComponent<Rigidbody>();
        player = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //playerRB.velocity = transform.right * playerSpeed;
            player.transform.position += transform.right * playerSpeed;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //playerRB.velocity = -transform.right * playerSpeed;
            player.transform.position += -transform.right * playerSpeed;
        }
    }
}

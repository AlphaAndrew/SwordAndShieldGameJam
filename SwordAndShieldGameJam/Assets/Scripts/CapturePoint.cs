using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class CapturePoint : NetworkBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> playersInRange;
    public List<GameObject> waypoints;
    public bool isMoving;
    public float moveSpeed;
    public int waypointCounter;

    void Start()
    {
        if (isServer)
        {
            playersInRange = new List<GameObject>();
            waypointCounter = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isServer)
        {
            string colliderTag = other.gameObject.tag;
            switch (colliderTag)
            {
                case "Player":
                    playersInRange.Add(other.gameObject);
                    break;
                case "Waypoint":
                    waypointCounter++;
                    break;

                default:
                    // Debug.Log("Collided with something other then player)"
                    break;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (isServer)
        {
            string colliderTag = other.gameObject.tag;
            switch (colliderTag)
            {
                case "Player":
                    playersInRange.Remove(other.gameObject);
                    break;
                default:
                    // Debug.Log("Collided with something other then player)"
                    break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isServer && isMoving) {
            //move towards the waypoint
            Vector3 targetDir = waypoints[waypointCounter].transform.position - this.transform.position;
            this.gameObject.transform.Translate(targetDir * Time.deltaTime * moveSpeed);
        }
    }
}

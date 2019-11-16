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
    private float rotSpeed;
    public float distanceToWaypoint;

    void Start()
    {
        if (isServer)
        {
            playersInRange = new List<GameObject>();
            waypointCounter = 0;
            rotSpeed = 10;
            isMoving = true;
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
                    Debug.Log("Added Player to control point list");
                    playersInRange.Add(other.gameObject);
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

            
            distanceToWaypoint = Vector3.Distance(waypoints[waypointCounter].transform.position, this.gameObject.transform.position);

            ////Rotate to the target point
            Quaternion targetRotation = Quaternion.LookRotation(waypoints[waypointCounter].transform.position - this.gameObject.transform.position);
            targetRotation = Quaternion.Slerp(this.gameObject.transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
            this.gameObject.transform.rotation = targetRotation;
            ////move forward
            this.gameObject.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

            if(distanceToWaypoint <= 3.0f)
            {
                if (waypointCounter < waypoints.Count -1)
                {
                    waypointCounter++;
                    Debug.Log("waypoint counter: " + waypointCounter);
                   
                }
                else
                {
                    Debug.Log("reset counter to zero");

                    waypointCounter = 0;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CapturePoint : NetworkBehaviour
{
    //Reference to StatsManager
    StatsManager statsManager;

    // Start is called before the first frame update
    public List<GameObject> playersInRadius;
    public List<GameObject> waypoints;

    //All movement variables
    public bool canMove;
    public float moveSpeed;
    private int waypointCounter;
    private float rotSpeed;
    public float distanceToWaypoint;

    //Battle Variables 
    public float addPointAmount;
    public float addPointInterval;
    public enum PointStatus{
        Contested,
        Controlled,
        Uncontested
    }
    public PointStatus pointStatus;


    void Start()
    {
        if (isServer)
        {
            statsManager = GameObject.Find("StatsManager").GetComponent<StatsManager>();

            playersInRadius = new List<GameObject>();
            waypointCounter = 0;
            rotSpeed = 10;
            canMove = true;
            StartCoroutine(DoBattle());
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            if (canMove)
            {
                distanceToWaypoint = Vector3.Distance(waypoints[waypointCounter].transform.position, this.gameObject.transform.position);
                Pathfind(distanceToWaypoint);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (isServer)
        {
            string colliderTag = other.tag;
            switch (colliderTag)
            {
                case "PlayerObject":
                    playersInRadius.Add(other.gameObject);
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
                case "PlayerObject":
                    playersInRadius.Remove(other.gameObject);
                    break;
                default:
                    // Debug.Log("Collided with something other then player)"
                    break;
            }
        }
    }
    //handles moving to a node
    private void Move()
    {
        ////Rotate to the target point
        Quaternion targetRotation = Quaternion.LookRotation(waypoints[waypointCounter].transform.position - this.gameObject.transform.position);
        targetRotation = Quaternion.Slerp(this.gameObject.transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
        this.gameObject.transform.rotation = targetRotation;
        ////move forward
        this.gameObject.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }

    private PointStatus SetPointStatus()
    {
        //Checks the number of players in the radius and sets the point status
        if (playersInRadius.Count == 1)
        {
            return PointStatus.Controlled;
        }
        else if (playersInRadius.Count > 1)
        {
            return PointStatus.Contested;
        }
        else
        {
            return PointStatus.Uncontested;
        }
    }

    /// <summary>
    /// The main function for battle. Checks the points status and rewards points.
    /// </summary>
    private IEnumerator DoBattle()
    {
        while (true)
        {
            //sets the status of the point (contested, controlled,etc)
            yield return new WaitForEndOfFrame();
            pointStatus = SetPointStatus();
            switch (pointStatus)
            {
                case PointStatus.Contested:
                    //contested
                    break;
                case PointStatus.Controlled:
                    //buffer for gameplay balance
                    yield return new WaitForSeconds(.25f);
                    if (playersInRadius.Count == 1)
                    {
                        playersInRadius[0].gameObject.GetComponent<PlayerControl>().IncrimentPoints(addPointAmount);

                        string team = playersInRadius[0].gameObject.GetComponent<PlayerControl>().GetTeam();
                        switch (team)
                        {
                            case "Red":
                                statsManager.AddRedPoints(addPointAmount);
                                break;
                            case "Blue":
                                statsManager.AddBluePoints(addPointAmount);
                                break;
                            default:
                                break;
                        }
                        yield return new WaitForSeconds(addPointInterval);
                    }
                    break;
                case PointStatus.Uncontested:
                    //Uncontested
                    break;
                default:
                    break;

            }
        }
    }
    /// <summary>
    /// Checks distance between the capture point and the node and paths to the next waypoint 
    /// </summary>
    /// <param name="distanceToNode"> Distance between the capturepoint and the node </param>
    public void Pathfind(float distanceToNode)
    {
        Move();
        if (distanceToWaypoint <= 3.0f)
        {
            if (waypointCounter < waypoints.Count - 1)
            {
                waypointCounter++;
            }
            else
            {
                waypointCounter = 0;
            }
        }
    }
}

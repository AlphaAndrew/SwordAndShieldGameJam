using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StatsManager : NetworkBehaviour
{
    public float redTeamScore;
    public float blueTeamScore;
    public GameObject[] players;
    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            FillPlayerList();
            GetScores();
        }

    }
    public void FillPlayerList()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
    public void GetScores()
    {
        foreach (GameObject player in players)
        {
            if (player.GetComponent<PlayerControl>().GetTeam() != null)
            {
                string team = player.GetComponent<PlayerControl>().GetTeam();
                switch (team)
                {
                    case "Red":
                        redTeamScore += player.GetComponent<PlayerControl>().playerScore;
                        break;
                    case "Blue":
                        blueTeamScore += player.GetComponent<PlayerControl>().playerScore;
                        break;

                    default:
                        Debug.Log("Player is on a team color that is not in the available set of teams");
                        break;

                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StatsManager : NetworkBehaviour
{
    public float redTeamScore;
    public float blueTeamScore;
    public GameObject[] players;
    public bool activeBattle;

    
    void Start()
    {
        if (isServer)
        {
            StartCoroutine(FillPlayerList());
           // StartCoroutine(GetScores());
           
        }
    }
    void Update()
    {
        CheckForBattle();
    }
    //get all active players
    public IEnumerator FillPlayerList()
    {
        yield return new WaitForEndOfFrame();
        players = GameObject.FindGameObjectsWithTag("PlayerObject");
        yield return null;
    }

    //probably shouldnt loop through players every frame so I commented for now
    //only two players so its not that expensive

  /*  public IEnumerator GetScores()
    {
        yield return new WaitForSeconds(.25f);
        foreach (GameObject player in players)
        {
            var redScore = 0.0f;
            var blueScore = 0.0f;
            if (player.GetComponent<PlayerControl>().GetTeam() != null)
            {
                string team = player.GetComponent<PlayerControl>().GetTeam();

                  
                switch (team)
                {
                    case "Red":
                        redScore += player.GetComponent<PlayerControl>().playerScore;
                        break;
                    case "Blue":
                        blueScore += player.GetComponent<PlayerControl>().playerScore;
                        break;

                    default:
                        Debug.Log("Player is on a team color that is not in the available set of teams");
                        break;

                }
            }
            if(redTeamScore != redScore)
            {
                redTeamScore += redScore;
            }
            if(blueTeamScore != blueScore)
            {
                blueTeamScore += blueScore;
            }
        }
        yield return null;
    }*/

    /// <summary>
    /// Returns whether or not there is an active battle;
    /// </summary>
    public void CheckForBattle()
    {
        if (players.Length == 0)
        {
            //if no players are found, there is no active battle
            activeBattle = false;
        }
        else
        {
            //if alteast one player is in the scene
            activeBattle =true;
        }
    }
}

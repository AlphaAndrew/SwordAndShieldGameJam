using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class StatsManager : NetworkBehaviour
{
    [SyncVar]
    public float redTeamScore = 0;
    [SyncVar]
    public float blueTeamScore = 0;

    public Text blueScoreText;
    public Text redScoreText;
    public GameObject[] players;
    public bool activeBattle;
    public int finalScore;

    private float updateInterval = .25f;
    private float updateIntervalTimer = 0;
    
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
        if (!isServer)
        { return; }

        updateIntervalTimer += Time.deltaTime;
        if (updateIntervalTimer > updateInterval)
        {
            Debug.Log("Update");
            CheckForBattle();
            RpcUpdateScoreText();
            
            updateIntervalTimer = 0;
            if(redTeamScore >= finalScore || blueTeamScore >= finalScore)
            {
                RpcEndGame();
            }
        }
            
        
    }

    public void AddBluePoints(float value)
    {
        blueTeamScore += value;
    }
    public void AddRedPoints(float value)
    {
        redTeamScore += value;
    }

    [ClientRpc]
    public void RpcUpdateScoreText()
    {
        blueScoreText.text = "Blue Score: "+ blueTeamScore;
        redScoreText.text = "Red Score: " + redTeamScore;
    }
    [ClientRpc]
    public void RpcEndGame()
    {
        foreach(GameObject play in players)
        {
            if (redTeamScore > blueTeamScore)
            {
                if (play.GetComponent<PlayerControl>().playerTeam == "Red")
                {
                    play.GetComponent<PlayerControl>().Win();
                }
                else
                {
                    play.GetComponent<PlayerControl>().Lose();
                }
            }else if (blueTeamScore > redTeamScore)
            {
                if (play.GetComponent<PlayerControl>().playerTeam == "Blue")
                {
                    play.GetComponent<PlayerControl>().Win();
                }
                else
                {
                    play.GetComponent<PlayerControl>().Lose();
                }
            }

        }
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

    //public IEnumerator GetScores()
    //{
    //    yield return new WaitForSeconds(.25f);
    //    foreach (GameObject player in players)
    //    {
    //        var redScore = 0.0f;
    //        var blueScore = 0.0f;
    //        if (player.GetComponent<PlayerControl>().GetTeam() != null)
    //        {
    //            string team = player.GetComponent<PlayerControl>().GetTeam();

    //            switch (team)
    //            {
    //                case "Red":
    //                    redScore += player.GetComponent<PlayerControl>().playerScore;
    //                    break;
    //                case "Blue":
    //                    blueScore += player.GetComponent<PlayerControl>().playerScore;
    //                    break;

    //                default:
    //                    Debug.Log("Player is on a team color that is not in the available set of teams");
    //                    break;

    //            }
    //        }
    //        if(redTeamScore != redScore)
    //        {
    //            redTeamScore += redScore;
    //        }
    //        if(blueTeamScore != blueScore)
    //        {
    //            blueTeamScore += blueScore;
    //        }
    //    }
    //    yield return null;
    //}

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StatsManager : NetworkBehaviour
{
    public int redTeamScore;
    public int blueTeamScore;
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
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

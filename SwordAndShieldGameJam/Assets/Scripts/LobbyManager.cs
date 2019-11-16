using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyManager : NetworkLobbyManager
{
    public GameObject lobby;
    public GameObject canvas;
    public GameObject[] lobbyPlayers;
    private void Start()
    {
        lobby.SetActive(false);
    }
    public override void OnStartHost()
    {
        base.OnStartHost();
        lobby.SetActive(true);
        Debug.Log("Game Start");
    }
    public override void OnStartClient(NetworkClient lobbyClient)
    {
        base.OnStartClient(lobbyClient);
        lobby.SetActive(true);
    }

    public override void OnLobbyServerPlayersReady()
    {
        base.OnLobbyServerPlayersReady();
        lobbyPlayers = GameObject.FindGameObjectsWithTag("LobbyPlayer");
        foreach(GameObject player in lobbyPlayers)
        {
            player.transform.parent = null;
        }
        canvas.SetActive(false);
    }
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
        canvas.SetActive(false);
    }
}

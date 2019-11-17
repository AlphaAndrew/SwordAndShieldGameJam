using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyManager : NetworkLobbyManager
{
    public GameObject cam;
    private SceneChanger sceneChangerScript;
    public GameObject lobby;
    public GameObject canvas;
    public GameObject[] lobbyPlayers;
    public int playerNum = 0;
    private void Start()
    {
        sceneChangerScript = cam.GetComponent<SceneChanger>();
        lobby.SetActive(false);
    }
    public override void OnLobbyStartHost()
    {
        base.OnLobbyStartHost();
        lobby.SetActive(true);
        sceneChangerScript.LobbyEnter();
    }
    public override void OnStopHost()
    {
        base.OnStopHost();
        ServerReturnToLobby();
        Debug.Log("Host Stop");
    }
    public override void OnLobbyStartClient(NetworkClient lobbyClient)
    {
        base.OnLobbyStartClient(lobbyClient);
        lobby.SetActive(true);
        sceneChangerScript.LobbyEnter();
    }
    public override void OnStopClient()
    {
        base.OnStopClient();
        ServerReturnToLobby(); 
        Debug.Log("Client Stop");
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

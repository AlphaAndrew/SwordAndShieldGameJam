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
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        playerNum++;
        var player = (GameObject)GameObject.Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        if (playerNum == 1) { player.GetComponent<PlayerControl>().playerTeam = "Red"; }
        if (playerNum == 2) { player.GetComponent<PlayerControl>().playerTeam = "Blue"; }
        //PlayerControl playerScript;
        //playerScript = GetComponent<PlayerControl>();
        //ManGame.playerNumber++;
        //var player = (GameObject)GameObject.Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //if (ManGame.playerNumber == 1) { player.GetComponent<NetPlayer>().team = 1; }
        //if (ManGame.playerNumber == 2) { player.GetComponent<NetPlayer>().team = 2; }
        //NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        //player.GetComponent<NetPlayer>().Init();
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

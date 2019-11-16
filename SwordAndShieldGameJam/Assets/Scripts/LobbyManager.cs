using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyManager : NetworkLobbyManager
{
    public GameObject lobby;

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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyScript : NetworkLobbyPlayer
{
    public GameObject parentPref;
    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();
        parentPref = GameObject.FindGameObjectWithTag("ParentPref");
        gameObject.transform.SetParent(parentPref.transform);
    }
}

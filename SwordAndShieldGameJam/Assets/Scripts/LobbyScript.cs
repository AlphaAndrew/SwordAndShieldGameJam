using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyScript : NetworkLobbyPlayer
{
    private GameObject parentPref;
    public Button joinButton;
    public Text playerText;
    public Text joinText;

    public void OnClickJoinButton()
    {
        if (isLocalPlayer)
        {
            SendReadyToBeginMessage();
            joinText.text = "Ready";
        }

    }
    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();
        parentPref = GameObject.FindGameObjectWithTag("ParentPref");
        gameObject.transform.SetParent(parentPref.transform);
        
    }
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Setup();
    }

    private void Setup()
    {
        if (isLocalPlayer)
        {
            playerText.text = "You";
            joinText.text = "Join";
            joinButton.enabled = true;
        }
        else
        {
            playerText.text = "Enemy";
            joinText.text = "...";
            joinButton.enabled = false;
        }
    }
}

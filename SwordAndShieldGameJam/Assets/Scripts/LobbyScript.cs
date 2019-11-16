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
    private bool setup = false;

    public void OnClickJoinButton()
    {
        SendReadyToBeginMessage();
        joinText.text = "Ready";
    }
    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();
        parentPref = GameObject.FindGameObjectWithTag("ParentPref");
        gameObject.transform.SetParent(parentPref.transform);
        if (setup)
        {
            Setup();
        }
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
        setup = true;
    }
}

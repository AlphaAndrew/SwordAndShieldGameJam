using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SceneChanger : NetworkBehaviour
{
    public string sceneOne;
    public string credits;
    public GameObject host;
    public GameObject play;
    public GameObject join;
    public GameObject back;
    public GameObject creditsButton;
    public GameObject joinFieldParent;
    public InputField joinField;
    public GameObject network;
    private NetworkLobbyManager manager;
    public bool inLobby = false;
    public bool isHost = false;

    private void Start()
    {
        play.SetActive(true);
        creditsButton.SetActive(true);
        joinFieldParent.SetActive(false);
        host.SetActive(false);
        join.SetActive(false);
        back.SetActive(false);
        manager = network.GetComponent<NetworkLobbyManager>();
        joinField.text = "localhost";
    }
    public void Play()
    {
        play.SetActive(false);
        creditsButton.SetActive(false);
        joinFieldParent.SetActive(true);
        host.SetActive(true);
        join.SetActive(true);
        back.SetActive(true);
    }
    public void Host()
    {
        manager.StartHost();
        isHost = true;
        //manager.StartServer();
        //NetworkServer.AddPlayerForConnection(conn, playerPrefab, 0);        
        //SceneManager.LoadScene(sceneOne);
        //ClientScene.AddPlayer(0);
    }
    public void Join()
    {
        manager.StartClient();
        isHost = false;
        //ClientScene.AddPlayer(0);
        //SceneManager.LoadScene(sceneOne);
    }
    public void Back()
    {
        if (inLobby)
        {
            play.SetActive(false);
            creditsButton.SetActive(false);
            joinFieldParent.SetActive(true);
            host.SetActive(true);
            join.SetActive(true);
            back.SetActive(true);
            if (isHost)
            {
                manager.StopHost();
            }else if (!isHost)
            {
                manager.StopClient();
            }
        }
        else
        {
            play.SetActive(true);
            creditsButton.SetActive(true);
            joinFieldParent.SetActive(false);
            host.SetActive(false);
            join.SetActive(false);
            back.SetActive(false);
        }

    }
    //Load Credits
    public void Credits()
    {
        SceneManager.LoadScene(credits);
    }

    private void Update()
    {
        manager.networkAddress = joinField.text;
    }

    public void LobbyEnter()
    {
        play.SetActive(false);
        creditsButton.SetActive(false);
        joinFieldParent.SetActive(false);
        host.SetActive(false);
        join.SetActive(false);
        back.SetActive(true);
        inLobby = true;
    }
    public void LobbyLeave()
    {

    }
}

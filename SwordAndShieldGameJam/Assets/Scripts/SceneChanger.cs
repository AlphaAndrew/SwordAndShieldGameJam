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
    public GameObject creditsButton;
    public GameObject joinFieldParent;
    public InputField joinField;
    public GameObject network;
    public GameObject backGround1;
    public GameObject backGround2;
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
        backGround1.SetActive(true);
        backGround2.SetActive(false);
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

        backGround1.SetActive(false);
        backGround2.SetActive(true);
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
        inLobby = true;
    }
    public void LobbyLeave()
    {

    }
}

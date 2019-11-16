using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SceneChanger : MonoBehaviour
{
    public string sceneOne;
    public string credits;
    public GameObject host;
    public GameObject play;
    public GameObject join;
    public GameObject back;
    public GameObject creditsButton;
    public GameObject network;
    private NetworkLobbyManager manager;

    private void Start()
    {
        play.SetActive(true);
        creditsButton.SetActive(true);
        host.SetActive(false);
        join.SetActive(false);
        back.SetActive(false);
        manager = network.GetComponent<NetworkLobbyManager>();
    }
    public void Play()
    {
        play.SetActive(false);
        creditsButton.SetActive(false);
        host.SetActive(true);
        join.SetActive(true);
        back.SetActive(true);
    }
    public void Host()
    {
        manager.StartHost();
        //manager.StartServer();
        //NetworkServer.AddPlayerForConnection(conn, playerPrefab, 0);        
        //SceneManager.LoadScene(sceneOne);
        //ClientScene.AddPlayer(0);
    }
    public void Join()
    {
        manager.StartClient();
        //ClientScene.AddPlayer(0);
        //SceneManager.LoadScene(sceneOne);
    }
    public void Back()
    {
        play.SetActive(true);
        creditsButton.SetActive(true);
        host.SetActive(false);
        join.SetActive(false);
        back.SetActive(false);
    }
    //Load Credits
    public void Credits()
    {
        SceneManager.LoadScene(credits);
    }

}

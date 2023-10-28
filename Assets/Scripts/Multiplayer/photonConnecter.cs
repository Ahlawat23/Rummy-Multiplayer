using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class photonConnecter : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";

    [SerializeField] private InputField joinFeild;
    public string levelToLoad;

    [Header("Artworks")]
    public Text connectingText;
    public GameObject gamePanel;
    public InputField userNameField;

    [Header("debug:")]
    public Text debugText;
    public string roomCode;

    private void Start()
    {
        gamePanel.SetActive(false);
        connectingText.gameObject.SetActive(true);
        connect();
    }

    public void connect()
    {
        
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
        
    }

    public override void OnConnectedToMaster()
    {   //have to wait for this message in order to use the create button
        Debug.Log("the server has made or connected, now we can create room");
     
        gamePanel.SetActive(true);
        connectingText.gameObject.SetActive(false);
    }

    public void createRoom()
    {
        PhotonNetwork.NickName = userNameField.text;
        roomCode = gernrateRandomRoomCode();
        PhotonNetwork.CreateRoom(roomCode);
        Debug.Log(" the room code is : " + roomCode);
    }

    private string gernrateRandomRoomCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        char[] stringchars = new char[4];

        for (int i = 0; i < stringchars.Length; i++)
        {
            stringchars[i] = chars[Random.Range(0, chars.Length - 1)];
        }
        string finalRoomCode = new string(stringchars);
        return finalRoomCode;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        debugText.text = message + " " + returnCode.ToString();
        Debug.Log(returnCode.ToString());
    }




    //playewithFriends join Room
    public void joinRoom()
    {
        PhotonNetwork.NickName = userNameField.text;
        PhotonNetwork.JoinRoom(joinFeild.text);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        debugText.text = message + " " + returnCode.ToString();
        Debug.Log(returnCode.ToString());
    }

    public override void OnJoinedRoom()
    {   //meaning that you have created or joined room;
        PhotonNetwork.LoadLevel(levelToLoad);
    }


    public override void OnJoinedLobby()
    {

        Debug.Log("current lobby is " + PhotonNetwork.CurrentLobby.Name);
        PhotonNetwork.JoinRandomOrCreateRoom();

    }
}

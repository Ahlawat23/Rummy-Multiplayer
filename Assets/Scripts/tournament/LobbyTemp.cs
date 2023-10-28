using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyTemp : MonoBehaviour
{
    public TypedLobby points1 = new TypedLobby("points1", LobbyType.Default);
    public TypedLobby points5 = new TypedLobby("points5", LobbyType.Default);
    public TypedLobby points10 = new TypedLobby("points10", LobbyType.Default);

    public TypedLobby pools20 = new TypedLobby("pools20", LobbyType.Default);
    public TypedLobby pools40 = new TypedLobby("pools40", LobbyType.Default);
    public TypedLobby pools80 = new TypedLobby("pools80", LobbyType.Default);

    public TypedLobby deals20 = new TypedLobby("deals20", LobbyType.Default);
    public TypedLobby deals40 = new TypedLobby("deals40", LobbyType.Default);




    public void jointPoints1()
    {

        PhotonNetwork.JoinLobby(points1);
        Debug.Log("the joined lobby is : " + PhotonNetwork.CurrentLobby);
    }

    public void jointPoints5()
    {

        PhotonNetwork.JoinLobby(points5);
        Debug.Log("the joined lobby is : " + PhotonNetwork.CurrentLobby);
    }

    public void jointPoints10()
    {

        PhotonNetwork.JoinLobby(points10);
        Debug.Log("the joined lobby is : " + PhotonNetwork.CurrentLobby);
    }

    public void Joinpools20()
    {

        PhotonNetwork.JoinLobby(pools20);
        Debug.Log("the joined lobby is : " + PhotonNetwork.CurrentLobby);
    }

    public void Joinpools40()
    {

        PhotonNetwork.JoinLobby(pools40);
        Debug.Log("the joined lobby is : " + PhotonNetwork.CurrentLobby);
    }

    public void Joinpools80()
    {

        PhotonNetwork.JoinLobby(pools80);
        Debug.Log("the joined lobby is : " + PhotonNetwork.CurrentLobby);
    }

    public void joindeals20()
    {

        PhotonNetwork.JoinLobby(deals20);
        Debug.Log("the joined lobby is : " + PhotonNetwork.CurrentLobby);
    }
    public void joindeals40()
    {

        PhotonNetwork.JoinLobby(deals40);
        Debug.Log("the joined lobby is : " + PhotonNetwork.CurrentLobby);
    }


    //geting online players in a lobby
    public void getOnlinePlayerInLoody(TypedLobby lobby)
    {
        Debug.Log(PhotonNetwork.CountOfPlayers);
    }

}

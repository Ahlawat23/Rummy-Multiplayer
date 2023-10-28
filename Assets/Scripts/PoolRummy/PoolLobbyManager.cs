using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class PoolLobbyManager : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks, IPunObservable
{
    public int totalPlayersCanPlay;

    [Header("Player card")]
    public GameObject[] playerCards;
    public TextMeshProUGUI[] lobbyText;


    [Header("Lobby Panel")]
    public GameObject lobbyPanel;
    public TextMeshProUGUI debugtextLobby;
    public Button startButton;

    [Header("inGameObjects")]
    //the three button in a list
    public List<GameObject> objectList = new List<GameObject>();

    public GameObject[] playerGemsList;
    public Text[] playerInGameTextList;


    [Header("syncing deck")]
    public List<int> deckValues;


    [Header("SEAT ARRANGMENT VARIABLES")]
    public Vector2 active2player;
    public Vector2 second2player;

    public Vector2 second3player;
    public Vector2 third3player;

    public Vector2 second4playerButFourthElement;
    public Vector2 fifth4player;

    public Vector2 active6player;
    public Vector2 sixth6player;

    private void Awake()
    {
        deckValues = new List<int>(106);
    }
    private void Start()
    {
        debugtextLobby.text = PhotonNetwork.CurrentRoom.Name;
        updateLobbyDataCaller();
    }

    public void updateLobbyDataCaller()
    {
        photonView.RPC("updateLobbyData", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    void updateLobbyData()
    {
        totalPlayersCanPlay = PhotonNetwork.PlayerList.Length;
        for (int i = 0; i < totalPlayersCanPlay; i++)
        {
            setplayerCards(i);
            lobbyText[i].text = PhotonNetwork.PlayerList[i].NickName;
        }

    }

    public void setplayerCards(int index)
    {
        for (int i = 0; i < playerCards.Length; i++)
        {
            if (i <= index)
                playerCards[i].SetActive(true);
            else
                playerCards[i].SetActive(false);
        }
    }
    public void lobbyStartButton()
    {
        if (PhotonNetwork.PlayerList.Length >= 2)
            photonView.RPC("distirbuteCard", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    void distirbuteCard()
    {
        lobbyPanel.SetActive(false);
        PoolRummyManager.instance.totalPlayers = PhotonNetwork.PlayerList.Length;
        PoolRummyManager.instance.shuffleDeck();
        PoolRummyManager.instance.DrawWildCard();

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].SetActive(true);
        }
    }

    public void SetOwnership()
    {
        photonView.RPC("setOwnership_RPC", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    void setOwnership_RPC()
    {

        setPlayerPosAndGem();

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            PoolRummyManager.instance.PlayerList[i].GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.PlayerList[i]);

            if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[i])
                PoolRummyManager.instance.setActivePlayer(i);
        }
    }

    public void setPlayerPosAndGem()
    {
        switch (totalPlayersCanPlay)
        {
            case 2:
                setPlayergemFor2Players();
                break;
            case 3:
                setPlayergemFor3Players();
                break;
            case 4:
                setPlayergemFor4Players();
                break;
            case 5:
                setPlayergemFor5Players();
                break;
            case 6:
                setPlayergemFor6Players();
                break;
        }

        PoolRummyManager.instance.distibutingAnimation(PhotonNetwork.PlayerList.Length);
    }

    void setPlayergemFor2Players()
    {
        for (int i = 0; i < PoolRummyManager.instance.PlayerList.Count; i++)
        {
            if (PoolRummyManager.instance.PlayerList[i] == PoolRummyManager.instance.GetActivePlayer)
                setPlayergemFor(i, 0);
            else
                setPlayergemFor(i, 1);
        }

        //rearrangin the seat
        playerGemsList[0].GetComponent<RectTransform>().anchoredPosition = active2player;
        playerGemsList[1].GetComponent<RectTransform>().anchoredPosition = second2player;
    }

    void setPlayergemFor3Players()
    {
        for (int i = 0; i < PoolRummyManager.instance.PlayerList.Count; i++)
        {
            if (PoolRummyManager.instance.PlayerList[i] == PoolRummyManager.instance.GetActivePlayer)
            {
                if (i == 0)
                {
                    setPlayergemFor(0, 0);
                    setPlayergemFor(1, 1);
                    setPlayergemFor(2, 2);
                }
                if (i == 1)
                {
                    setPlayergemFor(1, 0);
                    setPlayergemFor(2, 1);
                    setPlayergemFor(0, 2);
                }
                if (i == 2)
                {
                    setPlayergemFor(2, 0);
                    setPlayergemFor(1, 1);
                    setPlayergemFor(0, 2);
                }
            }
        }

        //rearrangin the seat
        playerGemsList[0].GetComponent<RectTransform>().anchoredPosition = active2player;
        playerGemsList[1].GetComponent<RectTransform>().anchoredPosition = second3player;
        playerGemsList[2].GetComponent<RectTransform>().anchoredPosition = third3player;
    }

    void setPlayergemFor4Players()
    {
        for (int i = 0; i < PoolRummyManager.instance.PlayerList.Count; i++)
        {
            if (PoolRummyManager.instance.PlayerList[i] == PoolRummyManager.instance.GetActivePlayer)
            {
                if (i == 0)
                {
                    setPlayergemFor(0, 0);
                    setPlayergemFor(1, 3);
                    setPlayergemFor(2, 1);
                    setPlayergemFor(3, 4);
                }
                if (i == 1)
                {
                    setPlayergemFor(1, 0);
                    setPlayergemFor(2, 3);
                    setPlayergemFor(3, 1);
                    setPlayergemFor(0, 4);
                }
                if (i == 2)
                {
                    setPlayergemFor(2, 0);
                    setPlayergemFor(3, 3);
                    setPlayergemFor(0, 1);
                    setPlayergemFor(1, 4);
                }
                if (i == 3)
                {
                    setPlayergemFor(3, 0);
                    setPlayergemFor(0, 3);
                    setPlayergemFor(1, 1);
                    setPlayergemFor(2, 4);
                }
            }
        }

        //rearrangin the seat
        playerGemsList[0].GetComponent<RectTransform>().anchoredPosition = active2player;
        playerGemsList[3].GetComponent<RectTransform>().anchoredPosition = second4playerButFourthElement;
        playerGemsList[1].GetComponent<RectTransform>().anchoredPosition = second2player;
        playerGemsList[4].GetComponent<RectTransform>().anchoredPosition = fifth4player;
    }

    void setPlayergemFor5Players()
    {
        for (int i = 0; i < PoolRummyManager.instance.PlayerList.Count; i++)
        {
            if (PoolRummyManager.instance.PlayerList[i] == PoolRummyManager.instance.GetActivePlayer)
            {
                if (i == 0)
                {
                    setPlayergemFor(0, 0);
                    setPlayergemFor(1, 3);
                    setPlayergemFor(2, 1);
                    setPlayergemFor(3, 2);
                    setPlayergemFor(4, 4);
                }
                if (i == 1)
                {

                    setPlayergemFor(1, 0);
                    setPlayergemFor(2, 3);
                    setPlayergemFor(3, 1);
                    setPlayergemFor(4, 2);
                    setPlayergemFor(0, 4);
                }
                if (i == 2)
                {

                    setPlayergemFor(2, 0);
                    setPlayergemFor(3, 3);
                    setPlayergemFor(4, 1);
                    setPlayergemFor(0, 2);
                    setPlayergemFor(1, 4);
                }
                if (i == 3)
                {

                    setPlayergemFor(3, 0);
                    setPlayergemFor(4, 3);
                    setPlayergemFor(0, 1);
                    setPlayergemFor(1, 2);
                    setPlayergemFor(2, 4);
                }
                if (i == 4)
                {

                    setPlayergemFor(4, 0);
                    setPlayergemFor(0, 3);
                    setPlayergemFor(1, 1);
                    setPlayergemFor(2, 2);
                    setPlayergemFor(3, 4);
                }
            }
        }

        //rearrangin the seat
        playerGemsList[0].GetComponent<RectTransform>().anchoredPosition = active2player;
        playerGemsList[3].GetComponent<RectTransform>().anchoredPosition = second4playerButFourthElement;
        playerGemsList[1].GetComponent<RectTransform>().anchoredPosition = second3player;
        playerGemsList[2].GetComponent<RectTransform>().anchoredPosition = third3player;
        playerGemsList[4].GetComponent<RectTransform>().anchoredPosition = fifth4player;
    }

    void setPlayergemFor6Players()
    {
        for (int i = 0; i < PoolRummyManager.instance.PlayerList.Count; i++)
        {
            if (PoolRummyManager.instance.PlayerList[i] == PoolRummyManager.instance.GetActivePlayer)
            {
                if (i == 0)
                {
                    setPlayergemFor(0, 0);
                    setPlayergemFor(1, 3);
                    setPlayergemFor(2, 1);
                    setPlayergemFor(3, 2);
                    setPlayergemFor(4, 4);
                    setPlayergemFor(5, 5);
                }
                if (i == 1)
                {

                    setPlayergemFor(1, 0);
                    setPlayergemFor(2, 3);
                    setPlayergemFor(3, 1);
                    setPlayergemFor(4, 2);
                    setPlayergemFor(5, 4);
                    setPlayergemFor(0, 5);
                }
                if (i == 2)
                {

                    setPlayergemFor(2, 0);
                    setPlayergemFor(3, 3);
                    setPlayergemFor(4, 1);
                    setPlayergemFor(5, 2);
                    setPlayergemFor(0, 4);
                    setPlayergemFor(1, 5);
                }
                if (i == 3)
                {

                    setPlayergemFor(3, 0);
                    setPlayergemFor(4, 3);
                    setPlayergemFor(5, 1);
                    setPlayergemFor(0, 2);
                    setPlayergemFor(1, 4);
                    setPlayergemFor(2, 5);
                }
                if (i == 4)
                {

                    setPlayergemFor(4, 0);
                    setPlayergemFor(5, 3);
                    setPlayergemFor(0, 1);
                    setPlayergemFor(1, 2);
                    setPlayergemFor(2, 4);
                    setPlayergemFor(3, 5);
                }
                if (i == 5)
                {

                    setPlayergemFor(5, 0);
                    setPlayergemFor(0, 3);
                    setPlayergemFor(1, 1);
                    setPlayergemFor(2, 2);
                    setPlayergemFor(3, 4);
                    setPlayergemFor(4, 5);
                }
            }
        }

        //rearrangin the seat
        playerGemsList[0].GetComponent<RectTransform>().anchoredPosition = active6player;
        playerGemsList[3].GetComponent<RectTransform>().anchoredPosition = second4playerButFourthElement;
        playerGemsList[1].GetComponent<RectTransform>().anchoredPosition = second3player;
        playerGemsList[2].GetComponent<RectTransform>().anchoredPosition = third3player;
        playerGemsList[4].GetComponent<RectTransform>().anchoredPosition = fifth4player;
        playerGemsList[5].GetComponent<RectTransform>().anchoredPosition = sixth6player;
    }
    void setPlayergemFor(int playerIndexInPlayerList, int playerGemIndexInGemList)
    {
        playerGemsList[playerGemIndexInGemList].SetActive(true);

        PoolRummyManager.instance.PlayerList[playerIndexInPlayerList].playerGem = playerGemsList[playerGemIndexInGemList];
        PoolRummyManager.instance.PlayerList[playerIndexInPlayerList].inGamePlayerName = playerInGameTextList[playerGemIndexInGemList];

        playerInGameTextList[playerGemIndexInGemList].text = PhotonNetwork.PlayerList[playerIndexInPlayerList].NickName;
    }


    #region ipuncallbacks
    public void OnOwnershipRequest(PhotonView targetView, Photon.Realtime.Player requestingPlayer)
    {
        throw new System.NotImplementedException();
    }

    public void OnOwnershipTransfered(PhotonView targetView, Photon.Realtime.Player previousOwner)
    {

    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Photon.Realtime.Player senderOfFailedRequest)
    {
        throw new System.NotImplementedException();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            for (int i = 0; i < deckValues.Count; i++)
            {
                stream.SendNext(deckValues[i]);
            }

        }
        else if (stream.IsReading)
        {
            for (int i = 0; i < deckValues.Count; i++)
            {
                deckValues[i] = (int)stream.ReceiveNext();
            }


        }
    }
    #endregion
}

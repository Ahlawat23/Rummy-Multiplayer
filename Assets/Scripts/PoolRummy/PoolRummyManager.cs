using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolRummyManager : MonoBehaviourPun
{
    public static PoolRummyManager instance;

    public PoolRoomData _roomData;

    [Header("The buttons")]
    public Button discardButton;
    public Button RummyButton;

    [Header("Artworks")]
    public Text debugText;
    public GameObject cardBackPrefab;
    public GameObject DeckBoundryGlow;
    public float distributeAnimeSpeed = 0.1f;
    public Vector2 cardWidthAndHeight = new Vector2(60, 90);

    [Header("WIN SCREEN")]
    public GameObject winScreen;
    public Text[] nameText;
    public Text[] pointsText;

    [Header("DECK")]
    public List<Card> Deck = new List<Card>();
    public List<Card> DiscardPile = new List<Card>();
    public Transform discardPileTransoform;
    public Transform deckTransform;

    [Header("THE PLAYERS")]
    public int totalPlayers;
    public GameObject playerPrefab;
    public Transform playerSpawner;
    public List<PoolPlayer> PlayerList = new List<PoolPlayer>();



    private PoolPlayer activePlayer;
    public PoolPlayer GetActivePlayer
    {
        get { return activePlayer; }
    }
    public void setActivePlayer(int index)
    {
        activePlayer = PlayerList[index];
    }
    public void disableAllPlayerObject()
    {
        for (int i = 0; i < PlayerList.Count; i++)
            PlayerList[i].gameObject.SetActive(false);
    }


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void shuffleDeck()
    {
        for (int i = Deck.Count - 1; i > 0; i--)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                int k = Random.Range(0, i);
                photonView.RPC("swapCard_RPC", RpcTarget.AllBuffered, k, i);
            }

        }
        ensureDeckSync();
    }

    [PunRPC]
    void swapCard_RPC(int value, int index)
    {
        Card newCard = Deck[value];
        Deck[value] = Deck[index];
        Deck[index] = newCard;
    }

    void ensureDeckSync()
    {
        StartCoroutine(ensureDeckSync_Coroutitne());
    }

    IEnumerator ensureDeckSync_Coroutitne()
    {
        yield return new WaitForSeconds(2);

        DistributeCards(totalPlayers);
    }

    //last funtion called before starting the game
    void DistributeCards(int numOfplayers)
    {
        //genrated all the players
        for (int i = 0; i < numOfplayers; i++)
            PlayerList.Add(genrateSinglePlayer());

        //distribute cards to them
        for (int i = 0; i < 13; i++)
            for (int j = 0; j < PlayerList.Count; j++)
                PlayerList[j].RecieveACard(DrawFromTopOfTheDeck());

        //sorting the card in 4 groups for the first time
        for (int j = 0; j < PlayerList.Count; j++)
            PlayerList[j].sortCardFirstTime();

        //disabling player objectes so we can show the animation
        disableAllPlayerObject();
        gameObject.GetComponent<PoolLobbyManager>().SetOwnership();
        startGame();

    }

    int playerCounter;
    PoolPlayer genrateSinglePlayer()
    {
        playerCounter++;
        PoolPlayer newPlayer = Instantiate(playerPrefab, playerSpawner).GetComponent<PoolPlayer>();
        newPlayer.gameObject.name = "Player#" + playerCounter;
        newPlayer.gameObject.GetPhotonView().ViewID = playerCounter + 1;

        return newPlayer;
    }

    //DISTRIBUTING ANIMATION
    public void distibutingAnimation(int totalPlayers) => StartCoroutine(distributingAnimation_coroutine(totalPlayers));
    IEnumerator distributingAnimation_coroutine(int totalplayers)
    {
        for (int i = 0; i < 13; i++)
        {
            StartCoroutine(distAnimeSingleRotation_coroutine(totalplayers));
            yield return new WaitForSeconds(distributeAnimeSpeed * totalPlayers);
        }
        GetActivePlayer.gameObject.SetActive(true);
    }
    IEnumerator distAnimeSingleRotation_coroutine(int totalPlayers)
    {
        for (int i = 0; i < totalPlayers; i++)
        {
            StartCoroutine(distSingleCardAnime_coroutine(i));
            yield return new WaitForSeconds(distributeAnimeSpeed);
        }
    }
    IEnumerator distSingleCardAnime_coroutine(int playerIndexInPlayerList)
    {
        GameObject newCard = Instantiate(cardBackPrefab, deckTransform);

        //animating the card movement till its postion;
        newCard.GetComponent<RectTransform>().sizeDelta = cardWidthAndHeight;
        newCard.GetComponent<RectTransform>().LeanSize(new Vector2(10, 10), distributeAnimeSpeed);
        newCard.transform.LeanMove(PlayerList[playerIndexInPlayerList].playerGem.transform.position, distributeAnimeSpeed);


        yield return new WaitForSeconds(distributeAnimeSpeed);

        Destroy(newCard);
    }



    //DRAWING CARDS
    Card DrawFromTopOfTheDeck()
    {
        Card _card = Deck[0];
        Deck.Remove(_card);
        return _card;
    }
    Card DrawFromTopOfTheDeck(int playerIndex)
    {
        Card _card = Deck[0];
        Deck.Remove(_card);

        StartCoroutine(DeckToPlayerAnimation_coroutine(playerIndex, deckTransform));
        return _card;
    }

    Card DrawFromTopOfTheDiscardPile(int playerIndex)
    {
        Debug.Log("drawing from descardPile");
        Card _card = DiscardPile[DiscardPile.Count - 1];
        DiscardPile.Remove(_card);


        StartCoroutine(DeckToPlayerAnimation_coroutine(playerIndex, discardPileTransoform));
        return _card;
    }

    //card draw animation
    IEnumerator DeckToPlayerAnimation_coroutine(int playerIndexInPlayerList, Transform position)
    {
        GameObject newCard = Instantiate(cardBackPrefab, position);

        //animating the card movement till its postion;
        newCard.GetComponent<RectTransform>().sizeDelta = cardWidthAndHeight;
        newCard.GetComponent<RectTransform>().LeanSize(new Vector2(10, 10), 0.2f);
        newCard.transform.LeanMove(PlayerList[playerIndexInPlayerList].playerGem.transform.position, 0.2f);

        yield return new WaitForSeconds(0.5f);
        Destroy(newCard);
    }

    float nextRotation = 0;
    void addToDiscardPile(Card discardedCard, int playerIndexInPlayerList)
    {
        DiscardPile.Add(discardedCard);
        discardedCard.transform.SetParent(discardPileTransoform);

        //animating the card movement till its postion;
        discardedCard.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 10);
        discardedCard.transform.position = PlayerList[playerIndexInPlayerList].playerGem.transform.position;
        discardedCard.transform.LeanMoveLocal(Vector3.zero, 0.2f);
        discardedCard.GetComponent<RectTransform>().LeanSize(cardWidthAndHeight, 0.2f);
        //adding the rotation
        discardedCard.transform.LeanRotateAround(Vector3.forward, nextRotation, 0.2f);
        nextRotation = nextRotation - 20;
    }

    public void DrawWildCard()
    {
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("DrawWildCard_Rpc", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void DrawWildCard_Rpc()
    {
        Card wildCard = DrawFromTopOfTheDeck();
        wildCard.isWildCard = true;

        //setting every other card with same cardNum as wild card
        for (int i = 0; i < Deck.Count; i++)
            if (Deck[i]._cardNum == wildCard._cardNum)
                Deck[i].isWildCard = true;


        //wildCardAnimation

        //setting its posstion
        wildCard.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
        wildCard.GetComponent<RectTransform>().localPosition = new Vector2(25, 0);
    }



    //PLAYING THE TURN


    //draw from DECK
    public void drawFromDeckForTurn()
    {
        if (instance.GetActivePlayer.turnState != PlayerState.canDraw)
            return;

        photonView.RPC("drawFromDeckForTurn_RPC", RpcTarget.AllBufferedViaServer, getplayerIndexInList());

    }
    [PunRPC]
    public void drawFromDeckForTurn_RPC(int playerIndex)
    {
        whoCanDraw().PlayTurn(DrawFromTopOfTheDeck(playerIndex));
    }


    //draw from DISCARDED
    public void DrawFromDiscardPileForTurn()
    {
        if (DiscardPile.Count == 0)
            return;

        if (instance.GetActivePlayer.turnState != PlayerState.canDraw)
            return;


        photonView.RPC("DrawFromDiscardPileForTurn_RPC", RpcTarget.AllBufferedViaServer, getplayerIndexInList());

    }
    [PunRPC]
    public void DrawFromDiscardPileForTurn_RPC(int playerIndex)
    {
        whoCanDraw().PlayTurn(DrawFromTopOfTheDiscardPile(playerIndex));
    }


    //discard your card
    public void discardYourCard()
    {
        if (instance.GetActivePlayer.turnState != PlayerState.hasDrawn)
            return;

        if (instance.GetActivePlayer.selectedCard == null)
            return;


        Card _cardToBeDiscarded = instance.GetActivePlayer.selectedCard;

        photonView.RPC("discardYourCard_RPC", RpcTarget.AllBufferedViaServer, (int)_cardToBeDiscarded._cardNum, (int)_cardToBeDiscarded._cardSuit, getplayerIndexInList());

    }
    [PunRPC]
    public void discardYourCard_RPC(int cardNum, int cardSuit, int playerIndex)
    {
        Card newCard = genrateCardforDiscard(cardNum, cardSuit);

        addToDiscardPile(newCard, playerIndex);
        whoCanDiscard().DiscardCard(newCard);
        shiftTurn();

    }
    Card genrateCardforDiscard(int cardNum, int cardSuit)
    {
        Card genratedCard = null;

        //finds the card though handheld card
        for (int i = 0; i < whoCanDiscard().handHeldCards.Count; i++)
            if ((int)whoCanDiscard().handHeldCards[i]._cardNum == cardNum)
                if ((int)whoCanDiscard().handHeldCards[i]._cardSuit == cardSuit)
                    genratedCard = whoCanDiscard().handHeldCards[i];




        return genratedCard;
    }


    //PACK your turn
    public void PackYourTurn()
    {
        photonView.RPC("PackYourTurn_RPC", RpcTarget.AllBufferedViaServer, getplayerIndexInList());
    }
    [PunRPC]
    public void PackYourTurn_RPC(int index)
    {
        for (int i = 0; i < PlayerList[index].handHeldCards.Count; i++)
            Destroy(PlayerList[index].handHeldCards[i].gameObject);

        PlayerList[index].handHeldCards.Clear();
        PlayerList.Remove(PlayerList[index]);
    }

    int getplayerIndexInList()
    {
        for (int i = 0; i < PlayerList.Count; i++)
            if (GetActivePlayer == PlayerList[i])
                return i;

        return -1;
    }



    //SHIFTING TURN
    public void startGame()
    {
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (i == 0)
            {
                PlayerList[i].setTurn();
                DeckBoundryGlow.SetActive(true);
            }
            else
            {
                PlayerList[i].unSetTurn();
                DeckBoundryGlow.SetActive(false);
            }

        }

    }
    void shiftTurn()
    {
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (PlayerList[i].turnState == PlayerState.discarded)
            {
                if (i != PlayerList.Count - 1)
                {

                    PlayerList[i + 1].setTurn();
                    PlayerList[i].unSetTurn();
                }
                else
                {
                    PlayerList[0].setTurn();
                    PlayerList[i].unSetTurn();
                }
            }
        }

        //turning the deck glow on for the turn player
        if (GetActivePlayer.turnState == PlayerState.canDraw)
            DeckBoundryGlow.SetActive(true);
        else
            DeckBoundryGlow.SetActive(false);

    }
    PoolPlayer whoCanDraw()
    {
        for (int i = 0; i < PlayerList.Count; i++)
            if (PlayerList[i].turnState == PlayerState.canDraw)
                return PlayerList[i];


        return null;
    }
    PoolPlayer whoCanDiscard()
    {
        for (int i = 0; i < PlayerList.Count; i++)
            if (PlayerList[i].turnState == PlayerState.hasDrawn)
                return PlayerList[i];

        return null;
    }


    //ENDGAME
    public void DeclareWin()
    {
        if (checkIfCanWin(GetActivePlayer))
        {
            photonView.RPC("DeclareWinRPC", RpcTarget.AllBufferedViaServer, getplayerIndexInList());
        }
    }

    [PunRPC]
    public void DeclareWinRPC(int Index)
    {
        checkWinAll();
        caculateAllPlayerPoints();
        showWinScreen();
        debugText.gameObject.SetActive(true);
        debugText.text = "Player #" + Index.ToString() + "has won";
    }

    public void checkWinAll()
    {
        for (int i = 0; i < PlayerList.Count; i++)
            checkIfCanWin(PlayerList[i]);
    }
    public bool checkIfCanWin(PoolPlayer player)
    {
        player.canWin = true;
        player.syncGroupText();

        for (int i = 0; i < instance.GetActivePlayer.groupTextList.Count; i++)
        {
            if (player.groupTextList[i].text.Contains("Invalid"))
                player.canWin = false;

            if (player.groupTextList[i].text.Contains("Pure"))
                player.pureSequenceFound = true;

            if (player.pureSequenceFound && player.groupTextList[i].text.Contains("sequence"))
                player.otherSequenceFound = true;

        }


        if (!player.canWin)
        {
            player.canWin = false;
            return false;
        }


        if (player.pureSequenceFound && player.otherSequenceFound)
        {
            player.canWin = true;
            return true;
        }


        player.canWin = false;
        return false;
    }


    public void caculateAllPlayerPoints()
    {
        for (int i = 0; i < PlayerList.Count; i++)
            PlayerList[i].finalPoints = TotalPoints(PlayerList[i]);

        addPlayerPointsToRoom();
    }
    public int TotalPoints(PoolPlayer player)
    {
        int finalPoints = 0;

        if (player.canWin)
            return finalPoints;


        if (player.pureSequenceFound && player.otherSequenceFound)
        {
            for (int i = 0; i < player.groupTextList.Count; i++)
            {
                if (player.groupTextList[i].text.Contains("Invalid"))
                {
                    List<Card> groupCardList = player.fetchCardListFromGroup(player.groupList[i]);
                    for (int j = 0; j < groupCardList.Count; j++)
                    {
                        if ((int)groupCardList[j]._cardNum > 10)
                            finalPoints = finalPoints + 10;
                        else
                            finalPoints = finalPoints + (int)groupCardList[j]._cardNum;
                    }
                }
            }
        }

        else
        {
            for (int i = 0; i < player.groupList.Count; i++)
            {
                List<Card> groupCardList = player.fetchCardListFromGroup(player.groupList[i]);
                for (int j = 0; j < groupCardList.Count; j++)
                {
                    if ((int)groupCardList[j]._cardNum > 10)
                        finalPoints = finalPoints + 10;
                    else
                        finalPoints = finalPoints + (int)groupCardList[j]._cardNum;
                }
            }
        }



        if (finalPoints > 80)
            finalPoints = 80;

        return finalPoints;
    }

    public void addPlayerPointsToRoom()
    {
        switch (totalPlayers)
        {
            case 2:
                _roomData.AddPlayerPointForTwo(
                    PlayerList[0].finalPoints,
                    PlayerList[1].finalPoints);
                break;
            case 3:
                _roomData.AddPlayerPointForThree(
                    PlayerList[0].finalPoints,
                    PlayerList[1].finalPoints,
                    PlayerList[2].finalPoints);
                break;
            case 4:
                _roomData.AddPlayerPointForFour(
                     PlayerList[0].finalPoints,
                     PlayerList[1].finalPoints,
                     PlayerList[2].finalPoints,
                     PlayerList[3].finalPoints);
                break;
            case 5:
                _roomData.AddPlayerPointForFive(
                      PlayerList[0].finalPoints,
                      PlayerList[1].finalPoints,
                      PlayerList[2].finalPoints,
                      PlayerList[3].finalPoints,
                      PlayerList[4].finalPoints);
                break;
            case 6:
                _roomData.AddPlayerPointForSix(
                   PlayerList[0].finalPoints,
                      PlayerList[1].finalPoints,
                      PlayerList[2].finalPoints,
                      PlayerList[3].finalPoints,
                      PlayerList[4].finalPoints,
                      PlayerList[5].finalPoints);
                break;
        }
    }
    public void showWinScreen()
    {
        winScreen.SetActive(true);

        int[] playerpoints = _roomData.GetPlayerFinalPoints(totalPlayers);

        for (int i = 0; i < PlayerList.Count; i++)
        {
            nameText[i].text = PhotonNetwork.PlayerList[i].NickName;
            pointsText[i].text = playerpoints[i].ToString();
        }

        StartCoroutine(reloadTheGame_coroutine());
    }

    IEnumerator reloadTheGame_coroutine()
    {
        yield return new WaitForSeconds(5);
        //check for kicking 
        //wait for another few second and then kick and reload
        PhotonNetwork.LoadLevel(3);
    }
}

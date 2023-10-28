using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Events;

public class Player : MonoBehaviourPun, IPunObservable
{
    public List<Card> handHeldCards = new List<Card>();

    public PlayerState turnState;

    public HorizontalLayoutGroup _horzionLayerGroup;

    [Header("In-game artwork for animation")]
    public GameObject playerGem;
    public Text inGamePlayerName;
    public GameObject turnGlow;


    [Header("for interacting with menu")]
    public float selectedCardOffset = 20f;
    public float goupAddButtonYoffset = -150;
    public Card dummyCardPrefab;
   
    [Header("CardInfo")]
    public Card selectedCard;
    public Card previousSelectedCard;

    public Card currentlyDraggin;
    public Card nextInLine;
    public Card previousInLine;

    [Header("Group")]
    
    public GameObject groupPrefab;
    public GameObject buttonPrefab;
    public List<GameObject> groupList = new List<GameObject> ();
    public List<Text> groupTextList = new List<Text>();
    public List<Button> groupButtonList = new List<Button> ();

    [Header("For checking win")]
    public bool canWin;
    public bool pureSequenceFound;
    public bool otherSequenceFound;
    public int finalPoints;

    public void RecieveACard(Card newCard)
    {
        newCard.transform.rotation = Quaternion.identity;
        handHeldCards.Add(newCard);
        addCardToGroup(GetTheLastGroup(), newCard);
        settleCards();
    }

    public void settleCards()
    {
        _horzionLayerGroup.enabled = true;
        setAllGroupLayout();

        for (int i = 0; i < handHeldCards.Count; i++)
            handHeldCards[i].gameObject.transform.position = Vector3.zero;
    
    }

    public void sortCardFirstTime()
    {

        handHeldCards[0].transform.SetParent(groupList[0].transform);
        handHeldCards[1].transform.SetParent(groupList[0].transform);
        handHeldCards[2].transform.SetParent(groupList[0].transform);
        handHeldCards[3].transform.SetParent(groupList[1].transform);
        handHeldCards[4].transform.SetParent(groupList[1].transform);
        handHeldCards[5].transform.SetParent(groupList[1].transform);
        handHeldCards[6].transform.SetParent(groupList[2].transform);
        handHeldCards[7].transform.SetParent(groupList[2].transform);
        handHeldCards[8].transform.SetParent(groupList[2].transform);
        handHeldCards[9].transform.SetParent(groupList[3].transform);
        handHeldCards[10].transform.SetParent(groupList[3].transform);
        handHeldCards[11].transform.SetParent(groupList[3].transform);
        handHeldCards[12].transform.SetParent(groupList[3].transform);

    }





    //FOR INTERACTION
    int childCount;
    public void SetSelectedCard(Card newCard)
    {
        //getting the sibling index
        int selectedCardIndex = newCard.transform.GetSiblingIndex();
        currentlyDraggin = newCard;
        currentlyDraggin.childIndex = selectedCardIndex;
        
        //removing the dragged card from the group and setting the dummy card in 
        //its place
        dummyCardPrefab.gameObject.SetActive(true);
        dummyCardPrefab.transform.SetSiblingIndex(selectedCardIndex);

        dummyCardPrefab.transform.SetParent(currentlyDraggin.transform.parent);
        currentlyDraggin.transform.SetParent(transform.parent);


        //setting up the next card and previous card
        childCount = dummyCardPrefab.transform.parent.childCount;

        if (selectedCardIndex + 1 < childCount)            
            nextInLine = dummyCardPrefab.transform.parent.GetChild(selectedCardIndex + 1).GetComponent<Card>();

        if (selectedCardIndex - 1 >= 0)
            previousInLine = dummyCardPrefab.transform.parent.GetChild(selectedCardIndex - 1).GetComponent<Card>();
    }

    public void moveCard(Vector2 postion)
    {
        setAllGroupLayout();

        if (currentlyDraggin != null)
        {
            currentlyDraggin.transform.position = postion;
            checkWithNextCard();
            checkWithPreviousCard();
        }
    }

    void checkWithNextCard()
    {
        if (nextInLine != null)
        {
            if (currentlyDraggin.transform.position.x > nextInLine.transform.position.x)
            {
                int index = nextInLine.transform.GetSiblingIndex();
                nextInLine.transform.SetSiblingIndex(dummyCardPrefab.transform.GetSiblingIndex());
                dummyCardPrefab.transform.SetSiblingIndex(index);

                previousInLine = nextInLine;
                if (index + 1 < childCount)
                    nextInLine =  dummyCardPrefab.transform.parent.GetChild(index + 1).GetComponent<Card>();
                else
                    nextInLine = null;          
            }
        }
    }

    void checkWithPreviousCard()
    {
        if (previousInLine != null)
        {
            if (currentlyDraggin.transform.position.x < previousInLine.transform.position.x)
            {
                int index = previousInLine.transform.GetSiblingIndex();
                previousInLine.transform.SetSiblingIndex(dummyCardPrefab.transform.GetSiblingIndex());
                dummyCardPrefab.transform.SetSiblingIndex(index);

                nextInLine = previousInLine;
                if (index - 1 >= 0)
                    previousInLine = dummyCardPrefab.transform.parent.GetChild(index - 1).GetComponent<Card>();
                else
                    previousInLine = null;
            }
        }
    }


    public void releasCard()
    {
        //RELEASE THE CARD FORM DRAG

        if (currentlyDraggin != null)
        {
            //setting the current card to the dummy card transfro
            dummyCardPrefab.gameObject.SetActive(false);
            currentlyDraggin.transform.SetParent(dummyCardPrefab.transform.parent);
            
            //snapping it back to place if you are dragging it from bellow else we switch the place
            if (Mathf.Abs(currentlyDraggin.transform.position.y - dummyCardPrefab.transform.position.y) > 80)
            {
                dummyCardPrefab.transform.SetParent(transform);
                currentlyDraggin.transform.SetSiblingIndex(currentlyDraggin.childIndex);
            }
            else
            {
                currentlyDraggin.transform.SetSiblingIndex(dummyCardPrefab.transform.GetSiblingIndex());
                dummyCardPrefab.transform.SetParent(transform);
            }

            setAllGroupLayout();
            StartCoroutine(selectDragCard_coroutin(currentlyDraggin));
            checkSequenceOfAllGroup();
            AddButtonToAllGoupExcept(currentlyDraggin.transform.parent.gameObject);
            currentlyDraggin = null;
        }     
    }


    IEnumerator selectDragCard_coroutin(Card _card)
    {
        yield return new WaitForEndOfFrame();

        _horzionLayerGroup.enabled = false;
        UnsetAllGroupLayout();
       
        if(selectedCard != null)
        {  
            if (selectedCard == _card)
            {
                selectedCard = null;
                destroyAllGroupButtons();
                RummyManager.instance.discardButton.gameObject.SetActive(false);
            }
            else
            {        
                ChangeCardPostion(_card, selectedCardOffset);
                selectedCard = _card;
                shouldShowDiscardButton();
            }    
 
        }
        else
        {
            ChangeCardPostion(_card, selectedCardOffset);
            selectedCard = _card;
            shouldShowDiscardButton();
        }
    }

    void shouldShowDiscardButton()
    {
        if (RummyManager.instance.GetActivePlayer.turnState == PlayerState.hasDrawn)
            RummyManager.instance.discardButton.gameObject.SetActive(true);
        else
            RummyManager.instance.discardButton.gameObject.SetActive(false);
    }

    void ChangeCardPostion(Card _card, float Yoffset)
    {
        _card.GetComponent<RectTransform>().anchoredPosition = new Vector2(_card.GetComponent<RectTransform>().anchoredPosition.x, _card.GetComponent<RectTransform>().anchoredPosition.y + Yoffset);
    }


    //FOR PLAYING CARDS
    public Card justDrawnCard;
    public void PlayTurn(Card _card)
    {   
        RecieveACard(_card);

        justDrawnCard = _card;

        turnState = PlayerState.hasDrawn;
        inGamePlayerName.color = Color.blue;
        RummyManager.instance.DeckBoundryGlow.SetActive(false);
    }

    public void DiscardCard(Card discardCard)
    {
        handHeldCards.Remove(discardCard);
        
        selectedCard = null;
        previousSelectedCard = null;

        settleCards();

        turnState = PlayerState.discarded;
        shouldShowDiscardButton();
    }

 

    //setting up turn
    public void setTurn()
    {

        RummyManager.instance._roomData.Turn = RummyManager.instance.GetPlayerIndexInList(this);
        turnState = PlayerState.canDraw;
        setAllcardsColor(Color.white);  
    }

    public void unSetTurn()
    {
        turnState = PlayerState.nonTurn;
        setAllcardsColor(Color.grey);   
    }

  


    void setAllcardsColor(Color newColor)
    {
        for (int i = 0; i < handHeldCards.Count; i++)
            handHeldCards[i]._image.color = newColor;
    }

    //GROUPS
    public void addCardToGroup(GameObject group, Card _card, bool fromSelction = false)
    {
        if (fromSelction && selectedCard == null)
            return;

        _card.transform.SetParent(group.transform);
        setAllGroupLayout();
        destroyAllGroupButtons();
        checkSequenceOfAllGroup();
        _horzionLayerGroup.enabled = true;
    }

    GameObject GetTheLastGroup()
    {
       return groupList[groupList.Count - 1];
    }

    public void setAllGroupLayout()
    {
        for (int i = 0; i < groupList.Count; i++)
            groupList[i].GetComponent<HorizontalLayoutGroup>().enabled = true;
    }

    public void UnsetAllGroupLayout()
    {
        for (int i = 0; i < groupList.Count; i++)
            groupList[i].GetComponent<HorizontalLayoutGroup>().enabled = false;
    }


    //GROUP BUTTONS
    void AddButtonToAllGoupExcept(GameObject exceptionalGroup)
    {
        destroyAllGroupButtons();

        for (int i = 0; i < groupList.Count; i++)
            if (groupList[i] != exceptionalGroup)
                addButtonToOneGroup(groupList[i]);
    }

    void addButtonToOneGroup(GameObject goup)
    {
        //instantiating the button
        Button newButton = Instantiate(buttonPrefab, transform.parent.transform.parent).GetComponent<Button>();
        newButton.gameObject.transform.position = goup.transform.position;

        //positioning at right place
        RectTransform newButtonRect = newButton.gameObject.GetComponent<RectTransform>();
        newButtonRect.anchoredPosition = new Vector2(newButtonRect.anchoredPosition.x, newButtonRect.anchoredPosition.y + goupAddButtonYoffset);


        //addint the listener
        newButton.onClick.AddListener(delegate () { addCardToGroup(goup, selectedCard, true); });
        groupButtonList.Add(newButton);
    }

    void destroyAllGroupButtons()
    {
        for (int i = 0; i < groupButtonList.Count; i++)
           Destroy(groupButtonList[i].gameObject);

        groupButtonList.Clear();
    }

    //SEQUENCE CHECKERS

    public void checkSequenceOfAllGroup()
    {
        for (int i = 0; i < groupList.Count; i++)
            checkSequenceOfOneGroup(groupList[i], groupTextList[i]);
    }

    public void checkSequenceOfOneGroup(GameObject group, Text groupText)
    {
        List<Card> groupCardList = fetchCardListFromGroup(group);

        if (groupCardList == null)
            return;

        if (isThisPureSequence(groupCardList))
            groupText.text = "Pure sequence";

        else if (isThisImpurePureSequence(groupCardList))
            groupText.text = "Impure sequence";

        else if (isThisSet(groupCardList))
            groupText.text = "Set";

        else
            groupText.text = "Invalid sequence";

    }

    public  List<Card> fetchCardListFromGroup(GameObject group)
    {
        List<Card> cardList = new List<Card>();

        for (int i = 0; i < group.transform.childCount; i++)
            cardList.Add(group.transform.GetChild(i).GetComponent<Card>());

        return cardList;
    }

    bool isThisPureSequence(List<Card> cardTobeChecked)
    {
        //if the group dosent have enough card it is not a sequence
        if (cardTobeChecked.Count < 3)
            return false;

        //sort the list
        sortTheList(cardTobeChecked);


        //checking the suit
        for (int i = 0; i < cardTobeChecked.Count; i++)
            if (cardTobeChecked[0]._cardSuit != cardTobeChecked[i]._cardSuit)
                return false;

 
        //checking for sequence
        for (int i = 0; i < cardTobeChecked.Count; i++)
        {
            if (cardTobeChecked[i]._cardNum != CardNum.Ace)
            {
                if (i + 1 < cardTobeChecked.Count)
                {
                    if (cardTobeChecked[i]._cardNum != cardTobeChecked[i + 1]._cardNum - 1)
                        return false;
                }
            }
            else
            {
                if (i + 1 < cardTobeChecked.Count)
                {
                    if (cardTobeChecked[i + 1]._cardNum != CardNum.Two)
                        return false;
                }
            }
        }


        return true;
    }

    bool isThisImpurePureSequence(List<Card> cardTobeChecked)
    {
        //if the group dosent have enough card it is not a sequence
        if (cardTobeChecked.Count < 3)
            return false;



        //sort the list


        //checking for sequence
        for (int i = 0; i < cardTobeChecked.Count; i++)
        {
            //if one of the two card we are checking righ now is a wild card then ignore
            if (i + 1 < cardTobeChecked.Count)
                if (!(cardTobeChecked[i].isWildCard || cardTobeChecked[i + 1].isWildCard))
                {


                    if (cardTobeChecked[i]._cardNum != CardNum.Ace)
                    {
                        if (i + 1 < cardTobeChecked.Count)
                            if (cardTobeChecked[i]._cardNum != cardTobeChecked[i + 1]._cardNum - 1)
                                return false;
                    }
                    else
                    {
                        if (i + 1 < cardTobeChecked.Count)
                            if (cardTobeChecked[i + 1]._cardNum != CardNum.Two)
                                return false;
                    }
                }
        }


        return true;
    }

    bool isThisSet(List<Card> cardTobeChecked)
    {
        //if the group dosent have enough card it is not a sequence
        if (cardTobeChecked.Count < 3)
            return false;

        //check everone have the same value
        for (int i = 0; i < cardTobeChecked.Count; i++)
            if (cardTobeChecked[0]._cardNum != cardTobeChecked[i]._cardNum)
                if (!cardTobeChecked[i].isWildCard)
                     return false;

        //check if we have duplicate suit
        for (int i = 0; i < cardTobeChecked.Count; i++)
        {
            for (int j = 0; j < cardTobeChecked.Count; j++)
            {
                if (i < j)
                {
                    if (cardTobeChecked[i]._cardSuit == cardTobeChecked[j]._cardSuit)
                    {
                        return false;
                    }
                }
            }
        }



        return true;
    }

    void sortTheList(List<Card> cardToBeCehccked)
    {
        cardToBeCehccked.Sort(new compareCards());

        for (int i = 0; i < cardToBeCehccked.Count; i++)
        {
            if (cardToBeCehccked[i]._cardNum == CardNum.Ace)
                cardToBeCehccked[i].transform.SetSiblingIndex(0);
            else
                cardToBeCehccked[i].transform.SetSiblingIndex(i);           
        }
    }

    //SENDING SEQUENCE TO OTHERS
    public void syncGroupText()
    {
        //string[] allgroupStrings = new string[groupTextList.Count];

        //for (int i = 0; i < groupTextList.Count; i++)
        //    allgroupStrings[i] = groupTextList[i].text;

        //photonView.RPC("syncGroupText_RPC", RpcTarget.AllBufferedViaServer, allgroupStrings);

        finalPoints =  RummyManager.instance.TotalPoints(this);
        Debug.Log("caculating totalpoint for playre #" + RummyManager.instance.GetPlayerIndexInList(this));

    }

    [PunRPC]
    public void syncGroupText_RPC(string[] ALLgroupStrings)
    {
        Debug.Log("recived the rpc");
        for (int i = 0; i < groupTextList.Count; i++)
            groupTextList[i].text = ALLgroupStrings[i];


        RummyManager.instance.checkWinAll();
        RummyManager.instance.caculateAllPlayerPoints();
    }

    bool checkIfCanWin()
    {
        canWin = true;
       

        for (int i = 0; i < groupTextList.Count; i++)
        {
            if (groupTextList[i].text.Contains("Invalid"))
                canWin = false;

            if (groupTextList[i].text.Contains("Pure"))
                pureSequenceFound = true;

            if (pureSequenceFound && groupTextList[i].text.Contains("sequence"))
                otherSequenceFound = true;

        }

       

        if (!canWin)
        {
            canWin = false;
            return false;
        }


        if (pureSequenceFound && otherSequenceFound)
        {
            canWin = true;
            return true;
        }


        canWin = false;
        return false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(finalPoints);
        }
        else if (stream.IsReading)
        {
            finalPoints = (int)stream.ReceiveNext();
        }
    }
}

public enum PlayerState
{
    nonTurn,
    canDraw,
    hasDrawn,
    discarded
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("Basic card details")]
    
    public CardNum _cardNum;
    public CardSuit _cardSuit;
    public bool isWildCard;
    public Image _image;
    


    [Header("for interaction")]
    public int childIndex;

   
}

public enum CardNum
{
    joker,
    Two ,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace 
     
}

public enum CardSuit
{
    joker,
    Spade,
    Club ,
    Diamond,
    Heart 
    
}

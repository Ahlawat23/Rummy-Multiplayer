using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compareCards : IComparer<Card>
{
    public int Compare(Card x, Card y)
    {
        int xNum =(int)x._cardNum;
        int yNum =(int)y._cardNum;



        return xNum.CompareTo(yNum);
    }
}

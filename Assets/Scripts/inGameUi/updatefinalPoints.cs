using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class updatefinalPoints : MonoBehaviour
{
    public List<Text> scoreTextList = new List<Text>();
    void Update()
    {
        for (int i = 0; i < RummyManager.instance.PlayerList.Count; i++)
        {
           scoreTextList[i].text = RummyManager.instance.PlayerList[i].finalPoints.ToString();
        }
    }
}

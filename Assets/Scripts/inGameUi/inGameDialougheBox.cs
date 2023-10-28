using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inGameDialougheBox : MonoBehaviour
{
    public CanvasGroup backgroungImage;
    public Transform dialougeBox;

    public void OnEnable()
    {
        backgroungImage.alpha = 0;
        backgroungImage.LeanAlpha(1, 0.3f);

        dialougeBox.localPosition = new Vector2(0, -Screen.height);
        dialougeBox.LeanMoveLocalY(0, 0.3f).setEaseOutExpo().delay = 0.05f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RummyInputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Card>() != null)
            {
                RummyManager.instance.GetActivePlayer.SetSelectedCard(eventData.pointerCurrentRaycast.gameObject.GetComponent<Card>());
            }
        }

    }
    public void OnDrag(PointerEventData eventData)
    {
       
        RummyManager.instance.GetActivePlayer.moveCard(eventData.position);

     
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       

        if(RummyManager.instance.GetActivePlayer != null)
        RummyManager.instance.GetActivePlayer.releasCard();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PoolRummyInputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    
    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Card>() != null)
            {
                PoolRummyManager.instance.GetActivePlayer.SetSelectedCard(eventData.pointerCurrentRaycast.gameObject.GetComponent<Card>());
            }
        }

    }
    public void OnDrag(PointerEventData eventData)
    {
       
        PoolRummyManager.instance.GetActivePlayer.moveCard(eventData.position);


    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
        if (PoolRummyManager.instance.GetActivePlayer != null)
            PoolRummyManager.instance.GetActivePlayer.releasCard();
    }
}

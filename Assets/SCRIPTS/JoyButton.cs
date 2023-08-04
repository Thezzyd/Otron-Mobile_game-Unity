using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public bool pressd;

    public void OnPointerDown(PointerEventData eventData)
    {
        pressd = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressd = false;
    }


}

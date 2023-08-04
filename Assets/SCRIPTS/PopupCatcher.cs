using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupCatcher : MonoBehaviour
{
    public GameObject serverPopup;
    public GameObject clientPopup;
    public void Awake()
    {
        if (GameObject.FindGameObjectWithTag("PopupHandler").GetComponent<DisconnectionPopupHandler>().serverPopup)
            serverPopup.SetActive(true);
        if (GameObject.FindGameObjectWithTag("PopupHandler").GetComponent<DisconnectionPopupHandler>().clientPopup)
            clientPopup.SetActive(true);
    }
}

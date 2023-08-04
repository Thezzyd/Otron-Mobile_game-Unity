using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OKButton : MonoBehaviour
{
    public GameObject Popup;

    public void OK()
    {
        Popup.SetActive(false);
        GameObject.FindGameObjectWithTag("PopupHandler").GetComponent<DisconnectionPopupHandler>().serverPopup = false;
        GameObject.FindGameObjectWithTag("PopupHandler").GetComponent<DisconnectionPopupHandler>().clientPopup = false;

    }
}

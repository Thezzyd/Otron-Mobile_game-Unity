using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectionPopupHandler : MonoBehaviour
{
    public bool serverPopup;
    public bool clientPopup;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetServerDisconnectedPopup()
    {
        serverPopup = true;
    }

    public void SetClientDisconnectedPopup()
    {
        clientPopup = true;
    }

}

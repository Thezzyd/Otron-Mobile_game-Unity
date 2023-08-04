using Mirror;
using UnityEngine;

public class Modes : NetworkBehaviour
{
    //  public GameObject checkFriendlyFire;
    //  public GameObject checkNightMode;

    [SyncVar] public bool isNightModeOn;
    [SyncVar] public bool isFriendlyFireOn;
    [SyncVar] public bool isWinLost;


  /*  public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

    }*/

    /*
    public void Start()
    {

        if (isNightModeOn)
            checkNightMode.SetActive(true);
        else
            checkNightMode.SetActive(false);

        if (isFriendlyFireOn)
            checkFriendlyFire.SetActive(true);
        else
            checkFriendlyFire.SetActive(false);
}*/
}

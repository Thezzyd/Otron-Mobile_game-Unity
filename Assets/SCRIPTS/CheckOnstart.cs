using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CheckOnstart : NetworkBehaviour
{

    public GameObject checkNightMode;
    public GameObject checkFriendlyfire;
    public GameModeSettings gamesettingsInfo;
    public Modes gamesettingsInfdo;

    private void FixedUpdate()
    {
        if (isServer)
        {
            gamesettingsInfo.gameObject.SetActive(true);

            if (gamesettingsInfo.isNightModeOn)
            {
                checkNightMode.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                checkNightMode.transform.localScale = new Vector3(0, 0, 1);
            }
            if (gamesettingsInfo.isFriendlyFireOn)
            {
                checkFriendlyfire.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                checkFriendlyfire.transform.localScale = new Vector3(0, 0, 1);
            }
        }  
    }
}
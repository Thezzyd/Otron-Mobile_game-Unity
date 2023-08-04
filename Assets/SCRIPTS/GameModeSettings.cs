using Mirror;
using UnityEngine;

public class GameModeSettings : NetworkBehaviour
{
 
    [SyncVar]  public bool isNightModeOn;
    [SyncVar]  public bool isFriendlyFireOn;
    [SyncVar]  public bool isWinLost;

}

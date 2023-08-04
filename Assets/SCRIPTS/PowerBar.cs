using Mirror;
using UnityEngine;

public class PowerBar : NetworkBehaviour
{
  //  public GameObject powerBar;
  [SyncVar]  public float targetScale;
    public PlayerControl playerControl1;
    public PlayerControl playerControl2;
    public bool isP1;
    public LevelManager levelManager;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();

        if (CompareTag("BulletBarP1"))
            isP1 = true;
        else
            isP1 = false;
        
    }


    public void FixedUpdate()
    {
        if (isServer && levelManager.isGameAllowed && levelManager.lobbyStart)
        {
            if (isP1 && GameObject.FindGameObjectWithTag("Player1"))
            {
                playerControl1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerControl>();

                targetScale = ((float)playerControl1.shootingTimer / (float)playerControl1.reloadTime);
                if (targetScale > 1f) targetScale = 1f;
            }
            else if (!isP1 && GameObject.FindGameObjectWithTag("Player2"))
            {
                playerControl2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerControl>();

                targetScale = ((float)playerControl2.shootingTimer / (float)playerControl2.reloadTime);
                if (targetScale > 1f) targetScale = 1f;

            }

            transform.localScale = new Vector3(1f, targetScale, 1f);
        }
    }
    /*
    [Command]
    public void CmdBarRefreash()
    {
        if (isP1)
        {
            playerControl1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerControl>();
            targetScale = ((float)playerControl1.shootingTimer / (float)playerControl1.reloadTime);
            if (targetScale > 1f) targetScale = 1f;
        }
        else
        {
            playerControl2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerControl>();
            targetScale = ((float)playerControl2.shootingTimer / (float)playerControl2.reloadTime);
            if (targetScale > 1f) targetScale = 1f;
        }

        transform.localScale = new Vector3(1f, targetScale, 1f);
    }

    [ClientRpc]
    public void RpcBarRefresh()
    {
        if (isP1)
        {
            playerControl1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerControl>();

            targetScale = ((float)playerControl1.shootingTimer / (float)playerControl1.reloadTime);
            if (targetScale > 1f) targetScale = 1f;
        }
        else
        {
            playerControl2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerControl>();

            targetScale = ((float)playerControl2.shootingTimer / (float)playerControl2.reloadTime);
            if (targetScale > 1f) targetScale = 1f;

        }

        transform.localScale = new Vector3(1f, targetScale, 1f);
    }
   */
}

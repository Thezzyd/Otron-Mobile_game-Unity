using UnityEngine;
using Mirror;

public class LifeSystem : NetworkBehaviour
{

    public int livesP1;
    public int livesP2;
    public int maxLives;
    public GameObject[] livesP1GO;
    public GameObject[] livesP2GO;
    public GameObject winLostCanvas;
    public GameObject p1Won;
    public GameObject p2Won;


    void Start()
    {
        livesP1 = 3;
        livesP2 = 3;
        maxLives = 4;

        RefreshLifes();
    }

    public void RefreshLifes()
    {
        switch (livesP1)
        {
            case 4:
                livesP1GO[3].SetActive(true);
                livesP1GO[2].SetActive(true);
                livesP1GO[1].SetActive(true);
                livesP1GO[0].SetActive(true);
                break;
            case 3: 
                livesP1GO[3].SetActive(false);
                livesP1GO[2].SetActive(true);
                livesP1GO[1].SetActive(true);
                livesP1GO[0].SetActive(true);
                break;
            case 2: 
                livesP1GO[3].SetActive(false); 
                livesP1GO[2].SetActive(false); 
                livesP1GO[1].SetActive(true); 
                livesP1GO[0].SetActive(true); 
                break;
            case 1:
                livesP1GO[3].SetActive(false);
                livesP1GO[2].SetActive(false);
                livesP1GO[1].SetActive(false);
                livesP1GO[0].SetActive(true);
                break;
            case 0:
                livesP1GO[3].SetActive(false);
                livesP1GO[2].SetActive(false);
                livesP1GO[1].SetActive(false);
                livesP1GO[0].SetActive(false);
               if (isServer)
                    RpcWinLost("blue");
               else
                   CmdWinLost("blue");

                break;
        }

        switch (livesP2)
        {
            case 4:
                livesP2GO[3].SetActive(true);
                livesP2GO[2].SetActive(true);
                livesP2GO[1].SetActive(true);
                livesP2GO[0].SetActive(true);
                break;
            case 3:
                livesP2GO[3].SetActive(false);
                livesP2GO[2].SetActive(true);
                livesP2GO[1].SetActive(true);
                livesP2GO[0].SetActive(true);
                break;
            case 2:
                livesP2GO[3].SetActive(false);
                livesP2GO[2].SetActive(false);
                livesP2GO[1].SetActive(true);
                livesP2GO[0].SetActive(true);
                break;
            case 1:
                livesP2GO[3].SetActive(false);
                livesP2GO[2].SetActive(false);
                livesP2GO[1].SetActive(false);
                livesP2GO[0].SetActive(true);
                break;
            case 0:
                livesP2GO[3].SetActive(false);
                livesP2GO[2].SetActive(false);
                livesP2GO[1].SetActive(false);
                livesP2GO[0].SetActive(false);
                if (isServer)
                    RpcWinLost("pink");
                else
                    CmdWinLost("pink");
                
                break;
        }


    }

    public void Skucie(int wchichPlayer)
    {
        if (wchichPlayer == 1)
        {
            livesP1--;
            if (livesP1 <= 0) livesP1 = 0;
        }
        else if (wchichPlayer == 2)
        {
            livesP2--;
            if (livesP2 <= 0) livesP2 = 0;
        }
        RefreshLifes();


    }

    public void ZebranieZycia(int wchichPlayer)
    {
        if (wchichPlayer == 1)
        {
            livesP1+= 1;
            if (livesP1 >= 4) livesP1 = 4;
        }
        else if (wchichPlayer == 2)
        {
            livesP2+= 1;
            if (livesP2 >= 4) livesP2 = 4;
        }
        RefreshLifes();

    }

    [Command]
    public void CmdWinLost(string who)
    {
        RpcWinLost(who);
    }

    [ClientRpc]
    public void RpcWinLost(string who)
    {
        FindObjectOfType<GameModeSettings>().isWinLost = true;
        winLostCanvas.SetActive(true);

        if (who == "pink")
        {
            p1Won.SetActive(true);
            p2Won.SetActive(false);
        }
        else
        {
            p2Won.SetActive(true);
            p1Won.SetActive(false);
        }

        Time.timeScale = 0.0f;
  }  
}

using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class LobbyCanvas : NetworkBehaviour
{
    public LevelManager levelManager;
    public Button startButton;
    public GameObject waitingText;
    public TextMeshProUGUI infoP1Text;
    public TextMeshProUGUI infoP2Text;
    public NetworkManager netManager;
    public GameObject lobbyCanvas;
    public GameObject checkboxFriendlyFire;
    public GameObject checkboxNightMode;
    public GameObject checkFriendlyFire;
    public GameObject checkNightMode;
    public Material[] wallMaterials; // [0] Unlit wallGlow, [1] Lit WallGlow
    public Material[] stationaryTurretMaterials; // [0]- Unlit base/head. [1]- Lit base/head, [2]- Unlit laserSprite, [3]- Lit LaserSprite, [4]-Unlit ParticleLaser, [5]-Lit ParticleLaser
    public Material[] p1movingTurretMaterials; // [0]- Unlit base/head. [1]- Lit base/head
    public Material[] p2movingTurretMaterials; // [0]- Unlit base/head. [1]- Lit base/head

    public AudioSource lobbyTheme;

    [SyncVar] public bool isCanvasON;

    public void Start()
    {
        AudioListener.volume = 0.0f;
        isCanvasON = true;
        netManager = FindObjectOfType<NetworkManager>();
        levelManager = FindObjectOfType<LevelManager>();
        FindObjectOfType<AudioManager>().Play("LobbyTheme");
       
    }


    public void Update()
    {
        if (isCanvasON)
        {
            PlayerControl[] playerScript = FindObjectsOfType<PlayerControl>();

            if (playerScript.Length == 2)
            {
                if (isServer)
                {
                    startButton.gameObject.SetActive(true);
                    startButton.interactable = true;
                }

                infoP1Text.gameObject.SetActive(true);
                infoP2Text.gameObject.SetActive(true);

                if (isServer)
                    waitingText.GetComponent<TextMeshProUGUI>().text = "(Players are ready. You can now start the game)";
                else
                    waitingText.GetComponent<TextMeshProUGUI>().text = "(Players are ready. Waiting for server to start the game)";

                infoP1Text.text = "Ready!";
                infoP2Text.text = "Ready!";
            }
            else if (playerScript.Length == 1)
            {
                if (isServer)
                {
                    startButton.gameObject.SetActive(true);
                    startButton.interactable = false;
                }
                waitingText.GetComponent<TextMeshProUGUI>().text = "(Waiting For Second Player...)";

                infoP1Text.gameObject.SetActive(true);
                infoP2Text.gameObject.SetActive(true);
                infoP2Text.text = "Waiting...";
                infoP1Text.text = "Ready!";
            }
        }
        else
        {
            lobbyCanvas.SetActive(false);
            AudioListener.volume = 1.0f;
        }
    }

    public void LoobbyStartButton()
    {
        isCanvasON = false;
        FindObjectOfType<LevelManager>().lobbyStart = true;
        RpcSetNightModeSettings();
        levelManager.CmdAllowMoveToResp();
        levelManager.RpcStartGame();
        lobbyTheme.Stop();
        // FindObjectOfType<AudioManager>().Stop("LobbyTheme");
    }

    [ClientRpc]
    public void RpcSetNightModeSettings()
    {
        lobbyTheme.Stop();

        PlayerControl[] playerScript = FindObjectsOfType<PlayerControl>();
        foreach (PlayerControl player in playerScript)
        {
            player.SetNightModeMaterials();
        }

        SetNightModeTurretsMaterials();
        SetNightModeP1TurretMat();
        SetNightModeP2TurretMat();

        if (FindObjectOfType<GameModeSettings>().isNightModeOn)
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
            foreach (GameObject wallPiece in walls)
            {
                wallPiece.GetComponent<SpriteRenderer>().material = wallMaterials[1];
            }
        }
    }

/*
    [ClientRpc]
    public void RpcStartGame()
    {
        GameObject.FindGameObjectWithTag("LobbyCanvas").SetActive(false);
        AudioListener.volume = 1.0f;
   } */

    
    public void CheckCheckboxFriendlyFire()
    {
        if (!FindObjectOfType<GameModeSettings>().isFriendlyFireOn)
        {
            FindObjectOfType<GameModeSettings>().isFriendlyFireOn = true;
        }
        else
        {
            FindObjectOfType<GameModeSettings>().isFriendlyFireOn = false;
        }
    }

    public void CheckCheckboxNightMode()
    {
         if (!FindObjectOfType<GameModeSettings>().isNightModeOn)
          {
              FindObjectOfType<GameModeSettings>().isNightModeOn = true;
          }
          else
          {
              FindObjectOfType<GameModeSettings>().isNightModeOn = false;
          }
    }

    public void SetNightModeTurretsMaterials()
    {
        if (FindObjectOfType<GameModeSettings>().isNightModeOn)
        {
            GameObject[] allTurretFragments = GameObject.FindGameObjectsWithTag("MovingCannon");

            foreach (GameObject obj in allTurretFragments)
            {
                if (obj.GetComponent<SpriteRenderer>())
                    obj.GetComponent<SpriteRenderer>().material = stationaryTurretMaterials[1];
            }

            GameObject[] allLaserSprites = GameObject.FindGameObjectsWithTag("LaserSprite");

            foreach (GameObject obj in allLaserSprites)
            {
                if (obj.GetComponent<SpriteRenderer>())
                    obj.GetComponent<SpriteRenderer>().material = stationaryTurretMaterials[3];
            }

            GameObject[] allLaserParticles = GameObject.FindGameObjectsWithTag("LaserParticles");

            foreach (GameObject obj in allLaserParticles)
            {
                if (obj.GetComponent<ParticleSystemRenderer>())
                    obj.GetComponent<ParticleSystemRenderer>().material = stationaryTurretMaterials[5];
                    obj.GetComponent<ParticleSystemRenderer>().trailMaterial = stationaryTurretMaterials[5];
            }
          
        }
        else
        {
            GameObject[] allTurretFragments = GameObject.FindGameObjectsWithTag("MovingCannon");

            foreach (GameObject obj in allTurretFragments)
            {
                if (obj.GetComponent<SpriteRenderer>())
                    obj.GetComponent<SpriteRenderer>().material = stationaryTurretMaterials[0];
            }

            GameObject[] allLaserSprites = GameObject.FindGameObjectsWithTag("LaserSprite");

            foreach (GameObject obj in allLaserSprites)
            {
                if (obj.GetComponent<SpriteRenderer>())
                    obj.GetComponent<SpriteRenderer>().material = stationaryTurretMaterials[2];
            }

            GameObject[] allLaserParticles = GameObject.FindGameObjectsWithTag("LaserParticles");

            foreach (GameObject obj in allLaserParticles)
            {
                if (obj.GetComponent<ParticleSystemRenderer>())
                    obj.GetComponent<ParticleSystemRenderer>().material = stationaryTurretMaterials[4];
                    obj.GetComponent<ParticleSystemRenderer>().trailMaterial = stationaryTurretMaterials[4];

            }
        
        }
    }

    public void SetNightModeP1TurretMat()
    {
        if (FindObjectOfType<GameModeSettings>().isNightModeOn && !isServer)
        {

            GameObject[] p1Turret = GameObject.FindGameObjectsWithTag("StationaryCannon");

            foreach (GameObject obj in p1Turret)
            {
                if (obj.GetComponent<SpriteRenderer>())
                    obj.GetComponent<SpriteRenderer>().material = p1movingTurretMaterials[1];
            }
            GameObject[] p1LaserSprites = GameObject.FindGameObjectsWithTag("LaserSpriteP1");

            foreach (GameObject obj in p1LaserSprites)
            {
                if (obj.GetComponent<SpriteRenderer>())
                    obj.GetComponent<SpriteRenderer>().material = stationaryTurretMaterials[3];
            }

            GameObject[] p1LaserParticles = GameObject.FindGameObjectsWithTag("LaserParticlesP1");

            foreach (GameObject obj in p1LaserParticles)
            {
                if (obj.GetComponent<ParticleSystemRenderer>())
                    obj.GetComponent<ParticleSystemRenderer>().material = stationaryTurretMaterials[5];
                obj.GetComponent<ParticleSystemRenderer>().trailMaterial = stationaryTurretMaterials[5];
            }
        }
        else
        {
            GameObject[] p1Turret = GameObject.FindGameObjectsWithTag("StationaryCannon");

            foreach (GameObject obj in p1Turret)
            {
                if (obj.GetComponent<SpriteRenderer>())
                    obj.GetComponent<SpriteRenderer>().material = p1movingTurretMaterials[0];
            }

            GameObject[] p1LaserSprites = GameObject.FindGameObjectsWithTag("LaserSpriteP1");

            foreach (GameObject obj in p1LaserSprites)
            {
                if (obj.GetComponent<SpriteRenderer>())
                    obj.GetComponent<SpriteRenderer>().material = stationaryTurretMaterials[2];
            }

            GameObject[] p1LaserParticles = GameObject.FindGameObjectsWithTag("LaserParticlesP1");

            foreach (GameObject obj in p1LaserParticles)
            {
                if (obj.GetComponent<ParticleSystemRenderer>())
                    obj.GetComponent<ParticleSystemRenderer>().material = stationaryTurretMaterials[4];
                obj.GetComponent<ParticleSystemRenderer>().trailMaterial = stationaryTurretMaterials[4];
            }
        }
      
    }

    public void SetNightModeP2TurretMat()
    {
        if (FindObjectOfType<GameModeSettings>().isNightModeOn && isServer)
        {
            GameObject[] p2Turret = GameObject.FindGameObjectsWithTag("StationaryCannonP2");

            foreach (GameObject obj in p2Turret)
            {
                if (obj.GetComponent<SpriteRenderer>())
                    obj.GetComponent<SpriteRenderer>().material = p2movingTurretMaterials[1];
            }

            GameObject[] p2LaserSprites = GameObject.FindGameObjectsWithTag("LaserSpriteP2");

            foreach (GameObject obj in p2LaserSprites)
            {
                if (obj.GetComponent<SpriteRenderer>())
                    obj.GetComponent<SpriteRenderer>().material = stationaryTurretMaterials[3];
            }

            GameObject[] p2LaserParticles = GameObject.FindGameObjectsWithTag("LaserParticlesP2");

            foreach (GameObject obj in p2LaserParticles)
            {
                if (obj.GetComponent<ParticleSystemRenderer>())
                    obj.GetComponent<ParticleSystemRenderer>().material = stationaryTurretMaterials[5];
                obj.GetComponent<ParticleSystemRenderer>().trailMaterial = stationaryTurretMaterials[5];
            }
        }
        else
        {
            GameObject[] p2Turret = GameObject.FindGameObjectsWithTag("StationaryCannonP2");

            foreach (GameObject obj in p2Turret)
            {
                if (obj.GetComponent<SpriteRenderer>())
                    obj.GetComponent<SpriteRenderer>().material = p2movingTurretMaterials[0];
            }

            GameObject[] p2LaserSprites = GameObject.FindGameObjectsWithTag("LaserSpriteP2");

            foreach (GameObject obj in p2LaserSprites)
            {
                if (obj.GetComponent<SpriteRenderer>())
                    obj.GetComponent<SpriteRenderer>().material = stationaryTurretMaterials[2];
            }

            GameObject[] p2LaserParticles = GameObject.FindGameObjectsWithTag("LaserParticlesP2");

            foreach (GameObject obj in p2LaserParticles)
            {
                if (obj.GetComponent<ParticleSystemRenderer>())
                    obj.GetComponent<ParticleSystemRenderer>().material = stationaryTurretMaterials[4];
                obj.GetComponent<ParticleSystemRenderer>().trailMaterial = stationaryTurretMaterials[4];
            }
        }
      
    }

}
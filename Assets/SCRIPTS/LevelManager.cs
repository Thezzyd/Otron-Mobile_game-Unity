using Mirror.Discovery;
using UnityEngine;
using Mirror;

    public class LevelManager : NetworkBehaviour
    {
            public Transform[] playerRespawnPoints;
  [SyncVar] public bool isGameAllowed;
  [SyncVar] public bool isPlayer1On;
  [SyncVar] public bool isPlayer2On;
            public AudioSource audio;
            public GameObject LobbyCanvas;
  [SyncVar] public bool lobbyStart;
  [SyncVar] public float timeOfBgMusic;
            public NetworkDiscovery networkDiscovery;
    public SceneTransitionCanvas sceneTransition;
    public GameModeSettings gameModeSettings;
    public AudioSource lobbyTheme;



        void Start()
        {
        networkDiscovery = FindObjectOfType<NetworkDiscovery>();
        lobbyTheme.Stop();
            audio = GetComponent<AudioSource>();
            if (isClient)
            {
                audio.time = timeOfBgMusic;
            }
            audio.Play();

      /*   NetworkIdentity[] tak = FindObjectsOfType<NetworkIdentity>();
        foreach (NetworkIdentity hh in tak)
        {
            hh.gameObject.name = hh.netId.ToString();
        }
       */

        }

        public void Update()
        {
            PlayerControl[] playerScript = FindObjectsOfType<PlayerControl>();
            if (playerScript.Length < 2)
            {
                LobbyCanvas.SetActive(true);
            }


            if (isServer)
            {
                if (GameObject.FindGameObjectWithTag("Player1"))
                    isPlayer1On = true;
                else isPlayer1On = false;
                if (GameObject.FindGameObjectWithTag("Player2"))
                    isPlayer2On = true;
                else isPlayer2On = false;

                if (isPlayer1On && isPlayer2On && lobbyStart)
                {
                    isGameAllowed = true;

                }
                else
                {
                    isGameAllowed = false;
                    LobbyCanvas.SetActive(true);
                }
                timeOfBgMusic = audio.time;
            }


        }

        [Command]
        public void CmdAllowMoveToResp()
        {
            RpcMoveToResp();
        }

        [ClientRpc]
        public void RpcMoveToResp()
        {
            GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
            GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
            player1.GetComponent<NetworkTransform>().enabled = false;
            player2.GetComponent<NetworkTransform>().enabled = false;

            player2.GetComponent<NetworkAnimator>().SetTrigger("death");
            player1.GetComponent<NetworkAnimator>().SetTrigger("death");


            player1.transform.position = playerRespawnPoints[0].position;
            player1.transform.rotation = new Quaternion(0, 0, 0, player1.transform.rotation.w);
            player2.transform.position = playerRespawnPoints[1].position;
            player2.transform.rotation = new Quaternion(0, 0, 180, player2.transform.rotation.w);

            player1.GetComponent<NetworkTransform>().enabled = true;
            player2.GetComponent<NetworkTransform>().enabled = true;
        }

        [Command]
        public void CmdAllowStartGame()
        {
            RpcStartGame();
        }

        [ClientRpc]
        public void RpcStartGame()
        {
            GameObject.FindGameObjectWithTag("LobbyCanvas").SetActive(false);
            AudioListener.volume = 1.0f;
        }

        public void PlayeAgainButton()
        {
             Time.timeScale = 1.0f;

              if (isServer)
              {
            NetworkManager.singleton.StopHost();
            networkDiscovery.StartDiscovery();
            FindObjectOfType<SceneTransitionCanvas>().ResetLevel();
            gameModeSettings.isWinLost = false;
          


         

        }
            else
            {
            NetworkManager.singleton.StopClient();
            networkDiscovery.StartDiscovery();
            FindObjectOfType<SceneTransitionCanvas>().ResetLevel();
            gameModeSettings.isWinLost = false;
           


       

        }
        }

    public void BackToMenuButton()
    {

        Time.timeScale = 1.0f;
        if (isServer)
        {

            FindObjectOfType<SceneTransitionCanvas>().LoadPreviousScene();
       
            

            NetworkManager.singleton.StopHost();
            networkDiscovery.StartDiscovery();

             gameModeSettings.isWinLost = false;
        }
        else
        {

            FindObjectOfType<SceneTransitionCanvas>().LoadPreviousScene();

          

            NetworkManager.singleton.StopClient();
            networkDiscovery.StartDiscovery();
              gameModeSettings.isWinLost = false;

        }
    }


}

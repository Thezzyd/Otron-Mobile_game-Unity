using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Mirror
{
    
    [AddComponentMenu("")]
    public class NetworkManagerProjekt : NetworkManager
    {
        public GameObject playerPrefab2;
        public Transform leftPlayerSpawn;
        public Transform rightPlayerSpawn;
        public Transform barP1SpawnPoint;
        public Transform barP2SpawnPoint;
        public GameObject barP1;
        public GameObject barP2;
        public bool isPlayer1On;
        public bool isPlayer2On;
        public TextMeshProUGUI waitingText;
   



        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            leftPlayerSpawn = GameObject.FindGameObjectWithTag("P1Respawn").transform;
            rightPlayerSpawn = GameObject.FindGameObjectWithTag("P2Respawn").transform;
            barP1 = GameObject.FindGameObjectWithTag("P1BarParent");
            barP2 = GameObject.FindGameObjectWithTag("P2BarParent");
            barP1SpawnPoint = GameObject.FindGameObjectWithTag("P1BulletBarSpawn").transform;
            barP2SpawnPoint = GameObject.FindGameObjectWithTag("P2BulletBarSpawn").transform;

            waitingText = GameObject.FindGameObjectWithTag("WaitingText").GetComponent<TextMeshProUGUI>();



            Transform start = numPlayers == 0 ? leftPlayerSpawn : rightPlayerSpawn;
            if (numPlayers == 0)
            {
                GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
                player.name = "Player1";
                NetworkServer.AddPlayerForConnection(conn, player);
                isPlayer1On = true;
            }
            else
            {
                GameObject player = Instantiate(playerPrefab2, start.position, start.rotation);
                player.name = "Player2";

                NetworkServer.AddPlayerForConnection(conn, player);
                isPlayer2On = true;

            }

        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            isPlayer2On = false;

            if (!FindObjectOfType<GameModeSettings>().isWinLost)
            {

                if (!GameObject.FindGameObjectWithTag("P1Model"))
                {
                    singleton.StopHost();
                    GameObject.FindGameObjectWithTag("PopupHandler").GetComponent<DisconnectionPopupHandler>().serverPopup = true;

                    int previousScene = SceneManager.GetActiveScene().buildIndex - 1;
                    SceneManager.LoadScene(previousScene);

                }

            }
                base.OnServerDisconnect(conn);
            
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {

            isPlayer1On = false;
            if (!FindObjectOfType<GameModeSettings>().isWinLost)
            {
                singleton.StopClient();
                if (FindObjectOfType<serverButton>())
                    FindObjectOfType<serverButton>().OnclientDisconnected();

                if (!GameObject.FindGameObjectWithTag("P1Model"))
                {
                    GameObject.FindGameObjectWithTag("PopupHandler").GetComponent<DisconnectionPopupHandler>().clientPopup = true;

                    int previousScene = SceneManager.GetActiveScene().buildIndex - 1;
                    SceneManager.LoadScene(previousScene);
                }

            }
                base.OnClientDisconnect(conn);
            

        }
    }
}
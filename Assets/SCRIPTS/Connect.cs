using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Mirror.Discovery
    {

    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkDiscoveryHUD")]
    [HelpURL("https://mirror-networking.com/docs/Components/NetworkDiscovery.html")]
    [RequireComponent(typeof(NetworkDiscovery))]

    public class Connect : MonoBehaviour
{

      public  readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();


      //  NetworkManager manager;
        public NetworkDiscovery networkDiscovery;
        public InputField InputFieldIp;
        public GameObject HostConnect_go;
        public GameObject foundSerwerButton;
        public Transform[] buttonsSpawnPoint;
        public float nextButtonYdistance;
        public int btnCounter = 0;
        public ServerResponse[] foundServerInfo = new ServerResponse[5];
        public GameObject backToMenuBtn;
        public GameObject backToLobbyBtn;
        public GameObject serverNotFound;
        public bool isNotFound;
        public bool isDiscovering;
        public Animator[] animations;
        public float timer;
        public GameObject serverNotFoundBtn;
        public GameObject checkboxFriendlyFire;
        public GameObject checkboxNightMode;

        void Awake()
        { 
            timer = 0f;
            btnCounter = 0;
            nextButtonYdistance = 0f;
        //    manager = FindObjectOfType<NetworkManager>();
            networkDiscovery = GetComponent<NetworkDiscovery>();
        }

      
        public void HostFunction()
        {
            serverNotFoundBtn.SetActive(false);

            Debug.Log("Kliknieto HostFunction");
            backToMenuBtn.SetActive(false);
            backToLobbyBtn.SetActive(true);
            discoveredServers.Clear();

            NetworkManager.singleton.StartHost();

            networkDiscovery.AdvertiseServer();
            HostConnect_go.SetActive(false);

            AnimationReset();


            checkboxFriendlyFire.SetActive(true);
            checkboxNightMode.SetActive(true);

        }
    

        public void FindServerFunction()
        {
            serverNotFoundBtn.SetActive(false);
            GameObject[] btns = GameObject.FindGameObjectsWithTag("FoundServerButton");
            foreach (GameObject button in btns)
            {
                Destroy(button);
                btnCounter = 0;
            }
         
            isDiscovering = true;
            Debug.Log("Kliknieto FindServerFunction");

            discoveredServers.Clear();
            networkDiscovery.StartDiscovery();

            AnimationReset();
            isNotFound = false;
          
        }
        public void Update()
        {
            if (isDiscovering)
            {

                foreach (ServerResponse info in discoveredServers.Values)
                {
                    Debug.Log(info.EndPoint.Address.ToString());

                    if (btnCounter < discoveredServers.Count && btnCounter < 5)
                    {
                        serverNotFoundBtn.SetActive(false);


                        foundServerInfo[btnCounter] = info;

                        foreach (Animator anim in animations)
                        {
                            anim.Play(0, -1, 0);
                        }

                        if (btnCounter == 0)
                        {
                            var btn = Instantiate(foundSerwerButton, buttonsSpawnPoint[0].position , foundSerwerButton.transform.rotation, HostConnect_go.transform);
                            btn.GetComponentInChildren<TextMeshProUGUI>().text = info.EndPoint.Address.ToString();
                            btn.name = "SerwerButton1";
                        }
                        if (btnCounter == 1)
                        {
                            var btn = Instantiate(foundSerwerButton, buttonsSpawnPoint[1].position , foundSerwerButton.transform.rotation, HostConnect_go.transform);
                            btn.GetComponentInChildren<TextMeshProUGUI>().text = info.EndPoint.Address.ToString();
                            btn.name = "SerwerButton2";
                        }
                        if (btnCounter == 2)
                        {
                            var btn = Instantiate(foundSerwerButton, buttonsSpawnPoint[2].position , foundSerwerButton.transform.rotation, HostConnect_go.transform);
                            btn.GetComponentInChildren<TextMeshProUGUI>().text = info.EndPoint.Address.ToString();
                            btn.name = "SerwerButton3";
                        }
                        if (btnCounter == 3)
                        {
                            var btn = Instantiate(foundSerwerButton, buttonsSpawnPoint[3].position , foundSerwerButton.transform.rotation, HostConnect_go.transform);
                            btn.GetComponentInChildren<TextMeshProUGUI>().text = info.EndPoint.Address.ToString();
                            btn.name = "SerwerButton4";
                        }
                        if (btnCounter == 4)
                        {
                            var btn = Instantiate(foundSerwerButton, buttonsSpawnPoint[4].position, foundSerwerButton.transform.rotation, HostConnect_go.transform);
                            btn.GetComponentInChildren<TextMeshProUGUI>().text = info.EndPoint.Address.ToString();
                            btn.name = "SerwerButton5";
                        }
                        btnCounter += 1;
                    }
                }

                if (btnCounter == 0)
                {
                    serverNotFoundBtn.SetActive(true);
                }
            }
        }

        public void ConnectToServer01()
        {
            NetworkManager.singleton.StartClient(foundServerInfo[0].uri);
            HostConnect_go.SetActive(false);
            backToMenuBtn.SetActive(false);
            backToLobbyBtn.SetActive(true);
            checkboxFriendlyFire.SetActive(true);
            checkboxNightMode.SetActive(true);
            AnimationReset();
        }

        public void ConnectToServer02()
        {
            NetworkManager.singleton.StartClient(foundServerInfo[1].uri);
            HostConnect_go.SetActive(false);
            backToMenuBtn.SetActive(false);
            backToLobbyBtn.SetActive(true);
            checkboxFriendlyFire.SetActive(true);
            checkboxNightMode.SetActive(true);
            AnimationReset();
        }
        public void ConnectToServer03()
        {
            NetworkManager.singleton.StartClient(foundServerInfo[2].uri);
            HostConnect_go.SetActive(false);
            backToMenuBtn.SetActive(false);
            backToLobbyBtn.SetActive(true);
            checkboxFriendlyFire.SetActive(true);
            checkboxNightMode.SetActive(true);
            AnimationReset();
        }

        public void ConnectToServer04()
        {
            NetworkManager.singleton.StartClient(foundServerInfo[3].uri);
            HostConnect_go.SetActive(false);
            backToMenuBtn.SetActive(false);
            backToLobbyBtn.SetActive(true);
            checkboxFriendlyFire.SetActive(true);
            checkboxNightMode.SetActive(true);
            AnimationReset();
        }

        public void ConnectToServer05()
        {
            NetworkManager.singleton.StartClient(foundServerInfo[4].uri);
            HostConnect_go.SetActive(false);
            backToMenuBtn.SetActive(false);
            backToLobbyBtn.SetActive(true);
            checkboxFriendlyFire.SetActive(true);
            checkboxNightMode.SetActive(true);
            AnimationReset();
        }

        public void Connectt(ServerResponse info)
        {
            NetworkManager.singleton.StartClient(info.uri);
        }

        public void OnDiscoveredServer(ServerResponse info)
        {
            discoveredServers[info.serverId] = info;
        }

        public void AnimationReset()
        {
            foreach (Animator anim in animations)
            {
                if (anim.isActiveAndEnabled)
                    anim.Play(0, -1, 0);
            }
        }
    }
}
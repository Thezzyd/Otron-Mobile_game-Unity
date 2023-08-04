using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;
using UnityEngine.SceneManagement;
using TMPro;

public class serverButton : MonoBehaviour
{
    public NetworkManager manager;
    public NetworkDiscovery networkDiscovery;
    public GameObject HostConnect_go;
    public GameObject BackToMenuBtn;
    public GameObject BackToLobbyBtn;
    public GameObject startButton;
    public GameObject p1InfoText;
    public GameObject p2InfoText;
    public GameObject waitingInfoText;
    public Animator[] animations;
    public GameObject checkboxFriendlyFire;
    public GameObject checkboxNightMode;
    public GameObject checkFriendlyFire;
    public GameObject checkNightMode;

    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();


    public void Start()
    {
        manager = FindObjectOfType<NetworkManager>();
        networkDiscovery = FindObjectOfType<NetworkDiscovery>(); 
    }

    public void ConnectToServer()
    {

            if (name == "SerwerButton1")
            FindObjectOfType<Connect>().ConnectToServer01();
      else  if (name == "SerwerButton2")
            FindObjectOfType<Connect>().ConnectToServer02();
       else if (name == "SerwerButton3")
            FindObjectOfType<Connect>().ConnectToServer03();
       else if (name == "SerwerButton4")
            FindObjectOfType<Connect>().ConnectToServer04();
      else  if (name == "SerwerButton5")
            FindObjectOfType<Connect>().ConnectToServer05();
    }

    public void BackToMenu()
    {
       

        int previousScene = SceneManager.GetActiveScene().buildIndex - 1;
        SceneManager.LoadScene(previousScene);
    }

    public void StopHostFunction()
    {
        checkFriendlyFire.SetActive(false);
        checkNightMode.SetActive(false);
        checkboxFriendlyFire.SetActive(false);
        checkboxNightMode.SetActive(false);
        waitingInfoText.GetComponent<TextMeshProUGUI>().text = "";

        p1InfoText.GetComponent<TextMeshProUGUI>().text = "Waiting...";
        p2InfoText.GetComponent<TextMeshProUGUI>().text = "Waiting...";
        p1InfoText.SetActive(false);
        p2InfoText.SetActive(false);
        BackToMenuBtn.SetActive(true);
        BackToLobbyBtn.SetActive(false);
        startButton.SetActive(false);
        networkDiscovery.StartDiscovery();

        Debug.Log("Kliknieto StopHostFunction");
        discoveredServers.Clear();

        NetworkManager.singleton.StopHost();
        networkDiscovery.StartDiscovery();

        HostConnect_go.SetActive(true);

        foreach (Animator anim in animations)
        {
            anim.Play(0, -1, 0);
        }

        FindObjectOfType<Connect>().FindServerFunction();

    }

    public void OnclientDisconnected()
    {

        checkFriendlyFire.SetActive(false);
        checkNightMode.SetActive(false);
        checkboxFriendlyFire.SetActive(false);
        checkboxNightMode.SetActive(false);

        waitingInfoText.GetComponent<TextMeshProUGUI>().text = "";

        p1InfoText.GetComponent<TextMeshProUGUI>().text = "Waiting...";
        p2InfoText.GetComponent<TextMeshProUGUI>().text = "Waiting...";
        p1InfoText.SetActive(false);
        p2InfoText.SetActive(false);
        BackToMenuBtn.SetActive(true);
        BackToLobbyBtn.SetActive(false);
        startButton.SetActive(false);
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();

        HostConnect_go.SetActive(true);

        foreach (Animator anim in animations)
        {
            anim.Play(0, -1, 0);
        }

        FindObjectOfType<Connect>().FindServerFunction();

    }
}

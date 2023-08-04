using Mirror;
using UnityEngine.UI;

public class OnlyServerBtn : NetworkBehaviour
{
    void Start()
    {
        if (isServer)
        {
            GetComponent<Button>().interactable = true;
        }
    }
}

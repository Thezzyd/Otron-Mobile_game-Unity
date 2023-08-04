using Mirror;
using UnityEngine;

public class WallHandler : NetworkBehaviour
{
    private void Start()
    {
        if (isServer)
        {
            GetComponent<Animator>().enabled = true;
        }
    }

    [ClientRpc]
    public void TransformOff()
    {
        GetComponent<NetworkTransform>().enabled = false;
    }

    [ClientRpc]
    public void TransformON()
    {
        GetComponent<NetworkTransform>().enabled = true;
    }
}

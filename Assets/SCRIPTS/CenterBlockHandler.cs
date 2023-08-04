using UnityEngine;
using Mirror;

public class CenterBlockHandler : NetworkBehaviour
{
  private void Start()
    {
        if (isServer)
        {
            GetComponent<Animator>().enabled = true;
        } 
    }
}

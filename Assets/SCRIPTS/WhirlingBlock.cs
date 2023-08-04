using UnityEngine;
using Mirror;

public class WhirlingBlock : NetworkBehaviour
{
   public float timer = 0f;

    void FixedUpdate()
    {
        if (isServer)
        {
            timer += Time.deltaTime * 120f;
            if (timer > 360f)
            {
                timer = 0f;
            }

            transform.Rotate(Vector3.forward * 5f);
        }
    }
}

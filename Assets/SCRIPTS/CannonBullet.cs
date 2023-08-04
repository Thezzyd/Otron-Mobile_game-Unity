using UnityEngine;
using Mirror;
public class CannonBullet : NetworkBehaviour
{

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (isServer)
        {
            if (CompareTag("P1CannonBullet"))
                BulletCrash(12);
            else if (CompareTag("P2CannonBullet"))
                BulletCrash(13);
            else
                BulletCrash(9);
        }
    }

    public void BulletCrash(int wchichBullet)
    {
        RpcPlaySound();
        GameObject crash1Prefab = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().spawnPrefabs[wchichBullet];
        GameObject crash1 = Instantiate(crash1Prefab, transform.position, crash1Prefab.transform.rotation);
        NetworkServer.Spawn(crash1);
        Destroy(crash1, 1f);
        NetworkServer.Destroy(gameObject);
        FindObjectOfType<AudioManager>().Play("WybuchTurretLaser");
        CineMachineShake.Instance.ShakeCamera(3f, 0.1f);

    }

    [ClientRpc]
    public void RpcPlaySound()
    {
        FindObjectOfType<AudioManager>().Play("WybuchTurretLaser");
        CineMachineShake.Instance.ShakeCamera(3f, 0.1f);
    }
}

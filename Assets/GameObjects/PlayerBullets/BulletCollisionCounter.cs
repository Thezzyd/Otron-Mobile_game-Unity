using UnityEngine;
using System.Collections;

namespace Mirror
{
    public class BulletCollisionCounter : NetworkBehaviour
    {

        [HideInInspector] public LevelManager levelManager;
        [SyncVar] private float collisionCounter;
        [SyncVar] public float collisionMax;
        public float rotateSpeed;
        public Rigidbody2D rb;
        public float bulletlifeTimer;

        void Start()
        {
            levelManager = FindObjectOfType<LevelManager>();
            collisionCounter = 0f;
            rb = GetComponent<Rigidbody2D>();
            bulletlifeTimer = 0f;

          /*  if (isServer)
                StartCoroutine(BulletColliderOn(0.2f));
*/
        }

/*public IEnumerator BulletColliderOn(float time)
        {
            yield return new WaitForSeconds(time);
            GetComponent<Collider2D>().enabled = true;
        }*/

        public void FixedUpdate()
        {

            if (isServer)
            {
                bulletlifeTimer += Time.fixedDeltaTime;
            Vector3 dir = rb.velocity;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.fixedDeltaTime);
            Destroy(gameObject, 10f);
            }
        }

        public void BulletCrash(int i)
        {
            switch (i)
            {
                case 1:
                    GameObject crash1Prefab = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().spawnPrefabs[5];
                    GameObject crash1 = Instantiate(crash1Prefab, transform.position, crash1Prefab.transform.rotation);
                    NetworkServer.Spawn(crash1);
                    Destroy(crash1, 1f);
                    NetworkServer.Destroy(gameObject);
                    break;
                case 2:
                    GameObject crash2Prefab = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().spawnPrefabs[6];
                    GameObject crash2 = Instantiate(crash2Prefab, transform.position, crash2Prefab.transform.rotation);
                    NetworkServer.Spawn(crash2);
                    Destroy(crash2, 1f);
                    NetworkServer.Destroy(gameObject); 
                    break;
            }
        }

        public void OnCollisionEnter2D(Collision2D col)
           {
            if (isServer)
            {   
                collisionCounter++;
                RpcPlayeSound(1);
                if (collisionCounter >= collisionMax)
                {
                    if (CompareTag("BulletPlayer1"))
                    {
                        RpcPlayeSound(2);
                        BulletCrash(1);
                        CineMachineShake.Instance.ShakeCamera(3.4f, 0.1f);
                        FindObjectOfType<AudioManager>().Play("WybuchPlayerLaser");


                    }
                    if (CompareTag("BulletPlayer2"))
                    {
                        RpcPlayeSound(2);
                        BulletCrash(2);
                        CineMachineShake.Instance.ShakeCamera(3.4f, 0.1f);
                        FindObjectOfType<AudioManager>().Play("WybuchPlayerLaser");


                    }
                }
            }
        }

        [ClientRpc]
        public void RpcPlayeSound(int i)
        {
            switch (i)
            {
                case 1:
                    FindObjectOfType<AudioManager>().Play("OdbiciePlayerLaser");
                    break;
                case 2:
                    FindObjectOfType<AudioManager>().Play("WybuchPlayerLaser");
                    CineMachineShake.Instance.ShakeCamera(3.4f, 0.1f);

                    break;
            }

        }
    }
}
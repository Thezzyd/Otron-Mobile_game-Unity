using System.Collections;
using UnityEngine;
using Mirror;

public class CannonRotating : NetworkBehaviour
{
    [SyncVar] public bool right;
    [SyncVar] public bool moveRight;
    [SyncVar] public Transform rayCastStartPoint;
    [SyncVar] public float bulletSpeed = 5;
    [SyncVar] public float timer = 0;
    [SyncVar] public Transform parentTransform;
    [SyncVar] public Transform leftBarier;
    [SyncVar] public Transform rightBarier;
    [SyncVar] public float moveSpeed = 1f;
    [SyncVar] public Transform laserTransform;
    public ParticleSystem laserHitEffect;
    [SyncVar] public bool isStationary;
    [SyncVar] public bool isPlayer1Cannon;
    [SyncVar] public bool isPlayer2Cannon;
    [SyncVar] public bool isNeutral;
    [SyncVar]  public RaycastHit2D hit;
    public bool isChangeRotatingDirectionAllowed = true;
    public LevelManager levelManager;
    public Material[] stationaryTurretMaterials; // [0]- Unlit base/head. [1]- Lit base/head, [2]- Unlit laserSprite, [3]- Lit LaserSprite, [4]-Unlit ParticleLaser, [5]-Lit ParticleLaser
    public Material[] p1movingTurretMaterials; // [0]- Unlit base/head. [1]- Lit base/head, [2]- Unlit laserSprite, [3]- Lit LaserSprite, [4]-Unlit ParticleLaser, [5]-Lit ParticleLaser
    public Material[] p2movingTurretMaterials; // [0]- Unlit base/head. [1]- Lit base/head, [2]- Unlit laserSprite, [3]- Lit LaserSprite, [4]-Unlit ParticleLaser, [5]-Lit ParticleLaser


    public void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        parentTransform = gameObject.transform.parent.gameObject.transform;
        isChangeRotatingDirectionAllowed = true;
        if (CompareTag("StationaryCannon") || CompareTag("StationaryCannonP2"))
            isStationary = true;
        if (gameObject.transform.parent.gameObject.name == "P1Cannon")
            isPlayer1Cannon = true;
        else if (gameObject.transform.parent.gameObject.name == "P2Cannon")
            isPlayer2Cannon = true;
        else
            isNeutral = true;
    }



    public void SetNightModeMaterials()
    {
        if (FindObjectOfType<GameModeSettings>().isNightModeOn)
        {
            GameObject[] allTurretFragments = GameObject.FindGameObjectsWithTag("MovingCannon");

            foreach (GameObject obj in allTurretFragments)
            {
                if (obj.GetComponent<SpriteRenderer>())
                    obj.GetComponent<SpriteRenderer>().material = stationaryTurretMaterials[1];
            }

            GameObject[] allLaserSprites = GameObject.FindGameObjectsWithTag("LaserSprite");

            foreach (GameObject obj in allLaserSprites)
            {
                if (obj.GetComponent<SpriteRenderer>())
                    obj.GetComponent<SpriteRenderer>().material = stationaryTurretMaterials[3];
            }

            GameObject[] allLaserParticles = GameObject.FindGameObjectsWithTag("LaserParticles");

            foreach (GameObject obj in allLaserParticles)
            {
                if (obj.GetComponent<ParticleSystemRenderer>())
                    obj.GetComponent<ParticleSystemRenderer>().material = stationaryTurretMaterials[5];
            }
        }
        else
        {
            GameObject[] allTurretFragments = GameObject.FindGameObjectsWithTag("MovingCannon");

            foreach (GameObject obj in allTurretFragments)
            {
                if (obj.GetComponent<SpriteRenderer>())
                    obj.GetComponent<SpriteRenderer>().material = stationaryTurretMaterials[0];
            }

            GameObject[] allLaserSprites = GameObject.FindGameObjectsWithTag("LaserSprite");

            foreach (GameObject obj in allLaserSprites)
            {
                if (obj.GetComponent<SpriteRenderer>())
                    obj.GetComponent<SpriteRenderer>().material = stationaryTurretMaterials[2];
            }

            GameObject[] allLaserParticles = GameObject.FindGameObjectsWithTag("LaserParticles");

            foreach (GameObject obj in allLaserParticles)
            {
                if (obj.GetComponent<ParticleSystemRenderer>())
                    obj.GetComponent<ParticleSystemRenderer>().material = stationaryTurretMaterials[4];
            }
        }
    }

    void FixedUpdate()
    {
        PlayerControl[] playerScript = FindObjectsOfType<PlayerControl>();

        if (isServer && playerScript.Length == 2 && levelManager.lobbyStart)
        {
            if (!isStationary)
            {
                if (moveRight)
                {
                    parentTransform.Translate(Vector2.right * moveSpeed * Time.fixedDeltaTime);
                }
                else
                {
                    parentTransform.Translate(Vector2.left * moveSpeed * Time.fixedDeltaTime);
                }

                if (parentTransform.position.x < leftBarier.position.x)
                {
                    moveRight = true;
                }

                if (parentTransform.position.x > rightBarier.position.x)
                {
                    moveRight = false;
                }
            }

            timer += Time.deltaTime;

            if (!right)
            {
                transform.Rotate(Vector3.forward * 0.8f);
            }

            else
            {
                transform.Rotate(-Vector3.forward * 0.8f);
            }

            Vector3 forwardVel = transform.forward;
            Vector3 horizontalVel = transform.right;
            var layerMask = ~ (1 << 10);

            hit = Physics2D.Raycast(rayCastStartPoint.position, (forwardVel + horizontalVel) * 5f, 3.59f, layerMask);
            float laserScaleY = hit.distance / 1.795f;
            if (hit.collider)
            {
                if (isNeutral)
                {
                    if (hit.collider.gameObject.layer == 9 && timer >= 1f)
                    {
                        SpawnBullet(8);
                    }

                }
                else if (isPlayer1Cannon)
                {
                    if (hit.collider.gameObject.CompareTag("Player2Collider") && timer >= 1f)
                    {
                        SpawnBullet(10);
                    }
                }
                else if (isPlayer2Cannon)
                {
                    if (hit.collider.gameObject.CompareTag("Player1Collider") && timer >= 1f)
                    {
                        SpawnBullet(11);
                    }
                }
                laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserScaleY, 1);
                RpcParticleEmit();
            }
            else
                laserTransform.localScale = new Vector3(laserTransform.localScale.x, 2f, 1f);
        }
    }

   /* [Command]
    public void CmdAskServerForParticles()
    {
        RpcParticleEmit();
    */

    [ClientRpc]
    public void RpcParticleEmit()
    {
        laserHitEffect.Emit(2);
    }

    [ClientRpc]
    public void RpcPlaySound()
    {
        FindObjectOfType<AudioManager>().Play("TurretLaser");
    }

    /* [Command]
     public void CmdSpawnBullet(int wchichPrefab)
     {
         RpcSpawnBullet(wchichPrefab);
     }*/


    public void SpawnBullet(int wchichPrefab)
    {
        RpcPlaySound();
        GameObject prefabOfBullet = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().spawnPrefabs[wchichPrefab];
        GameObject bullet = Instantiate(prefabOfBullet, rayCastStartPoint.position, transform.rotation);
        Destroy(bullet, 5f);
        NetworkServer.Spawn(bullet);
        Vector3 forwardVel = transform.forward;
        Vector3 horizontalVel = transform.right;
        bullet.GetComponent<Rigidbody2D>().velocity = (forwardVel + horizontalVel) * bulletSpeed;
        timer = 0f;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (isServer)
        {
            if (isChangeRotatingDirectionAllowed)
            {
                isChangeRotatingDirectionAllowed = false;
                StartCoroutine(AlloowingRotationChange());
                if (right) right = false;
                else right = true;
            }
        }
    }


    public IEnumerator AlloowingRotationChange()
    {
        yield return new WaitForSeconds(0.5f);
        isChangeRotatingDirectionAllowed = true;
    }
  
}

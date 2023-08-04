using System.Collections;
using UnityEngine;
using Mirror;

    public class PlayerControl : NetworkBehaviour
    {
              //Deklaracja zmiennych

              private Rigidbody2D rb;  
   [SyncVar]  public float moveSpeed;  
   [SyncVar]  public float rotateSpeed; 
              public Transform bulletSpawnPoint; 
                                                
              public float bulletSpeed; 
   [SyncVar]  public float shootingTimer; 
              public float reloadTime; 
              public Joystick joystick; 
              public JoyButton joybutton;  
              public LevelManager levelManager; 
              public Animator anim; 
              public LifeSystem lifeSystem; 
              public AudioSource audio;
              public bool isShootingAllowed;   
              public GameObject frontPlayerLight;
              public GameObject aroundPlayerLight; 
              public Material[] p1Materials; 

              public Material[] p2Materials; 

    void Start()  
    {
        rb = GetComponent<Rigidbody2D>(); 
        joystick = FindObjectOfType<Joystick>();  
                                                 
        joybutton = FindObjectOfType<JoyButton>();
        levelManager = FindObjectOfType<LevelManager>(); 
        audio = GetComponent<AudioSource>();
        lifeSystem = FindObjectOfType<LifeSystem>();

        shootingTimer = 0f;
        reloadTime = 1f;
        isShootingAllowed = true;


        if (CompareTag("Player2")) 
            transform.rotation = new Quaternion(0,0,180,transform.rotation.w);  
        if (!isLocalPlayer)  
        {
            rb.bodyType = RigidbodyType2D.Kinematic; 
                                                   
        }

    }


    public void SetNightModeMaterials() 
    {
        if (FindObjectOfType<GameModeSettings>().isNightModeOn) 
        {                                                       
            if (isServer)     
            {
                if (CompareTag("Player1"))  
                {
                    frontPlayerLight.SetActive(true); 
                    aroundPlayerLight.SetActive(true); 
                    GetComponent<SpriteRenderer>().material = p1Materials[0]; 
                    GetComponentInChildren<ParticleSystemRenderer>().material = p1Materials[2]; 
                }
            }
            else 
            {
                if (CompareTag("Player2"))
                {
                    // AllowTurnOnLight();
                    frontPlayerLight.SetActive(true);
                    aroundPlayerLight.SetActive(true);
                    GetComponent<SpriteRenderer>().material = p2Materials[0];
                    GetComponentInChildren<ParticleSystemRenderer>().material = p2Materials[2];
                }
            }
        }
        else  
        {
            if (CompareTag("Player1"))
            {
                frontPlayerLight.SetActive(true);
                aroundPlayerLight.SetActive(true);
                GetComponent<SpriteRenderer>().material = p1Materials[0];
                GetComponentInChildren<ParticleSystemRenderer>().material = p1Materials[2];
            }

            else if (CompareTag("Player2"))
            {
                // AllowTurnOnLight();
                frontPlayerLight.SetActive(true);
                aroundPlayerLight.SetActive(true);
                GetComponent<SpriteRenderer>().material = p2Materials[0];
                GetComponentInChildren<ParticleSystemRenderer>().material = p2Materials[2];
            }
        }
    }
    void Update()  
    {

        if (isServer)
        {
            shootingTimer += Time.deltaTime; 
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().enabled = false;
            FindObjectOfType<LevelManager>().GetComponent<AudioSource>().volume = 0.3f;
        }
      if (isLocalPlayer)
      {
                if (levelManager.isGameAllowed)
                {
                    rb.velocity = new Vector2(joystick.Horizontal * moveSpeed, joystick.Vertical * moveSpeed);
                if (rb.velocity != Vector2.zero)
                {
                    Vector3 dir = rb.velocity;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

                    if (!audio.isPlaying)
                    {
                        if (isServer)
                            RpcPlaySound(5);
                        else
                            CmdAllowPlaySound(5);
                    }
                }
                else
                {
                    if (isServer)
                        RpcStopPlaySound();
                    else
                        CmdAllowStopPlayingSound();
                }
                    if (joybutton.pressd && shootingTimer >= reloadTime && isShootingAllowed)
                    {    
                         isShootingAllowed = false;
                         Shooting(bulletSpawnPoint.position, gameObject.tag);
                    if (isServer)
                        RpcPlaySound(3);
                    else
                    {
                        CmdAllowPlaySound(3);
                    //    CmdResetShootingTimerP2();
                    }
                 //   isShootingAllowed = false;
                    StartCoroutine(shootingBrake(0.1f));
                    }
            }
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().enabled = false;
            FindObjectOfType<LevelManager>().GetComponent<AudioSource>().volume = 0.3f;


        }

      

    }

    [Command]
    public void CmdResetShootingTimerP2()
    {
        GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerControl>().shootingTimer = 0f;

    }

    public IEnumerator shootingBrake(float brakeTime)
    {
        yield return new WaitForSeconds(brakeTime);
        isShootingAllowed = true;
    }


    [Command]
    public void Shooting(Vector3 position, string tag)
    {
         shootingTimer = 0f;
      //  GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerControl>().shootingTimer = 0f;
        if (tag == "Player1")
        {
            GameObject prefabOfBullet = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().spawnPrefabs[1];
            GameObject bullet = Instantiate(prefabOfBullet, position, transform.rotation);
            NetworkServer.Spawn(bullet);
            Vector3 forwardVel = bullet.transform.forward * bulletSpeed;
            Vector3 horizontalVel = bullet.transform.right * bulletSpeed;
            bullet.GetComponent<Rigidbody2D>().velocity = forwardVel + horizontalVel;
        }
        else if(tag == "Player2")
        {
            GameObject prefabOfBullet = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().spawnPrefabs[2];
            GameObject bullet = Instantiate(prefabOfBullet, position, transform.rotation);
            NetworkServer.Spawn(bullet);
            Vector3 forwardVel = bullet.transform.forward * bulletSpeed;
            Vector3 horizontalVel = bullet.transform.right * bulletSpeed;
            bullet.GetComponent<Rigidbody2D>().velocity = forwardVel + horizontalVel;
        }
       
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (isServer)
        {
            if (!FindObjectOfType<GameModeSettings>().isFriendlyFireOn)
            {
                if (col.gameObject.CompareTag("BulletPlayer2") && gameObject.CompareTag("Player1"))
                {
                    BulletCrash(6, col.gameObject);
                    RpcPlaySound(1);
                    Death(1);
                }

                if (col.gameObject.CompareTag("BulletPlayer1") && gameObject.CompareTag("Player2"))
                {
                    BulletCrash(5, col.gameObject);
                    RpcPlaySound(1);
                    Death(2);
                }

                if (col.gameObject.CompareTag("CannonBullet") && gameObject.CompareTag("Player1"))
                {
                    BulletCrash(9, col.gameObject);
                    RpcPlaySound(2);
                    Death(1);
                }

                if (col.gameObject.CompareTag("CannonBullet") && gameObject.CompareTag("Player2"))
                {
                    BulletCrash(9, col.gameObject);
                    RpcPlaySound(2);
                    Death(2);
                }

                if (col.gameObject.CompareTag("P1CannonBullet") && gameObject.CompareTag("Player2"))
                {
                    BulletCrash(12, col.gameObject);
                    RpcPlaySound(2);
                    Death(2);
                }

                if (col.gameObject.CompareTag("P2CannonBullet") && gameObject.CompareTag("Player1"))
                {

                    BulletCrash(13, col.gameObject);
                    RpcPlaySound(2);
                    Death(1);
                }
            }
            else
            {
                if (col.gameObject.CompareTag("BulletPlayer2"))
                {

                    if (CompareTag("Player1"))
                    {
                        BulletCrash(6, col.gameObject);
                        RpcPlaySound(1);
                        Death(1);
                    }
                    else if (col.gameObject.GetComponent<BulletCollisionCounter>().bulletlifeTimer >= 0.1f)
                    {
                        BulletCrash(6, col.gameObject);
                        RpcPlaySound(1);
                        Death(2);
                    }
                }

                if (col.gameObject.CompareTag("BulletPlayer1"))
                {
                   
                    if (CompareTag("Player2"))
                    {
                        BulletCrash(5, col.gameObject);
                        RpcPlaySound(1);
                        Death(2);
                    }
                    else if (col.gameObject.GetComponent<BulletCollisionCounter>().bulletlifeTimer >= 0.3f)
                    {
                        BulletCrash(5, col.gameObject);
                        RpcPlaySound(1);
                        Death(1);
                    }
                }

                if (col.gameObject.CompareTag("CannonBullet"))
                {
                    BulletCrash(9, col.gameObject);
                    RpcPlaySound(2);
                    if (CompareTag("Player1"))
                        Death(1);
                    else
                        Death(2);
                }
                if (col.gameObject.CompareTag("P1CannonBullet"))
                {
                    BulletCrash(12, col.gameObject);
                    RpcPlaySound(2);
                    if (CompareTag("Player1"))
                        Death(1);
                    else
                        Death(2);
                }

                if (col.gameObject.CompareTag("P2CannonBullet"))
                {

                    BulletCrash(13, col.gameObject);
                    RpcPlaySound(2);
                    if (CompareTag("Player1"))
                        Death(1);
                    else
                        Death(2);
                }
            }
        }
    }
    [Command]
    public void CmdAllowForLifeRefresh(int who)
    {
        RpcRefreshLifes(who);
    }

    [ClientRpc]
    public void RpcRefreshLifes(int who)
    {
        FindObjectOfType<LifeSystem>().Skucie(who);
        FindObjectOfType<LifeSystem>().RefreshLifes();
    }

     [Command]
    public void CmdMoveToResp()
    {
        RpcMoveToResp();
    }


    [ClientRpc]
    public void RpcMoveToResp()
    {
       GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
       GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
        player1.GetComponent<NetworkTransform>().enabled = false;
        player2.GetComponent<NetworkTransform>().enabled = false;

        player2.GetComponent<NetworkAnimator>().SetTrigger("death");
        player1.GetComponent<NetworkAnimator>().SetTrigger("death");


        player1.transform.position = levelManager.playerRespawnPoints[0].position;
        player1.transform.rotation = new Quaternion(0, 0, 0, player1.transform.rotation.w);
        player2.transform.position = levelManager.playerRespawnPoints[1].position;
        player2.transform.rotation = new Quaternion(0, 0, 180, player2.transform.rotation.w);

        player1.GetComponent<NetworkTransform>().enabled = true;
        player2.GetComponent<NetworkTransform>().enabled = true;
    }

    public void Death(int wchichPlayer)
    {
        switch (wchichPlayer)
        {
            case 1:              
                RpcPlaySound(4);
                DestructionEffect(1);
                RpcMoveToResp();
                RpcRefreshLifes(1);
                break;

            case 2:
                RpcPlaySound(4);
                DestructionEffect(2);
                RpcMoveToResp();
                RpcRefreshLifes(2);
                break;
        }
    }

    [ClientRpc]
    public void RpcScreenShake()
    {
        CineMachineShake.Instance.ShakeCamera(5.5f, 0.2f);
    }

    public void DestructionEffect(int who)
    {
        if (who == 2)
        {
            RpcScreenShake();
            GameObject shatred2Prefab = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().spawnPrefabs[4];
            GameObject effect2 = Instantiate(shatred2Prefab, GameObject.FindGameObjectWithTag("Player2").transform.position, shatred2Prefab.transform.rotation);
            NetworkServer.Spawn(effect2);
            Destroy(effect2, 3f);
        }
        if (who == 1)
        {
            RpcScreenShake();
            GameObject shatred1Prefab = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().spawnPrefabs[3];
            GameObject effect1 = Instantiate(shatred1Prefab, GameObject.FindGameObjectWithTag("Player1").transform.position, shatred1Prefab.transform.rotation);
            NetworkServer.Spawn(effect1);
            Destroy(effect1, 3f);
        }
    }

    public void BulletCrash(int wchichInNetworkManagerPrefabHierarchy, GameObject col)
    {
        CineMachineShake.Instance.ShakeCamera(3.4f, 0.1f);
        NetworkServer.Destroy(col);
        GameObject crash1Prefab = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().spawnPrefabs[wchichInNetworkManagerPrefabHierarchy];
        GameObject crash1 = Instantiate(crash1Prefab, transform.position, crash1Prefab.transform.rotation);
        NetworkServer.Spawn(crash1);
        Destroy(crash1, 1f);
    }


    [Command]
    public void CmdAllowPlaySound(int i)
    {
        RpcPlaySound(i);
    }

    [ClientRpc]
    public void RpcPlaySound(int i)
    {
        switch (i)
        {
            case 1:
                FindObjectOfType<AudioManager>().Play("WybuchPlayerLaser");
                break;
            case 2:
                FindObjectOfType<AudioManager>().Play("WybuchTurretLaser");
                break;
            case 3:
                FindObjectOfType<AudioManager>().Play("PlayerLaser");
                break;
            case 4:
                FindObjectOfType<AudioManager>().Play("PlayerDeath");
                break;
            case 5:
                audio.Play();
                break;
        }    
    }
    [Command]
    public void CmdAllowStopPlayingSound()
    {
        RpcStopPlaySound();
    }

    [ClientRpc]
    public void RpcStopPlaySound()
    {
        audio.Stop();
    }

}

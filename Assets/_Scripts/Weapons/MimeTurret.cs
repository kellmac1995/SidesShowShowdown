using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimeTurret : TD2D_Enemy
{

    public GameObject bulletPrefab;
    //public int ticketDropAmount = 30;
    public float bulletSpeed;
    //public Transform target;
    public Transform bulletSpawn;
    public float fireRate = 2;
    public float currAmmo = 3;
    public float minAmmo = 0;
    public float initialWait = 1.5f;
    //public GameObject deathParticle;
    public Animator anim;
    public GameObject spawnParticle; 

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        target = TD2D_PlayerController.Instance.gameObject.transform;
        StartCoroutine(SpawnWait());
        Instantiate(spawnParticle, transform.position, transform.rotation);
        AudioManager.Instance.mimeThudEv.start();

    }

    public override void StopEnemyAI()
    {
        
    }


    public override void Shrink()
    {
        
    }

    public override void Stun()
    {
        
    }


    public override void Stun(float _stunTime)
    {
        
    }



    public override void StopEnemyAI(float _blah)
    {

    }

    public override void StartEnemyAI()
    {
        
    }


    public override void Update()
    {
        

    }


    public override void OnEnable()
    {

    }


    public void Shoot()
    {
        anim.SetTrigger("Shoot");

        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Vector2 dir = (target.position - bulletSpawn.position).normalized;
        bullet.GetComponent<Rigidbody2D>().AddForce(dir * (bulletSpeed * 100));
        currAmmo--;
        AudioManager.Instance.mimeShootEv.start();

        if (currAmmo == 0)
        {
            KillObject();
            return;
        }

        StartCoroutine(FireWeapon());
        //print(currAmmo); 
    }


    public IEnumerator FireWeapon()
    {
        //print("firing");
        yield return new WaitForSeconds(fireRate);
        Shoot();
        
    }

    public void KillObject()
    {
        anim.SetTrigger("Dead");        
        StartCoroutine(KillWait());
    }

   public IEnumerator SpawnWait()
    {
        yield return new WaitForSeconds(initialWait);
        StartCoroutine(FireWeapon());
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("PlayerBullet"))
        {


            KillObject();
           

        }
    }


    public IEnumerator KillWait()
    {

        yield return new WaitForSeconds(0.5f);
        Death();

    }

    public override void Death()
    {

        //if (GameManager.Instance.CheckForDrop())
        //{            
        //    // Spawn Pickup
        //    Instantiate(GameManager.Instance.PickupObject(), transform.position, Quaternion.identity);

        //    GameManager.Instance.pickupChance = 0;

        //}


        AudioManager.Instance.enemyDeathkEv.start();

        //Player animation here
        ObjectPooler.Instance.Spawn(deathParticle, transform.position, Quaternion.identity);

        GameManager.Instance.AddTicketsToScore(ticketDropAmount);
        

        // Run the base class death
        base.Death();

    }


}

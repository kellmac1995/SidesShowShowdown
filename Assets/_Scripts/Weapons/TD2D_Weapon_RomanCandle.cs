using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TD2D_Weapon_RomanCandle : TD2D_Weapon
{

    public Transform bulletSpawn;

    public GameObject bulletPrefab;
    
    // The amount of ammo the player will start with and the maxiumum this gun can carry
    public int maxAmmoAmt;

    private int ammoCount = 0;

    public Slider powerMeter;

    //public float weaponCoolDown;

    //private float timeStamp;

    public int bulletSpeed;

    public float bulletDeviation = 20.0f;

    public int burstAmount = 3;

    private void Awake()
    {

        weaponType = TD2D_WeaponController.WeaponTypes.DragonGun;

    }

    public override void Start()
    {

        base.Start();

        //TD2D_PlayerController.Instance.useReticle = true;

        //powerMeter.maxValue = maxAmmoAmt;

    }

    public override void OnEnable()
    {

        base.OnEnable();

        powerMeter.maxValue = maxAmmoAmt;

        UpdateAmmoCount(maxAmmoAmt);
        powerMeter.gameObject.SetActive(true);
        

    }

    public override void OnDisable()
    {
        base.OnDisable();

        powerMeter.gameObject.SetActive(false);

        //if (UIManager.Instance.dragonImage)
        //    UIManager.Instance.dragonImage.SetActive(false);
    }

    public override void Activate()
    {

        base.Activate();
        
        TD2D_WeaponController.Instance.canActivateWeapon = false;

        // Shoots bullet

        //TODO FIX EVENT
        //AudioManager.Instance.clownPopEv.start(); // Audio for shooting
        
        StartCoroutine(BurstShoot());

    }


    public override void Deactivate()
    {

        
        //TriggerParticles();

        // Dont need to do this if doing it in the behavious script
        TD2D_WeaponController.Instance.canActivateWeapon = true;

        if (ammoCount <= 0)
        {
            // if no ammo switch to hammer
            TD2D_WeaponController.Instance.ChangeWeapon(TD2D_WeaponController.WeaponTypes.Hammer);
            Debug.Log("Changing back to hammer!");
        }

        base.Deactivate();

    }


    IEnumerator BurstShoot()
    {
        AudioManager.Instance.dragonShootEv.start();

        for (int i = 0; i < burstAmount; i++)
        {

            Shoot();
            yield return new WaitForSeconds(Random.Range(0.1f,0.2f));

        }

    }

    void Shoot()
    {


        //Quaternion shotAngle = Quaternion.identity;

        // shotAngle.eulerAngles = new Vector3(Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f), 0);

        //bulletSpawn.transform.eulerAngles += new Vector3(Random.Range(-bulletDev, bulletDev), Random.Range(-bulletDev, bulletDev), 0);

        
        // TODO Pool bullets
        var bullet = ObjectPooler.Instance.Spawn(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // Randomize angle variation between bullets
        float spreadAngle = Random.Range(-bulletDeviation, bulletDeviation);

        // Take the random angle variation and add it to the initial
        // desiredDirection (which we convert into another angle), which in this case is the players aiming direction
        var x = bulletSpawn.position.x - TD2D_WeaponController.Instance.CurrentWeapon.grip.position.x;
        var y = bulletSpawn.position.y - TD2D_WeaponController.Instance.CurrentWeapon.grip.position.y;
        float rotateAngle = spreadAngle + (Mathf.Atan2(y, x) * Mathf.Rad2Deg);

        // Calculate the new direction we will move in which takes into account 
        // the random angle generated
        var MovementDirection = new Vector2(Mathf.Cos(rotateAngle * Mathf.Deg2Rad), Mathf.Sin(rotateAngle * Mathf.Deg2Rad)).normalized;

        bullet.GetComponent<Rigidbody2D>().velocity = MovementDirection * bulletSpeed;


        
        //bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.right * (bulletSpeed * 100));

        UpdateAmmoCount(-1);


    }

    public void UpdateAmmoCount(int amount)
    {

        ammoCount += amount;
        
        // Prevents ammo going above max amount
        ammoCount = Mathf.Min(ammoCount, maxAmmoAmt);
        
        powerMeter.value = ammoCount;

    }
    
    
}

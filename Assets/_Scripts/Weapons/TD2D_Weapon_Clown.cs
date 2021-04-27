using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TD2D_Weapon_Clown : TD2D_Weapon
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

    private void Awake()
    {

        weaponType = TD2D_WeaponController.WeaponTypes.ClownGun;

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

        //if (UIManager.Instance.clownImage)
        //    UIManager.Instance.clownImage.SetActive(false);
    }

    public override void Activate()
    {

        base.Activate();
        TD2D_WeaponController.Instance.canActivateWeapon = false;

        // Shoots bullet
        Shoot();

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

    void Shoot()
    {
             
        // TODO Pool bullets
        var bullet = ObjectPooler.Instance.Spawn(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        //TODO FIX EVENT
        AudioManager.Instance.clownPopEv.start();

        bullet.GetComponent<Rigidbody2D>().AddForce(bulletSpawn.right * (bulletSpeed * 100));

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TD2D_Weapon_Bear : TD2D_Weapon
{

    bool lasering = false;

    public LineRenderer laser;
    public Transform laserHit;
    //public ParticleSystem muzzleFlash;
    public ParticleSystem hitpointParticles;

    //public float weaponCoolDown;

    //private float timeStamp;

    public int bulletSpeed;

    private void Awake()
    {

        weaponType = TD2D_WeaponController.WeaponTypes.Bear;

    }

    public override void Start()
    {

        base.Start();

        //TD2D_PlayerController.Instance.useReticle = true;

        //powerMeter.maxValue = maxAmmoAmt;

        //muzzleFlash = gameObject.GetComponent<ParticleSystem>();
        hitpointParticles = laserHit.gameObject.GetComponent<ParticleSystem>();
        laser = gameObject.GetComponent<LineRenderer>();
        laser.enabled = false;
        laser.useWorldSpace = true;

    }


    private void Update()
    {

        if (lasering)
        {
            Shoot();
        }

    }

    public override void OnEnable()
    {

        base.OnEnable();


        //powerMeter.maxValue = maxAmmoAmt;
        //UpdateAmmoCount(maxAmmoAmt);
        //powerMeter.gameObject.SetActive(true);
        

    }

    public override void OnDisable()
    {
        base.OnDisable();

        //powerMeter.gameObject.SetActive(false);

        //if (UIManager.Instance.clownImage)
        //    UIManager.Instance.clownImage.SetActive(false);
    }

    public override void Activate()
    {

        base.Activate();
        TD2D_WeaponController.Instance.canActivateWeapon = false;
        TD2D_PlayerController.Instance.isAttacking = true;
        lasering = true;
        hitpointParticles.Play();
        //muzzleFlash.Play();

    }


    public override void Deactivate()
    {


        // Dont need to do this if doing it in the behavious script
        TD2D_WeaponController.Instance.canActivateWeapon = true;

        TD2D_PlayerController.Instance.isAttacking = false;

        lasering = false;

        //muzzleFlash.Stop();
        hitpointParticles.Stop();
        laser.enabled = false;

        base.Deactivate();

    }

    void Shoot()
    {


        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
        Debug.DrawLine(transform.position, hit.point);
        laserHit.position = hit.point;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, laserHit.position);
        laser.enabled = true;

        print(hit.collider.gameObject.name);

        if (hit.collider.gameObject.CompareTag("Enemy"))
        {
            print(hit.collider.gameObject.name);
            hit.collider.gameObject.GetComponent<TD2D_Enemy>().TakeDamage();
        }


    }
    
}

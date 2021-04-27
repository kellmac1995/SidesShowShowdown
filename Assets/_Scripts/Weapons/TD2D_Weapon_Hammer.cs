using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TD2D_Weapon_Hammer : TD2D_Weapon
{

    //public GameObject hammerHitBox;

    //public Transform hammerReticle;

    public GameObject particleEffect;

    //public Transform leftHitBoxPos, rightHitBoxPos, upHitBoxPos, downHitBoxPos;

    public float cooldown = 1;

    public float chargeTime = 2;

    public float minHitRadius = 1;
    public float maxHitRadius = 5;

    public LayerMask killLayers;

    public bool drawGizmos = false;

    public float killRadius;



    private void Awake()
    {
        weaponType = TD2D_WeaponController.WeaponTypes.Hammer;
    }

    public override void Start()
    {

        base.Start();

        //TD2D_PlayerController.Instance.useReticle = false;

        reticle.gameObject.SetActive(true);

        //TD2D_WeaponController.Instance.powerMeter.minValue = 0;
        //TD2D_WeaponController.Instance.powerMeter.maxValue = chargeTime;

    }


    public override void OnEnable()
    {
        // TD2D_WeaponController.Instance.powerMeter.minValue = 0;
        //  TD2D_WeaponController.Instance.powerMeter.maxValue = chargeTime;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        StopAllCoroutines();
        //if (UIManager.Instance.hammerImage)
        //    UIManager.Instance.hammerImage.SetActive(false);

    }


    public override void Activate()
    {

        base.Activate();
        //AudioManager.Instance.playerSource.PlayOneShot(AudioManager.Instance.hammerHit);

        //TODO FIX EVENT
        AudioManager.Instance.hammerHitEv.start();

        TD2D_WeaponController.Instance.ActivateWeaponScreenShake();

        DoHitCheck();

        //hammerHitBox.SetActive(true);
        TriggerActivateParticles();

    }

    public override void Deactivate()
    {

        TriggerDeactivateParticles();

        //TD2D_PlayerController.Instance.canRotate = true;

        TD2D_WeaponController.Instance.powerMeter.gameObject.SetActive(false);

        //TD2D_PlayerController.Instance.canMove = true;
        // TODO ienum
        StartCoroutine(ReEnableWeapon());

        //base.Deactivate();

        //Invoke("ReEnableWeapon", cooldown);

    }



    private void DoHitCheck()
    {

        //float radiusRange = maxHitRadius - minHitRadius;

        ////print(radiusRange+ "radiusRange");

        //float powerPercent = (100 * Mathf.Min(TD2D_WeaponController.Instance.chargeTimer, chargeTime)) / chargeTime;

        //////print(powerPercent + "powerPercent");

        //float radiusPercent = radiusRange * (powerPercent / 100);

        //print(radiusPercent+ "radiusPercent");

        //killRadius = minHitRadius + radiusPercent;

        //print(killRadius+ "killRadius");

        //killRadius = Mathf.Clamp(TD2D_WeaponController.Instance.chargeTimer + minHitRadius, minHitRadius,maxHitRadius);


        //if (killRadius > maxHitRadius)
        //{
        //    killRadius = maxHitRadius;
        //}
        //else if (killRadius < minHitRadius)
        //{
        //    killRadius = minHitRadius;
        //}


        Collider2D[] killObjects = Physics2D.OverlapCircleAll(reticle.transform.position, killRadius, killLayers);

        foreach (Collider2D killedObject in killObjects)
        {

            if (killedObject.CompareTag("Enemy"))
            {

                killedObject.GetComponent<TD2D_Enemy>().TakeDamage();
            }
            else if (killedObject.CompareTag("BreakingBarrel"))
            {

                killedObject.GetComponent<BreakingBarrel>().Break();

            }
            else if (killedObject.CompareTag("ExplodingBarrel"))
            {

                killedObject.GetComponent<ExplosiveBarrel>().PreExplode();
            }
            else if (killedObject.CompareTag("StreamBarrel"))
            {
                killedObject.GetComponent<Barrel>().ActivateBarrel();
            }
            else if (killedObject.CompareTag("RestartButton"))
            {
                GameManager.Instance.RestartRound();
            }
            else if (killedObject.CompareTag("ShopButton"))
            {
                GameManager.Instance.LoadShopScene();
            }
            else if (killedObject.CompareTag("MimeEnemy"))
            {
                killedObject.GetComponent<MimeTurret>().KillObject();
            }


        }

        TD2D_WeaponController.Instance.chargeTimer = 0;

    }


    IEnumerator ReEnableWeapon()
    {

        yield return new WaitForSeconds(cooldown);

        TD2D_WeaponController.Instance.canActivateWeapon = true;

        base.Deactivate();

    }

    //void ReEnableWeapon()
    //{

    //    TD2D_WeaponController.Instance.canActivateWeapon = true;

    //}

    public override void TriggerActivateParticles()
    {
        var particlesObj = ObjectPooler.Instance.Spawn(particleEffect, reticle.transform.position, particleEffect.transform.rotation);
        particlesObj.transform.localScale = new Vector2(killRadius, killRadius);
    }

    public override void TriggerDeactivateParticles()
    {
        // Any particles that are activated at the and of an activation
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(reticle.transform.position, killRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(reticle.transform.position, minHitRadius);
            Gizmos.DrawWireSphere(reticle.transform.position, maxHitRadius);
        }
    }

}

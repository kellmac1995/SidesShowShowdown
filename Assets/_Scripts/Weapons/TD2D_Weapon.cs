using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD2D_Weapon : MonoBehaviour
{

    public TD2D_WeaponController.WeaponTypes weaponType;

    public Animator WeaponAnimator { get; set; }

    public SpriteRenderer WeaponRenderer { get; set; }

    //public GameObject weaponCard;
    public Animator weaponCardAnimator;

    public bool canRotate = false;

    public Transform grip;

    // The amount of camera shake on the weapon.
    public float camShakeAmount = 0.1f;

    // The length of the camera shake for the weapon.
    public float camShakeLength = 0.2f;

    // The length of the pause (slown down) before the camera shake for the weapon.
    public float camShakePrePause;

    // The length of the pause (slown down) after the camera shake for the weapon.
    public float camShakePostPause;

    public TD2D_Reticle reticle;


    public virtual void Start()
    {

        WeaponAnimator = GetComponent<Animator>();
        WeaponRenderer = GetComponent<SpriteRenderer>();

    }

    public virtual void Init() { }



    public virtual void TriggerActivateParticles() { }

    public virtual void TriggerDeactivateParticles() { }

    public virtual void SetHitBoxLocation(Transform hitBoxPos) { }

    public virtual void Activate()
    {
        //TD2D_PlayerController.Instance.isAttacking = true;
    }

    public virtual void Deactivate()
    {
        TD2D_PlayerController.Instance.isAttacking = false;
    }

    public virtual void OnEnable()
    {
        //reticle.gameObject.SetActive(true);
    }

    public virtual void OnDisable()
    {
        //reticle.gameObject.SetActive(false);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Heelies : Ability
{
    public float heeliesSpeed = 5;
    // public GameObject heeliesTrail;
    public bool heeliesActive = false;
    public Animator heeliesVFX; 

    //[HideInInspector]
    //Checks if activated
    public GameObject dustParticles; 
    
    public float coolDownWait = 1;

    public float currCoolDownWait = 0;

    public GameObject heeliesCoolDownAnimation;

    public override void Start()
    {
        base.Start();
        heeliesVFX = UIManager.Instance.playerSpeedVignette.GetComponent<Animator>();
    }


    public override void Update()
    {


        if (starting)
        {
            currStartTime += Time.deltaTime;

            cooldownFill.fillAmount = 1 - (currStartTime / startTime);

            if (currStartTime >= startTime)
            {

                starting = false;
                DisableCooldown();


            }
        }


        if (onCoolDown && !locked)
        {
            if (currCoolDownWait > 0)
            {
                currCoolDownWait -= Time.deltaTime;             
            }
            else
            {
                currCoolDown += Time.deltaTime;

                cooldownFill.fillAmount = 1 - (currCoolDown / coolDown);

                if (currCoolDown >= coolDown)
                {

                    DisableCooldown();
                }
            }
        }


        bool buttonPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetAxisRaw("LeftTrigger") == 1;


        if (!onCoolDown && !heeliesActive && buttonPressed && !GameManager.Instance.isPaused && GameManager.Instance.isPlaying)
        {
            ActivateHeelies();
        }
        else if(heeliesActive && !buttonPressed && GameManager.Instance.isPlaying)
        {
            DeactivateHeelies(); 
        }
    }


    public void ActivateHeelies()
    {
        heeliesVFX.gameObject.SetActive(true);
        heeliesVFX.SetBool("Activated", true);
        heeliesActive = true;
        dustParticles.SetActive(true);
        TD2D_PlayerController.Instance.currentSpeed += heeliesSpeed;
        EventsManager.Instance.PopupEvent(EventsManager.Instance.sisEvent);
        AudioManager.Instance.sisterMumbleEv.start();
        GameManager.Instance.cameraZoom.ZoomOut(15, 1);
        //heeliesTrail.SetActive(true);
        // Deactivate abilitiy availible animation here

    }

    public void DeactivateHeelies()
    {
        UIManager.Instance.UIController.SisAbilityDeactivatedAnimation();
        heeliesVFX.SetBool("Activated", false);
        dustParticles.SetActive(false);
        AudioManager.Instance.sisterMumbleEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        TD2D_PlayerController.Instance.currentSpeed = TD2D_PlayerController.Instance.maxSpeed;
        GameManager.Instance.cameraZoom.ZoomIn(10, 1);
        print("deactivated"); 
        heeliesActive = false;
        heeliesVFX.gameObject.SetActive(false);

        currCoolDownWait = coolDownWait;
        EnableCooldown();
    }
}

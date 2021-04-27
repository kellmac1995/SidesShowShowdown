using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedController : MonoBehaviour
{

    public Transform aimer;

    [HideInInspector]
    public RangedController rangedController;

    public TD2D_Enemy_Ranged[] rangedEnemies;
    public BulletRanged[] activeBullets;

    public float rangedCountdown = 15;
        
    public int minLockOnTime;
    public float maxLockOnTime;

    [Header("Status")]
    public bool isTargeting;

    private float currRangedCountdown = 15;



    private void Start()
    {

        currRangedCountdown = rangedCountdown;

    }


    public void TargetPlayer()
    {


        isTargeting = true;

        EventsManager.Instance.PopupEvent(EventsManager.Instance.aimEvent);

        Invoke("LockOnTarget", Random.Range(minLockOnTime, maxLockOnTime));

    }


    // Update is called once per frame
    void Update()
    {

        if (!GameManager.Instance.isPlaying)
        {
            aimer.gameObject.SetActive(false);

            return;
        }

        if (currRangedCountdown > 0)
        {
            currRangedCountdown -= Time.deltaTime;

            CheckRangedCountdown();
        }


        if (isTargeting)
        {

            aimer.position = TD2D_PlayerController.Instance.transform.position;

            float angle = Mathf.Sin(Time.time) * 250; //tweak this to change frequency

            aimer.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }

    }


    /// <summary>
    /// Check the countdown for ranged enemies and activates the controller if the countdown hits 0
    /// </summary>
    private void CheckRangedCountdown()
    {

        if (currRangedCountdown <= 0)
        {
            
            aimer.gameObject.SetActive(true);

            TargetPlayer();

            currRangedCountdown = rangedCountdown;

        }


    }



    void LockOnTarget()
    {

        isTargeting = false;
        EventsManager.Instance.textPopupTMPro.text = "FIRE!!!!";
        FireCannons();

    }

    void FireCannons()
    {
        foreach (TD2D_Enemy_Ranged enemy in rangedEnemies)
        {

            enemy.Shoot();

        }

        // TODO move to somewhere else not on this script
        Invoke("FinishedFiring", 3);

    }


    void FinishedFiring()
    {

        //aimingComp.firing = false;
        //GameManager.Instance.fireText.gameObject.SetActive(false);
        aimer.gameObject.SetActive(false);
        //GameManager.Instance.dialoguePanel.gameObject.SetActive(false);


    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_MothersGaze : Ability
{
    private LineRenderer lineRenderer;
    public Transform laserHit;
    public float stopDelay = 5f;
    public float beamTime;
    public float countDown;
    public LayerMask interactionLayer;
    public GameObject mothersGaze;
    private Camera cam;

    public Transform startPoint, midPoint, endPoint;

    public float reticleSpeed = 0.5f;

    float count = 0;

    public bool laserActive = false;

    float countTimer = 0;

           

    bool returning = false;

    // Start is called before the first frame update
    public override void Start()
    {

        cam = Camera.main;
        countDown = beamTime;
        lineRenderer = GetComponent<LineRenderer>();

        startPoint.position = cam.ViewportToWorldPoint(new Vector2(-0.5f, 0));
        midPoint.position = cam.ViewportToWorldPoint(new Vector2(-0.5f, 1.5f));
        endPoint.position = cam.ViewportToWorldPoint(new Vector2(1.5f, 1.5f));

        base.Start();

    }

    // Update is called once per frame
    public override void Update()
    {

        base.Update();

        bool buttonPressed = Input.GetKeyDown(KeyCode.Alpha1) || Input.GetButtonDown("ButtonX");

        if (buttonPressed && !onCoolDown && !laserActive && !GameManager.Instance.isPaused && GameManager.Instance.isPlaying)
        {
            laserActive = true;


            AudioManager.Instance.mothersGazeEv.start();
            AudioManager.Instance.motherMumbleEv.start(); 

            count = 0;
            countTimer = 0;


            EventsManager.Instance.PopupEvent(EventsManager.Instance.mumEvent);


            //UIManager.Instance.arenaUIController.MumAbilityAnimation();
            

        }

        if (laserActive)
        {

            Vector3 gazePOS = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

            gazePOS.z = 0;

            mothersGaze.transform.position = gazePOS;


            startPoint.position = cam.ViewportToWorldPoint(new Vector2(-0.5f, 0));
            midPoint.position = cam.ViewportToWorldPoint(new Vector2(-0.5f, 1.5f));
            endPoint.position = cam.ViewportToWorldPoint(new Vector2(1.5f, 1.5f));


            FireLaser();

        }


    }

    public void FireLaser()
    {


        countTimer += Time.deltaTime;
        
        count = Mathf.PingPong(reticleSpeed * countTimer, 1);

        Vector3 p1 = Vector3.Lerp(startPoint.position, midPoint.position, count);
        Vector3 p2 = Vector3.Lerp(midPoint.position, endPoint.position, count);

        laserHit.position = Vector3.Lerp(p1, p2, count);
        
        Vector3 direction = transform.position - laserHit.position;

        RaycastHit2D[] enemies = Physics2D.RaycastAll(transform.position, -direction, 500, interactionLayer);

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, laserHit.position);
        lineRenderer.enabled = true;
        
        foreach (RaycastHit2D enemy in enemies)
        {

            enemy.transform.GetComponent<TD2D_Enemy>().Shrink();

        }


        if (Vector2.Distance(laserHit.position, endPoint.position) < 0.5f && !returning)
        {
            returning = true;
            return;
        }else if (Vector2.Distance(laserHit.position, startPoint.position) < 0.5f && returning)
        {
            DeactivateLaser();
            returning = false;
            return;
        }

    }


    public void DeactivateLaser()
    {

       // trail.SetActive(false); 
        lineRenderer.enabled = false;
        laserActive = false;

        EnableCooldown();

        AudioManager.Instance.motherGazeTimerEv.start();
        AudioManager.Instance.motherMumbleEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);


    }

    public void AnimEventTrigger()
    {
        laserActive = true;
        AudioManager.Instance.mothersGazeEv.start();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringTrap : MonoBehaviour
{

    public float playerThrust = 5;
    public float enemyThrust = 15;

    public float jumpTime = 0.5f;
    private Vector3 velocity;
    private Vector3 direction;

    public GameObject boing;

    private Rigidbody2D enemy_rb;
    //private TD2D_PlayerController playerController;

    private TD2D_Enemy_Melee enemy;
    private Pathfinding.AIPath enemyPath;

    public Animator spring_animation;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(boingEffect());
            velocity = TD2D_PlayerController.Instance.CalculateVelocity() * playerThrust;
            GameManager.Instance.cameraZoom.ZoomOutZoomIn(18, 0.3f, .2f, 0.3f);
            TD2D_PlayerController.Instance.Jump(velocity, jumpTime);

            //TD2D_PlayerController.Instance.currentSpeed = TD2D_PlayerController.Instance.maxSpeed * playerThrust;

            spring_animation.SetTrigger("SpringActive");

            GameManager.Instance.controllerViber.Vibrate(0.5f, 0.5f, 0.5f);

            AudioManager.Instance.springsEv.start();

        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("Enemy Activated Spring Trap");
            StartCoroutine(boingEffect());
            enemyPath = collision.GetComponent<Pathfinding.AIPath>();


            velocity = enemyPath.desiredVelocity * enemyThrust;


            collision.gameObject.GetComponent<TD2D_Enemy>().Jump(velocity, jumpTime, true);


            spring_animation.SetTrigger("SpringActive");

        }

    }

    private IEnumerator boingEffect()
    {
        boing.SetActive(true);
        yield return new WaitForSeconds(1);
        boing.SetActive(false);
    }

    

}





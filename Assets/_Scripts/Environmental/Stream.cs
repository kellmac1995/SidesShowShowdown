using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour {

    public Transform particle;

    private MeshRenderer meshRenderer;

    public float streamForce = 500;

    private WaterStreamPivot streamPivot;


    bool endingPush = false;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        streamPivot = FindObjectOfType<WaterStreamPivot>();

    }
    

    void OnTriggerStay(Collider collision)
    {
        UpdateStream(GetHeight(collision));
    }

    void OnTriggerExit(Collider other)
    {
        UpdateStream(0);    
    }

    private float GetHeight(Collider collider)
    {
        return collider.transform.position.y + collider.bounds.size.y;
    }

    private void UpdateStream(float newHeight)
    {
        //for the Particle
        Vector3 newPosition = new Vector3(transform.position.x, newHeight, transform.position.z);
        particle.position = newPosition;

        //height cutoff
        newHeight /= transform.localScale.y;
        meshRenderer.material.SetFloat("_Cutoff", newHeight);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && streamPivot.active && !TD2D_PlayerController.Instance.inAir)
        {
                        
            TD2D_PlayerController.Instance.beingPushed = true;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * streamForce);
            GameManager.Instance.controllerViber.Vibrate(0.5f, 0.5f);
            //Debug.Log(TD2D_PlayerController.Instance.beingPushed);

        }
        else if (collision.gameObject.CompareTag("Enemy") & streamPivot.active)
        {

            collision.gameObject.GetComponent<TD2D_Enemy>().StopEnemyAI(1);

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * streamForce);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {

            if (!endingPush)
            {
                Invoke("EndPush", 0.25f);
                endingPush = true;
            }
            //TD2D_PlayerController.Instance.beingPushed = false;
            //Debug.Log(TD2D_PlayerController.Instance.beingPushed);
            //collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

    }


    private void EndPush()
    {
        GameManager.Instance.controllerViber.StopVibrate();
        TD2D_PlayerController.Instance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        TD2D_PlayerController.Instance.beingPushed = false;
        endingPush = false;
    }


}

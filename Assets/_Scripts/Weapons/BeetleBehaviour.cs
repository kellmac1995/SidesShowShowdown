using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleBehaviour : MonoBehaviour
{
    public Transform target;
    private Rigidbody2D rb;

    public float speed; 
    public float rotateSpeed;

    private Pathfinding.AIDestinationSetter aiDestination;
    private Pathfinding.AILerp aiLerp;
    
    public float lifeTime; 

    public float enemyHealth;
    public float attackDmg; 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        aiLerp = GetComponent<Pathfinding.AILerp>();
        SetSpeed();
    }

    public void SetAIDestComponent()
    {
        aiDestination = GetComponent<Pathfinding.AIDestinationSetter>();
    }


    public void SetSpeed()
    {

        aiLerp.speed = UnityEngine.Random.Range(7, 17);

    }

    //// Update is called once per frame
    //void FixedUpdate()
    //{
    //    //Vector2 dir = (Vector2)target.position - rb.position;
    //    //dir.Normalize();
    //    //float rotateAmount = Vector3.Cross(dir, transform.up).z;
    //    //rb.angularVelocity = -rotateAmount * rotateSpeed; 
    //    //rb.velocity = transform.up * speed; 
    //}

    public void SetAiTarget(Transform _target)
    {
        aiDestination.target = _target;
    }
   
}

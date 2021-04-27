using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldLaser : MonoBehaviour
{
    public LineRenderer laser;
    public Transform laserHit;
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitpointParticles;

    void Start()
    {
        muzzleFlash = gameObject.GetComponent<ParticleSystem>();
        hitpointParticles = laserHit.gameObject.GetComponent<ParticleSystem>();
        laser = gameObject.GetComponent<LineRenderer>();
        laser.enabled = false;
        laser.useWorldSpace = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
            hitpointParticles.Play();
            muzzleFlash.Play();
        }
        else
        {
            DeactivateLaser();
        }
    }


    public void Shoot()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
        Debug.DrawLine(transform.position, hit.point);
        laserHit.position = hit.point;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, laserHit.position);
        laser.enabled = true;

        print(hit.collider.gameObject.name);

        if(hit.collider.gameObject.CompareTag("Enemy"))
        {
            print(hit.collider.gameObject.name);
            hit.collider.gameObject.GetComponent<TD2D_Enemy>().TakeDamage();
        }
    }

    public void DeactivateLaser()
    {
        muzzleFlash.Stop();
        hitpointParticles.Stop();
        laser.enabled = false; 
    }


    

}

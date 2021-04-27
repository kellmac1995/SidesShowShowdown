using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAim : MonoBehaviour
{

    public Transform startPoint, midPoint, endPoint;
    
    public float reticleSpeed = 0.5f;
    
    float count = 0;

    public bool firing = false;

    void Update()
    {

        if (firing)
            return;

        count = Mathf.PingPong(reticleSpeed * Time.time, 1);
               
        Vector3 p1 = Vector3.Lerp(startPoint.position, midPoint.position, count);
        Vector3 p2 = Vector3.Lerp(midPoint.position, endPoint.position, count);
        transform.position = Vector3.Lerp(p1, p2, count);

    }
    
}

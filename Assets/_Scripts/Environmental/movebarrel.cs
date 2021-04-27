using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movebarrel : MonoBehaviour
{
    public GameObject trackPoint;
    public float speed = 5; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2.MoveTowards(transform.position, trackPoint.transform.position, Time.deltaTime * speed);
        
    }
}

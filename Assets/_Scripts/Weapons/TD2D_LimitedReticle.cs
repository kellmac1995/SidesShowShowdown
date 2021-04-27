using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD2D_LimitedReticle : MonoBehaviour {


    public Transform hammerPivot;

    public float radius = 3;
    


    // Update is called once per frame
    void Update () {

        //centerPt =  TD2D_PlayerController.Instance.transform.position;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 center = hammerPivot.transform.position;

        Vector2 direction = mousePosition - center; //direction from Center to Cursor
        Vector2 normalizedDirection = direction.normalized;

        transform.position = center + (normalizedDirection * radius);


        //transform.position = Input.mousePosition;

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterStreamPivot : MonoBehaviour {

    public float maxSize;
    public float growFactor;
    public float waitTime;
    public bool active = false;

    private Vector3 newSizeVect;

    public void ActivateStream()
    {
        active = true;
        
        ScaleUp();
    }

    private void Update()
    {

        if (active)
         transform.localScale = Vector3.Lerp(transform.localScale, newSizeVect, growFactor);

        if (Vector3.Distance(transform.localScale, new Vector3(1, 0, 1)) < 0.01f)
        {
            active = false;
        }

    }

    void ScaleUp()
    {

        newSizeVect = new Vector3(1, maxSize, 1);

        Invoke("ScaleDown", waitTime);

    }

    void ScaleDown()
    {

        newSizeVect = new Vector3(1, 0, 1);        

    }
}

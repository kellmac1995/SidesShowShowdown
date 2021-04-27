using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedReticle : MonoBehaviour
{

    public enum Side { Up, Down, Left, Right };

    public Side ReticleSide;

    private Vector3 startPosition, endPosition;

    public RangedController rangedController;

    float t;

    private void Awake()
    {

        startPosition = transform.localPosition;

        endPosition = startPosition;

        if (ReticleSide == Side.Right)
        {
            endPosition.x = startPosition.x - 20;
        }
        else if (ReticleSide == Side.Left)
        {
            endPosition.x = startPosition.x + 20;
        }
        else if (ReticleSide == Side.Up)
        {
            endPosition.y = startPosition.y - 20;
        }
        else if (ReticleSide == Side.Down)
        {
            endPosition.y = startPosition.y + 20;
        }


    }


    private void OnEnable()
    {

        t = 0;

        transform.localPosition = startPosition;

    }


    // Update is called once per frame
    void Update()
    {

        if (rangedController.isTargeting)
        {

            t += Time.deltaTime / 2;

            transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
        }

    }
}

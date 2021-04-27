using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public float slowdownFactor = 0.05f;
    public float slowdownLength = 0.5f;
    public float speedupLength = 0.5f;

    public bool speedUp = false;
    public bool slowDown = false;


    void Update()
    {

        if (slowDown)
        {
            
            float val = (1f / slowdownLength) * Time.unscaledDeltaTime;

            Time.timeScale -= val;
            
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            AudioManager.Instance.UpdatePitch(Time.timeScale);

            if (Time.timeScale <= slowdownFactor)
            {
                slowDown = false;
            }

        }
        else if (speedUp)
        {
            float val = (1f / speedupLength) * Time.unscaledDeltaTime;
            
            Time.timeScale += val;
            
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);


            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            AudioManager.Instance.UpdatePitch(Time.timeScale);

            if (Time.timeScale >= 1f)
            {
                speedUp = false;
            }

        }

    }

    public void InstantSlowmotion(float _factor, float _length)
    {

        slowdownFactor = _factor;
        slowdownLength = _length;
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.2f;
        speedUp = true;

    }


    public void Slowmotion(float _factor, float _slowLength, float _speedLength)
    {

        slowdownFactor = _factor;
        slowdownLength = _slowLength;
        speedupLength = +speedupLength;
        slowDown = true;
        speedUp = true;

    }


    public void SlowDown(float _factor, float _length)
    {

        slowdownFactor = _factor;
        slowdownLength = _length;
        slowDown = true;

    }


    public void SpeedUp(float _factor, float _length)
    {

        slowdownFactor = _factor;
        slowdownLength = _length;
        speedUp = true;

    }

}

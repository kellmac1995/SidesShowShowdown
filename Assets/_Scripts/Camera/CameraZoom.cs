using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    public Camera mainCam;

    private float t;
    private float zoomTime;
    public float startCamSize;
    private float targetCamSize;


    private bool zoomIn = false;
    private bool zoomOut = false;



    public void GetMainCam()
    {

        mainCam = Camera.main;

    }


    public void SetStartCamSize(float _size)
    {

        startCamSize = _size;

    }


    public void GetStartCamSize()
    {

        startCamSize = mainCam.orthographicSize;

    }

    // Update is called once per frame
    void Update()
    {
        if (zoomIn)
        {
            t += Time.deltaTime / zoomTime;
            mainCam.orthographicSize = Mathf.SmoothStep(mainCam.orthographicSize, targetCamSize, t);

            if (mainCam.orthographicSize <= targetCamSize + 0.1f)
            {

                mainCam.orthographicSize = targetCamSize;
                zoomIn = false;

            }
        }
        else if (zoomOut)
        {

            t += Time.deltaTime / zoomTime;
            mainCam.orthographicSize = Mathf.SmoothStep(mainCam.orthographicSize, targetCamSize, t);

            if (mainCam.orthographicSize >= targetCamSize - 0.1f)
            {

                mainCam.orthographicSize = targetCamSize;
                zoomOut = false;

            }
        }
    }


    public void ZoomIn(float _size, float _time)
    {

        targetCamSize = _size;
        zoomTime = _time; // Convert to seconds
        t = 0;
        zoomIn = true;

    }


    public void ZoomOut(float _size, float _time)
    {

        targetCamSize = _size;
        zoomTime = _time; // Convert to seconds
        t = 0;
        zoomOut = true;

    }


    public void ZoomOutZoomIn(float _size, float _zoomOutTime,  float _zoomWaitTime, float _zoomInTime)
    {

        StartCoroutine(DoZoomOutZoomIn(_size, _zoomOutTime, _zoomWaitTime, _zoomInTime));

    }


    IEnumerator DoZoomOutZoomIn(float _size, float _zoomOutTime, float _zoomWaitTime, float _zoomInTime)
    {

        ZoomOut(_size, _zoomOutTime);

        yield return new WaitForSeconds(_zoomOutTime + _zoomWaitTime);

        ZoomIn(startCamSize, _zoomInTime);

    }


}

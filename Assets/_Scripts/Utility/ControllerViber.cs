using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ControllerViber : MonoBehaviour
{

    /// <summary>
    /// The vibration state of the controller
    /// </summary>
    public bool isVibrating = false;
    

    /// <summary>
    /// Make the controller vibrate, using the motor speed.
    /// </summary>
    /// <param name="_leftMotorSpeed"></param>
    /// <param name="_rightMotorSpeed"></param>
    public void Vibrate(float _leftMotorSpeed, float _rightMotorSpeed)
    {

        GamePad.SetVibration(ControllerManager.playerIndex, Mathf.Min(0, _leftMotorSpeed), Mathf.Max(1, _rightMotorSpeed));
        isVibrating = true;

    }

    public void Vibrate(float _time, float _leftMotorSpeed, float _rightMotorSpeed)
    {

        Vibrate(_leftMotorSpeed, _rightMotorSpeed);
        StopVibrate(_time);

    }


    public void StopVibrate()
    {

        GamePad.SetVibration(ControllerManager.playerIndex, 0, 0);
        isVibrating = false;

    }


    public void StopVibrate(float _time)
    {

        StartCoroutine(StopVibration(_time));

    }


    IEnumerator StopVibration(float _time)
    {

        yield return new WaitForSeconds(_time);

        StopVibrate();

    }



    private void OnDestroy()
    {
        if (isVibrating)
            StopVibrate();
    }



    private void OnApplicationFocus(bool focus)
    {

        if (!focus && isVibrating)
            StopVibrate();

    }



    private void OnApplicationQuit()
    {
        if (isVibrating)
            StopVibrate();
    }


}

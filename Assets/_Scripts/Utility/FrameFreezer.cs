using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameFreezer : MonoBehaviour
{

    public bool isFrozen;

    private float freezeDuration = 0;


    /// <summary>
    /// Freezes the game for duration of float in seconds.
    /// </summary>
    /// <param name="_duration"></param>
    public void Freeze(float _duration)
    {
        if (!isFrozen && GameManager.Instance.isPlaying)
        {
            freezeDuration = _duration;
            StartCoroutine("DoFreeze");
        }
    }

    IEnumerator DoFreeze()
    {
        isFrozen = true;
        var original = Time.timeScale; 
        Time.timeScale = 0; 
        yield return new WaitForSecondsRealtime(freezeDuration);
        Time.timeScale = original;
        freezeDuration = 0;
        isFrozen = false;
    }
}

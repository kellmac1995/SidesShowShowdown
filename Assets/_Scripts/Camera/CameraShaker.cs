using UnityEngine;

public class CameraShaker : MonoBehaviour {

    [HideInInspector]
    public Camera mainCam;

	float shakeAmount = 0;

    public Vector3 camStartPos;


    public void Shake(float amt, float length)
	{
        shakeAmount = amt;
        camStartPos = mainCam.transform.localPosition;
        InvokeRepeating("DoShake", 0, 0.01f);
		Invoke("StopShake", length);
	}


	void DoShake()
	{
		if (shakeAmount > 0)
		{

            Vector3 camPos = camStartPos;

			float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
			float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
			camPos.x += offsetX;
			camPos.y += offsetY;

			mainCam.transform.position = camPos;

		}
	}


	public void StopShake()
	{
		CancelInvoke("DoShake");
		mainCam.transform.localPosition = camStartPos;
	}

}

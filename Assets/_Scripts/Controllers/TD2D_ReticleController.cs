using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD2D_ReticleController : MonoBehaviour {

    
    private  TD2D_Reticle _currentReticle;

    public TD2D_Reticle CurrentReticle
     {   
        get
        {
            if (_currentReticle == null)
            {
                //_currentReticle = TD2D_WeaponController.Instance.CurrentWeapon.reticle;
                UpdateReticle();

                if (_currentReticle == null)
                {
                    Debug.LogError("Please check the player has a reticle");
                }

            }

            return _currentReticle;

        }
    }
            

    public float reticleSpeed = 10;

    private Vector2 aimDir;

    TD2D_PlayerController playerController;

    private void Start()
    {

        playerController = TD2D_PlayerController.Instance;

    }


    private void OnEnable()
    {
        UpdateReticle();
    }


    void Update () {


        if (playerController.enableMouse)
        {

            Vector2 center = playerController.aimPivot.transform.position;
                     
            Vector2 direction = aimDir - center;

            Vector2 normalizedDirection = direction.normalized;
                       
            CurrentReticle.transform.position = center + (normalizedDirection * CurrentReticle.radius);

        }

        if (!playerController.isAttacking)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, aimDir), Time.deltaTime * reticleSpeed);
        }

        

    }
    

    public void ToggleReticle()
    {

        CurrentReticle.gameObject.SetActive(!CurrentReticle.gameObject.activeSelf);

    }



    public void UpdateReticle()
    {
        if (_currentReticle != null)
            _currentReticle.gameObject.SetActive(false);

        _currentReticle = TD2D_WeaponController.Instance.CurrentWeapon.reticle;

        _currentReticle.gameObject.SetActive(true);
        
        transform.rotation = Quaternion.identity;

        _currentReticle.transform.position = transform.position + new Vector3(0, _currentReticle.radius, 0);

    }


    public void RotatePivot(Vector2 normalizedPos)
    {

        aimDir = normalizedPos;

    }

}

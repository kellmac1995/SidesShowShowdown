using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDisable : MonoBehaviour {

    public float disableTime = 0.5f;

    private void OnEnable()
    {
        Invoke("DisableMe", disableTime);
    }

    void DisableMe()
    {
        TD2D_WeaponController.Instance.canActivateWeapon = true;
        gameObject.SetActive(false);
    }
}

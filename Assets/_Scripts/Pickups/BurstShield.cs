using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstShield : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("ButtonY"))
        {
            TD2D_PlayerController.Instance.PlayerHealth.DeactivateShield();
        }
    }
}

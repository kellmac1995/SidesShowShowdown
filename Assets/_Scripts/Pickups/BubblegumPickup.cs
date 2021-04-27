using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblegumPickup : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            // set shield to on
            TD2D_PlayerController.Instance.PlayerHealth.ActivateShield();

            // Do any particle effects here
            Destroy(gameObject);
        }
    }

}

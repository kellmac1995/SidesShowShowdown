using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleBagPickup : MonoBehaviour
{

    // public WaveSpawner waveMan;

    //private void Start()
    //{
    //      waveMan = FindObjectOfType<WaveSpawner>();
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (TD2D_WeaponController.Instance.currentWeaponType == TD2D_WeaponController.WeaponTypes.BeetleBag)
            {
                //TODO Make aloott betterrr
                TD2D_Weapon_BeetleBag tempWeapon = (TD2D_Weapon_BeetleBag)TD2D_WeaponController.Instance.CurrentWeapon;
                tempWeapon.UpdateAmmoCount(tempWeapon.maxAmmoAmt);
                //waveMan.SpawnClownWave();
            }
            else
            {
                TD2D_WeaponController.Instance.ChangeWeapon(TD2D_WeaponController.WeaponTypes.BeetleBag);
                //waveMan.SpawnClownWave();
            }

            

            // Do any particle effects here
            Destroy(gameObject);
        }
    }

}

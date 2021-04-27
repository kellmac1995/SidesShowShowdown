using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownGunPickup : MonoBehaviour
{

    public EnemySpawner waveMan;

    private void Start()
    {
        waveMan = FindObjectOfType<EnemySpawner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (TD2D_WeaponController.Instance.currentWeaponType == TD2D_WeaponController.WeaponTypes.ClownGun)
            {
                //TODO Make aloott betterrr
                TD2D_Weapon_Clown tempWeapon = (TD2D_Weapon_Clown)TD2D_WeaponController.Instance.CurrentWeapon;
                tempWeapon.UpdateAmmoCount(tempWeapon.maxAmmoAmt);
                
            }
            else
            {
                TD2D_WeaponController.Instance.ChangeWeapon(TD2D_WeaponController.WeaponTypes.ClownGun);
                
            }

            

            // Do any particle effects here
            Destroy(gameObject);
        }
    }

}

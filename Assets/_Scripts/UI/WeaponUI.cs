using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{

    //public GameObject clownUI;
    //public GameObject dragonUI;
    //public GameObject hammerUI;
    //public GameObject beetleUI;

    public Animator[] cards;

    public Animator hammerAnim;
    public Animator clownAnim;
    public Animator beetleAnim;
    public Animator dragonAnim;

    public void UpdateWeaponUi()
    {

        foreach (Animator card in cards)
        {

            card.SetBool("isActive", false);

        }

    }


}

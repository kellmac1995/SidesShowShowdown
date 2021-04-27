using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbilitiesController : GenericSingletonClass<AbilitiesController>
{

   // public Animator characterAnim;

    [Header("Shop Objects")]
    public GameObject mothersGaze;
    public GameObject heelies;
    public GameObject babyRattle;
    public GameObject jokeBook;
    //public GameObject abilitySlot1;
    //public GameObject abilitySlot2;
    //public GameObject abilitySlot3;
    //public GameObject abilitySlot4;


    public Ability[] abilites;


    [HideInInspector]
    public GameObject rattleObject;

    public bool rattleOn = false;

    // Start is called before the first frame update
    void Start()
    {


        UpdateAbilityObjectStates();
        
        
    }


    public void DisableAbilites()
    {

        foreach (Ability ability in abilites)
        {

            ability.locked = true;
            //ability.EnableCooldown();

        }

    }



    public void EnableAbilites()
    {

        foreach (Ability ability in abilites)
        {

            ability.locked = false;
            //ability.DisableCooldown();

        }

    }



    public void UpdateAbilityObjectStates()
    {

        foreach (ShopItem item in ShopManager.Instance.shopItems)
        {

            if (item.isAbility)
            {

                if (item.itemType == ShopManager.ShopItemType.Sister && item.isPurchased)
                {
                    ActivateHeeliesObject();
                }
                if (item.itemType == ShopManager.ShopItemType.Dad && item.isPurchased)
                {
                    ActivateStunObject();
                }
                if (item.itemType == ShopManager.ShopItemType.Mum && item.isPurchased)
                {
                    ActivateMotehersGazeObject();
                }
                if (item.itemType == ShopManager.ShopItemType.Baby && item.isPurchased)
                {
                    ActivateRattleObject();
                }

            }

        }

    }



    public void ActivateHeeliesObject()
    {
        
        heelies.SetActive(true);
        heelies.GetComponent<Ability>().UISlot.SetActive(true);

    }

    public void ActivateStunObject()
    {
        
        jokeBook.SetActive(true);
        jokeBook.GetComponent<Ability>().UISlot.SetActive(true);

    }

    public void ActivateMotehersGazeObject()
    {
        
        mothersGaze.SetActive(true);
        mothersGaze.GetComponent<Ability>().UISlot.SetActive(true);

    }

    public void ActivateRattleObject()
    {
        
        babyRattle.SetActive(true);
        babyRattle.GetComponent<Ability>().UISlot.SetActive(true);

    }



}

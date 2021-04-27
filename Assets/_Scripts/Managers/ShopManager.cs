using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : GenericSingletonClass<ShopManager>
{

    public enum ShopItemType { ClownGun, BeetleBag, DragonGun, Carnie, Baby, Dad, Mum, Sister, Bear, None }

    public Animator familyDropper;

    public bool onTopShelf = false;

    public ShopItem selectedItem;

    public int abilitiesPurchased = 0;

    public float itemPriceModifier = 1f;

    public GameObject buttonEvents;

    public GameObject textPopup;

    public GameObject buttonPopup;

    public GameObject[] buyButtons;

    public GameObject[] tryButtons;

    public GameObject[] equipButtons;

    public GameObject[] backButtons;

    public GameObject[] shelfButtons;


    public float priceMultiplier0 = 4;
    public float priceMultiplier1 = 9f;
    public float priceMultiplier2 = 11;
    public float priceMultiplier3 = 14;

    public List<ShopItem> shopItems;


    public override void Awake()
    {

        base.Awake();

        LoadShopValues();

        CalculateModifier();

    }


    /// <summary>
    /// Loads all values of the store from the player prefs
    /// </summary>
    public void LoadShopValues()
    {

        abilitiesPurchased = 0;

        foreach (ShopItem item in shopItems)
        {

            item.isPurchased = PlayerPrefs.GetInt(item.itemType.ToString() + "Purchased", 0) == 1 ? true : false;

            if (item.isAbility && item.isPurchased)
            {
                abilitiesPurchased++;
                CalculateModifier();
            }

        }


    }


    /// <summary>
    /// Save all values of the store into the player prefs
    /// </summary>
    public void SaveShopValues()
    {

        foreach (ShopItem item in shopItems)
        {

            PlayerPrefs.SetInt(item.itemType.ToString() + "Purchased", item.isPurchased ? 1 : 0);

        }

    }


    public void CalculateModifier()
    {
        if (abilitiesPurchased == 0)
        {
            itemPriceModifier = priceMultiplier0;
        }
        else if (abilitiesPurchased == 1)
        {
            itemPriceModifier = priceMultiplier1;

        }
        else if (abilitiesPurchased == 2)
        {
            itemPriceModifier = priceMultiplier2;
        }
        else if (abilitiesPurchased == 3)
        {
            itemPriceModifier = priceMultiplier3;
        }

    }



    public void ClearItemButtons()
    {

        buttonPopup.SetActive(false); // Disable button panel

        foreach (GameObject sprite in buyButtons)
            sprite.gameObject.SetActive(false);

        foreach (GameObject sprite in tryButtons)
            sprite.gameObject.SetActive(false);

        foreach (GameObject sprite in equipButtons)
            sprite.gameObject.SetActive(false);

        foreach (GameObject sprite in backButtons)
            sprite.gameObject.SetActive(false);

        foreach (GameObject sprite in shelfButtons)
            sprite.gameObject.SetActive(false);

    }


    public void UpdateItemCaption()
    {

        textPopup.GetComponentInChildren<TMPro.TextMeshPro>().text = selectedItem.itemCaption;
        textPopup.SetActive(true);

    }


    public void UpdateItemButtons()
    {

        ClearItemButtons();


        int active = 0;


        if (GameManager.Instance.controllerActive)
        {
            active = 0;
        }
        else
        {
            active = 1;
        }


        if (selectedItem.itemType == ShopItemType.Carnie)
        {

            shelfButtons[active].SetActive(true);

        }
        else
        {

            if (selectedItem.isPurchased && !selectedItem.isAbility)
            {

                equipButtons[active].SetActive(true);

            }
            if (!selectedItem.isPurchased)
            {

                buyButtons[active].SetActive(true);

            }
            if (selectedItem.isAbility)
            {

                backButtons[active].SetActive(true);

            }
            if (!selectedItem.isPurchased && !selectedItem.isAbility)
            {

                tryButtons[active].SetActive(true);

            }



                       
        }

        UpdateItemCaption();
        buttonPopup.SetActive(true); // re enable button panel

    }
}



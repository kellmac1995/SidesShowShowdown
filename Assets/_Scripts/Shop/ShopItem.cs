using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem : MonoBehaviour
{

    public ShopManager.ShopItemType itemType;

    public TD2D_WeaponController.WeaponTypes weaponType;

    public int itemPrice;

    public bool isPurchased;

    public bool isAbility = false;

    public string itemCaption;

    public ShopItemObject shopItemObj;


    public void UpdateItemText()
    {

        if (shopItemObj.itemPriceText == null)
        {
            print("No text set for " + itemType);
            return;
        }

            if (isPurchased)
            {

            shopItemObj.itemPriceText.text = "OWNED"; //"Owned";

            }
            else if (isAbility)
            {

                shopItemObj.itemPriceText.text = (itemPrice * ShopManager.Instance.itemPriceModifier).ToString("0000");

            }
            else
            {

                shopItemObj.itemPriceText.text = itemPrice.ToString("0000");

            }


    }
}

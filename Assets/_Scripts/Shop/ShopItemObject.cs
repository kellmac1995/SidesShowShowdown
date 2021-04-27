using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ShopItemObject : MonoBehaviour
{

    public ShopItem shopItemRef;

    public ShopManager.ShopItemType itemObjType;

    public Animator itemAnimator;
    
    public UnityEngine.UI.Text itemPriceText;

    public GameObject abilityGUI;


    private void Start()
    {

        shopItemRef = ShopManager.Instance.shopItems.Find(delegate (ShopItem _item)
        {

            return _item.itemType == itemObjType;

        });


        if (shopItemRef.isAbility && shopItemRef.isPurchased)
        {

            abilityGUI.SetActive(true);

        }
        

        shopItemRef.shopItemObj = this;

        shopItemRef.UpdateItemText();
        


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            ShopManager.Instance.selectedItem = ShopManager.Instance.shopItems.Find(delegate (ShopItem _item)
            {

                return _item.itemType == itemObjType;

            });

            ShopManager.Instance.UpdateItemButtons();            

            itemAnimator.SetBool("selected", true);

            if (itemObjType == ShopManager.ShopItemType.Carnie)
            {

                ShopManager.Instance.familyDropper.SetBool("isDropped", true);

            }

        }

    }


    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            if (ShopManager.Instance.selectedItem != null && ShopManager.Instance.selectedItem.itemType == itemObjType)
            {
                ShopManager.Instance.selectedItem = null;
            }

            ShopManager.Instance.textPopup.SetActive(false);

            ShopManager.Instance.buttonPopup.SetActive(false);

            itemAnimator.SetBool("selected", false);

            if (itemObjType == ShopManager.ShopItemType.Carnie)
            {

                ShopManager.Instance.familyDropper.SetBool("isDropped", false);

            }


        }

    }


}

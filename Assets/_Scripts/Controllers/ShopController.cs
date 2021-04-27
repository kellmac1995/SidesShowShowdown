using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopController : MonoBehaviour
{
    
    #region Price Variables & Text items
    
    public Text ticketAmount;
    
    #endregion
        

    public void Start()
    {

        GameManager.Instance.isShopping = true;

        UpdateTicketText();

    }


    public void UpdateTicketText()
    {

        //assigns the tickets text in the corner of the screen to the total tickets earned. 
        ticketAmount.text = GameManager.Instance.totalTickets.ToString("0000");
     
    }



    private void Update()
    {

        if (ShopManager.Instance.selectedItem != null)
        {
            if (Input.GetButtonDown("ButtonX") && !ShopManager.Instance.onTopShelf)
            {

                print("trying");
                TryItem();

            }
            else if (Input.GetButtonDown("ButtonA"))
            {

                print("buying");
                PurchaseItem();

            }
            else if ((Input.GetButtonDown("ButtonB") || Input.GetKeyDown(KeyCode.Escape)) && ShopManager.Instance.onTopShelf)
            {

                print("exiting top shelf");

                ShopManager.Instance.selectedItem = ShopManager.Instance.shopItems.Find(delegate (ShopItem _item)
                {

                    return _item.itemType == ShopManager.ShopItemType.Carnie;

                });

                ShopManager.Instance.UpdateItemButtons();

                ToggleButtonPanel();

            }

        }
    }


    void TryItem()
    {

        switch (ShopManager.Instance.selectedItem.itemType)
        {

            //TODO add a cool down

            case ShopManager.ShopItemType.ClownGun:
                TD2D_WeaponController.Instance.ChangeWeapon(TD2D_WeaponController.WeaponTypes.ClownGun);
                break;
            case ShopManager.ShopItemType.DragonGun:
                TD2D_WeaponController.Instance.ChangeWeapon(TD2D_WeaponController.WeaponTypes.DragonGun);
                break;
            case ShopManager.ShopItemType.BeetleBag:
                TD2D_WeaponController.Instance.ChangeWeapon(TD2D_WeaponController.WeaponTypes.BeetleBag);
                break;

            default:
                // If not any of these items do nothing!
                print("Nothing");
                break;


        }

    }


    public void PurchaseItem()
    {

        if (ShopManager.Instance.selectedItem.itemType == ShopManager.ShopItemType.Carnie)
        {

            ToggleButtonPanel();

            return;

        }
        

        if (ShopManager.Instance.selectedItem.itemPrice <= GameManager.Instance.totalTickets && !ShopManager.Instance.selectedItem.isPurchased)
        {
            
            if (ShopManager.Instance.selectedItem.isAbility)
            {

                GameManager.Instance.UpdateTotalTickets(-(int)(ShopManager.Instance.selectedItem.itemPrice * ShopManager.Instance.itemPriceModifier));

                ShopManager.Instance.abilitiesPurchased++;

                //PlayerPrefs.SetInt("abilitiesPurchased", abilitiesPurchased);

                ShopManager.Instance.CalculateModifier();

                ShopManager.Instance.selectedItem.shopItemObj.abilityGUI.SetActive(true);

            }
            else
            {

                GameManager.Instance.UpdateTotalTickets(-ShopManager.Instance.selectedItem.itemPrice);

            }

            ShopManager.Instance.selectedItem.isPurchased = true;

            AudioManager.Instance.purchaseSuccessEv.start();

            print(ShopManager.Instance.selectedItem + " purchsed");

            ShopManager.Instance.selectedItem.UpdateItemText();

            UpdateTicketText();

            ShopManager.Instance.SaveShopValues();
        }
        else
        {

            // Unable to buy play animation and sounds here.
            ShopManager.Instance.selectedItem.shopItemObj.itemAnimator.SetTrigger("denied");
            AudioManager.Instance.purchaseDeniedEv.start();
            print("not enough money");

        }


    }



    void ToggleButtonPanel()
    {


        ShopManager.Instance.buttonEvents.SetActive(!ShopManager.Instance.buttonEvents.activeSelf);
        ShopManager.Instance.onTopShelf = ShopManager.Instance.buttonEvents.activeSelf;
        TD2D_PlayerController.Instance.isMovementLocked = ShopManager.Instance.onTopShelf;


    }


}
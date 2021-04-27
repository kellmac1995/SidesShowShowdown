using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopButton : MonoBehaviour, ISelectHandler// required interface when using the OnSelect method.
{
    
    //Do this when the selectable UI object is selected.
    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(gameObject.name + " was selected");

        ShopManager.Instance.selectedItem = ShopManager.Instance.shopItems.Find(delegate (ShopItem _item)
        {

            return _item.itemType == GetComponent<ShopItemObject>().itemObjType;

        });


        ShopManager.Instance.UpdateItemButtons();

    }


}

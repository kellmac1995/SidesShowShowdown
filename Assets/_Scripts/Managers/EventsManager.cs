using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : GenericSingletonClass<EventsManager>
{
    [Space]
    [Header("-Events-")]
    public PopupEventSO comboEvent;
    public PopupEventSO hammerHitEvent;
    public PopupEventSO changeWeaponEvent;
    public PopupEventSO useAbilityEvent;
    public PopupEventSO normalEvent;
    public PopupEventSO aimEvent;
    public PopupEventSO endroundTauntEvent;
    public PopupEventSO tauntEvent2;
    public PopupEventSO tauntEvent3;
    public PopupEventSO sadEvent;
    public PopupEventSO dadEvent;
    public PopupEventSO mumEvent;
    public PopupEventSO sisEvent;
    public PopupEventSO babyEvent;



    [Space]
    [Header("-Settings-")]
    public Animator characterAnimator;
    public TMPro.TextMeshProUGUI textPopupTMPro;


    public bool characterVisible = false;

    public bool canOverride = true;


    //private void Start()
    //{

    //}



    public void PopupEvent(PopupEventSO eventSO)
    {


        if (eventSO.overrideAll)
        {
            StopAllCoroutines();
            canOverride = false;
            StartCoroutine(DoPopupEvent(eventSO));
        }

        if (canOverride)
        {

            StartCoroutine(DoPopupEvent(eventSO));

        }
        
        

    }





    IEnumerator DoPopupEvent(PopupEventSO eventSO)
    {

        if (eventSO.overrideAll)
        {

            characterAnimator.SetBool("Idle", true);

        }

        yield return new WaitUntil(() => characterVisible == false);

        characterVisible = true;

        // Start the animation for the event
        characterAnimator.SetBool(eventSO.animStartName, true);


        if (eventSO.textboxCaptions.Length > 0)
        {
            textPopupTMPro.text = eventSO.textboxCaptions[Random.Range(0, eventSO.textboxCaptions.Length - 1)];
        }
        else
        {
            textPopupTMPro.text = "...";
        }


        if (eventSO.overrideAll)
        {
            yield return new WaitUntil(() => characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));

            canOverride = true;

        }

    }
        
}

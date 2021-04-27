using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{

    public GameObject UISlot;

    public UnityEngine.UI.Image cooldownFill;

    public Animator coolDownAnimation;

    public float coolDown = 10;

    protected float currCoolDown;

    public bool onCoolDown = false;

    public bool locked = false;

    public float startTime = 3;

    protected float currStartTime;

    protected bool starting = true;

    // Start is called before the first frame update
    public virtual void Start()
    {

        cooldownFill.fillAmount = 1;
        startTime = GameManager.Instance.startCountDown;
        starting = true;

    }

    // Update is called once per frame
    public virtual void Update()
    {

        if (starting)
        {
                currStartTime += Time.deltaTime;

                cooldownFill.fillAmount = 1 - (currStartTime / startTime);

                if (currStartTime >= startTime)
                {

                    starting = false;
                    DisableCooldown();

                }
        }


        if (onCoolDown && !locked)
        {
            currCoolDown += Time.deltaTime;

            cooldownFill.fillAmount = 1 - (currCoolDown / coolDown);

            if (currCoolDown >= coolDown)
            {


                DisableCooldown();


            }
        }



    }



    public virtual void EnableCooldown()
    {

        currCoolDown = 0;
        cooldownFill.fillAmount = 1;
        coolDownAnimation.gameObject.SetActive(false);
        onCoolDown = true;

    }


    public virtual void DisableCooldown()
    {

        onCoolDown = false;
        coolDownAnimation.gameObject.SetActive(true);

    }


}

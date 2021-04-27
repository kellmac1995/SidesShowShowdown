using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCarnie : MonoBehaviour
{

    public void DisableTickets()
    {

        GetComponent<Animator>().SetBool("carneyTickets", false);

    }

    public void DisableHappy()
    {

        GetComponent<Animator>().SetBool("carneyHappy", false);

    }


    public void DisableAngry()
    {

        GetComponent<Animator>().SetBool("carneyAngry", false);

    }

}

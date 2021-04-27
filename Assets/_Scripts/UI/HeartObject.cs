using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartObject : MonoBehaviour
{

    public Sprite[] sprites;
    public Image imageContainer;
    //public Image sheildImage;
    public Animator anim;

    public bool isSheilded = false;


    // Start is called before the first frame update
    void Start()
    {

        //imageContainer = GetComponent<Image>();
        anim = GetComponent<Animator>();

    }

    public void PopHeart()
    {

        //imageContainer.enabled = false;

        if (null != anim)
        {
            // play Bounce but start at a quarter of the way though
            anim.Play("Heart_Burst", 0, 0);
        }

    }

    public void PopShield()
    {

       /// imageContainer.sprite = sprites[0];
        anim.Play("HeartRemoveShield", 0, 0);
        isSheilded = false;
        //popAnimation.Play();

    }


    public void AddHeart()
    {

        //imageContainer.sprite = sprites[0];
        //imageContainer.enabled = true;
        anim.Play("HeartSpawn", 0, 0);
        //popAnimation.Play(); // Do another animation here for heart.

    }


    public void AddShield()
    {

        //imageContainer.sprite = sprites[1];
        //popAnimation.Play(); // Do another animation here for sheild.
        anim.Play("HeartAddShield", 0, 0);
        isSheilded = true;

    }




}

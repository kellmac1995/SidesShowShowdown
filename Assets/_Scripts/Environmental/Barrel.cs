using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{

    public WaterStreamPivot waterStream;
    public GameObject pop;

    [Tooltip("If the barrel is randomly activated.")]
    public bool randomizeActivate = false;

    [Tooltip("The chance the barrel will activate when repeat is triggered.")]
    [Range(1,100)]
    public int activationChance = 10;

    //public bool barrelPop;

    // Time taken to repeat in seconds
    [Tooltip("The time in seconds the barrel tries to activate.")]
    [Range(1, 30)]
    public float repeatRate = 2;
    
    public Sprite barrelClosed, barrelOpen;
    private SpriteRenderer spriteRenderer;

    

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        waterStream = GetComponentInChildren<WaterStreamPivot>();

        spriteRenderer.sprite = barrelClosed;

        if (randomizeActivate)
            StartCoroutine(Randomizer());

    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("PlayerBullet") && !waterStream.active)
        {

            ActivateBarrel();

        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("PlayerBullet") && !waterStream.active)
        {

            ActivateBarrel();

        }
    }



    public void ActivateBarrel()
    {

        AudioManager.Instance.corkPopEv.start(); 
        waterStream.ActivateStream();
        spriteRenderer.sprite = barrelOpen;
        StartCoroutine(popEffect());
    }


    private IEnumerator Randomizer()
    {
        while (true)
        {
            yield return new WaitForSeconds(repeatRate);
            
            if (Random.value <= activationChance / 100 && !waterStream.active)
            {
                ActivateBarrel();
            }
        }
    }

    private IEnumerator popEffect()
    {
        pop.SetActive(true);
        yield return new WaitForSeconds(1);
        pop.SetActive(false);
    }

}

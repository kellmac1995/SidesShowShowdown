using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMime : MonoBehaviour
{
    private Sprite defaultSprite;
    public Sprite muzzleFlash;

    public int framesToFlash = 3;
    public float destroyTime = 10;

    private SpriteRenderer spriteRend;

    // Use this for initialization
    void Start()
    {
        TrailRenderer line = GetComponent<TrailRenderer>();
        line.sortingLayerName = "Foreground";

        spriteRend = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRend.sprite;

        StartCoroutine(FlashMuzzleFlash());
        StartCoroutine(FlashDestruction());
    }

    IEnumerator FlashMuzzleFlash()
    {
        spriteRend.sprite = muzzleFlash;

        for (int i = 0; i < framesToFlash; i++)
        {
            yield return 0;
        }

        spriteRend.sprite = defaultSprite;
    }

    IEnumerator FlashDestruction()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            TD2D_PlayerController.Instance.PlayerHealth.TakeDamage(1);
            //print(collision.gameObject.name);
            Destroy(gameObject);
        }

        else
        {

            Destroy(gameObject);
        }
    }

}

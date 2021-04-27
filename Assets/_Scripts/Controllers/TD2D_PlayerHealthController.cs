using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD2D_PlayerHealthController : MonoBehaviour
{

    public bool isDead = false;

    [HideInInspector]
    public int maxHealth;

    public int startingHealth = 3;                            // The amount of health the player starts the game with.
    public int currentHealth;

    public float takeDamageCoolDown = 0.5f;
    public bool onCoolDown;

    public int maxHearts = 3;
    //public int healthPerHeart = 1;

    public Color healthFlashColor;
    public Color sheildFlashColor;


    //public float freezeOnDamageTime = 0.1f;

    public float slowdownAmount = 0.2f;

    public float slowdownTime = 0.5f;
    public float speedupTime = 0.5f;
    

    public GameObject playerShield;
    public GameObject shieldSplatter;

    public bool sheildActive = false;

    public HeartObject[] hearts;



    // Start is called before the first frame update
    private void Start()
    {

        currentHealth = startingHealth;

        //maxHealth = maxHearts * healthPerHeart;

    }
    


    public void TakeDamage(int amount)
    {


        if (onCoolDown)
        {
            return;
        }
        

        // if heart is sheilded then remove sheild
        // else
        // remove health and pop heart.

        if (sheildActive)
        {

            for (int i = hearts.Length; i > 0; i--)
            {

                if (hearts[i - 1].isSheilded)
                {

                    hearts[i - 1].PopShield();

                    UIManager.Instance.playerDamagedFlash.color = sheildFlashColor;
                    UIManager.Instance.playerDamagedFlash.gameObject.SetActive(true);
                    //Instantiate(shieldSplatter, transform.position, transform.rotation);

                    GameManager.Instance.timeManager.Slowmotion(slowdownAmount, slowdownTime, speedupTime);

                    if (i == 1)
                    {

                        DeactivateShield();
                        

                    }

                    // play sheild dmaged sound
                    break;

                }

            }
        }
        else
        {

            if (currentHealth == 0)
                return;

            hearts[currentHealth- 1].PopHeart();

            // Reduce the current health by the damage amount.
            currentHealth -= amount;

            UIManager.Instance.playerDamagedFlash.color = healthFlashColor;
            UIManager.Instance.playerDamagedFlash.gameObject.SetActive(true);

            //GameManager.Instance.freezer.Freeze(freezeOnDamageTime);

            GameManager.Instance.timeManager.Slowmotion(slowdownAmount, slowdownTime, speedupTime);

            //GameManager.Instance.timeManager.InstantSlowmotion(0.5f, 1.5f);

            // Play the hurt sound effect.
            AudioManager.Instance.playerDamagedEv.start();

        }

        onCoolDown = true;
        StartCoroutine(DisableCooldown());

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Death();
        }

    }





    IEnumerator DisableCooldown()
    {

        yield return new WaitForSeconds(takeDamageCoolDown);

        onCoolDown = false;

    }


    public void AddHeart(int amount)
    {

        if (currentHealth == startingHealth)
            return;

        currentHealth += amount;

        hearts[currentHealth - 1].AddHeart();

        if (sheildActive)
            hearts[currentHealth - 1].AddShield();

    }


    public void ActivateShield()
    {

        for (int i = 0; i < hearts.Length; i++)
        {

            if (hearts[i].imageContainer.enabled == true)
                hearts[i].AddShield();

        }

        playerShield.SetActive(true);
        sheildActive = true;

    }



    public void DeactivateShield()
    {

        for (int i = 0; i < hearts.Length; i++)
        {

            if (hearts[i].isSheilded)
                hearts[i].PopShield();

        }

        ObjectPooler.Instance.Spawn(shieldSplatter, transform.position, Quaternion.identity);

        playerShield.SetActive(false);
        sheildActive = false;

    }



    public void Death()
    {


        isDead = true;
        GameManager.Instance.EndRound();
        //TODO FIX EVENT
        AudioManager.Instance.playerDeathEv.start();
        // Death animation here

    }


}

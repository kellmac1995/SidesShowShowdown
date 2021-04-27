using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    public enum FacingDirection { Left, Right } //, Front}

    public FacingDirection direction;

    public Sprite doorLeftOpen; //, doorLeftClosed, doorFrontClosed;
    public Sprite doorRightOpen; //, doorRightClosed, doorFrontOpen;

    private SpriteRenderer spriteRenderer;

    public GameObject zap;
    public GameObject bubblegum;

    public Transform doorSpawn;

    public Teleporter targetTeleporter;

    [HideInInspector]
    public bool isTeleporting = false;

    public ParticleSystem shinyLines;
    public ParticleSystem doorActive;
    public ParticleSystem doorInactive;

    public float teleportTime = 2f;


    public bool onCoolDown = false;

    public float doorCoolDown = 15;


    private void Awake()
    {

        if (!spriteRenderer)
        {

            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        }

        if (direction == FacingDirection.Left)
        {
            spriteRenderer.sprite = doorLeftOpen;
        }
        else if (direction == FacingDirection.Right)
        {
            spriteRenderer.sprite = doorRightOpen;
        }

    }


    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && !isTeleporting && !onCoolDown)
        {
            //Debug.Log("Teleporting Player");

            isTeleporting = true;
            onCoolDown = true;
            targetTeleporter.onCoolDown = true;

            
            StartCoroutine(DoTeleport());
            StartCoroutine(ResetCooldown());
        }

    }

    private IEnumerator DoTeleport()
    {
        GameManager.Instance.cameraZoom.ZoomOutZoomIn(18, 0.5f, 1f, 0.5f);
        StartCoroutine(zapEffect());
        AudioManager.Instance.doorOpenEv.start();
        TD2D_PlayerController.Instance.TogglePlayerVisuals();

        TD2D_PlayerController.Instance.LockPlayer();
        
        AbilitiesController.Instance.jokeBook.GetComponent<Ability_JokeBook>().StunEnemies(teleportTime);

        shinyLines.Play();
        targetTeleporter.shinyLines.Play();

        yield return new WaitForSeconds(teleportTime / 2); // half the total time to teleport as there are two waits.

        TD2D_PlayerController.Instance.Teleport(targetTeleporter.doorSpawn.position);
        
        yield return new WaitForSeconds(teleportTime / 2);

        shinyLines.Stop();
        targetTeleporter.shinyLines.Stop();

        //shinyLines.Play();
        // TODO play shiny lines on other door
        TD2D_PlayerController.Instance.TogglePlayerVisuals();
        TD2D_PlayerController.Instance.UnlockPlayer();

        targetTeleporter.isTeleporting = false;
        isTeleporting = false;
        doorInactive.Play();
        targetTeleporter.doorInactive.Play();
        doorActive.Stop();
        targetTeleporter.doorActive.Stop();


    }


    private IEnumerator ResetCooldown()
    {

        yield return new WaitForSeconds(doorCoolDown);

        onCoolDown = false;
        targetTeleporter.onCoolDown = false;

        targetTeleporter.doorInactive.Stop();
        doorInactive.Stop();

        targetTeleporter.doorActive.Play();
        doorActive.Play();
    }


    private IEnumerator zapEffect()
    {
        zap.SetActive(true);
        yield return new WaitForSeconds(1);
        zap.SetActive(false);
    }

}

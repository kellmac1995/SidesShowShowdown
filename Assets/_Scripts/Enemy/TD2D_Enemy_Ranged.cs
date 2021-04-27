using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD2D_Enemy_Ranged : MonoBehaviour
{

    // enum used for the facing direction
    public enum EnemyDirection { Up, Left, Down, Right };

    // stores the direction the player is facing
    public EnemyDirection facingDirection;

    // The sprites used by the player transofrm
    [Header("Loaded")]
    public Sprite loadedSprite;
    //public Sprite rightSprite, upSprite, downSprite;

    // The sprites used by the player transofrm
    [Header("Fired")]
    public Sprite firedSprite;
    //public Sprite rightESprite, upESprite, downESprite;

    //[Header("Anchors")]
    //public Transform leftAnchor;
    //public Transform rightAnchor, upAnchor, downAnchor;

    //public Transform reticlePoint;

    public Transform targetReticle;

    public GameObject bulletPrefab;

    public float bulletSpeed;

    bool loaded = true;

    [HideInInspector]
    public SpriteRenderer visualsRenderer;



    private void Start()
    {

        visualsRenderer = GetComponentInChildren<SpriteRenderer>();
        
        if (visualsRenderer != null)
            UpdateVisuals();

    }


    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Shoot();
        //    print("pew");
        //}

        //Vector3 aimPosition = new Vector3(reticle.position.x, reticle.position.y, visualsRenderer.transform.position.z);
        Vector3 aimPosition = new Vector3(TD2D_PlayerController.Instance.transform.position.x, TD2D_PlayerController.Instance.transform.position.y, visualsRenderer.transform.position.z);
        visualsRenderer.transform.up = aimPosition - visualsRenderer.transform.position;

    }

    void UpdateVisuals()
    {

        if (loaded)
        {
            switch (facingDirection)
            {
                case EnemyDirection.Right:
                    //visualsRenderer.sprite = rightSprite;
                    break;
                case EnemyDirection.Left:
                    //visualsRenderer.sprite = leftSprite;
                    break;
                case EnemyDirection.Up:
                    //visualsRenderer.sprite = upSprite;
                    break;
                case EnemyDirection.Down:
                    //visualsRenderer.sprite = downSprite;
                    break;
                default:
                    Debug.Log("Error with direction case for enemy");
                    break;
            }
            
        }
        else
        {
            switch (facingDirection)
            {
                case EnemyDirection.Right:
                    //visualsRenderer.sprite = rightESprite;
                    break;
                case EnemyDirection.Left:
                    //visualsRenderer.sprite = leftESprite;
                    break;
                case EnemyDirection.Up:
                    //visualsRenderer.sprite = upESprite;
                    break;
                case EnemyDirection.Down:
                    //visualsRenderer.sprite = downESprite;
                    break;
                default:
                    Debug.Log("Error with direction case for enemy");
                    break;
            }
        }

    }


    public void Shoot()
    {


        //aimingComp.firing = true;

        AudioManager.Instance.cannonFireEv.start();

        Vector3 aimDirection = -(transform.position - targetReticle.position);

        var bullet = ObjectPooler.Instance.Spawn(bulletPrefab, transform.position, Quaternion.identity);

        //bullet.LookAt(reticle.position);

        Vector3 aimPosition = new Vector3(targetReticle.position.x, targetReticle.position.y, transform.position.z);

        bullet.transform.up = aimPosition - transform.position;

        bullet.GetComponent<Rigidbody2D>().velocity = aimDirection.normalized * bulletSpeed;

        bullet.GetComponent<BulletRanged>().target = targetReticle;



    }





}

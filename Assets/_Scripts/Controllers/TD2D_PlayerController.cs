using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Player controller
/// </summary>
public class TD2D_PlayerController : GenericSingletonClass<TD2D_PlayerController>
{
    public float footstep;

    #region StickAxisNames Struct
    // Struct for storing information for gamepad controller.
    [System.Serializable]
    public struct StickAxisNames
    {
        public string Horizontal, Vertical;

        //Create a vector to handle movement
        public Vector2 Gather(bool mouseEnabled)
        {

            float h = Input.GetAxis(Horizontal);
            float v = Input.GetAxis(Vertical);

            Vector2 hv = new Vector2(h, v);

            if (!mouseEnabled)
            {

                float hvmag = hv.magnitude;
                if (hvmag > 1)
                {
                    hv.Normalize();
                }
                else if (Mathf.Approximately(hvmag, 0)) // TODO Should probably change check to a deadzone instead of apporox
                {
                    return Vector2.zero;
                }

            }

            return hv;
        }

        public bool CheckForInput()
        {

            bool on;

            if (Input.GetAxisRaw(Horizontal) != 0)
            {
                on = true;
            }
            else if (Input.GetAxis(Vertical) != 0)
            {
                on = true;
            }
            else
            {
                on = false;
            }

            return on;

        }

    }
    #endregion

    #region Populate Params Function
    /// <summary>
    /// Use this function if there are not parameters in the animator fields
    /// </summary>
    [ContextMenu("Populate Paramenters")]
    void PopulateParamenters()
    {
        if (parameters == null || PlayerAnimator.parameters.Length > 0)
        {
            parameters = new string[PlayerAnimator.parameters.Length];

            int index = 0;

            foreach (AnimatorControllerParameter parameter in PlayerAnimator.parameters) //.Where(n => n.type == AnimatorControllerParameterType.Trigger))
            {
                parameters[index] = parameter.name;
                index++;
            }

        }
    }
    #endregion

    #region Public Properties
    private Animator _playerAnimator;

    /// <summary>
    /// The Animator on the player object;
    /// </summary>
    public Animator PlayerAnimator
    {
        get
        {
            if (_playerAnimator == null)
            {

                _playerAnimator = GetComponent<Animator>();

                if (_playerAnimator == null)
                {
                    Debug.LogError("Please check the player has an animator component attached");
                }
            }

            return _playerAnimator;

        }
    }


    private SpriteRenderer _playerRenderer;

    /// <summary>
    /// The Animator on the player object;
    /// </summary>
    public SpriteRenderer PlayerRenderer
    {
        get
        {
            if (_playerRenderer == null)
            {

                _playerRenderer = GetComponent<SpriteRenderer>();

                if (_playerRenderer == null)
                {
                    Debug.LogError("Please check the player has a renderer component attached");
                }
            }

            return _playerRenderer;

        }
    }


    private TD2D_ReticleController _reticleCtrl;

    /// <summary>
    /// The cursor gameobject in the scene
    /// </summary>
    public TD2D_ReticleController ReticleCtrl
    {
        get
        {
            if (_reticleCtrl == null)
            {
                _reticleCtrl = FindObjectOfType<TD2D_ReticleController>();

                if (_reticleCtrl == null)
                {
                    Debug.LogError("Please check there is a reticle controller on the player aimer");
                }

            }

            return _reticleCtrl;

        }

    }


    private Transform _target;

    /// <summary>
    /// The cursor gameobject in the scene
    /// </summary>
    public Transform Target
    {
        get
        {
            if (_target == null)
            {
                _target = ReticleCtrl.CurrentReticle.transform;

                if (_target == null)
                {
                    Debug.LogError("Please check the player has a reticle");
                }

            }

            return _target;

        }

    }



    private TD2D_PlayerHealthController _playerHealth;

    /// <summary>
    /// The Animator on the player object;
    /// </summary>
    public TD2D_PlayerHealthController PlayerHealth
    {
        get
        {
            if (_playerHealth == null)
            {

                _playerHealth = GetComponent<TD2D_PlayerHealthController>();

                if (_playerHealth == null)
                {
                    Debug.LogError("Please check the player has an health component attached");
                }
            }

            return _playerHealth;

        }
    }


    #endregion

    #region Public Variables

    //public GameObject mothersGaze;
    // enum used for the facing direction
    public enum PlayerDirection { Up, Left, Down, Right };

    // stores the direction the player is facing
    public PlayerDirection facingDirection = PlayerDirection.Down;

    // detect an object or the mouse
    //public bool useReticle;

    // The target the player will be looking at (Reticle/Mouse)
    public Transform target;

    public Transform aimPivot;

    // Using sprites or animator
    //public bool usingAnimator = true;

    public bool isAttacking = false;
    
    public bool isMovementLocked = false;
    public bool isWeaponLocked = false;


    // The position of the hand
    public Transform leftHand, rightHand;//, upHand, downHand;

    // The index of the trigger used in the animator
    public int leftTrigger, rightTrigger, upTrigger, downTrigger;

    public float dirDetectionDeadzone = 0.15f;

    public float maxHealth = 100;
    public float maxSpeed = 13;
    public GameObject splatter;
    public float currentSpeed = 0;

    // Controller  Stuff
    //public float TargetMovementSmoothing = 0.05f;
    //public float StartingMovementSmoothing = 1f;

    //public float acceleration = 1;
    //public float deceleration = 1;


    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

    //private float m_AccelerationStep = .1f;
    //private float m_DecelerationStep = .1f;

    // starting value for the Lerp
    //static float at = 0.0f;
    //static float dt = 0.0f;

    //private bool accelerating = false;
    //private bool decelerating = false;
    //private bool atSpeed = false;

    private Vector3 m_Velocity = Vector3.zero;


    //public bool normalizedMovement = false;

    //public Transform playerHand;

    public Transform leftHitBoxPos, rightHitBoxPos, upHitBoxPos, downHitBoxPos;

    public string[] parameters;

    public bool inAir = false;

    public bool beingPushed = false;

    public bool enableMouse = false;

    public StickAxisNames leftStick, rightStick;

    //public float jumpTime = 0.5f;

    public bool canMove = true;

    public bool canRotate = true;


    #endregion

    #region Private Variables

    // Used to store the targets/mouse position
    //private float targetX, targetY;

    // Used when calculating the difference between the targets pos and the players pos
    //private float xDifference, yDifference;

    private PlayerDirection previousDirection;

    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;

    //[HideInInspector]
    //public Vector2 moveVelocity;

    private Vector2 moveInput = Vector2.zero;

    float coolDownTimer = 0f;

    #endregion
    
        
    public override void Awake()
    {

        base.Awake();
        PopulateParamenters();

    }


    private void Start()
    {

        // Sets components
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        currentSpeed = maxSpeed;

        facingDirection = PlayerDirection.Down;


        if (GameManager.Instance.controllerActive)
        {
            enableMouse = false;
        }
        else
        {

            enableMouse = true;

        }

    }


    private void Update()
    {

        if (coolDownTimer > 0)
        {
            coolDownTimer -= Time.deltaTime;

            if (coolDownTimer <= 0)
            {
                UnlockPlayer();
            }

        }


        if (inAir || beingPushed || isMovementLocked)
        {
            return;
        }


        if (!isAttacking) //&& !isShopping)  //&& canRotate// if not attacking change player direction
        {
            DeterminePlayerDirection();
        }


        // Handles the current inputs.
        HandleInput();


    }



    private void FixedUpdate()
    {

        if (inAir || beingPushed || isMovementLocked)
        {
            return;
        }

        Move();

    }


    /// <summary>
    /// 
    /// </summary>
    private void HandleInput()
    {

        // If not jumping then get the input details
        if (!inAir)
            moveInput = leftStick.Gather(enableMouse);


        if (!enableMouse)
        {
            //Aim the reticle controller in the direction of the right stick
            Vector2 rightStickRes = rightStick.Gather(enableMouse);
            if (rightStickRes.magnitude > 0)
            {
                ReticleCtrl.RotatePivot(rightStickRes);
            }
        }
        else
        {
            ReticleCtrl.RotatePivot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            ////Aim the reticle controller in the direction of the right stick
            //Vector2 rightStickRes = GatherMouseAxis();

            //if (rightStickRes.magnitude > 0)
            //{
            //    ReticleCtrl.RotatePivot(rightStickRes);
            //}
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Vector3 CalculateVelocity()
    {

        return moveInput;

        #region Unused


        //moveVelocity = moveInput * currentSpeed;

        //endPos = rb.position + moveVelocity * Time.fixedDeltaTime;

        //bool horizOn = Input.GetAxisRaw("Horizontal") != 0 ? true : false;
        //bool vertOn = Input.GetAxisRaw("Vertical") != 0 ?  true : false;


        //if (horizOn || vertOn)
        //{



        //}
        //else if (!isLerping)
        //{

        //    isLerping = true;
        //    slowDownSpeed = currentSpeed;

        //}else if (isLerping)
        //{

        //    slowDownSpeed = Mathf.Lerp(currentSpeed, 0, slowDownStep);

        //    moveVelocity = moveInput * slowDownSpeed;

        //    if (slowDownSpeed == 0)
        //    {
        //        isLerping = false;
        //    }

        //}



        //endPos = Vector3.SmoothDamp(transform.position, endPos, ref velocity, smoothTime);

        //endPos = rb.position + moveVelocity * Time.fixedDeltaTime;

        #endregion

    }


    /// <summary>
    /// Move the player transform using targetVelocity (moveinput * currentSpeed) gathered from HandleInput()
    /// </summary>
    private void Move()
    {


        Vector3 targetVelocity = moveInput * currentSpeed;

        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);


        // Sets the speed using the magnitude of the axis
        PlayerAnimator.SetFloat("Speed", moveInput.magnitude);


        #region Unused

        //rb.AddForce(targetVelocity);

        //// Move the rigidbody
        //rb.MovePosition(endPos);


        //if (!accelerating && !atSpeed && leftStick.CheckForInput())
        //{
        //    accelerating = true;
        //    decelerating = false;
        //}
        //else if (accelerating && !leftStick.CheckForInput())
        //{

        //    at = 0;
        //    dt = 0;
        //    accelerating = false;
        //    decelerating = true;

        //}

        //if (accelerating)
        //{

        //    at += m_AccelerationStep * Time.fixedDeltaTime;
        //    m_MovementSmoothing = Mathf.Lerp(m_MovementSmoothing, TargetMovementSmoothing, at);
        //    if (Mathf.Approximately(m_MovementSmoothing, TargetMovementSmoothing))
        //    {

        //        m_MovementSmoothing = TargetMovementSmoothing;
        //        at = 0;
        //        accelerating = false;
        //        atSpeed = true;

        //    }
        //}
        //else if (atSpeed && !leftStick.CheckForInput())
        //{

        //    decelerating = true;
        //    atSpeed = false;
        //    dt = 0;

        //}

        //if (decelerating)
        //{

        //    dt += m_DecelerationStep * Time.fixedDeltaTime;
        //    m_MovementSmoothing = Mathf.Lerp(m_MovementSmoothing, StartingMovementSmoothing, dt);

        //    if (Mathf.Approximately(m_MovementSmoothing,  StartingMovementSmoothing))
        //    {

        //        m_MovementSmoothing = StartingMovementSmoothing;
        //        decelerating = false;

        //    }

        //}



        //if (!accelerating && !atSpeed && leftStick.CheckForInput())
        //{
        //    accelerating = true;
        //    decelerating = false;
        //}
        //else if (accelerating && !leftStick.CheckForInput())
        //{

        //    at = 0;
        //    dt = 0;
        //    accelerating = false;
        //    decelerating = true;

        //}

        //if (accelerating)
        //{

        //    at += m_AccelerationStep * Time.fixedDeltaTime;
        //    currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, at);
        //    if (Mathf.Approximately(currentSpeed, maxSpeed))
        //    {

        //        currentSpeed = maxSpeed;
        //        at = 0;
        //        accelerating = false;
        //        atSpeed = true;

        //    }
        //}
        //else if (atSpeed && !leftStick.CheckForInput())
        //{

        //    decelerating = true;
        //    atSpeed = false;
        //    dt = 0;

        //}

        //if (decelerating)
        //{

        //    dt += m_DecelerationStep * Time.fixedDeltaTime;
        //    currentSpeed = Mathf.Lerp(currentSpeed, 0, dt);

        //    if (Mathf.Approximately(0, currentSpeed))
        //    {

        //        currentSpeed = 0;
        //        decelerating = false;

        //    }

        //}



        //rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

        //if (moveVelocity != Vector2.zero)
        //{
        //    //endPos = rb.position + moveVelocity * Time.fixedDeltaTime;

        //}


        //isLerping = Vector3.Distance(transform.position, endPos) <= stoppingDistance ? false : true;

        //if (isLerping)
        //{

        //    Vector3 movePosition = Vector3.Slerp(transform.position, endPos * stoppingDistance, stoppingStep * Time.fixedDeltaTime);
        //    //rb.MovePosition(movePosition);
        //    rb.MovePosition(movePosition);

        //}



        //MoveToPosition(rb.position + moveVelocity * Time.fixedDeltaTime); 
        #endregion


    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="_time"></param>
    public void EnableMovementCoolDown(float _time)
    {

        coolDownTimer = _time;
        LockPlayer();

    }


    /// <summary>
    /// Stops the player from moving and shooting.
    /// </summary>
    public void LockPlayer()
    {

        canMove = false;
        isMovementLocked = true;
        isWeaponLocked = true;

    }


    /// <summary>
    /// Disables LockPLayer().
    /// </summary>
    public void UnlockPlayer()
    {

        canMove = true;
        isMovementLocked = false;
        isWeaponLocked = false;

    }


    public void TogglePlayerVisuals()
    {

        ObjectPooler.Instance.Spawn(GameManager.Instance.despawnPoof, transform.position, Quaternion.identity);

        PlayerRenderer.enabled = !PlayerRenderer.enabled;

        if (PlayerHealth.sheildActive)
        {
            PlayerHealth.playerShield.SetActive(!PlayerHealth.playerShield.activeSelf);
        }

        ReticleCtrl.ToggleReticle();

        foreach (CapsuleCollider2D coll in GetComponents<CapsuleCollider2D>())
        {
            coll.isTrigger = !coll.isTrigger;
        }

        //leftHand.gameObject.SetActive(false);
        //rightHand.gameObject.SetActive(false);

    }


    /// <summary>
    /// Checks the location of the target compared to the player and sets the players direction.
    /// </summary>
    public void DeterminePlayerDirection()
    {

        float xVal, yVal;


        xVal = Target.position.x - transform.position.x;
        yVal = Target.position.y - transform.position.y;


        if (Mathf.Abs(xVal) > Mathf.Abs(yVal))
        {
            if (Mathf.Abs(xVal) < dirDetectionDeadzone)
                return;

            if (xVal > 0)
            {

                facingDirection = PlayerDirection.Right;
                //print("right");

            }
            else if (xVal < 0)
            {

                facingDirection = PlayerDirection.Left;
                //print("left");
            }
        }
        else
        {

            if (Mathf.Abs(yVal) < dirDetectionDeadzone)
                return;

            if (yVal > 0)
            {
                facingDirection = PlayerDirection.Up;
                //print("up");
            }
            else if (yVal < 0)
            {

                facingDirection = PlayerDirection.Down;
                //print("down");
            }
        }


        if (facingDirection != previousDirection)
        {
            previousDirection = facingDirection;
            UpdatePlayerVisual();
        }


        #region Unused
        //targetX = target.position.x;
        //targetY = target.position.y;


        //xDifference = Mathf.Abs(targetX - transform.position.x);
        //yDifference = Mathf.Abs(targetY - transform.position.y);


        //{

        //    if (xDifference > yDifference)
        //    {
        //        if (targetX > transform.position.x)
        //        {
        //            facingDirection = PlayerDirection.Right;
        //        }
        //        else if (targetX < transform.position.x)
        //        {
        //            facingDirection = PlayerDirection.Left;
        //        }

        //    }
        //    else if (xDifference < yDifference)
        //    {
        //        if (targetY > transform.position.y)
        //        {
        //            facingDirection = PlayerDirection.Up;
        //        }
        //        else if (targetY < transform.position.y)
        //        {
        //            facingDirection = PlayerDirection.Down;
        //        }
        //    }

        //}

        //if (facingDirection != previousDirection)
        //{
        //    previousDirection = facingDirection;
        //    UpdatePlayerVisual();
        //}

        #endregion


    }


    /// <summary>
    /// Sets the trigger for the animator or changes to suit the direction the player is facing.
    /// </summary>
    public void UpdatePlayerVisual()
    {

        if (facingDirection == PlayerDirection.Right)
        {
            PlayerAnimator.SetTrigger(PlayerAnimator.parameters[rightTrigger].name);
        }
        else if (facingDirection == PlayerDirection.Left)
        {
            PlayerAnimator.SetTrigger(PlayerAnimator.parameters[leftTrigger].name);
        }
        else if (facingDirection == PlayerDirection.Up)
        {
            PlayerAnimator.SetTrigger(PlayerAnimator.parameters[upTrigger].name);
        }
        else if (facingDirection == PlayerDirection.Down)
        {
            PlayerAnimator.SetTrigger(PlayerAnimator.parameters[downTrigger].name);
        }

    }


    public void Teleport(Vector3 targetPosition)
    {

        rb.velocity = Vector2.zero;
        transform.position = targetPosition;

    }


    public void Jump(Vector3 _velocity, float _jumpTime)
    {

        inAir = true;


        foreach (CapsuleCollider2D coll in GetComponents<CapsuleCollider2D>())
        {
            coll.isTrigger = true;
        }


        // trigger animator for jumping here (Scale Up/Down)
        PlayerAnimator.ResetTrigger("isSprung");
        PlayerAnimator.SetTrigger("isSprung");


        rb.velocity = _velocity;


        // TODO Run Land at the end of the jump animation instead of invoke
        Invoke("Land", _jumpTime);

    }


    private void Land()
    {

        //currentSpeed = maxSpeed;

        rb.velocity = Vector2.zero;

        UpdatePlayerVisual();

        foreach (CapsuleCollider2D coll in GetComponents<CapsuleCollider2D>())
        {
            coll.isTrigger = false;
        }

        Teleport(transform.position);

        inAir = false;

    }





    ////TODO move to walls
    private void OnTriggerEnter2D(Collider2D collision)
    {


        if ((collision.gameObject.tag == "Wall" && inAir))
        {
            PlayerAnimator.ResetTrigger("isSprung");
            Land();
        }
        else if (collision.CompareTag("Entrance"))
        {

            AudioManager.Instance.shopTrackEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            AudioManager.Instance.backgroundTrackEv.start();
            GameSceneManager.LoadScene(GameSceneManager.Scenes.Arena);

        }



    }


}




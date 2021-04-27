using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UI;

public class TD2D_WeaponController : GenericSingletonClass<TD2D_WeaponController>
{

    // TODO get the values from the weapons.
    // Ensure the Parameters in the animtor are the exact same
    public enum WeaponTypes { Hammer, ClownGun, DragonGun, BeetleBag, Bear, None };

    public WeaponTypes currentWeaponType = WeaponTypes.Hammer;

    public bool canActivateWeapon = true;

    public bool weaponCharging = false;

    public bool weaponLasering = false;

    public float chargeTimer = 0;

    public Slider powerMeter;

    public Slider cooldownSlider;

    public float weaponChangeCooldown = 0.75f;

    float currWeaponChangeCooldown = 0;

    public float weaponSwitchCooldown = 0.25f;

    float currWeaponSwitchCooldown = 0;

    public bool onWeaponChangeCooldown = false;

    public bool onWeaponSwitchCooldown = false;



    public Image cooldownImage;

    public List<TD2D_Weapon> weapons;

    public List<WeaponTypes> availibleWeapons;

    private TD2D_Weapon_Hammer hammerInstance;

    private TD2D_Weapon_Bear bearInstance;

    public int selectedWeapon = 0;
    private int previousSelectedWeapon = 0;

    private bool isSelectingWeapons = false;

    //private Animator previousWeaponCardAnimator;

    private TD2D_Weapon _previousWeapon;

    private TD2D_Weapon _currentWeapon;

    public TD2D_Weapon CurrentWeapon
    {
        get
        {

            if (_currentWeapon == null)
            {

                ChangeWeapon(currentWeaponType);

                if (_currentWeapon == null)
                {
                    Debug.LogError("Please check the weapon settings and weapon list");
                }
            }

            return _currentWeapon;

        }
    }


    public override void Awake()
    {
        base.Awake();
        currentWeaponType = WeaponTypes.Hammer;
    }

    private void Start()
    {

        ChangeWeapon(currentWeaponType);

    }


    public void GetWeaponAvailability()
    {

        if (GameManager.Instance.bearPurchased)
        {

            // bear weapon card animator show bear card only.

            availibleWeapons.Add(WeaponTypes.Bear);

            ChangeWeapon(WeaponTypes.Bear);

        }
        else
        {

            availibleWeapons.Add(WeaponTypes.Hammer); // Always add the hammer to the avail weapons

            hammerInstance = (TD2D_Weapon_Hammer)weapons.Find(delegate (TD2D_Weapon _weapon)
            {

                return _weapon.weaponType == WeaponTypes.Hammer;

            });

            hammerInstance.weaponCardAnimator.transform.parent.gameObject.SetActive(true);
            
            ChangeWeapon(WeaponTypes.Hammer);

            // Check shop for purchased items and set up as availiible and weapon cards
            foreach (ShopItem item in ShopManager.Instance.shopItems)
            {

                if (item.weaponType != WeaponTypes.None && item.isPurchased && item.weaponType != WeaponTypes.Bear)
                {

                    availibleWeapons.Add(item.weaponType);

                    weapons.Find(delegate (TD2D_Weapon _weapon)
                    {

                        return _weapon.weaponType == item.weaponType;

                    }).weaponCardAnimator.transform.parent.gameObject.SetActive(true);


                }

            }

        }

    }



    /// <summary>
    /// Sets the current weapon using the weapon type provided.
    /// </summary>
    /// <param name="newWeaponType"></param>
    public void ChangeWeapon(WeaponTypes newWeaponType)
    {


        if (availibleWeapons.Count == 0)
        {

            GetWeaponAvailability();

            return;

        }


        if (GameManager.Instance.isPlaying)
        {
            AudioManager.Instance.swapWeaponEv.start();
        }


        //if (_currentWeapon != null && currentWeaponType == newWeaponType)
        //{

        //    _currentWeapon.weaponCardAnimator.SetBool("isSelected", true);
        //    _currentWeapon.weaponCardAnimator.SetBool("isActive", true);
        //    return;

        //}


        // Change the selected weapopn to 0 if come from weapon change after ammo depleted.
        if (newWeaponType == WeaponTypes.Hammer)
        {
            selectedWeapon = 0;
        }

        currentWeaponType = newWeaponType;

        UpdateCurrentWeapon();

    }

    /// <summary>
    /// Handles the changes to the current weapon gameobject to match the current weapon in the weapon controller.
    /// </summary>
    private void UpdateCurrentWeapon()
    {

        if (_currentWeapon != null && _currentWeapon != _previousWeapon)
        {
            _previousWeapon = _currentWeapon;
        }


        _currentWeapon = weapons.Find(delegate (TD2D_Weapon _weapon)
        {

            return _weapon.weaponType == currentWeaponType;

        }
        );


        if (_currentWeapon != null)
        {
            Debug.Log("Weapon " + currentWeaponType + " found!");

            // Disable all the bools in the animtor before setting the correct bool
            DisablePlayerAnimatorBools();

            // Set new params for weapon being used
            TD2D_PlayerController.Instance.PlayerAnimator.SetBool(currentWeaponType.ToString(), true);

            ToggleWeaponStates();

            // Update the players apperance when weapon changes
            TD2D_PlayerController.Instance.UpdatePlayerVisual();
        }
        else
        {
            Debug.LogError("No weapon found in the weapons list for the type " + currentWeaponType);
        }

    }


    /// <summary>
    /// Enables the current weapon and diables the previous weapon.
    /// </summary>
    void ToggleWeaponStates()
    {

        if (_previousWeapon != null)
        {

            _previousWeapon.gameObject.SetActive(false);

        }

        _currentWeapon.gameObject.SetActive(true);
        

        UpdateWeaponCards();


        TD2D_PlayerController.Instance.ReticleCtrl.UpdateReticle();

    }


    private void Update()
    {

        // Exit the function if the weapon is locked (shop)
        if (TD2D_PlayerController.Instance.isWeaponLocked)
        {

            return;

        }


        if (CurrentWeapon.canRotate) // Rotate the weapon to face the target/reticle
        {
            RotateWeapon();
        }


        if (availibleWeapons.Count > 1 && !onWeaponSwitchCooldown && !GameManager.Instance.isShopping && GameManager.Instance.isPlaying)
        {
            if (Input.GetButtonDown("RightBumper") || Input.GetAxisRaw("Mouse ScrollWheel") == 0.1f)
            {

                UpdateWeaponSelection(1);

            }
            else if (Input.GetButtonDown("LeftBumper") || Input.GetAxisRaw("Mouse ScrollWheel") == -0.1f)
            {

                UpdateWeaponSelection(-1);

            }
        }


        if (onWeaponSwitchCooldown)
        {

            currWeaponSwitchCooldown += Time.deltaTime;

            if (currWeaponSwitchCooldown >= weaponSwitchCooldown)
            {

                onWeaponSwitchCooldown = false;
                //cooldownImage.gameObject.SetActive(true);

            }

        }



        if (onWeaponChangeCooldown)
        {


            currWeaponChangeCooldown += Time.deltaTime;


            //cooldownSlider.value = currWeaponChangeCooldown / weaponChangeCooldown;


            if (currWeaponChangeCooldown >= weaponChangeCooldown)
            {

                onWeaponChangeCooldown = false;
                //cooldownImage.gameObject.SetActive(true);

                ChangeWeapon(availibleWeapons[selectedWeapon]);

            }


        }


        bool buttonPressed = Input.GetMouseButton(0) || Input.GetAxisRaw("RightTrigger") == 1;


        if (buttonPressed)
        {

            if (canActivateWeapon) // Check if player is able to activate weapon (Hit/Shoot)
            {

                bool chargable = GameManager.Instance.isShopping || GameManager.Instance.isPlaying;

                if (currentWeaponType == WeaponTypes.Hammer)// If cannot hit (already hitting) do fail feedback
                {

                    if (!weaponCharging)
                    {
                        weaponCharging = true;
                        // powerMeter.gameObject.SetActive(true);
                        // Activate Audio for weapon charging
                    }

                    if (chargable)
                    {
                        chargeTimer += Time.deltaTime;
                    }
                    else
                    {
                        chargeTimer = 0.01f;
                    }
                    // powerMeter.value = chargeTimer;


                    float radiusRange = hammerInstance.maxHitRadius - hammerInstance.minHitRadius;

                    //print(radiusRange+ "radiusRange");

                    float powerPercent = (100 * Mathf.Min(chargeTimer, hammerInstance.chargeTime)) / hammerInstance.chargeTime;

                    ////print(powerPercent + "powerPercent");

                    float radiusPercent = radiusRange * (powerPercent / 100);

                    //print(radiusPercent+ "radiusPercent");

                    hammerInstance.killRadius = hammerInstance.minHitRadius + radiusPercent;

                    CurrentWeapon.reticle.transform.localScale = new Vector3(hammerInstance.killRadius, hammerInstance.killRadius, 1);



                }
                else if (currentWeaponType == WeaponTypes.Bear)
                {

                    TD2D_PlayerController.Instance.DeterminePlayerDirection();

                    if (!weaponLasering)
                    {

                        weaponLasering = true;

                        // Play player animation for attacking
                        TD2D_PlayerController.Instance.PlayerAnimator.SetBool("Attacking", true);
                        CurrentWeapon.Activate();

                        // Camera Shake
                        //ActivateWeaponScreenShake();


                    }
                }
                else
                {

                    TD2D_PlayerController.Instance.DeterminePlayerDirection();

                    // Play player animation for attacking
                    TD2D_PlayerController.Instance.PlayerAnimator.SetTrigger("Attacking");



                    // Camera Shake
                    ActivateWeaponScreenShake();
                }

            }


        }
        else if (!buttonPressed && weaponCharging)
        {


            TD2D_PlayerController.Instance.DeterminePlayerDirection();

            TD2D_PlayerController.Instance.isAttacking = true;
            canActivateWeapon = false;
            weaponCharging = false;

            CurrentWeapon.reticle.transform.localScale = new Vector3(1, 1, 1);

            TD2D_PlayerController.Instance.PlayerAnimator.SetTrigger("Attacking");

        }
        else if (!buttonPressed && weaponLasering)
        {

            CurrentWeapon.Deactivate();
            TD2D_PlayerController.Instance.isAttacking = false;
            weaponLasering = false;


        }
    }




    void UpdateWeaponSelection(int valChange)
    {

        isSelectingWeapons = true;

        previousSelectedWeapon = selectedWeapon;

        selectedWeapon += valChange;

        // Check if in list bounds and skip to other end if not
        if (selectedWeapon < 0)
        {
            selectedWeapon = availibleWeapons.Count - 1;
        }
        else if (selectedWeapon > availibleWeapons.Count - 1)
        {
            selectedWeapon = 0;
        }

        UpdateWeaponCards();

        AudioManager.Instance.swapWeaponEv.start();

        onWeaponSwitchCooldown = true;
        currWeaponSwitchCooldown = 0;

        onWeaponChangeCooldown = true;
        currWeaponChangeCooldown = 0;

        isSelectingWeapons = false;

    }




    void UpdateWeaponCards()
    {

        if (isSelectingWeapons)
        {


            Animator previousSelectedWeaponAnim = weapons.Find(delegate (TD2D_Weapon _weapon)
            {

                return _weapon.weaponType == availibleWeapons[previousSelectedWeapon];

            }
    ).weaponCardAnimator;


            previousSelectedWeaponAnim.SetBool("isSelected", false);
            previousSelectedWeaponAnim.SetBool("isActive", false);


            weapons.Find(delegate (TD2D_Weapon _weapon)
            {

                return _weapon.weaponType == availibleWeapons[selectedWeapon];

            }
).weaponCardAnimator.SetBool("isSelected", true);
                       
        }
        else
        {

            if (_previousWeapon != null)
            {

                _previousWeapon.weaponCardAnimator.SetBool("isSelected", false);
                _previousWeapon.weaponCardAnimator.SetBool("isActive", false);

            }

            _currentWeapon.weaponCardAnimator.SetBool("isSelected", true);
            _currentWeapon.weaponCardAnimator.SetBool("isActive", true);

        }

    }


    /// <summary>
    /// Sets all the boolean paramteres in the player animator to false.
    /// </summary>
    void DisablePlayerAnimatorBools()
    {
        // Find each parameter which is a bool and diable it in the player animator
        foreach (AnimatorControllerParameter parameter in TD2D_PlayerController.Instance.PlayerAnimator.parameters.Where(n => n.type == AnimatorControllerParameterType.Bool))
        {
            TD2D_PlayerController.Instance.PlayerAnimator.SetBool(parameter.name, false);
        }
    }


    /// <summary>
    /// Rotates the weapon to point towards the mouse position.
    /// </summary>
    void RotateWeapon()
    {
        Vector3 difference = TD2D_PlayerController.Instance.ReticleCtrl.CurrentReticle.transform.position - CurrentWeapon.grip.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        CurrentWeapon.grip.rotation = Quaternion.Euler(0f, 0f, rotation_z);
    }


    /// <summary>
    /// Public weapon functions that can be called from within the animator
    /// </summary>
    #region Public Weapon Functions

    public void ActivateWeapon()
    {
        CurrentWeapon.Activate();
    }


    public void DeActivateWeapon()
    {
        CurrentWeapon.Deactivate();
    }


    public void ActivateWeaponParticles()
    {
        CurrentWeapon.TriggerActivateParticles();
    }


    public void DeactivateWeaponParticles()
    {
        CurrentWeapon.TriggerDeactivateParticles();
    }


    /// <summary>
    /// Pause before screen shake
    /// </summary>
    public void PreScreenShakePause()
    {
        // Do pause here
    }


    public void ActivateWeaponScreenShake()
    {
        GameManager.Instance.cameraShaker.Shake(CurrentWeapon.camShakeAmount, CurrentWeapon.camShakeLength);
    }


    /// <summary>
    /// Pause after screen shake
    /// </summary>
    public void PostScreenShakePause()
    {
        // Do pause here
    }

    #endregion

    #region Old functions
    //public void SetParent(Transform handTransform)
    //{

    //    CurrentWeapon.transform.parent = handTransform;
    //    CurrentWeapon.transform.localPosition = Vector3.zero;

    //}


    //public void SetPosition()
    //{

    //   // CurrentWeapon.transform.position = GameManager.Instance.PlayerController.playerHand.position;

    //}


    // TODO get ints for players at start
    //public void SendWeaponToBackground(bool toBackground)
    //{
    //    if (toBackground)
    //    {
    //        //sr.sortingOrder = 1;
    //        CurrentWeapon.WeaponRenderer.sortingLayerName = "Background";
    //    }
    //    else
    //    {
    //        //sr.sortingOrder = 2;
    //        CurrentWeapon.WeaponRenderer.sortingLayerName = "Foreground";
    //    }

    //} 
    #endregion
}

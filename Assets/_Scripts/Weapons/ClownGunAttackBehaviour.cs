using UnityEngine;

public class ClownGunAttackBehaviour : StateMachineBehaviour
{

    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //    TD2D_WeaponController.Instance.canActivateWeapon = false;

    //}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // Dont need to do this if doing it in the deactivate function
        //TD2D_WeaponController.Instance.canActivateWeapon = true;

        TD2D_WeaponController.Instance.DeActivateWeapon();

    }

}

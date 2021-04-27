using UnityEngine;

public class HammerAttackBehaviour : StateMachineBehaviour
{

    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //    //TD2D_WeaponController.Instance.canActivateWeapon = false;
    //    //TD2D_PlayerController.Instance.canMove = false;

    //}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //TD2D_WeaponController.Instance.canActivateWeapon = true;

        TD2D_WeaponController.Instance.DeActivateWeapon();

    }

}

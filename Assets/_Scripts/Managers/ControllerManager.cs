using System.Collections;
using UnityEngine;
using XInputDotNetPure;

public static class ControllerManager 
{

    static bool playerIndexSet = false;
    public static PlayerIndex playerIndex;
    public static bool controllerAvailable = false;


    public static bool GetControllerState()
    {

        bool detected = false;

        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        if (!playerIndexSet) //)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);

                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                    detected = true;
                    return detected;
                }
            }
        }

        return detected;

    }
}



////Get Joystick Names
//string[] temp = Input.GetJoystickNames();

////Check whether array contains anything
//if (temp.Length > 0)
//{
//    //Iterate over every element
//    for (int i = 0; i < temp.Length; ++i)
//    {

//        //Check if the string is empty or not
//        if (!string.IsNullOrEmpty(temp[i]))
//        {

//            //Not empty, controller temp[i] is connected
//            TD2D_PlayerController.Instance.enableMouse = false;
//            Debug.Log("Controller " + i + " is connected using: " + temp[i]);

//            detected = true;

//        }
//        else
//        {

//            //If it is empty, controller i is disconnected
//            //where i indicates the controller number
//            TD2D_PlayerController.Instance.enableMouse = true;
//            Debug.Log("Controller: " + i + " is disconnected.");

//            detected = true; ;

//        }

//    }
//}
//else
//{
//    TD2D_PlayerController.Instance.enableMouse = true;
//    Debug.Log("No Controller Detected.");

//    detected = false;
//}

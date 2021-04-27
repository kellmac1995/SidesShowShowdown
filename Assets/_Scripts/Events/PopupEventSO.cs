using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="PopupEvent", menuName = "Events/Popup", order = 1)]
public class PopupEventSO : ScriptableObject
{


    public enum buttons { None, ButtonA, ButtonB, ButtonX, ButtonY, LeftBumper, RightBumper, RightTrigger, LeftTrigger}

    public string animStartName;
    public string animEndName;

    public string[] textboxCaptions;

    public buttons actionButton;
    public float waitTime;

    public bool overrideAll = false;

}

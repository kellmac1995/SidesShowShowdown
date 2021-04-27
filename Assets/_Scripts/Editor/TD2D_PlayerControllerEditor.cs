using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(TD2D_PlayerController))]
public class TD2D_PlayerControllerEditor : Editor
{
    bool leftVals = false;
    bool rightVals = false;
    bool animVals = false;
    bool handVals = false;

    public override void OnInspectorGUI()
    {

        var controller = target as TD2D_PlayerController;

        EditorGUIUtility.labelWidth = 130;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Input Settings", EditorStyles.boldLabel);

        EditorGUI.indentLevel++;


        //TODO detect a change and OnMouseToggle() and set default values for input
        controller.enableMouse = EditorGUILayout.Toggle("Enable Mouse?", controller.enableMouse);


        using (new EditorGUI.DisabledScope(controller.enableMouse))
        {

            if (controller.enableMouse)
            {
                leftVals = EditorGUILayout.Foldout(leftVals, "Axis", true);
            }
            else
            {
                leftVals = EditorGUILayout.Foldout(leftVals, "Left Stick", true);
            }

            if (leftVals)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("V Axis", GUILayout.MaxWidth(72));
                controller.leftStick.Vertical = EditorGUILayout.TextField(controller.leftStick.Vertical);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("H Axis", GUILayout.MaxWidth(72));
                controller.leftStick.Horizontal = EditorGUILayout.TextField(controller.leftStick.Horizontal);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }

        }
        if (!controller.enableMouse)
        {
            rightVals = EditorGUILayout.Foldout(rightVals, "Right Stick", true);
            if (rightVals)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("V Axis", GUILayout.MaxWidth(72));
                controller.rightStick.Vertical = EditorGUILayout.TextField(controller.rightStick.Vertical);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("H Axis", GUILayout.MaxWidth(72));
                controller.rightStick.Horizontal = EditorGUILayout.TextField(controller.rightStick.Horizontal);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
        }

        EditorGUI.indentLevel--;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Player Settings", EditorStyles.boldLabel);

        EditorGUI.indentLevel++;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Aim Pivot", GUILayout.MaxWidth(110));
        controller.aimPivot = EditorGUILayout.ObjectField(controller.aimPivot, typeof(Transform), true) as Transform;
        EditorGUILayout.EndHorizontal();


        handVals = EditorGUILayout.Foldout(handVals, "Player Hands", true);

        if (handVals)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Right Hand", GUILayout.MaxWidth(95));
            controller.rightHand = EditorGUILayout.ObjectField(controller.rightHand, typeof(Transform), true) as Transform;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Left Hand", GUILayout.MaxWidth(95));
            controller.leftHand = EditorGUILayout.ObjectField(controller.leftHand, typeof(Transform), true) as Transform;
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
        }


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Max Speed", GUILayout.MaxWidth(110));
        controller.maxSpeed = EditorGUILayout.FloatField(controller.maxSpeed);
        EditorGUILayout.EndHorizontal();
        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("Max Health", GUILayout.MaxWidth(110));
        //controller.maxHealth = EditorGUILayout.FloatField(controller.maxHealth);
        //EditorGUILayout.EndHorizontal();
        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("Jump Time", GUILayout.MaxWidth(110));
        //controller.jumpTime = EditorGUILayout.FloatField(controller.jumpTime);
        //EditorGUILayout.EndHorizontal();
        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("Normalize Movement", GUILayout.MaxWidth(140));
        //controller.normalizedMovement = EditorGUILayout.Toggle(controller.normalizedMovement);
        //EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel--;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Animator Settings", EditorStyles.boldLabel);

        EditorGUI.indentLevel++;

        //controller.usingAnimator = EditorGUILayout.Toggle("Using Animator?", controller.usingAnimator);


        //if (controller.usingAnimator)
        //{
            EditorGUI.indentLevel++;
            animVals = EditorGUILayout.Foldout(animVals, "Triggers", true);

            if (animVals)
            {
                if (controller.parameters.Length > 0)
                {


                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Right", GUILayout.MaxWidth(80));
                    controller.rightTrigger = EditorGUILayout.Popup(controller.rightTrigger, controller.parameters);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Left", GUILayout.MaxWidth(80));
                    controller.leftTrigger = EditorGUILayout.Popup(controller.leftTrigger, controller.parameters);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Down", GUILayout.MaxWidth(80));
                    controller.downTrigger = EditorGUILayout.Popup(controller.downTrigger, controller.parameters);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Up", GUILayout.MaxWidth(80));
                    controller.upTrigger = EditorGUILayout.Popup(controller.upTrigger, controller.parameters);
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;

                }
                else
                {
                    Debug.LogError("Populate Paramters using the menu on player controller.");
                }
            }
            EditorGUI.indentLevel--;

        //}
        //else if (!controller.usingAnimator)//group.visible == true & !controller.usingAnimator)
        //{

        //    EditorGUI.indentLevel++;
        //    animVals = EditorGUILayout.Foldout(animVals, "Sprites", true);

        //    if (animVals)
        //    {

        //        EditorGUI.indentLevel++;

        //        EditorGUILayout.BeginHorizontal();
        //        EditorGUILayout.LabelField("Right", GUILayout.MaxWidth(80));
        //        controller.rightSprite = EditorGUILayout.ObjectField(controller.rightSprite, typeof(Sprite), true) as Sprite;
        //        EditorGUILayout.EndHorizontal();
        //        EditorGUILayout.BeginHorizontal();
        //        EditorGUILayout.LabelField("Left", GUILayout.MaxWidth(80));
        //        controller.leftSprite = EditorGUILayout.ObjectField(controller.leftSprite, typeof(Sprite), true) as Sprite;
        //        EditorGUILayout.EndHorizontal();
        //        EditorGUILayout.BeginHorizontal();
        //        EditorGUILayout.LabelField("Down", GUILayout.MaxWidth(80));
        //        controller.downSprite = EditorGUILayout.ObjectField(controller.downSprite, typeof(Sprite), true) as Sprite;
        //        EditorGUILayout.EndHorizontal();
        //        EditorGUILayout.BeginHorizontal();
        //        EditorGUILayout.LabelField("Up", GUILayout.MaxWidth(80));
        //        controller.upSprite = EditorGUILayout.ObjectField(controller.upSprite, typeof(Sprite), true) as Sprite;
        //        EditorGUILayout.EndHorizontal();
        //        EditorGUI.indentLevel--;

        //    }
        //}

        EditorGUI.indentLevel--;

        EditorGUILayout.Space();

        EditorUtility.SetDirty(controller);

    }
}

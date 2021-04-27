using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TD2D_WeaponController))]
public class TD2D_WeaponControllerEditor : Editor
{

    public override void OnInspectorGUI()
    {

        serializedObject.Update();

        var controller = target as TD2D_WeaponController;


        EditorGUIUtility.labelWidth = 130;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);

        EditorGUI.indentLevel++;
        controller.currentWeaponType = (TD2D_WeaponController.WeaponTypes)EditorGUILayout.EnumPopup("Current Weapon", controller.currentWeaponType);

        controller.canActivateWeapon = EditorGUILayout.Toggle("Weapon Active?", controller.canActivateWeapon);

        controller.weaponChangeCooldown = EditorGUILayout.FloatField("Lock-in Time", controller.weaponChangeCooldown);

        controller.weaponSwitchCooldown = EditorGUILayout.FloatField("Switch Rate Time", controller.weaponSwitchCooldown);



        EditorGUI.indentLevel--;

        SerializedProperty property = serializedObject.FindProperty("weapons");

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Weapons", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();

        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(property, new GUIContent("Weapons List"), true);
        EditorGUI.indentLevel--;

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
        
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Power Meter", GUILayout.MaxWidth(110));
        controller.powerMeter = EditorGUILayout.ObjectField(controller.powerMeter, typeof(UnityEngine.UI.Slider), true) as UnityEngine.UI.Slider;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Cooldown Meter", GUILayout.MaxWidth(110));
        controller.cooldownSlider = EditorGUILayout.ObjectField(controller.cooldownSlider, typeof(UnityEngine.UI.Slider), true) as UnityEngine.UI.Slider;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Activate Weapon Image", GUILayout.MaxWidth(110));
        controller.cooldownImage = EditorGUILayout.ObjectField(controller.cooldownImage, typeof(UnityEngine.UI.Image), true) as UnityEngine.UI.Image;
        EditorGUILayout.EndHorizontal();


        EditorUtility.SetDirty(controller);

    }

}

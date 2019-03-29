using UnityEngine;
 using System.Collections;
 using UnityEditor;
 using System.Collections.Generic;
[CustomEditor(typeof(CustomerCreator)), CanEditMultipleObjects]
public class CustomerEditor : Editor
{
    public SerializedProperty
        customers,
        gLogic
        ;

    void OnEnable()
    {
        customers = serializedObject.FindProperty("customers");
        gLogic = serializedObject.FindProperty("gLogic");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(customers,true);
        EditorGUILayout.PropertyField(gLogic, true);
        serializedObject.ApplyModifiedProperties();
        CustomerCreator customerCreate = (CustomerCreator)target;
        if (GUILayout.Button("Create"))
        {
            if (customerCreate.customers.Count>0)
                customerCreate.CreateCustomer();
            else
                Debug.LogError("Oluşturabilceğiniz bir eleman tanımlamadınız!");

        }
        if (GUILayout.Button("Clean"))
        {
            customerCreate.customers.Clear();
        }
    }

}

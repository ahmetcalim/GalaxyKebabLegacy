using UnityEngine;
 using System.Collections;
 using UnityEditor;
 using System.Collections.Generic;
[CustomEditor(typeof(IngredientCreator)), CanEditMultipleObjects]
public class IngredientEditor : Editor
{
    public SerializedProperty
        ingredients,
        gLogic,
        ingredientItem,
        actionViewport
        ;

    void OnEnable()
    {
        ingredients = serializedObject.FindProperty("ingredients");
        gLogic = serializedObject.FindProperty("gLogic");
        ingredientItem = serializedObject.FindProperty("ingredientItem");
        actionViewport = serializedObject.FindProperty("actionViewport");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(ingredients, true);
        EditorGUILayout.PropertyField(gLogic, true);
        EditorGUILayout.PropertyField(ingredientItem, true);
        EditorGUILayout.PropertyField(actionViewport, true);
        serializedObject.ApplyModifiedProperties();
        IngredientCreator customerCreate = (IngredientCreator)target;

        if (GUILayout.Button("Create"))
        {
            if (customerCreate.ingredients.Count>0)
                customerCreate.CreateIngredient();
            else
                Debug.LogError("Oluşturabilceğiniz bir eleman tanımlamadınız!");

        }
        if (GUILayout.Button("Clean"))
        {
            customerCreate.ingredients.Clear();
        }
    }

}

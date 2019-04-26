using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

public class IngredientCreator
{
    [MenuItem("Galaxy Kebab Tools/Create/Ingredient List")]
    public static void CreateMyAsset()
    {
        GameLogic gLogic = GameObject.FindObjectOfType<GameLogic>();
        gLogic.ingredients.Clear();
        string jsonString = "{ \"ingredientObjects\":" + Resources.Load<TextAsset>("JsonData/Ingredients").text + "}";
        RootIngredientCreator container = JsonUtility.FromJson<RootIngredientCreator>(jsonString);
        int i = 1;
        foreach (IngredientObject item in container.ingredientObjects)
        {
            Ingredient ingredientAsset = ScriptableObject.CreateInstance<Ingredient>();
            ingredientAsset.ID = i;
            ingredientAsset.ingredientName = item.ingredientName;
            ingredientAsset.actionInput = item.actionInput;
            ingredientAsset.inputUnitCost = item.inputUnitCost;
            AssetDatabase.CreateAsset(ingredientAsset, "Assets/Enes/Project_Folders/ScriptableObjects/Ingredients/" + ingredientAsset.ingredientName + ".asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = ingredientAsset;
            gLogic.ingredients.Add(ingredientAsset);
            i++;
        }
        i = 0;
    }
}
[Serializable]
public class RootIngredientCreator
{
    public List<IngredientObject> ingredientObjects = new List<IngredientObject>();
}
[Serializable]
public class IngredientObject
{
    public string ingredientName;
    public float actionInput;
    public float inputUnitCost;
}
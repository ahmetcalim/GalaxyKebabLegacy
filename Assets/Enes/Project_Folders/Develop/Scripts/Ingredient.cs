using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ingredient
{
    public int ID;
    public string ingredientName;
    public List<IngredientTaste> tastes;
    [System.NonSerialized]
    public double totalTasteInput;
    [System.NonSerialized]
    public double inputPercentage;

    public double GetTotalTasteInput()
    {
        totalTasteInput = 0;
        for (int i = 0; i < tastes.Count; i++)
        {
            totalTasteInput += tastes[i].tasteInput;
        }
        return totalTasteInput;
    }
}
[System.Serializable]
public class IngredientTaste
{
    public Taste.Tastes taste;
    public double tasteInput;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ingredient
{  
    public string ingredientName;
    public int ID;
    public List<IngredientTaste> tastes;
    [System.NonSerialized]
    public float rating=1;
    public int actionAmount;
   
}
[System.Serializable]
public class IngredientTaste
{
    public Taste taste;
    public float tasteInput;
}

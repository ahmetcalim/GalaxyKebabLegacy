using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Ingredient:ScriptableObject
{
    public int ID;
    public string ingredientName;
    public float actionInput;
    public float inputUnitCost;
    public float totalInputCount;
    public float totalCost;
    public double rating;

    public void CalculateAverageRating(double value)
    {
        rating = value;
    }
    public void CalculateCost()
    {
        totalCost = totalInputCount * inputUnitCost;
    }
}

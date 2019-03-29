using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Taste 
{
    public Tastes taste;
    public double x_zero;
    public double x_max;
    public double tasteRating;
    public double totalInputCount;
    public bool isLike = true;
    public enum Tastes 
    { 
    Astringent,
    Bitter,
    Pungent,
    Sour,
    Salty,
    Sweet,
    Savory 
    }

    public void CalculateAverageTasteRating(double value)
    {
        tasteRating = value;
    }
}

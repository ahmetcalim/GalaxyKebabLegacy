using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "New Customer", menuName = "Galaxy Kebab Legacy/Customer")]
public class Customer:ScriptableObject
{
    public string customerName;
    public GameObject model;
    public double averageRating;
    public Personality personality;
    public List<CustomerIngredient> c_Ingredients;
    [System.NonSerialized]
    public List<Ingredient> irrelevantIngredients=new List<Ingredient>();
    [System.NonSerialized]
    public int orderCount;

    public double CalculateAverageSatisfactionValue()
    {    
        averageRating = 0;
        for (int i = 0; i < c_Ingredients.Count; i++)
            averageRating += c_Ingredients[i].ingredient.rating;

        averageRating += CalculateIrrelevantRating();
        averageRating = averageRating / (orderCount+c_Ingredients.Where(i=>i.preference==CustomerIngredient.Preference.dislike).ToList().Count+3);
        return averageRating;      
    }
    double sum;
    public double CalculateIrrelevantRating()
    {
        sum = 0;
        for (int i = 0; i < irrelevantIngredients.Count; i++)
            sum += irrelevantIngredients[i].rating;
        return sum;
    }
    public void ClearCustomer()
    {
        sum = 0;
        orderCount = 0;
        averageRating = 0;
        for (int i = 0; i < irrelevantIngredients.Count; i++)
        {
            irrelevantIngredients[i].rating = 0;
            irrelevantIngredients[i].totalCost = 0;
            irrelevantIngredients[i].totalInputCount = 0;
        }
        for (int i = 0; i < c_Ingredients.Count; i++)
        {
            c_Ingredients[i].ingredient.rating = 0;
            c_Ingredients[i].ingredient.totalCost = 0;
            c_Ingredients[i].ingredient.totalInputCount = 0;
            c_Ingredients[i].inOrder = false;
        }
        irrelevantIngredients.Clear();
    }

}
[System.Serializable]
public class CustomerIngredient
{
    public Ingredient ingredient;
    public int x_zero,x_max;
    [System.NonSerialized]
    public bool inOrder;
    public Preference preference;
    public enum Preference { like, dislike,meat };
}


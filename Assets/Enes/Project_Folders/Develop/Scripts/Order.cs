using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Order : MonoBehaviour
{
    public Customer customer;
    public Order nextOrder,prevOrder;
    [System.NonSerialized]
    public GameObject orderPrefab;
    public bool isFinished;
    double totalInput;
    float difference;
    Ingredient temporalIngredient;
    List<Ingredient> ingredientsWithOrderByDescending;
    public List<Ingredient> finalIngredients;
    List<Ingredient> ingredients;
    List<Taste> isLikeTastes;
    List<Taste> disLikeTastes;

    public Order(Customer _customer,List<Ingredient> _ingredients,int _ingredientCount)
    {
        ingredientsWithOrderByDescending = new List<Ingredient>();
        ingredients = new List<Ingredient>(_ingredients);
        finalIngredients = new List<Ingredient>(); 
        this.customer = _customer;
        CreateOrder(_ingredientCount);
    }
    public void CreateOrder(int ingredientCount)
    {
        ingredientsWithOrderByDescending.Clear();
        finalIngredients.Clear();
        RemoveIngredientForDislikeTastes();
        SearchByLike();
        CalculateIngredientRating();
        CalculateSumRating();
        CalculateSequentialRating();
        GetRandomVariableValue();
    }

    void RemoveIngredientForDislikeTastes()
    {
        disLikeTastes = customer.Tastes.Where(t => t.preference == Taste.Preference.dislike).ToList();

        foreach (Ingredient ingredient in ingredients.ToArray())
        {
            foreach (IngredientTaste ingredientTaste in ingredient.tastes)
            {
                if (disLikeTastes.Where(t => t.taste == ingredientTaste.taste.taste).Any())
                {
                    ingredients.Remove(ingredient);
                    break;
                }
            }
        }      
    }
    void SearchByLike()
    {
        bool hasIsLike = false;
        isLikeTastes = customer.Tastes.Where(t => t.preference == Taste.Preference.like).ToList();

        foreach (Ingredient ingredient in ingredients.ToArray())
        {
            foreach (IngredientTaste ingredientTaste in ingredient.tastes)
            {
                if (isLikeTastes.Where(t => t.taste == ingredientTaste.taste.taste).Any())
                hasIsLike = true;
            }
            if (!hasIsLike)
                ingredients.Remove(ingredient);
                hasIsLike = false;
        }
     
    }
    void CalculateIngredientRating()
    {
        foreach (Ingredient ingredient in ingredients)
        {
            foreach (IngredientTaste ingredientTaste in ingredient.tastes)
            {
                if (isLikeTastes.Where(t => t.taste == ingredientTaste.taste.taste).Any())
                    ingredient.rating *= ingredientTaste.tasteInput;
                else
                    ingredient.rating *= 1;
            }
        }

        ingredientsWithOrderByDescending = ingredients.OrderByDescending(i=>i.rating).ToList();
        finalIngredients.Add(ingredientsWithOrderByDescending[0]);
        ingredientsWithOrderByDescending.RemoveAt(0);  
    }
    void CalculateSumRating()
    {
        float sumRate = 0;
        for (int i = 0; i < ingredientsWithOrderByDescending.Count; i++)
            sumRate+= ingredientsWithOrderByDescending[i].rating;
        for (int i = 0; i < ingredientsWithOrderByDescending.Count; i++)
            ingredientsWithOrderByDescending[i].rating /= sumRate;
    }
    void CalculateSequentialRating()
    {
        for (int i = 1; i < ingredientsWithOrderByDescending.Count; i++)
            ingredientsWithOrderByDescending[i].rating += ingredientsWithOrderByDescending[i - 1].rating;          
    }
    void GetRandomVariableValue()
    {
        double rndValue = Random.Range(0f, 1f);
        AddIngredientToOrder(rndValue);
    }
    void AddIngredientToOrder(double _rndValue)
    {
        difference = 1;
        temporalIngredient = null;

        for (int i = 0; i < ingredientsWithOrderByDescending.Count; i++)
        {
            float currentDifference = Mathf.Abs((float)_rndValue - (float)ingredientsWithOrderByDescending[i].rating);
            if (currentDifference <= difference)
            {
                if (_rndValue > ingredientsWithOrderByDescending[i].rating)
                    temporalIngredient = ingredientsWithOrderByDescending[i + 1];
                else
                    temporalIngredient = ingredientsWithOrderByDescending[i];

                difference = currentDifference;
            }
        }
        if (!finalIngredients.Contains(temporalIngredient))
            finalIngredients.Add(temporalIngredient);
        for (int i = 0; i < ingredients.Count; i++)
            ingredients[i].rating = 1;
    }
}

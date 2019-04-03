using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Order : MonoBehaviour
{
    public Customer customer;
    public Order nextOrder;
    [System.NonSerialized]
    public GameObject orderPrefab;
    public bool isFinished;
    double totalInput;
    float difference;
    Ingredient temporalIngredient;
    List<Ingredient> ingredientsWithSortByMaxMinValueTaste;
    public List<Ingredient> finalIngredients;
    List<Ingredient> ingredients;

    public Order(Customer _customer,List<Ingredient> _ingredients,int _ingredientCount)
    {
        ingredientsWithSortByMaxMinValueTaste = new List<Ingredient>();
        ingredients = new List<Ingredient>();
        this.ingredients = _ingredients;
        finalIngredients = new List<Ingredient>(); 
        this.customer = _customer;
        CreateOrder(_ingredientCount);
    }

    public void CreateOrder(int ingredientCount)
    {
        ingredientsWithSortByMaxMinValueTaste.Clear();
        finalIngredients.Clear();
        totalInput = 0;

        Taste.Tastes sortByMaxMinValueTaste = GetFavoriteTaste();

        Debug.Log(sortByMaxMinValueTaste);

        ListIngredientsForFavoriteTaste(sortByMaxMinValueTaste);

        CalculateTotalInputForEachIngredient();

        CalculateInputPercentageForEachIngredient();

        ingredientsWithSortByMaxMinValueTaste = ingredientsWithSortByMaxMinValueTaste.OrderBy(i => i.inputPercentage).ToList();

        CalculateSequentialPercantage();

        for (int i = 0; i < ingredientCount; i++)
        {
            GetRandomVariableValue();
        }
    }

    Taste.Tastes GetFavoriteTaste()
    {
        List<Taste> tastes= customer.Tastes.Where(t => t.preference==Taste.Preference.like).ToList();
        return tastes.OrderByDescending(t =>t.x_max - t.x_zero).ToList().First().taste;
    }

    void ListIngredientsForFavoriteTaste(Taste.Tastes _query)
    {
        foreach (Ingredient ingredient in ingredients)
        {
            foreach (IngredientTaste taste in ingredient.tastes)
            {
                if (taste.taste == _query)
                {
                    ingredientsWithSortByMaxMinValueTaste.Add(ingredient);
                }
            }
        }
    }

    void CalculateTotalInputForEachIngredient()
    {
        foreach (Ingredient ingredient in ingredientsWithSortByMaxMinValueTaste)
        {
            ingredient.totalTasteInput = ingredient.GetTotalTasteInput();
            totalInput += ingredient.totalTasteInput;
        }
    }

    void CalculateInputPercentageForEachIngredient()
    {
        foreach (Ingredient ingredient in ingredientsWithSortByMaxMinValueTaste)
            ingredient.inputPercentage = ingredient.totalTasteInput / totalInput;
    }

    void CalculateSequentialPercantage()
    {
        for (int i = 1; i < ingredientsWithSortByMaxMinValueTaste.Count; i++)
        {
            ingredientsWithSortByMaxMinValueTaste[i].inputPercentage += ingredientsWithSortByMaxMinValueTaste[i - 1].inputPercentage;
        }
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

        for (int i = 0; i < ingredientsWithSortByMaxMinValueTaste.Count; i++)
        {
            float currentDifference = Mathf.Abs((float)_rndValue - (float)ingredientsWithSortByMaxMinValueTaste[i].inputPercentage);
            if (currentDifference <= difference)
            {
                if (_rndValue > ingredientsWithSortByMaxMinValueTaste[i].inputPercentage)
                    temporalIngredient = ingredientsWithSortByMaxMinValueTaste[i + 1];
                else
                    temporalIngredient = ingredientsWithSortByMaxMinValueTaste[i];

                difference = currentDifference;
            }
        }
        if (!finalIngredients.Contains(temporalIngredient))
            finalIngredients.Add(temporalIngredient);
    }

    
}

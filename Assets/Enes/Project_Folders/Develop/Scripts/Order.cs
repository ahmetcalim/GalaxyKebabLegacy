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
    public List<Ingredient> finalIngredients;
    List<CustomerIngredient> islikeIngredients;


    public Order(Customer _customer,int _ingredientCount)
    {
        finalIngredients = new List<Ingredient>(); 
        this.customer = _customer;
        CreateOrder(_ingredientCount);
        _customer.orderCount = _ingredientCount;
    }
    public void CreateOrder(int ingredientCount)
    {
        finalIngredients.Clear();
        SearchByLike();
        GetRandomVariableValue(ingredientCount);
    }
    void SearchByLike()
    {
      islikeIngredients = customer.c_Ingredients.Where(i => i.preference == CustomerIngredient.Preference.like).ToList();
    }
    
    void GetRandomVariableValue(int _ingredientCount)
    {
        for (int i = 0; i < _ingredientCount; i++)
        AddIngredientToOrder(_ingredientCount);
    }
    void AddIngredientToOrder(int count)
    {
        int rndValue = Random.Range(0, islikeIngredients.Count);

        if (!finalIngredients.Contains(islikeIngredients[rndValue].ingredient))
        {
            finalIngredients.Add(islikeIngredients[rndValue].ingredient);
            islikeIngredients[rndValue].inOrder = true;
        }
        else
            AddIngredientToOrder(count);
    }
}

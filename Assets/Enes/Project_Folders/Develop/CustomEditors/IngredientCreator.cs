using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientCreator : MonoBehaviour
{
    public List<Ingredient> ingredients;
    public GameLogic gLogic;
    public IngredientItem ingredientItem;
    public Transform actionViewport;
    public void CreateIngredient()
    {
        for (int i = 0; i < ingredients.Count; i++)
        {
            ingredientItem.ID = ingredients[i].ID;
            ingredientItem.gLogic = this.gLogic;
            ingredientItem.gameObject.name = ingredients[i].ingredientName;
            ingredientItem.GetComponentInChildren<Text>().text = ingredientItem.name;
            GameObject g=Instantiate(ingredientItem.gameObject,actionViewport);        
            gLogic.ingredients.Add(ingredients[i]);
        }
    }


}

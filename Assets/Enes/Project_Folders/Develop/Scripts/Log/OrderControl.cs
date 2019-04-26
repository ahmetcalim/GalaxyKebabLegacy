using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderControl : MonoBehaviour
{
    public Text customerName;
    public List<IngredientView> ingredientItems;
    public IngredientView average;
    
    public void SetValues(Customer customer,List<CustomerIngredient> ingredients)
    {
        this.customerName.text = customer.customerName;
        for (int i = 0; i < ingredients.Count; i++)
        {

            switch (ingredients[i].preference)
            {
                case CustomerIngredient.Preference.like:
                    ingredientItems[i].ingredientText.color = Color.green;
                    break;
                case CustomerIngredient.Preference.dislike:
                    ingredientItems[i].ingredientText.color = Color.red;
                    break;
                default:
                    break;
            }

            ingredientItems[i].ingredientText.text = ingredients[i].ingredient.name.ToString();
            ingredientItems[i].valueText.text = ingredients[i].ingredient.rating.ToString();
        }
        average.ingredientText.text = "average";
        average.valueText.text = customer.averageRating.ToString();
    }

}
[System.Serializable]
public class IngredientView
{
    public Text ingredientText;
    public Text valueText;
}

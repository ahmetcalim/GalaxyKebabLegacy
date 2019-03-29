using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderControl : MonoBehaviour
{
    public Text customerName;
    public List<TasteItem> tasteItems;
    public TasteItem average;

    public void SetValues(Customer customer,List<Taste> tastes)
    {
        this.customerName.text = customer.customerName;
        for (int i = 0; i < tastes.Count; i++)
        {
            if (tastes[i].isLike)
                tasteItems[i].ingredient.color = Color.green;
            else
                tasteItems[i].ingredient.color = Color.red;


           tasteItems[i].ingredient.text =tastes[i].taste.ToString();
           tasteItems[i].value.text = tastes[i].tasteRating.ToString();
        }
        average.ingredient.text = "average";
        average.value.text = customer.averageTasteRatingnValue.ToString();
    }
}
[System.Serializable]
public class TasteItem
{
    public Text ingredient;
    public Text value;
}

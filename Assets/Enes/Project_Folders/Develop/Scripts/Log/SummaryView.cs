using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SummaryView : MonoBehaviour
{
    public SummaryIngredient prefabSummaryIngredient;
    public Text totalOrder, successOrder, dailyRating, globalFirst, globalLast;
    public Text totalCost, averageCost;
    public Transform orderView;
    public List<SessionIngredient> sessionIngrediens=new List<SessionIngredient>();
    public float f_totalCost;
    public float f_averageCost;

    public void CalculateOrderItems(List<SessionIngredient> _sessionIngredients)
    {
        foreach (SessionIngredient item in _sessionIngredients)
        {
            if (sessionIngrediens.Where(i=>i.ingredientID==item.ingredientID).Any())
            {
                SessionIngredient crr = sessionIngrediens.Where(i => i.ingredientID == item.ingredientID).FirstOrDefault();
                crr.totalInputAmount += item.totalInputAmount;
                crr.ingredientCost += item.ingredientCost;
            }
            else
            {
                sessionIngrediens.Add(item);
            }
        
        }
    }
    public void Print()
    {
        foreach (SessionIngredient item in sessionIngrediens)
        {
            f_totalCost += item.ingredientCost;
            prefabSummaryIngredient.i_name.text = item.ingredientName;
            prefabSummaryIngredient.i_cost.text = item.ingredientCost.ToString("0.##") + "$";
            prefabSummaryIngredient.i_amount.text = item.totalInputAmount.ToString();
            Instantiate(prefabSummaryIngredient.gameObject, orderView);
        }
        f_averageCost = f_totalCost / sessionIngrediens.Count;
    }
}


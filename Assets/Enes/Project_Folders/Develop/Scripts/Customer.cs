using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class Customer
{
    public string customerName;
    public GameObject model;
    public double averageTasteRatingnValue;
    public Personality personality;
    public List<Taste> Tastes;

    public double CalculateAverageSatisfactionValue()
    {
        averageTasteRatingnValue = 0;
        for (int i = 0; i < Tastes.Count; i++)
        {
            averageTasteRatingnValue += Tastes[i].tasteRating;
        }
        averageTasteRatingnValue = averageTasteRatingnValue / Tastes.Where(t => t.preference!=Taste.Preference.irrelevant).ToList().Count;
        return averageTasteRatingnValue;
    }

}

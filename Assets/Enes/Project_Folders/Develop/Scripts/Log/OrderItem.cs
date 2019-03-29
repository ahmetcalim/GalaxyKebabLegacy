using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderItem : MonoBehaviour
{
    public GameObject prefab;
    public List<Text> ingredients;
    public Text satisfaction;
    public Text customerName;
    public Text customerAverage;

    public void ClearTexts()
    {
        for (int i = 0; i < ingredients.Count; i++)
        {
            ingredients[i].text = "";
        }
        satisfaction.text = "";
        customerName.text = "";
        customerAverage.text = "";
    }
    public void SetColor(Color c)
    {
        this.GetComponent<Image>().color = c;
    }

}
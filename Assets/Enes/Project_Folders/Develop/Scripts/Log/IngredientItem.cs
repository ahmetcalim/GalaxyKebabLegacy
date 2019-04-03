using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientItem : MonoBehaviour
{
    public int ID;
    public GameLogic gLogic;

    private void OnEnable()
    {
        this.GetComponent<Button>().onClick.AddListener(() => Action());
    }

    public void Action()
    {
        gLogic.AddIngredient(this.ID);
    }
}

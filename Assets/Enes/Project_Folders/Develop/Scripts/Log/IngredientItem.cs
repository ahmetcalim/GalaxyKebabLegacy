using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientItem : MonoBehaviour
{
    public int ID;
    public GameLogic gLogic;
    

    public void Action()
    {
        gLogic.AddIngredient(this.ID);
    }
}

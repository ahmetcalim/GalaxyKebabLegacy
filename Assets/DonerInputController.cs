using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonerInputController : MonoBehaviour
{
    public RectTransform kadran;
    float xMax;
    public GameLogic gLogic;
    private void LateUpdate()
    {
        UpdateMeatBar();
    }
    public void UpdateMeatBar()
    {      
        kadran.anchoredPosition3D = new Vector3(gLogic.currentOrder.customer.c_Ingredients.Find(t => t.preference == CustomerIngredient.Preference.meat).ingredient.totalInputCount * 19f, kadran.anchoredPosition3D.y, kadran.anchoredPosition3D.z);
    }
}

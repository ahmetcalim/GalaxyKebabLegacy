using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientBehaviour : MonoBehaviour
{
    public SkinnedMeshRenderer skinned;
   
    public void UseIngredient()
    {
        skinned.SetBlendShapeWeight(0, skinned.GetBlendShapeWeight(0) - 1f);
    }
}

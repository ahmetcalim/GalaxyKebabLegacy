using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiceBehaviour : MonoBehaviour
{
    public SpiceContainerBehaviour spiceContainer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LavasIngredient")
        {
            if (GameLogic.hasOrder)
            {
                spiceContainer.Add();
            }
            Destroy(gameObject);
        }
    }
}

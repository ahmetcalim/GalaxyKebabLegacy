using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollBehaviour : MonoBehaviour
{
    private bool orderHasArrived;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Customer" && other.GetComponent<CustomerBehaviour>().isActive)
        {
            orderHasArrived = true;
            FindObjectOfType<RepomaticBehaviour>().ThrowDurum();
            FindObjectOfType<GameLogic>().FinishOrder();
            Destroy(gameObject);
        }
        if (other.tag == "WrongOrder")
        {
            FindObjectOfType<RepomaticBehaviour>().ThrowDurum();
            Destroy(gameObject, 5f);
        }
    }
}

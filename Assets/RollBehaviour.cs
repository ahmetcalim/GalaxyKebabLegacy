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
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;

        }
        if (other.tag == "WrongOrder")
        {
            FindObjectOfType<RepomaticBehaviour>().ThrowDurum();
            Destroy(gameObject, 5f);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Customer")
        {
            if (!orderHasArrived && other.GetComponent<CustomerBehaviour>().isActive)
            {
                transform.position = Vector3.MoveTowards(transform.position, other.GetComponent<CustomerBehaviour>().durumPosition.position, 0.001f);
                if (Vector3.Distance(transform.position, other.GetComponent<CustomerBehaviour>().durumPosition.position) <=.05f)
                {
                    orderHasArrived = true;
                    FindObjectOfType<RepomaticBehaviour>().ThrowDurum();
                    FindObjectOfType<GameLogic>().FinishOrder();
                    Destroy(gameObject);
                }
            }
            
           
        }

    }
}

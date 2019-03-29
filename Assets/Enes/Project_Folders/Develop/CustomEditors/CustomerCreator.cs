using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerCreator : MonoBehaviour
{
    public List<Customer> customers;
    public GameLogic gLogic;
    public void CreateCustomer()
    {
        foreach (Customer customer in customers)
        {
            gLogic.customers.Add(customer);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonerCollisionDeneme : MonoBehaviour
{
    public static Vector3 contactPoint;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Picak")
        {
            
            contactPoint = collision.GetContact(0).point;
            

        }
    }
}

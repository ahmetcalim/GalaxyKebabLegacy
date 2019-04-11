using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavasBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "DonerPiece")
        {
          
            if (collider.gameObject.GetComponent<FixedJoint>() == null)
            {
                collider.gameObject.AddComponent<FixedJoint>();
            }
            collider.gameObject.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();
        }
    }
}

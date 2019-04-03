using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavasBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "DonerPiece")
        {
          
            if (collider.gameObject.GetComponent<HingeJoint>() == null)
            {
                collider.gameObject.AddComponent<HingeJoint>();
            }
            collider.gameObject.GetComponent<HingeJoint>().connectedBody = GetComponent<Rigidbody>();
        }
    }
}

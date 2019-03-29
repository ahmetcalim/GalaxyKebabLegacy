using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavasBehaviour : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "DonerPiece")
        {
            Vector3 defaultScale = collision.gameObject.transform.localScale;
            collision.gameObject.transform.SetParent(gameObject.transform, true);
            collision.gameObject.transform.localScale = defaultScale;
            collision.gameObject.GetComponent<Rigidbody>().useGravity = false;
            collision.gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}

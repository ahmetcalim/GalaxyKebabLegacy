using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavasAddingManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AddableLavas")
        {
            Debug.Log("Lavas Eklendi.");
            WrapOMatic.lavasCount++;
            Destroy(other.gameObject);
        }
    }
}

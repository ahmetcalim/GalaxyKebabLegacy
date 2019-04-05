using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class RepomaticBehaviour : MonoBehaviour
{
    public GameObject durumPrefab;
    private bool canStart = false;
   
    public Transform durumPoint;
    private GameObject durumInstance;
    public LavasGenerator lavasGenerator;
    public static bool canThrow;
    public Transform durumSpawnPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Lavas")
        {
            Debug.Log("Yes");
            canThrow = false;
            canStart = true;
            durumInstance = Instantiate(durumPrefab, durumSpawnPoint.position, Quaternion.identity);
        }
        if (other.tag == "DonerPiece")
        {

            Destroy(other);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (canStart)
        {
            durumInstance.transform.position = Vector3.MoveTowards(durumInstance.transform.position, durumPoint.position, .003f);
        }
    } 
    public void ThrowDurum()
    {
        if (durumInstance != null)
        {
            Debug.Log("Throwlamam  gerek");
            if (durumInstance.transform.localPosition.y >= durumPoint.localPosition.y)
            {
                canThrow = true;
                canStart = false;
                LinearDrive.canUseWrapomatic = true;
                durumInstance.AddComponent<Rigidbody>();
                durumInstance.GetComponent<Rigidbody>().AddForce(Vector3.up * 7f, ForceMode.Impulse);
                durumInstance.GetComponent<Rigidbody>().AddForce(Vector3.left * 7f, ForceMode.Impulse);
            }
        }
    }
}

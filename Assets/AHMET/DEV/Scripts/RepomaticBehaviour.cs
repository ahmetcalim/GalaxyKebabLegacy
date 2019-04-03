using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepomaticBehaviour : MonoBehaviour
{
    public GameObject durumPrefab;
    private bool canStart = false;
    public Transform durumPoint;
    private GameObject durumInstance;
    public LavasGenerator lavasGenerator;
    public static bool canThrow;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Lavas")
        {
            canThrow = false;
            canStart = true;
            durumInstance = Instantiate(durumPrefab, transform.position, Quaternion.identity);
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
            durumInstance.transform.position = Vector3.MoveTowards(durumInstance.transform.position, durumPoint.position, .001f);
        }
    } 
    public void ThrowDurum()
    {
        if (durumInstance != null)
        {

            if (durumInstance.transform.position.y >= durumPoint.position.y)
            {
                canThrow = true;
                canStart = false;
                LavasGenerator.generatedLavasCount = 0;
                Destroy(lavasGenerator.currentLavas);
                durumInstance.AddComponent<Rigidbody>();
                durumInstance.GetComponent<Rigidbody>().AddForce(Vector3.up * 7f, ForceMode.Impulse);
                durumInstance.GetComponent<Rigidbody>().AddForce(Vector3.left * 7f, ForceMode.Impulse);
            }
        }
    }
}

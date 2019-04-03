using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonerKnifeBehaviour : MonoBehaviour
{
    public static bool isHit;
    private bool spawned;
    public  static float beginPointY;
    private float timeSinceTriggered;
    public static float velocity;
    public static GameObject currentDoner;
    public Transform donerObject;
    public static float distanceFromDoner;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Doner")
        {
            if (!spawned)
            {
                beginPointY = transform.localPosition.y;
                Debug.Log("Başlangıç değeri: " + beginPointY);
                spawned = true;
                currentDoner = Instantiate(FindObjectOfType<DonerController>().doner, transform.position, Quaternion.Euler(90f, 90f, 180f));
                StartCoroutine(CalculateTime());
            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Doner")
        {
            distanceFromDoner = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), new Vector3(donerObject.position.x, 0f, donerObject.position.z));
            Debug.Log(distanceFromDoner);
            velocity = Mathf.Abs(transform.localPosition.y - beginPointY);
            currentDoner.GetComponent<DonerBehaviour>().UpdateScaleOfDoner();
            isHit = true;
        }
            
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Doner")
        {
            spawned = false;
               isHit = false;
            if (currentDoner.GetComponent<Rigidbody>() == null || currentDoner.GetComponent<MeshCollider>() == null)
            {
                currentDoner.AddComponent<Rigidbody>();
                currentDoner.AddComponent<MeshCollider>();
            }
           
            timeSinceTriggered = 0f;
        }

    }
    IEnumerator CalculateTime()
    {
        yield return new WaitForSeconds(.01f);
       
        if (isHit)
        {
            
            timeSinceTriggered += 0.01f;
            StartCoroutine(CalculateTime());
        }
    }
}

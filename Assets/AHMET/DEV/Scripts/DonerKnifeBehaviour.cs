using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonerKnifeBehaviour : MonoBehaviour
{
    public static bool isHit;
    private bool spawned;
    private Vector3 contactPoint;
    public  static float beginPointY;
    private float timeSinceTriggered;
    public static float amount;
    public static float velocity;
    public static GameObject currentDoner;
    public Transform donerObject;
    public static float distanceFromDoner;
    private Collision col;
    public MeshCollider meshCollider;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ContactPoint")
        {
            contactPoint = collision.GetContact(0).point;
            meshCollider.isTrigger = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Doner")
        {
            if (!spawned)
            {
                beginPointY = transform.localPosition.y;
                Debug.Log("Başlangıç değeri: " + beginPointY);
                spawned = true;
                currentDoner = Instantiate(FindObjectOfType<DonerController>().doner, contactPoint, Quaternion.Euler(90f, 90f, 180f));
                StartCoroutine(CalculateTime());
            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Doner")
        {
            distanceFromDoner = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), new Vector3(donerObject.position.x, 0f, donerObject.position.z));
            amount = Mathf.Abs(transform.localPosition.y - beginPointY);
            velocity = GetComponent<Rigidbody>().velocity.magnitude;
            currentDoner.GetComponent<DonerBehaviour>().UpdateScaleOfDoner();
            isHit = true;
        }
            
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Doner")
        {
            meshCollider.isTrigger = false;
            spawned = false;
               isHit = false;
            
            if (currentDoner.GetComponent<Rigidbody>() == null || currentDoner.GetComponent<MeshCollider>() == null)
            {
                currentDoner.AddComponent<Rigidbody>();
                currentDoner.AddComponent<BoxCollider>();
                currentDoner.GetComponent<BoxCollider>().center = new Vector3(currentDoner.GetComponent<BoxCollider>().center.x, currentDoner.GetComponent<BoxCollider>().center.y, Mathf.Abs(currentDoner.GetComponent<DonerBehaviour>().startPoint.localPosition.z - currentDoner.GetComponent<DonerBehaviour>().endPoint.localPosition.z) / 2f);
                currentDoner.GetComponent<BoxCollider>().size = new Vector3(currentDoner.GetComponent<BoxCollider>().size.x, currentDoner.GetComponent<BoxCollider>().size.y, Mathf.Abs(currentDoner.GetComponent<DonerBehaviour>().startPoint.localPosition.z - currentDoner.GetComponent<DonerBehaviour>().endPoint.localPosition.z));
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

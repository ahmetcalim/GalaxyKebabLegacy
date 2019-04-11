using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonerBehaviour : MonoBehaviour
{
    public Transform endPoint;
    public Transform startPoint;
    public float amount = 0f;
    private float amountMax = .8f;
    private float maxScaleX = 2f;
    public void UpdateScaleOfDoner()
    {
        if (DonerKnifeBehaviour.velocity <= amountMax)
        {
            endPoint.localPosition = new Vector3(endPoint.localPosition.x, endPoint.localPosition.y, DonerKnifeBehaviour.velocity);
            transform.localScale = new Vector3(transform.localScale.x, (maxScaleX - DonerKnifeBehaviour.distanceFromDoner) * 2f, transform.localScale.z);
        }
        else
        {
            if (gameObject.GetComponent<BoxCollider>() == null)
            {
                gameObject.AddComponent<BoxCollider>();
                
            }
            else
            {
                DonerKnifeBehaviour.beginPointY = FindObjectOfType<DonerKnifeBehaviour>().transform.localPosition.y;
                DonerKnifeBehaviour.velocity = 0f;
                DonerKnifeBehaviour.currentDoner = Instantiate(FindObjectOfType<DonerController>().doner, new Vector3(FindObjectOfType<DonerKnifeBehaviour>().transform.localPosition.x, endPoint.position.y, FindObjectOfType<DonerKnifeBehaviour>().transform.localPosition.z), transform.rotation);
                gameObject.GetComponent<BoxCollider>().center = new Vector3(gameObject.GetComponent<BoxCollider>().center.x, gameObject.GetComponent<BoxCollider>().center.y, Mathf.Abs(startPoint.localPosition.z - endPoint.localPosition.z)/2f);
                gameObject.GetComponent<BoxCollider>().size = new Vector3(gameObject.GetComponent<BoxCollider>().size.x, gameObject.GetComponent<BoxCollider>().size.y, Mathf.Abs(startPoint.localPosition.z - endPoint.localPosition.z));
                gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            }
        }
    }
}

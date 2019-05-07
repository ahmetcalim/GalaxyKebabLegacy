using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonerBehaviour : MonoBehaviour
{
    public Transform endPoint;
    public Transform startPoint;
    public float amount = 0f;
    private GameLogic gameLogic;
    private float amountMax = .8f;
    private float maxScaleX = 2f;
    public void UpdateScaleOfDoner()
    {
        if (DonerKnifeBehaviour.velocity > .3 && endPoint.localPosition.z <= .8f)
        {
            endPoint.localPosition = new Vector3(endPoint.localPosition.x, endPoint.localPosition.y, 0.8f / DonerKnifeBehaviour.velocity);
            transform.localScale = new Vector3(transform.localScale.x, (maxScaleX - DonerKnifeBehaviour.distanceFromDoner) * 2f, transform.localScale.z);
            
        }
        else
        {
            if (endPoint.localPosition.z < 0.4f)
            {
                endPoint.localPosition = new Vector3(endPoint.localPosition.x, endPoint.localPosition.y, .4f);
            }
            if (endPoint.localPosition.z > .8f)
            {
                endPoint.localPosition = new Vector3(endPoint.localPosition.x, endPoint.localPosition.y, .8f);
            }
            if (gameObject.GetComponent<BoxCollider>() == null)
            {
                gameObject.AddComponent<BoxCollider>();
            }
            else
            {
                DonerKnifeBehaviour.beginPointY = FindObjectOfType<DonerKnifeBehaviour>().transform.localPosition.y;
                DonerKnifeBehaviour.currentDoner = Instantiate(FindObjectOfType<DonerController>().doner, DonerKnifeBehaviour.contactPoint - new Vector3(0f, endPoint.localPosition.z, 0f), transform.rotation);
                gameObject.GetComponent<BoxCollider>().center = new Vector3(gameObject.GetComponent<BoxCollider>().center.x, gameObject.GetComponent<BoxCollider>().center.y, Mathf.Abs(startPoint.localPosition.z - endPoint.localPosition.z) / 2f);
                gameObject.GetComponent<BoxCollider>().size = new Vector3(gameObject.GetComponent<BoxCollider>().size.x, gameObject.GetComponent<BoxCollider>().size.y, Mathf.Abs(startPoint.localPosition.z - endPoint.localPosition.z));
                gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
              
            }


        }
    }
    private void Start()
    {
        gameLogic = FindObjectOfType<GameLogic>();
    }
    private void OnTriggerEnter(Collider other)
    {
        gameLogic.AddMeat(22,endPoint.position.z,transform.localScale.y);
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}

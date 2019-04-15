using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class InteractionHandler : MonoBehaviour
{
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabAction;
    public GameObject CollidingObject { get; set; }
    public GameObject ObjectInHand { get; set; }
    public bool IsShaking { get; set; }
    public float power;
    public Hand hand;
    public List<RectTransform> kadranlar = new List<RectTransform>();
    public List<Text> texts = new List<Text>();
    private Vector3 kadranDefault;
    public Test ingredientGradientTest;
    private void Start()
    {
    }
    private void Update()
    {
        if (grabAction.GetStateDown(handType))
        {
            if (hand.currentAttachedObject != null)
            {

                if (hand.currentAttachedObject.GetComponent<SpiceContainerBehaviour>() != null)
                {
                    ObjectInHand = hand.currentAttachedObject;
                    hand.currentAttachedObject.GetComponent<SpiceContainerBehaviour>().isInHand = true;
                    hand.currentAttachedObject.GetComponent<Rigidbody>().useGravity = true;
                    hand.currentAttachedObject.GetComponent<Rigidbody>().isKinematic = false;
                            if (transform.eulerAngles.z > 90f && transform.eulerAngles.z < 270f)
                            {
                                hand.currentAttachedObject.transform.localScale = new Vector3(hand.currentAttachedObject.transform.localScale.x, hand.currentAttachedObject.transform.localScale.y *-1f, hand.currentAttachedObject.transform.localScale.z);
                            }
                            else
                            {
                            if (hand.currentAttachedObject.transform.localScale.y <0)
                            {
                                hand.currentAttachedObject.transform.localScale = new Vector3(hand.currentAttachedObject.transform.localScale.x, hand.currentAttachedObject.transform.localScale.y * -1f, hand.currentAttachedObject.transform.localScale.z);

                            }
                            else
                            {
                                hand.currentAttachedObject.transform.localScale = new Vector3(hand.currentAttachedObject.transform.localScale.x, hand.currentAttachedObject.transform.localScale.y * 1f, hand.currentAttachedObject.transform.localScale.z);
                            }

                    }


                }
              
            }
        }
        if (grabAction.GetLastStateUp(handType))
        {
          
            if (ObjectInHand != null)
            {
                ObjectInHand.GetComponent<Rigidbody>().useGravity = true;
                ObjectInHand.GetComponent<Rigidbody>().isKinematic = false;
                ObjectInHand.GetComponent<SpiceContainerBehaviour>().isInHand = false;
                switch (handType)
                {
                    case SteamVR_Input_Sources.LeftHand:
                        ingredientGradientTest.ResetGradients(0, false);
                        break;
                    case SteamVR_Input_Sources.RightHand:
                        ingredientGradientTest.ResetGradients(1, false);
                        break;
                    default:
                        break;
                }

            }
        }
        CheckDonerPieceGrabbed();
        if (controllerPose.GetVelocity().magnitude > power)
        {

            if (!IsShaking)
            {
                IsShaking = true;
                Shake();
                
            }
        }
        else
        {
            IsShaking = false;
        }
    }

    void CheckDonerPieceGrabbed()
    {
        if (hand.currentAttachedObject != null)
        {
            if (hand.currentAttachedObject.GetComponent<SpiceContainerBehaviour>() != null)
            {
                switch (handType)
                {
                    case SteamVR_Input_Sources.LeftHand:
                        ingredientGradientTest.ResetGradients(0, true);
                        break;
                    case SteamVR_Input_Sources.RightHand:
                        ingredientGradientTest.ResetGradients(1, true);
                        break;
                    default:
                        break;
                }
                hand.currentAttachedObject.GetComponent<SpiceContainerBehaviour>().UpdateBar(hand, kadranlar, texts);
                hand.currentAttachedObject.GetComponent<SpiceContainerBehaviour>().Particle();
            }
            
            if (hand.currentAttachedObject.GetComponent<DonerBehaviour>() != null)
            {
                if (hand.currentAttachedObject.GetComponent<FixedJoint>() != null)
                {

                    Destroy(hand.currentAttachedObject.GetComponent<FixedJoint>());
                    Debug.Log("Tuttum");
                }
            }

        }
    }
    private void Shake()
    {
        if (hand.currentAttachedObject != null)
        {
            if (hand.currentAttachedObject.GetComponent<SpiceContainerBehaviour>() != null)
            {
                hand.currentAttachedObject.GetComponent<SpiceContainerBehaviour>().InstantiateIngredient(hand, kadranlar, texts);
                hand.currentAttachedObject.GetComponent<SpiceContainerBehaviour>().Particle();
            }
        
            hand.hapticAction.Execute(3f, 3f, 300f, 1f, handType);
        }
    }
    private void Grab(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        ObjectInHand = CollidingObject;
        CollidingObject = null;
        if (GetComponent<FixedJoint>() == null)
        {
            var joint = AddFixedJoint();
            if (ObjectInHand != null)
            {
                joint.connectedBody = ObjectInHand.GetComponent<Rigidbody>();
            }
        }
    }
    private void Release(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (GetComponent<FixedJoint>())
        {
            if (ObjectInHand != null)
            {

                GetComponent<FixedJoint>().connectedBody = null;
                Destroy(GetComponent<FixedJoint>());
                ObjectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
                ObjectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
                ObjectInHand = null;
            }

        }
     
    }
    private void SetCollidingObject(Collider col)
    {
        if (CollidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        CollidingObject = col.gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }
    private void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!CollidingObject)
        {
            return;
        }
        CollidingObject = null;
    }
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

}

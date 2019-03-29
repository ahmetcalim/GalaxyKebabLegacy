using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    private void OnEnable()
    {
        controllerPose.onTransformChanged.AddListener(OnControllerTransformChanged);
    }
    private void OnControllerTransformChanged(SteamVR_Behaviour_Pose fromPose, SteamVR_Input_Sources fromSource)
    {
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
    private void Shake()
    {
        if (hand.currentAttachedObject != null)
        {
            if (hand.currentAttachedObject.GetComponent<SpiceContainerBehaviour>() != null)
            {
                hand.currentAttachedObject.GetComponent<SpiceContainerBehaviour>().Add();
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

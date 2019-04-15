//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Drives a linear mapping based on position between 2 positions
//
//=============================================================================

using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]
public class ParameterEvent : UnityEvent<int>
{

}
namespace Valve.VR.InteractionSystem
{
   
    //-------------------------------------------------------------------------
    [RequireComponent( typeof( Interactable ) )]
	public class LinearDrive : MonoBehaviour
    {
    
        public UnityEvent onEndPoint;
        public ParameterEvent onIngredientTrayChanged;
        public ParameterEvent onIngredientTrayChangedRight;
        public Transform startPosition;
		public Transform endPosition;
		public LinearMapping linearMapping;
		public bool repositionGameObject = true;
		public bool maintainMomemntum = true;
		public float momemtumDampenRate = 5.0f;
        public bool arrivedToEndpoint;
        protected Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.DetachFromOtherHand;
        public static bool canUseWrapomatic;
        protected float initialMappingOffset;
        protected int numMappingChangeSamples = 5;
        protected float[] mappingChangeSamples;
        protected float prevMapping = 0.0f;
        protected float mappingChangeRate;
        protected int sampleCount = 0;
        protected Interactable interactable;
        private int currentPoint = 0;
        public List<Transform> points;
        public Animator voidAnimator;
        public enum LinearDriveFor
        {
            WRAPOMATIC,
            INGREDIENT_1,
            INGREDIENT_2
        }
        public LinearDriveFor linearDriveFor;
        protected virtual void Awake()
        {
            mappingChangeSamples = new float[numMappingChangeSamples];
            interactable = GetComponent<Interactable>();
        }

        protected virtual void Start()
		{
            canUseWrapomatic = true;
			if ( linearMapping == null )
			{
				linearMapping = GetComponent<LinearMapping>();
			}

			if ( linearMapping == null )
			{
				linearMapping = gameObject.AddComponent<LinearMapping>();
			}

            initialMappingOffset = linearMapping.value;

			if ( repositionGameObject )
			{
				UpdateLinearMapping( transform );
			}
            switch (linearDriveFor)
            {
                case LinearDriveFor.WRAPOMATIC:
                    break;
                case LinearDriveFor.INGREDIENT_1:
                    voidAnimator.SetTrigger("OpenVoid");
                    StartCoroutine(ChangeIngredients());
                    break;
                case LinearDriveFor.INGREDIENT_2:
                    voidAnimator.SetTrigger("OpenVoid");
                    StartCoroutine(ChangeIngredients());
                    break;
                default:
                    break;
            }
        }

        protected virtual void HandHoverUpdate( Hand hand )
        {
            GrabTypes startingGrabType = hand.GetGrabStarting();

            if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
            {
                initialMappingOffset = linearMapping.value - CalculateLinearMapping( hand.transform );
				sampleCount = 0;
				mappingChangeRate = 0.0f;

                hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
            }
		}

        protected virtual void HandAttachedUpdate(Hand hand)
        {
            UpdateLinearMapping(hand.transform);

            if (hand.IsGrabEnding(this.gameObject))
            {
                hand.DetachObject(gameObject);
            }
        }

        protected virtual void OnDetachedFromHand(Hand hand)
        {
          
            CalculateMappingChangeRate();
            switch (linearDriveFor)
            {
                case LinearDriveFor.WRAPOMATIC:
                    break;
                case LinearDriveFor.INGREDIENT_1:
                    voidAnimator.SetTrigger("OpenVoid");
                    StartCoroutine(ChangeIngredients());
                    break;
                case LinearDriveFor.INGREDIENT_2:
                    voidAnimator.SetTrigger("OpenVoid");
                    StartCoroutine(ChangeIngredients());
                    break;
                default:
                    break;
            }
        }

        IEnumerator ChangeIngredients()
        {
            yield return new WaitForSeconds(1f);
            switch (linearDriveFor)
            {
                case LinearDriveFor.WRAPOMATIC:
                    break;
                case LinearDriveFor.INGREDIENT_1:
                    onIngredientTrayChanged.Invoke(currentPoint);
                    break;
                case LinearDriveFor.INGREDIENT_2:
                    onIngredientTrayChangedRight.Invoke(currentPoint);
                    break;
                default:
                    break;
            }
        }
        protected void CalculateMappingChangeRate()
		{
			//Compute the mapping change rate
			mappingChangeRate = 0.0f;
			int mappingSamplesCount = Mathf.Min( sampleCount, mappingChangeSamples.Length );
			if ( mappingSamplesCount != 0 )
			{
				for ( int i = 0; i < mappingSamplesCount; ++i )
				{
					mappingChangeRate += mappingChangeSamples[i];
				}
				mappingChangeRate /= mappingSamplesCount;
			}
		}

        protected void UpdateLinearMapping( Transform updateTransform )
		{
			prevMapping = linearMapping.value;
			linearMapping.value = Mathf.Clamp01( initialMappingOffset + CalculateLinearMapping( updateTransform ) );

			mappingChangeSamples[sampleCount % mappingChangeSamples.Length] = ( 1.0f / Time.deltaTime ) * ( linearMapping.value - prevMapping );
			sampleCount++;

			if ( repositionGameObject )
			{
				transform.position = Vector3.Lerp( startPosition.position, endPosition.position, linearMapping.value );
			}
		}

        protected float CalculateLinearMapping( Transform updateTransform )
		{
			Vector3 direction = endPosition.position - startPosition.position;
			float length = direction.magnitude;
			direction.Normalize();

			Vector3 displacement = updateTransform.position - startPosition.position;

			return Vector3.Dot( displacement, direction ) / length;
		}

        private void OnTriggerEnter(Collider other)
        {
            switch (linearDriveFor)
            {
                case LinearDriveFor.WRAPOMATIC:
                    if (other.tag == "EndPoint" && !arrivedToEndpoint)
                    {
                        if (canUseWrapomatic)
                        {
                            onEndPoint.Invoke();
                        }
                        canUseWrapomatic = false;
                        arrivedToEndpoint = true;


                        Debug.Log("Teslim et > Lavaş oluştur.");
                    }
                    if (other.tag == "StartPoint" && arrivedToEndpoint)
                    {
                        arrivedToEndpoint = false;
                        UpdateLinearMapping(transform);
                        Debug.Log("Çıktı");
                    }
                    break;
                case LinearDriveFor.INGREDIENT_1:
                    Debug.Log("SOL TRİGGER ÇALIŞTI");
                    if (other.tag == "Point_1")
                    {
                        currentPoint = 0;
                        
                        Debug.Log("Tepsi 1");
                        //Birinci tepsi
                    }
                    if (other.tag == "Point_2")
                    {
                        currentPoint = 1;
                        Debug.Log("Tepsi 2");
                        //İkinci tepsi
                    }
                    if (other.tag == "Point_3")
                    {
                        currentPoint = 2;
                        Debug.Log("Tepsi 3");
                        //Üçüncü tepsi
                    }
                    if (other.tag == "Point_4")
                    {
                        currentPoint = 3;
                        Debug.Log("Tepsi 4");
                        //Dördüncü tepsi
                    }
                    break;
                case LinearDriveFor.INGREDIENT_2:
                    Debug.Log("SAĞ TRİGGER ÇALIŞTI");
                    if (other.tag == "Point_1R")
                    {
                        currentPoint = 0;
                        Debug.Log("Tepsi 1");
                        //Birinci tepsi
                    }
                    if (other.tag == "Point_2R")
                    {
                        currentPoint = 1;
                        Debug.Log("Tepsi 2");
                        //İkinci tepsi
                    }
                    if (other.tag == "Point_3R")
                    {
                        currentPoint = 2;
                        Debug.Log("Tepsi 3");
                        //Üçüncü tepsi
                    }
                    if (other.tag == "Point_4R")
                    {
                        currentPoint = 3;
                        Debug.Log("Tepsi 4");
                        //Dördüncü tepsi
                    }
                   

                    break;
                default:
                    break;
            }
           
           
        }
        protected virtual void Update()
        {
            switch (linearDriveFor)
            {
                case LinearDriveFor.WRAPOMATIC:
                    if (maintainMomemntum && mappingChangeRate != 0.0f)
                    {
                        //Dampen the mapping change rate and apply it to the mapping
                        mappingChangeRate = Mathf.Lerp(mappingChangeRate, 0.0f, momemtumDampenRate * Time.deltaTime);
                        linearMapping.value = Mathf.Clamp01(linearMapping.value + (mappingChangeRate * Time.deltaTime));

                        if (repositionGameObject)
                        {
                            transform.position = Vector3.Lerp(startPosition.position, endPosition.position, linearMapping.value);
                        }


                    }
                    else
                    {
                        if (arrivedToEndpoint)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, startPosition.position, 0.1f);
                        }
                    }
                    break;
                case LinearDriveFor.INGREDIENT_1:
                    if (maintainMomemntum && mappingChangeRate != 0.0f)
                    {
                        //Dampen the mapping change rate and apply it to the mapping
                        mappingChangeRate = Mathf.Lerp(mappingChangeRate, 0.0f, momemtumDampenRate * Time.deltaTime);
                        linearMapping.value = Mathf.Clamp01(linearMapping.value + (mappingChangeRate * Time.deltaTime));

                        if (repositionGameObject)
                        {
                            transform.position = Vector3.Lerp(startPosition.position, endPosition.position, linearMapping.value);
                        }
                    }
                    else
                    {
                        transform.position = points[currentPoint].position;

                    }
                    break;
                default:
                    break;
            }
            
          

        }
    }
}

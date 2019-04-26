using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class SpiceContainerBehaviour : MonoBehaviour
{
    public enum SpiceType { salt, blackPepper, chiliPepper, mayonnaise, mustard, ketchup }
    public SpiceType spiceType;
    public bool isInHand;
    public int amountPerShake;
    public GameObject particleSystem;
    public bool isInMachine;
    public SkinnedMeshRenderer skinnedMesh;
    public bool isFluid;
    public bool canAdd;
    public GameLogic gLogic;
    private int tasteIndex;
    public GameObject ingredientObj;
    private Hand _hand;
    CustomerIngredient ingredientItem;
    Ingredient ingredient;
    RectTransform _rectTransform;
    Text _txt;

    private int lastIngredientID;
    
    int xMax;
    public Test test;
    
    public void UseIngredient(float amount)
    {

        if (!isFluid)
        {

            if (skinnedMesh.GetBlendShapeWeight(0) >= 10f)
            {
                Debug.Log(skinnedMesh.GetBlendShapeWeight(0));
                skinnedMesh.SetBlendShapeWeight(0, skinnedMesh.GetBlendShapeWeight(0) - amount);
            }
        }
      
    }
    public void InstantiateIngredient(Hand hand, RectTransform rectTransform, Text txt)
    {
        _hand = hand;
        _txt = txt;
        _rectTransform = rectTransform;
        GameObject copy = Instantiate(ingredientObj, particleSystem.transform.position, Quaternion.identity);
        copy.GetComponent<SpiceBehaviour>().spiceContainer = GetComponent<SpiceContainerBehaviour>();
    }
               
    public void Add()
    {
        GetComponent<IngredientItem>().Action();
    }
    public void UpdateBar(Hand hand, RectTransform rectTransform, Text txt)
    {
        if (hand.currentAttachedObject != null)
        {
            if (hand.currentAttachedObject.GetComponent<SpiceContainerBehaviour>() != null)
            {
                ingredientItem = gLogic.currentOrder.customer.c_Ingredients.Where(i => i.ingredient.ID == hand.currentAttachedObject.GetComponent<IngredientItem>().ID).ToList().FirstOrDefault();
                

                if (ingredientItem != null)
                {

                    xMax = ingredientItem.x_max;
                    txt.text = ingredientItem.ingredient.ToString();
                    rectTransform.anchoredPosition3D = new Vector3((float)ingredientItem.ingredient.totalInputCount * 19f, rectTransform.anchoredPosition3D.y, rectTransform.anchoredPosition3D.z);
                    UpdateGradient(hand);
                }
                else
                {
                    Irrelevant(hand, rectTransform, txt);
                }
            }
            else
            {
                ingredientItem = null;
                ingredient = null;
            }
        }
    }
    private void UpdateGradient(Hand hand)
    {
        if (ingredientItem.preference == CustomerIngredient.Preference.like)
        {
            if (hand.handType == Valve.VR.SteamVR_Input_Sources.LeftHand)
            {
                test.SetGradient1((float)xMax, 0);
            }
            else
            {
                test.SetGradient2((float)xMax, 0);
            }
        }
        else if (ingredientItem.preference == CustomerIngredient.Preference.dislike)
        {
            if (hand.handType == Valve.VR.SteamVR_Input_Sources.LeftHand)
            {
                test.SetGradient1((float)xMax, 1);
            }
            else
            {
                test.SetGradient2((float)xMax, 1);
            }
        }
    }

    private void Irrelevant(Hand hand, RectTransform rectTransform, Text txt)
    {
        //Irrelevant
        ingredient = gLogic.ingredients.Where(i => i.ID == hand.currentAttachedObject.GetComponent<IngredientItem>().ID).ToList().FirstOrDefault();
        if (hand.handType == Valve.VR.SteamVR_Input_Sources.LeftHand)
        {
            test.SetGradient1((float)ingredient.rating, 2);
        }
        else
        {
            test.SetGradient2((float)ingredient.rating, 2);
        }
        txt.text = ingredient.ingredientName.ToString();
        if (rectTransform.anchoredPosition3D.x <= 1850)
        {
            rectTransform.anchoredPosition3D = new Vector3((float)ingredient.totalInputCount * 19f, rectTransform.anchoredPosition3D.y, rectTransform.anchoredPosition3D.z);
        }
            
    }

    public void Particle()
    {
        StartCoroutine(ActivateParticle());
    }
    public IEnumerator ActivateParticle()
    {
       // particleSystem.Play();
        yield return new WaitForSeconds(1f);
       // if (particleSystem.isPlaying)
        //{
          //  particleSystem.Stop();
        //}
    }
    }

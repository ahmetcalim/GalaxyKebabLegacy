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
    public int amountPerShake;
    public GameObject particleSystem;
    public bool isInMachine;
    public SkinnedMeshRenderer skinnedMesh;
    public bool isFluid;
    public bool canAdd;
    public GameLogic gLogic;
    private int tasteIndex;
    List<Taste> tastes = new List<Taste>();
    public GameObject ingredientObj;
    private Hand _hand;

    List<RectTransform> _rectTransforms = new List<RectTransform>();
    List<Text> _txts = new List<Text>();
    private Taste taste_1;
    private Taste taste_2;

    private int lastIngredientID;
    
    float[] xMaxes = new float[2];
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
    public void InstantiateIngredient(Hand hand, List<RectTransform> rectTransforms, List<Text> txts)
    {
        Debug.Log(txts.Count);
        _hand = hand;
        _txts = txts;
        _rectTransforms = rectTransforms;
        GameObject copy = Instantiate(ingredientObj, particleSystem.transform.position, Quaternion.identity);
        copy.GetComponent<SpiceBehaviour>().spiceContainer = GetComponent<SpiceContainerBehaviour>();
    }
               
    public void Add()
    {
        GetComponent<IngredientItem>().Action();
        CheckTasteInput(_hand, _rectTransforms, _txts);
    }
    public void UpdateBar(Hand hand, List<RectTransform> rectTransforms, List<Text> txts)
    {
        if (hand.currentAttachedObject != null)
        {
            if (hand.currentAttachedObject.GetComponent<SpiceContainerBehaviour>() != null)
            {
                if (gLogic.ingredients[hand.currentAttachedObject.GetComponent<IngredientItem>().ID].tastes[0].tasteInput > gLogic.ingredients[hand.currentAttachedObject.GetComponent<IngredientItem>().ID].tastes[1].tasteInput)
                {
                    tasteIndex = 0;
                }
                else
                {
                    tasteIndex = 1;
                }
                foreach (var item in gLogic.currentOrder.customer.Tastes)
                {

                    if (item.taste == gLogic.ingredients[hand.currentAttachedObject.GetComponent<IngredientItem>().ID].tastes[0].taste.taste)
                    {
                        taste_1 = item;
                    }
                    if (item.taste == gLogic.ingredients[hand.currentAttachedObject.GetComponent<IngredientItem>().ID].tastes[1].taste.taste)
                    {
                        taste_2 = item;
                    }
                }
                xMaxes[0] = (float)taste_1.x_max;
                txts[0].text = taste_1.taste.ToString();
                foreach (var item in gLogic.currentOrder.customer.Tastes)
                {
                    if (item == taste_1)
                    {
                        rectTransforms[0].anchoredPosition3D = new Vector3((float)item.totalInputCount * 19f, rectTransforms[0].anchoredPosition3D.y, rectTransforms[0].anchoredPosition3D.z);
                    }
                }


                xMaxes[1] = (float)taste_2.x_max;

                foreach (var item in gLogic.currentOrder.customer.Tastes)
                {
                    if (item == taste_2)
                    {
                        rectTransforms[1].anchoredPosition3D = new Vector3((float)item.totalInputCount * 19f, rectTransforms[1].anchoredPosition3D.y, rectTransforms[1].anchoredPosition3D.z);
                    }
                }

                txts[1].text = taste_2.taste.ToString();
                
                    if (taste_1.preference == Taste.Preference.like)
                    {
                        if (hand.handType == Valve.VR.SteamVR_Input_Sources.LeftHand)
                        {
                            test.SetGradient1(xMaxes[0], 0);
                        }
                        else
                        {
                            test.SetGradient2(xMaxes[0], 0);
                        }
                    }
                    if (taste_1.preference == Taste.Preference.dislike)
                    {
                        if (hand.handType == Valve.VR.SteamVR_Input_Sources.LeftHand)
                        {
                            test.SetGradient1(xMaxes[0], 1);
                        }
                        else
                        {
                            test.SetGradient2(xMaxes[0], 1);
                        }
                    }
                    if (taste_1.preference == Taste.Preference.irrelevant)
                    {
                        if (hand.handType == Valve.VR.SteamVR_Input_Sources.LeftHand)
                        {
                            test.SetGradient1(xMaxes[0], 2);
                        }
                        else
                        {
                            test.SetGradient2(xMaxes[0], 2);
                        }
                    }
                    //İkinci Taste
                    if (taste_2.preference == Taste.Preference.like)
                    {
                        if (hand.handType == Valve.VR.SteamVR_Input_Sources.LeftHand)
                        {
                            test.SetGradient3(xMaxes[1], 0);
                        }
                        else
                        {
                            test.SetGradient4(xMaxes[1], 0);
                        }
                    }
                    if (taste_2.preference == Taste.Preference.dislike)
                    {
                        if (hand.handType == Valve.VR.SteamVR_Input_Sources.LeftHand)
                        {
                            test.SetGradient3(xMaxes[1], 1);
                        }
                        else
                        {
                            test.SetGradient4(xMaxes[1], 1);
                        }
                    }
                    if (taste_2.preference == Taste.Preference.irrelevant)
                    {
                        if (hand.handType == Valve.VR.SteamVR_Input_Sources.LeftHand)
                        {
                            test.SetGradient3(xMaxes[1], 2);
                        }
                        else
                        {
                            test.SetGradient4(xMaxes[1], 2);
                        }
                    }
            }
        }
    }
    public void CheckTasteInput(Hand hand, List<RectTransform> rectTransforms, List<Text> txts)
    {
        if (hand.currentAttachedObject != null)
        {
            if (hand.currentAttachedObject.GetComponent<SpiceContainerBehaviour>() != null)
            {
              
                foreach (var item in gLogic.currentOrder.customer.Tastes)
                {

                    if (item.taste == gLogic.ingredients[hand.currentAttachedObject.GetComponent<IngredientItem>().ID].tastes[0].taste.taste)
                    {
                        taste_1 = item;
                    }
                    if (item.taste == gLogic.ingredients[hand.currentAttachedObject.GetComponent<IngredientItem>().ID].tastes[1].taste.taste)
                    {
                        taste_2 = item;
                    }
                }
                xMaxes[0] = (float)taste_1.x_max;
                        txts[0].text = taste_1.taste.ToString();
                        if (rectTransforms[0].anchoredPosition3D.x <= 1900)
                        {
                            rectTransforms[0].anchoredPosition3D += new Vector3((gLogic.ingredients[hand.currentAttachedObject.GetComponent<IngredientItem>().ID].tastes[0].tasteInput) * 19f, 0f, 0f);
                        }



                xMaxes[1] = (float)taste_2.x_max;
                        txts[1].text = taste_2.taste.ToString();

                        if (rectTransforms[1].anchoredPosition3D.x <=1900)
                        {
                            rectTransforms[1].anchoredPosition3D += new Vector3((gLogic.ingredients[hand.currentAttachedObject.GetComponent<IngredientItem>().ID].tastes[1].tasteInput) * 19f, 0f, 0f);
                        }
                           
                        
            }
            if (taste_1.preference == Taste.Preference.like)
            {
                if (hand.handType == Valve.VR.SteamVR_Input_Sources.LeftHand)
                {
                    test.SetGradient1(xMaxes[0], 0);
                }
                else
                {
                    test.SetGradient2(xMaxes[0], 0);
                }
            }
            if (taste_1.preference == Taste.Preference.dislike)
            {
                if (hand.handType == Valve.VR.SteamVR_Input_Sources.LeftHand)
                {
                    test.SetGradient1(xMaxes[0], 1);
                }
                else
                {
                    test.SetGradient2(xMaxes[0], 1);
                }
            }
            if (taste_1.preference == Taste.Preference.irrelevant)
            {
                if (hand.handType == Valve.VR.SteamVR_Input_Sources.LeftHand)
                {
                    test.SetGradient1(xMaxes[0], 2);
                }
                else
                {
                    test.SetGradient2(xMaxes[0], 2);
                }
            }
            
            if (taste_2.preference == Taste.Preference.like)
            {
                if (hand.handType == Valve.VR.SteamVR_Input_Sources.LeftHand)
                {
                    test.SetGradient3(xMaxes[1], 0);
                }
                else
                {
                    test.SetGradient4(xMaxes[1], 0);
                }
            }
            if (taste_2.preference == Taste.Preference.dislike)
            {
                if (hand.handType == Valve.VR.SteamVR_Input_Sources.LeftHand)
                {
                    test.SetGradient3(xMaxes[1], 1);
                }
                else
                {
                    test.SetGradient4(xMaxes[1], 1);
                }
            }
            if (taste_2.preference == Taste.Preference.irrelevant)
            {
                if (hand.handType == Valve.VR.SteamVR_Input_Sources.LeftHand)
                {
                    test.SetGradient3(xMaxes[1], 2);
                }
                else
                {
                    test.SetGradient4(xMaxes[1], 2);
                }
            }

               
            
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

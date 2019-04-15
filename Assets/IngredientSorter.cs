using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class IngredientSorter : MonoBehaviour
{
    public GameLogic gameLogic;
    List<int> ingredient = new List<int>();
    public List<int> ingredients1;
    public List<int> ingredients2;
    public List<Transform> ingredientTransforms_1;
    public List<Transform> ingredientTransforms_2;
    public Transform ingredientParent;
    private Taste.Tastes taste_1;
    private Taste.Tastes taste_2;
    public LinearDrive linearDrive_1;
    public LinearDrive linearDrive_2;
    public IngredientItem[] ingredientItems;
    public Transform leftStorageParent;
    public Transform rightStorageParent;
    public Transform dummy;
    public List<Light> leftLights;
    public List<Light> rightLights;
    public List<IngredientItem> ingredientItemsUnsorted;
    public List<Transform> ingredientTransforms;
    int index1;
    int index2;
    private void Start()
    {

        SortIngredients();
        //linearDrive_1.onIngredientTrayChanged.AddListener(GetIngredientsToTray);
        //linearDrive_2.onIngredientTrayChangedRight.AddListener(GetIngredientsToTrayRight);
        // ingredients2 = GetIngredientsByTaste(taste_2);

        //RepositionIngredientsByTaste(ingredientTransforms_2, ingredientParent, ingredients2);
    }

    public void CheckTasteLights()
    {
        foreach (var item in gameLogic.currentOrder.customer.Tastes)
        {
            switch (item.taste)
            {
                case Taste.Tastes.Astringent:
                    if (item.preference == Taste.Preference.like)
                    {
                        leftLights[1].enabled = true;
                    }
                    else
                    {
                        leftLights[1].enabled = false;

                    }
                    break;
                case Taste.Tastes.Bitter:
                    if (item.preference == Taste.Preference.like)
                    {
                        rightLights[1].enabled = true;
                    }
                    else
                    {
                        rightLights[1].enabled = false;

                    }
                    break;
                case Taste.Tastes.Pungent:
                    if (item.preference == Taste.Preference.like)
                    {
                        rightLights[0].enabled = true;
                    }
                    else
                    {
                        rightLights[0].enabled = false;

                    }
                    break;
                case Taste.Tastes.Sour:
                    if (item.preference == Taste.Preference.like)
                    {
                        rightLights[2].enabled = true;
                        rightLights[3].enabled = true;
                    }
                    else
                    {
                        rightLights[2].enabled = false;
                        rightLights[3].enabled = false;

                    }
                    break;
                case Taste.Tastes.Salty:
                    if (item.preference == Taste.Preference.like)
                    {
                        leftLights[2].enabled = true;
                    }
                    else
                    {
                        leftLights[2].enabled = false;

                    }
                    break;
                case Taste.Tastes.Sweet:
                    if (item.preference == Taste.Preference.like)
                    {
                        leftLights[0].enabled = true;
                    }
                    else
                    {
                        leftLights[0].enabled = false;

                    }
                    break;
                case Taste.Tastes.Savory:
                    if (item.preference == Taste.Preference.like)
                    {
                        leftLights[3].enabled = true;
                    }
                    else
                    {
                        leftLights[3].enabled = false;

                    }
                    break;
                default:
                    break;
            }

        }
    }

    public void SortIngredients()
    {
        ingredientItemsUnsorted = ingredientItems.OrderBy(t => t.name).ToList();
        for (int i = 0; i < ingredientItemsUnsorted.Count; i++)
        {
            ingredientItemsUnsorted[i].transform.position = ingredientTransforms[i].position;
        }
    }
    public void GetIngredientsToTray(int tasteIndex)
    {
        switch (tasteIndex)
        {
            case 0:
                taste_1 = Taste.Tastes.Sweet;
                break;
            case 1:
                taste_1 = Taste.Tastes.Astringent;
                break;
            case 2:
                taste_1 = Taste.Tastes.Salty;
                break;
            case 3:
                taste_1 = Taste.Tastes.Savory;
                break;
            default:
                break;
        }
       
            ingredients1 = GetIngredientsByTaste(taste_1);
        RepositionIngredientsByTaste();
    }
    public void GetIngredientsToTrayRight(int tasteIndex)
    {
        Debug.Log("Sağ çalıştı");
        switch (tasteIndex)
        {
            case 0:
                taste_2 = Taste.Tastes.Pungent;
                break;
            case 1:
                taste_2 = Taste.Tastes.Bitter;
                break;
            case 2:
                taste_2 = Taste.Tastes.Sour;
                break;
            case 3:
                taste_2 = Taste.Tastes.Sour;
                break;
            default:
                break;
        }
        foreach (var item in gameLogic.currentOrder.customer.Tastes)
        {
            if (item.taste == taste_2)
            {

                if (item.preference == Taste.Preference.like)
                {
                    rightLights[tasteIndex].enabled = true;
                }
                else
                {
                    rightLights[tasteIndex].enabled = false;

                }
            }
        }
        ingredients2 = GetIngredientsByTaste(taste_2);
        RepositionIngredientsByTaste();
    }
    private List<int> GetIngredientsByTaste(Taste.Tastes tastes)
    {
        ingredient = new List<int>();
        foreach (var item in gameLogic.ingredients)
        {
            for (int i = 0; i < item.tastes.Count; i++)
            {
                if (item.tastes[i].taste.taste == tastes)
                {   ingredient.Add(item.ID);
                    
                }
            }
        }
        return ingredient;
    }
    private void RepositionIngredientsByTaste()
    {
        index1 = 0;
        index2 = 0;
        foreach (var item in ingredientItems)
        {
            if (!item.GetComponent<SpiceContainerBehaviour>().isInHand)
            {
                item.transform.SetParent(dummy);
                item.transform.position = dummy.position;
                item.GetComponent<Rigidbody>().useGravity = false;
                item.GetComponent<Rigidbody>().isKinematic = true;

            }
         
        }
        foreach (var item in ingredientItems)
        {
                if (ingredients1.Contains(item.ID) && !item.GetComponent<SpiceContainerBehaviour>().isInHand)
                {
                    item.transform.SetParent(leftStorageParent);
                    item.transform.position = ingredientTransforms_1[index1].position;
                item.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                index1++;
                }
                if (ingredients2.Contains(item.ID) && !item.GetComponent<SpiceContainerBehaviour>().isInHand)
                 {
                item.transform.SetParent(rightStorageParent);
                    item.transform.position = ingredientTransforms_2[index2].position;
                item.transform.localRotation = Quaternion.Euler(0f,0f,0f);
                    index2++;
                }
            
           
        }
    }
}

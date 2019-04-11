using System.Collections;
using System.Collections.Generic;
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
    public List<IngredientItem> ingredientItems;
    public Transform leftStorageParent;
    public Transform rightStorageParent;
    public Transform dummy;
    
    int index1;
    int index2;
    private void Start()
    {
        linearDrive_1.onIngredientTrayChanged.AddListener(GetIngredientsToTray);
        linearDrive_2.onIngredientTrayChangedRight.AddListener(GetIngredientsToTrayRight);
       // ingredients2 = GetIngredientsByTaste(taste_2);
       
        //RepositionIngredientsByTaste(ingredientTransforms_2, ingredientParent, ingredients2);
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
            item.transform.SetParent(dummy);
            item.transform.position = dummy.position;
            item.GetComponent<Rigidbody>().useGravity = false;
            item.GetComponent<Rigidbody>().isKinematic = true;
        }
        foreach (var item in ingredientItems)
        {
                if (ingredients1.Contains(item.ID))
                {

                    item.transform.SetParent(leftStorageParent);
                    item.transform.position = ingredientTransforms_1[index1].position;
                    item.GetComponent<Rigidbody>().useGravity = true;
                    item.GetComponent<Rigidbody>().isKinematic = false;
                    index1++;

                }
               
            
                if (ingredients2.Contains(item.ID))
                {


                    item.transform.SetParent(dummy);
                    item.transform.position = dummy.position;
                    item.GetComponent<Rigidbody>().useGravity = false;
                    item.GetComponent<Rigidbody>().isKinematic = true;


                    item.transform.SetParent(rightStorageParent);
                    item.transform.position = ingredientTransforms_2[index2].position;
                    item.GetComponent<Rigidbody>().useGravity = true;
                    item.GetComponent<Rigidbody>().isKinematic = false;
                    index2++;
                }
            
           
        }
    }
}

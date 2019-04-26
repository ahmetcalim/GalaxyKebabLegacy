using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

public class CustomerCreator
{
    static GameLogic gLogic;
    static System.Random rnd;
   [MenuItem("Galaxy Kebab Tools/Create/Customer List")]
    public static void CreateMyAsset()
    {
        gLogic = GameObject.FindObjectOfType<GameLogic>();
        gLogic.customers.Clear();
        string webString = "{ \"listXmaxXzero\":" + Resources.Load<TextAsset>("JsonData/xMax_xZero").text + "}";
        RootCustomerCreator container = JsonUtility.FromJson<RootCustomerCreator>(webString);
        rnd = new System.Random();
        for (int i = 0; i < 5; i++)
        {
            Customer customerAsset = ScriptableObject.CreateInstance<Customer>();
            customerAsset.customerName = "Alien" + (i + 1);
            customerAsset.model = gLogic.alienPrefabs[0];            
            AssetDatabase.CreateAsset(customerAsset, "Assets/Enes/Project_Folders/ScriptableObjects/Customers/Alien"+(i+1)+ ".asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = customerAsset;
            customerAsset.c_Ingredients = new List<CustomerIngredient>();
            for (int j = 0; j < 5; j++)
            {
                int rndValue = rnd.Next(0, gLogic.ingredients.Count);
                if (i==0)
                {
                    CustomerIngredient cIngredient = new CustomerIngredient { ingredient = gLogic.ingredients[rndValue] };
                    customerAsset.c_Ingredients.Add(cIngredient);
                }
            }
            
            gLogic.customers.Add(customerAsset);
        }

    }
    public static void SetIngredient(int i,Customer customer)
    {
        
        foreach (CustomerIngredient item in customer.c_Ingredients)
        {
            if (item.ingredient!=gLogic.ingredients[i])
            {
                customer.c_Ingredients.Add(new CustomerIngredient {ingredient=gLogic.ingredients[i]});
            }
            else
            {
                SetIngredient(rnd.Next(0, gLogic.ingredients.Count),customer);
            }
        }
    }
}
[Serializable]
public class RootCustomerCreator
{
    public List<XMaxXzeroObject> listXmaxXzero = new List<XMaxXzeroObject>();
}
[Serializable]
public class XMaxXzeroObject
{
    public string xZero;
    public float xMax;
    public float diffp0;
    public float diffp10;
}

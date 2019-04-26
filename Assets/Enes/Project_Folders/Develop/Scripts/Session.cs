using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Session
{
    public static string path = Application.persistentDataPath + "/session.txt";
    public List<SessionItem> sessionsItems;

    public void Activate()
    {
        sessionsItems = new List<SessionItem>();
        if (File.Exists(Session.path))
        {
            string jsonString = GetJsonPopularity();
            Debug.Log("jsonst: "+jsonString);
            Session s = ReadFromJSON(jsonString);
            this.sessionsItems = s.sessionsItems;
            Debug.Log("session active");
        }
    }
    public Session ReadFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Session>(jsonString);
    }
    public string GetJsonPopularity()
    {
        using (StreamReader sr = File.OpenText(Session.path))
        {
            return sr.ReadLine();
        }

    }
    public void SetGlobalPopularity()
    {
        if (!File.Exists(Session.path))
            using (StreamWriter sw = File.CreateText(Session.path))
                sw.WriteLine(JsonUtility.ToJson(this));
        else
            File.WriteAllText(Session.path, JsonUtility.ToJson(this));

    }
}
[Serializable]
public class SessionItem
{
    public int dayOfPlay;
    public string playingTime;
    [System.NonSerialized]
    public int orderCount = -1;
    public double dailyPopularity;
    public double averageCostOfSession;
    public List<SessionItemOrder> sessionOrders=new List<SessionItemOrder>();

    public SessionItem()
    {
        playingTime = DateTime.Now.ToString();
    }
    public void AddSessionItem()
    {
        orderCount++;
        sessionOrders.Add(new SessionItemOrder());
    }
    public void CalculateAverageCostOfSession()
    {
        averageCostOfSession = 0;
        for (int i = 0; i < sessionOrders.Count; i++)
        {
            averageCostOfSession += sessionOrders[i].averageCostOfOrder;
        }
        averageCostOfSession = averageCostOfSession / sessionOrders.Count;
    }
}
[Serializable]
public class SessionItemOrder
{
    public float averageCostOfOrder;
    public List<SessionIngredient> orderIngredients = new List<SessionIngredient>();

    public void CalculateAverageCostOfOrders()
    {
        averageCostOfOrder = 0;
        for (int i = 0; i < orderIngredients.Count; i++)
        {
            averageCostOfOrder += orderIngredients[i].ingredientCost;
        }
        averageCostOfOrder = averageCostOfOrder / orderIngredients.Count;
    }

    public void AddSessionIngredient(int ingredientID,string ingredientName,double ingredientSatisfaction,float ingredientCost, float totalInputAmount)
    {
        SessionIngredient result = orderIngredients.Where(oI => oI.ingredientID == ingredientID).FirstOrDefault();

        if (result!=null)
        {
            result.ingredientSatisfaction = ingredientSatisfaction;
            result.ingredientCost = ingredientCost;
            result.totalInputAmount = totalInputAmount;
        }
        else
        {
            SessionIngredient sCurrent = new SessionIngredient();
            sCurrent.ingredientID = ingredientID;
            sCurrent.ingredientName = ingredientName;
            orderIngredients.Add(sCurrent);
        }

        CalculateAverageCostOfOrders();
    }
}
[Serializable]
public class SessionIngredient
{
    public int ingredientID;
    public string ingredientName;
    public double ingredientSatisfaction;
    public float ingredientCost;
    public float totalInputAmount;

}



using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class Popularity
{
    public int userID=1;
    public double averageDailyPopularity;
    public double totalDailyPopularity;
    public double globalPopularity;
    public int activeDay; // actively played
    public int passiveDay; //have not played
    public float popularityDecayRate=1; //Pdec
    public float otherDecayRate = 1;  //c

    public void Activate()
    {
        if (File.Exists(DailyPopularity.path))
        {
            Debug.Log("popularity active");
            Popularity p = ReadFromJSON(GetJsonPopularity());
            userID = p.userID;
            averageDailyPopularity = p.averageDailyPopularity;
            totalDailyPopularity = p.totalDailyPopularity;
            globalPopularity = p.globalPopularity;
            popularityDecayRate = p.popularityDecayRate;
            activeDay = p.activeDay;
            passiveDay = p.passiveDay;
        }
    }

    public Popularity ReadFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Popularity>(jsonString);
    }

    public void CalculateDailyPopularity(double value)
    {
        DailyPopularity.index++;
        DailyPopularity.dailyPopularity += value;
    }

    public double CalculateGlobalPopularity()
    {
       return globalPopularity = totalDailyPopularity - (Mathf.Pow(passiveDay,2)/(passiveDay+Mathf.Pow(activeDay,2)))*popularityDecayRate*otherDecayRate;
    }

    public void SetGlobalPopularity()
    {
        activeDay += 1;
        totalDailyPopularity += DailyPopularity.dailyPopularity/DailyPopularity.index;
        averageDailyPopularity = totalDailyPopularity / activeDay;
        globalPopularity = CalculateGlobalPopularity();

        if (!File.Exists(DailyPopularity.path))    
            using (StreamWriter sw = File.CreateText(DailyPopularity.path))
                sw.WriteLine(JsonUtility.ToJson(this));
        else
            File.WriteAllText(DailyPopularity.path,JsonUtility.ToJson(this));

        DailyPopularity.dailyPopularity = 0;
        DailyPopularity.index = 0;
    }
    public string GetJsonPopularity()
    {
        using (StreamReader sr = File.OpenText(DailyPopularity.path))
        {
            return sr.ReadLine();
        }

    }
    public static class DailyPopularity
    {
        public static double dailyPopularity;
        public static string path = Application.persistentDataPath + "/popularity.txt";
        public static int index;
    }
}

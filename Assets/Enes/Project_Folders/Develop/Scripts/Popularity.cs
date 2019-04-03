using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class Popularity
{
    public int userID=1;
    public double averageDailyPopularity;
    public double globalPopularity;
    public int totalDay;

    public void Activate()
    {
        if (File.Exists(DailyPopularity.path))
        {
            Popularity p = ReadFromJSON(GetJsonPopularity());
            userID = p.userID;
            averageDailyPopularity = p.averageDailyPopularity;
            globalPopularity = p.globalPopularity;
            totalDay = p.totalDay;
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

    public void SetGlobalPopularity()
    {
        totalDay += 1;
        if (!File.Exists(DailyPopularity.path))
        {
            using (StreamWriter sw = File.CreateText(DailyPopularity.path))
            {
                globalPopularity =DailyPopularity.dailyPopularity / DailyPopularity.index;
                sw.WriteLine(JsonUtility.ToJson(this));
            }
        }
        else
        {
            globalPopularity =globalPopularity + DailyPopularity.dailyPopularity / DailyPopularity.index;
            File.WriteAllText(DailyPopularity.path,JsonUtility.ToJson(this));
        }
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

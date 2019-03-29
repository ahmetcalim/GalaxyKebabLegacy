using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class Popularity
{
    static int userID;
    static double globalPopularity;
    static List<Session> sessions;

    public static void Instance()
    {
        sessions = new List<Session>();
    }
    public static void CreateJson()
    {

    }
    public static Popularity ReadFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Popularity>(jsonString);
    }

    public static void CalculateDailyPopularity(double value)
    {
        DailyPopularity.index++;
        DailyPopularity.dailyPopularity += value;
    }
    public static void SetGlobalPopularity()
    {
        if (!File.Exists(DailyPopularity.path))
        {
            using (StreamWriter sw = File.CreateText(DailyPopularity.path))
            {
                sw.WriteLine(DailyPopularity.dailyPopularity / DailyPopularity.index);
            }
        }
        else
        {
            File.WriteAllText(DailyPopularity.path, (GetGlobalPopularity() + DailyPopularity.dailyPopularity / DailyPopularity.index).ToString());
        }
        DailyPopularity.dailyPopularity = 0;
        DailyPopularity.index = 0;
    }
    public static double GetGlobalPopularity()
    {
        using (StreamReader sr = File.OpenText(DailyPopularity.path))
        {
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                globalPopularity = float.Parse(s);
            }
        }
        return globalPopularity;
    }
    public static class DailyPopularity
    {
        public static double dailyPopularity;
        public static string path = Application.persistentDataPath + "/popularity.txt";
        public static int index;
    }
}

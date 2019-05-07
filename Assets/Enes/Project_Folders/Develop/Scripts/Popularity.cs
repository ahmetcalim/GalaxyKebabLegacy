using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class Popularity
{
    public int userID = 1;
    public double averageDailyPopularity;
    public double totalDailyPopularity;
    public double globalPopularity;
    public int kAct; // actively played
    public int kIdle; //have not played
    public int kConsIdle; //consecutive idle days
    public float pDecayRate = 1.13f; //Pdec
    public float scoreBase = 100f;
    public float dScoreActive; //Daily Popularity score for active game
    public float dScoreIdle; //Daily Popularity score for idle game

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
            pDecayRate = p.pDecayRate;
            kAct = p.kAct;
            kIdle = p.kIdle;
            kConsIdle = p.kConsIdle;
            pDecayRate = p.pDecayRate;
            scoreBase = p.scoreBase;
            dScoreActive = p.dScoreActive;
            dScoreIdle = p.dScoreIdle;
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
        return globalPopularity = CalculateScoreForActiveDay() + CalculateScoreForIdleDay();
    }

    public void SetGlobalPopularity()
    {
        kAct += 1;
        totalDailyPopularity += DailyPopularity.dailyPopularity / DailyPopularity.index;
        averageDailyPopularity = totalDailyPopularity / kAct;
        globalPopularity = CalculateGlobalPopularity();

        if (!File.Exists(DailyPopularity.path))
            using (StreamWriter sw = File.CreateText(DailyPopularity.path))
                sw.WriteLine(JsonUtility.ToJson(this));
        else
            File.WriteAllText(DailyPopularity.path, JsonUtility.ToJson(this));



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
    public float CalculateScoreForActiveDay()
    {
        dScoreActive += (float)(1.5f * DailyPopularity.dailyPopularity / DailyPopularity.index) * scoreBase;
        return dScoreActive;
    }
    public float CalculateScoreForIdleDay()
    {
        Debug.Log("consIdle: "+kConsIdle );
        if (kConsIdle > 0)
        {
            Debug.Log("Hesapladı!");
            dScoreIdle -= (float)(Math.Pow(pDecayRate, kConsIdle) * scoreBase);
        }


        Debug.Log("dScoreIdle: " + dScoreIdle);
        kConsIdle = 0;
        return dScoreIdle;
    }
    public static class DailyPopularity
    {
        public static double dailyPopularity;
        public static string path = Application.persistentDataPath + "/popularity.txt";
        public static int index;
    }
}

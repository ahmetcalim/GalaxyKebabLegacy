using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Satisfaction 
{
    static double returnValue;
    static double GetTasteInputValue(double tasteInput, double xMax)
    {
        if (tasteInput<=xMax)
            returnValue = tasteInput;
        else if(tasteInput>xMax)
            returnValue = 2*xMax-tasteInput;

        return returnValue;
    }

    static double GetSteepnessValue(double xMax, double xZero)
    {
        return 4 / (xMax - xZero);
    }

    public static double  CalculateSatisfaction(double xMax,double xZero,double tasteInput,int preference)
    {     
        if (tasteInput < 0)
            tasteInput = 0;

        return ((2/(1+ Math.Pow(2.71f,-GetSteepnessValue(xMax,xZero)*
        (GetTasteInputValue(tasteInput,xMax)+0.01f-xZero))))-1)*preference;
    }
    public static double CalculateSatisfactionMeat(double xMax, double xZero, double tasteInput, int preference)
    {
        if (tasteInput < 0)
            tasteInput = 0;

        return ((2 / (1 + Math.Pow(2.71f, -GetSteepnessValue(xMax, xZero) *
        (tasteInput + 0.01f - xZero)))) - 1) * preference;
    }

    public static double CalculateIrrelevantSatisfaction_OverTaste(double tasteInput)
    {
        return -Math.Pow(tasteInput/119.52f,2);
    }
    public static double CalculateIrrelevantSatisfaction_SweetBump(double tasteInput)
    {
        if (tasteInput < 71)
            return 0.34f - (1 + Math.Pow(tasteInput - 55, 2)) / 675;
        else
           return CalculateIrrelevantSatisfaction_OverTaste(tasteInput);
    }

    public static double CalculateImpactFactor(int orderTime,int tBase,double averageDailyPopularity,float pr,float cogs)
    {
        if (averageDailyPopularity != 0)
        {
           double value= CalculateWaitingTime(orderTime, tBase, averageDailyPopularity) * CalculatePriceJudgement(averageDailyPopularity, pr, cogs);
            if (value<0.87f)
            {
                return 0.87f;
            }
            else if (value>1.13f)
            {
                return 1.13f;
            }
            else
            {
                return value;
            }
        }           
        else
            return 1;
    }
    static double CalculateWaitingTime(int orderTime,int tBase,double averageDailyPopularity)
    {
        return (orderTime - ((tBase/2) - 3 * averageDailyPopularity))*(-(0.26f/6*averageDailyPopularity))+1.13f;
    }
    static double CalculatePriceJudgement(double averageDailyPopularity,float pr,float cogs)
    {
        return cogs * ((1.20f + (averageDailyPopularity / 100))/pr);
    }


}

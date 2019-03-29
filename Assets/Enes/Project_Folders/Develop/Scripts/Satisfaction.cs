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

}

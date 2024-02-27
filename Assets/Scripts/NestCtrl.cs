using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestCtrl : MonoBehaviour
{
    public static int totalPollen;
    public static int totalHoney;
    public static int totalWax;
    public static float nestTemp;
    public static float nestHumidity;
    
    public static float ambientTemp;
    public static float ambientHumidity;

    public static int totalHoneyPot = 1;
    public static int totalPollenPot = 1;
    public static int maxWax = 60;
    public static int totalBroodCell;

    public void AddResource(string resource)
    {
        if (resource == "Nectar")
        {
            totalPollen++;
            Debug.Log("Honey has been added to the nest reserve");
        }

        if (resource == "Pollen")
        {
            totalHoney++;
            Debug.Log("Pollen has been added to the nest reserve");
        }

        if (resource == "Resin")
        {
            totalWax++;
            Debug.Log("Wax has been added to the nest reserve");
        }
    }
    
    public void CreateStructure(string structure)
    {
        if (structure == "HoneyPot")
        {
            totalHoneyPot++;
            Debug.Log("A pot of honey was built");
        }

        if (structure == "PollenPot")
        {
            totalPollenPot++;
            Debug.Log("A pot of pollen was built");
        }

        if (structure == "BroodCell")
        {
            totalBroodCell++;
            Debug.Log("A brood cell was built");
        }
    }

    public void VaryTemperature(string variation, float degrees)
    {
        if (variation == "Ambient")
        {
            ambientTemp = ambientTemp + degrees;
        }

        if (variation == "Nest")
        {
            nestTemp = nestTemp + degrees;
        }
    }

    public void VaryHumidity(string variation, float percentage)
    {
        if (variation == "Ambient")
        {
            ambientHumidity = ambientHumidity + percentage;
        }

        if (variation == "Nest")
        {
            nestHumidity = nestHumidity + percentage;
        }
    }

}

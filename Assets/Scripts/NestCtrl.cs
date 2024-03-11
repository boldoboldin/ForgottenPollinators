using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestCtrl : MonoBehaviour
{
    [SerializeField] private UI_Ctrl uiCtrl;
    
    private static int totalPollen = 12, totalHoney = 12, totalWax = 30;
    private static float nestTemp, nestHumidity;

    public static int totalHoneyPot = 1, totalPollenPot = 1, totalBroodCell;

    private void Start()
    {
        uiCtrl.SetPollen(12, 1);
        uiCtrl.SetHoney(12, 1);

        uiCtrl.SetWax(30);

        uiCtrl.SetTemp(34, "nest");
        uiCtrl.SetHumidity(50, "nest");
    }

    public void AddResource(string resource)
    {
        if (resource == "Pollen")
        {
            totalPollen++;
            uiCtrl.SetPollen(totalPollen, totalPollenPot);
            Debug.Log("Pollen has been added to the nest reserve");
        }

        if (resource == "Nectar")
        {
            totalHoney++;
            uiCtrl.SetHoney(totalHoney, totalHoneyPot);
            Debug.Log("Honey has been added to the nest reserve");
        }

        if (resource == "Resin")
        {
            totalWax++;
            uiCtrl.SetWax(totalWax);
            Debug.Log("Wax has been added to the nest reserve");
        }
    }
    
    public void BuildStructure(string structure)
    {
        if (structure == "HoneyPot")
        {
            totalWax -= 10;
            totalHoneyPot++;
            uiCtrl.SetHoney(totalHoney, totalHoneyPot);
            uiCtrl.SetWax(totalWax);
            Debug.Log("A pot of honey was built");
        }

        if (structure == "PollenPot")
        {
            totalWax -= 10;
            totalPollenPot++;
            uiCtrl.SetPollen(totalPollen, totalPollenPot);
            uiCtrl.SetWax(totalWax);
            Debug.Log("A pot of pollen was built");
        }

        if (structure == "BroodCell")
        {
            totalWax -= 10;
            totalBroodCell++;
            uiCtrl.SetBroodCells(totalBroodCell);
            uiCtrl.SetWax(totalWax);
            Debug.Log("A brood cell was built");
        }
    }

    public void VaryNestTemperature(float degrees)
    {
  
    }

    public void VaryNestHumidity(float degrees)
    {
        
    }
}

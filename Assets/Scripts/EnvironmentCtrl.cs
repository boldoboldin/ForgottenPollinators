using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCtrl : MonoBehaviour
{
    [SerializeField] private UI_Ctrl uiCtrl;
    [SerializeField] private NestCtrl nestCtrl;
    
    private static float environmentTemp, environmentHumidity;

    void Start()
    {
        uiCtrl.SetTemp(27, "environment");
        uiCtrl.SetHumidity(72, "environment");
    }

    public void VaryTemperature(float variation)
    {
        environmentTemp = environmentTemp + variation;

        //nestCtrl.VaryNestTemperature(variation);

        uiCtrl.SetTemp(environmentTemp, "environment");
    }

    public void VaryHumidity(float variation)
    {
        environmentHumidity = environmentHumidity + variation;

        //nestCtrl.VaryNestHumidity(variation);

        uiCtrl.SetHumidity(environmentHumidity, "environment");
    }
}

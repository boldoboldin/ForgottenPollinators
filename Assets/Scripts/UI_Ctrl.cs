using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Ctrl : MonoBehaviour
{
    [SerializeField] private Gradient tempGradient;


    [Header("Hive variables")]
    [SerializeField] TMP_Text eggsText;
    [SerializeField] TMP_Text larvasText;
    [SerializeField] TMP_Text workersText;
    [SerializeField] TMP_Text queensText;
    [SerializeField] TMP_Text broodCellsText;

    [SerializeField] TMP_Text totalPollenText, honeyText, waxText;
    [SerializeField] Slider totalPollenBar, honeyBar, waxBar;

    [SerializeField] TMP_Text nestTempText, nestHumidityText;
    [SerializeField] Slider nestTempBar, nestHumidityBar;
    [SerializeField] Image nestTempFill, nestTempIcon;

    private List<WorkerBeeCtrl> workersList = new List<WorkerBeeCtrl>();

    [Header("Environment variables")]
    [SerializeField] TMP_Text environmentTempText, environmentHumidityText;
    [SerializeField] Slider environmentTempBar, environmentHumidityBar;
    [SerializeField] Image environmentTempFill, environmentTempIcon;

    void Update()
    {
        UpdateWorkersList();

        workersText.text = workersList.Count.ToString();
        //waxText.SetText(NestCtrl.totalWax + "/" + (NestCtrl.maxWax));
    }

    public void SetPollen(int pollenAmount, int pollenPotAmount)
    {
        totalPollenBar.value = pollenAmount;
        totalPollenBar.maxValue = pollenPotAmount * 30;
        totalPollenText.SetText(pollenAmount + "/" + (pollenPotAmount * 30));
    }

    public void SetHoney(int honeyAmount, int honeyPotAmount)
    {
        honeyBar.value = honeyAmount;
        honeyBar.maxValue = honeyPotAmount * 30;
        honeyText.SetText(honeyAmount + "/" + (honeyPotAmount * 30));
    }

    public void SetWax(int totalWax)
    {
        waxText.SetText(totalWax + "");
    }


    public void SetTemp(float temp, string target)
    {
        if (target == "nest")
        {
            nestTempBar.value = temp;
            nestTempText.SetText(temp + "°c");
            nestTempFill.color = tempGradient.Evaluate(nestTempBar.normalizedValue);
        }
        else if (target == "environment")
        {
            environmentTempBar.value = temp;
            environmentTempText.SetText(temp + "°c");
            environmentTempFill.color = tempGradient.Evaluate(environmentTempBar.normalizedValue);
        }
    }

    public void SetHumidity(float humidity, string target)
    {
        if (target == "nest")
        {
            nestHumidityBar.value = humidity;
            nestHumidityText.SetText(humidity + "%");
        }
        else if (target == "environment")
        {
            environmentHumidityBar.value = humidity;
            environmentHumidityText.SetText(humidity + "%");
        } 
    }

    public void SetBroodCells(int totalBroodCell)
    {
        broodCellsText.SetText("/" + totalBroodCell);
    }

public void UpdateWorkersList()
    {
        workersList.Clear();

        WorkerBeeCtrl[] foundObjects = FindObjectsOfType<WorkerBeeCtrl>();

        foreach (WorkerBeeCtrl workerBee in foundObjects)
        {
            workersList.Add(workerBee);
        }
    }
}

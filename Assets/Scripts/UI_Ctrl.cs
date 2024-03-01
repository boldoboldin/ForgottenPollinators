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

    [SerializeField] TMP_Text totalPollenText;
    [SerializeField] TMP_Text honeyText;
    [SerializeField] TMP_Text waxText;
    [SerializeField] TMP_Text broodCellsText;

    [SerializeField] TMP_Text nestTempText;
    [SerializeField] TMP_Text nestHumidityText;

    [SerializeField] Slider totalPollenBar;
    [SerializeField] Slider honeyBar;
    [SerializeField] Slider waxBar;

    [SerializeField] Slider nestTempBar;
    [SerializeField] Slider nestHumidityBar;

    [SerializeField] Image nestTempFill;
    [SerializeField] Image nestTempIcon;

    private List<WorkerBeeCtrl> workersList = new List<WorkerBeeCtrl>();

    [Header("Environment variables")]
    [SerializeField] TMP_Text environmentTempText;
    [SerializeField] TMP_Text environmentHumidityText;

    [SerializeField] Slider environmentTempBar;
    [SerializeField] Slider ambientHumidityBar;

    [SerializeField] Image environmentTempFill;
    [SerializeField] Image environmentTempIcon;

    private void Start()
    {
        SetPollen(0, 1);
        SetHoney(0, 1);

        SetTemp(34, 27);
        SetHumidity(50, 72);
    }

    void Update()
    {
        UpdateWorkersList();

        workersText.text = workersList.Count.ToString();

        broodCellsText.SetText("/" + NestCtrl.totalBroodCell);

        waxText.SetText(NestCtrl.totalWax + "/" + (NestCtrl.maxWax));
    }

    public void SetPollen(int pollenAmount, int pollenPotAmount)
    {
        totalPollenBar.value = pollenAmount;
        totalPollenBar.maxValue = pollenPotAmount;
        totalPollenText.SetText(pollenAmount + "/" + (pollenPotAmount * 60));
    }

    public void SetHoney(int honeyAmount, int honeyPotAmount)
    {
        honeyBar.value = honeyAmount;
        totalPollenBar.maxValue = honeyPotAmount;
        honeyText.SetText(honeyAmount + "/" + (honeyPotAmount * 60));
    }


    public void SetTemp(float nestTemp, float environmentTemp)
    {
        nestTempBar.value = nestTemp;
        nestTempText.SetText(nestTemp + "°c");
        nestTempFill.color = tempGradient.Evaluate(nestTempBar.normalizedValue);

        //environmentTempBar.value = environmentTemp;
        //environmentTempText.SetText(environmentTemp + "°c");
        //environmentTempFill.color = tempGradient.Evaluate(environmentTempBar.normalizedValue);
    }

    public void SetHumidity(float nestHumidity, float environmentHumidity)
    {
        nestHumidityBar.value = nestHumidity;
        nestHumidityText.SetText(nestHumidity + "%");

        //environmentHumidityBar.value = environmentHumidity;
        //environmentHumidityText.SetText(environmentHumidity + "%");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Ctrl : MonoBehaviour
{
    [SerializeField] NestCtrl nestCtrl;
    
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

    [SerializeField] Image totalPollenBar;
    [SerializeField] Image honeyBar;
    [SerializeField] Image waxBar;

    [SerializeField] Image tempBar;
    [SerializeField] Image humidityBar;

    [Header("Ambient variables")]
    [SerializeField] TMP_Text ambientTempText;
    [SerializeField] TMP_Text ambientHumidityText;

    [SerializeField] Image ambientTempBar;
    [SerializeField] Image ambientHumidityBar;

    // Update is called once per frame
    void Update()
    {
        eggsText.SetText("1");

        larvasText.SetText("2");

        broodCellsText.SetText("/" + NestCtrl.totalBroodCell);

        workersText.SetText("3");

        queensText.SetText("1");


        totalPollenText.SetText(NestCtrl.totalPollen + "/" + (NestCtrl.totalPollenPot * 60));

        honeyText.SetText(NestCtrl.totalHoney + "/" + (NestCtrl.totalHoneyPot * 60));

        waxText.SetText(NestCtrl.totalWax + "/" + (NestCtrl.maxWax));

        

        nestTempText.SetText(NestCtrl.nestTemp + "°c");
        nestHumidityText.SetText(NestCtrl.nestHumidity + "%");

        ambientTempText.SetText(NestCtrl.ambientTemp + "°c"); ;
        ambientHumidityText.SetText(NestCtrl.ambientHumidity + "%");
    }
}

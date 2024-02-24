using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Ctrl : MonoBehaviour
{
    [Header("Hive variables")]
    [SerializeField] TMP_Text eggsText;
    [SerializeField] TMP_Text larvasText;
    [SerializeField] TMP_Text workersText;
    [SerializeField] TMP_Text queensText;

    [SerializeField] TMP_Text totalPollenText;
    [SerializeField] TMP_Text honeyText;
    [SerializeField] TMP_Text waxText;
    [SerializeField] TMP_Text broodCellsText;

    [SerializeField] TMP_Text tempText;
    [SerializeField] TMP_Text humidityText;

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
        
    }
}

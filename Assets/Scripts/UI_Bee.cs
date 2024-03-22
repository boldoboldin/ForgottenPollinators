using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bee : MonoBehaviour
{
    [SerializeField] GameObject heartPrefab;
    [SerializeField] Bee bee;

    [SerializeField] private Slider staminaBar, corbiculaPollenBar, corbiculaResinBar, cropBar;
    [SerializeField] private Image currentActionIcon;
    [SerializeField] private Sprite pollenIcon, nectarIcon, resinIcon, forageIcon, idleIcon;


    List<HealthHeart> hearts = new List<HealthHeart>();

    public void DrawHearts()
    {
        ClearHearts();

        float maxHeathRemainder = bee.maxHP % 2;
        int heartsToMake = (int)((bee.maxHP / 2) + maxHeathRemainder);

        for (int i = 0; i < heartsToMake; i++) 
        {
            CreateEmptyHeart();
        }

        for(int i = 0; i < hearts.Count; i++)
        {
            int heartsStatusRemainder = (int)Mathf.Clamp(bee.currentHP - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartsStatusRemainder);
        }
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void ClearHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }

    public void UpdateStaminaBar(float currentStamina)
    {
        staminaBar.value = currentStamina;
        staminaBar.maxValue = bee.maxStamina;
    }

    public void UpdateCorbiculaPollenLoad(int corbiculaLoad, int corbiculaCapacity)
    {
        corbiculaPollenBar.value = corbiculaLoad;
        corbiculaPollenBar.maxValue = corbiculaCapacity;
    }

    public void UpdateCorbiculaResinLoad(int corbiculaResinLoad, int corbiculaResinCapacity)
    {
        corbiculaResinBar.value = corbiculaResinLoad;
        corbiculaResinBar.maxValue = corbiculaResinCapacity;
    }

    public void UpdateCropLoad(int cropLoad, int cropCapacity)
    {
        cropBar.value = cropLoad;
        cropBar.maxValue = cropCapacity;
    }

    public void UpdateCurrentActionIcon(string currentAction)
    {
        switch (currentAction)
        {
            case "CollectPollen":
                currentActionIcon.GetComponent<Image>().sprite = pollenIcon;
                break;
            case "CollectNectar":
                currentActionIcon.GetComponent<Image>().sprite = nectarIcon;
                break;
            case "CollectResin":
                currentActionIcon.GetComponent<Image>().sprite = resinIcon;
                break;
            case "Forage":
                currentActionIcon.GetComponent<Image>().sprite = forageIcon;
                break;
            case "Idle":
                currentActionIcon.GetComponent<Image>().sprite = idleIcon;
                break;
        }     
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCtrl : MonoBehaviour
{
    [SerializeField] private int pollenCapacity;
    private float pollenLoad;

    [SerializeField] private int nectarCapacity;
    private float nectarLoad;

    //[SerializeField] private List<int> bloomPeriod;
    [SerializeField] private float lifeTimeExpectancy;
    private float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBloomPeriod()
    {

    }

    public void EndBloomPeriod()
    {

    }

    private void OnMouseEnter()
    {
        if (pollenCapacity != 0 && CursorManager.somethingSelected)
        {
            CursorManager.instance.ChangeCursor("CollectPollen");
        }

        if (nectarCapacity != 0 && CursorManager.somethingSelected)
        {
            CursorManager.instance.ChangeCursor("CollectNectar");
        }

    }
    private void OnMouseExit()
    {
        CursorManager.instance.ChangeCursor("Default");
    }
}

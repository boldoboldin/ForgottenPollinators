using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCtrl : MonoBehaviour
{
    [SerializeField] private GameObject pollenSpawner;
    [SerializeField] private int minPollenAmount;
    [SerializeField] private int maxPollenAmount;
    [SerializeField] private int pollenCapacity;
    public float pollenLoad;

    [SerializeField] private GameObject nectarFX;
    [SerializeField] private int nectarCapacity;
    public float nectarLoad;

    

    //[SerializeField] private List<int> bloomPeriod;
    [SerializeField] private float lifeTimeExpectancy;
    private float lifeTime;

    public bool isOccupied = false;

    void Start()
    {
        pollenLoad = pollenCapacity;
        nectarLoad = nectarCapacity;
    }

    public void StartBloomPeriod()
    {
   
    }

    public void EndBloomPeriod()
    {

    }

    public void SpreadPollen()
    {
        float rndAmount = Random.Range(minPollenAmount, maxPollenAmount + 1);

        for (int i = 0; i < rndAmount; i++)
        {
            if (pollenLoad > 0)
            {
                Instantiate(pollenSpawner, transform.position, Quaternion.identity);
                pollenLoad--;
            }
        }

        Debug.Log("The flower " + this.name + " released pollen");  
    }

    public void TransferNectar()
    {
        Instantiate(nectarFX, transform.position, Quaternion.identity);

        Debug.Log("The flower " + this.name + " transferred nectar");
        pollenLoad--;
    }

    public void OccupyFlower(bool isOccupied)
    {
        this.isOccupied = isOccupied;
    }

    private void OnMouseEnter()
    {
        if (pollenCapacity != 0 && nectarCapacity == 0 && CursorManager.somethingSelected)
        {
            CursorManager.instance.ChangeCursor("CollectPollen");
        }

        if (nectarCapacity != 0 && pollenCapacity == 0 && CursorManager.somethingSelected)
        {
            CursorManager.instance.ChangeCursor("CollectNectar");
        }
    }

    private void OnMouseExit()
    {
        CursorManager.instance.ChangeCursor("Default");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bee")
        {
            isOccupied = false;
        }
    }
}

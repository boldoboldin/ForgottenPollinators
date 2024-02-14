using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCtrl : MonoBehaviour
{
    [SerializeField] private GameObject pollenFX;
    [SerializeField] private float minPollenSpd;
    [SerializeField] private float maxPollenSpd;
    [SerializeField] private int minPollenAmount;
    [SerializeField] private int maxPollenAmount;
    [SerializeField] private int pollenCapacity;
    private float pollenLoad;

    [SerializeField] private GameObject nectarFX;
    [SerializeField] private int nectarCapacity;
    private float nectarLoad;

    

    //[SerializeField] private List<int> bloomPeriod;
    [SerializeField] private float lifeTimeExpectancy;
    private float lifeTime;

    public bool isOccupied = false;

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
            GameObject pollenInstance = Instantiate(pollenFX, transform.position, Quaternion.identity);

            Vector2 rndDirection = Random.insideUnitCircle.normalized;

            float rndSpd = Random.Range(minPollenSpd, maxPollenSpd);

            Rigidbody2D pollenRb = pollenInstance.GetComponent<Rigidbody2D>();
            pollenRb.AddForce(rndDirection * rndSpd, ForceMode2D.Impulse);
            Destroy(pollenInstance, 7f);
        }

        Debug.Log("The flower " + this.name + " released pollen");
        
        //isOccupied = false;
    }

    public void TransferNectar()
    {
        Debug.Log("The flower " + this.name + " transferred nectar");
        Instantiate(nectarFX, transform.position, Quaternion.identity);
        //isOccupied = false;
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

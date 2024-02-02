using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerBeeCtrl : Bee
{
    [SerializeField] private int corbiculaCapacity;
    private float corbiculaLoad;

    [SerializeField] private int cropCapacity;
    private float cropLoad;

    public void Browsing(string resource, Vector2 areaTarget)
    {

    }

    private void Collect(string resource, GameObject plantTarget)
    {

    }

    private void ReturnToHive(Vector2 siteTarget)
    {

    }

    public void Guard(Vector2 siteTarget)
    {

    }

    public void Defend(GameObject enemyTarget)
    {

    }

    public void Construct(string structure, Vector2 siteTarget)
    {

    }
}

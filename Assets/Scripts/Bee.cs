using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bee : MonoBehaviour
{
    [SerializeField] private int maxHP;
    private int currentHP;

    

    [SerializeField] private float maxStamina;
    private float currentStamina;


    [SerializeField] private float lifeTimeExpectancy;
    private float lifeTime;


    public void Feed()
    {

    }
}

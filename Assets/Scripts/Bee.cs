using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bee : MonoBehaviour
{
    [SerializeField] private int maxHP;
    private int currentHP;

    [SerializeField] public float maxStamina;
    public float currentStamina;

    public bool IsExhausted = false;


    [SerializeField] private float lifeTimeExpectancy;
    private float lifeTime;


    private void Start()
    {
        currentHP = maxHP;
        currentStamina = maxStamina;
    }

    private void Update()
    {
        currentStamina = currentStamina - 0.001f;
        
        if (currentStamina <= 0)
        {
            IsExhausted = true;
        }
        else
        {
            IsExhausted = false;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollenSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pollen;
    [SerializeField] private float minPollenSpd;
    [SerializeField] private float maxPollenSpd;
    void Start()
    {
        Vector2 rndDirection = Random.insideUnitCircle.normalized;
        float rndSpd = Random.Range(minPollenSpd, maxPollenSpd);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = rndDirection * rndSpd;
    }

    void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.velocity -= rb.velocity.normalized * 2f * Time.deltaTime;

        if (rb.velocity.magnitude <= 0.01f)
        {
            Instantiate(pollen, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

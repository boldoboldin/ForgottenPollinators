using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTarget : MonoBehaviour
{
    [SerializeField] private float spd;
    private Rigidbody2D rb2D;
    Vector2 pos;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        pos.x = Input.GetAxisRaw("Horizontal");
        pos.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        pos.Normalize();
        rb2D.velocity = new Vector2(pos.x * spd * Time.deltaTime, pos.y * spd * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bee : MonoBehaviour
{
    [SerializeField] private int maxHP;
    private int currentHP;

    private bool isMoving;

    [SerializeField] private float maxStamina;
    private float currentStamina;


    [SerializeField] private float lifeTimeExpectancy;
    private float lifeTime;


    public static List<Bee> moveableUnits = new List<Bee>();
    public NavMeshAgent agent;
    public Vector3 destination;
    private bool isSelected;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;


        moveableUnits.Add(this);
        destination = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isSelected)
        {
            Debug.Log("Unit" + this.name + " selected");
            Move();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Deselected units");
            DeselectUnits();
        }
    }

    public void Feed()
    {

    }

    public void Move()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z; // Ajuste para a profundidade correta

        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, Vector2.zero);


        if (hit.collider != null)
        {
            Debug.Log("Clicked on: " + hit.collider.name + " " + hit.point);

            //agent.stoppingDistance = 0;
            agent.SetDestination(hit.point);
            destination = hit.point;
           
        }
    }

    private void DeselectUnits()
    {
        foreach (Bee obj in moveableUnits)
        {

            obj.isSelected = false;
            //obj.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
       
        }
    }

    private void OnMouseDown()
    {
        isSelected = true;
        //gameObject.GetComponent<SpriteRender>().color = Color.green;

        foreach(Bee obj in moveableUnits)
        {
            if(obj != this)
            {
                obj.isSelected = false;
                //obj.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

}

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

    [SerializeField] private GameObject nest;

    ForagingStates currentState;

    public static List<Bee> moveableUnits = new List<Bee>();
    public NavMeshAgent agent;
    public Vector3 destination;
    private bool isSelected;

    [SerializeField] private CircleCollider2D visionCol;
    [SerializeField] private Transform[] flowersPos;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        currentState = new Idle(gameObject, agent, visionCol, flowersPos);

        moveableUnits.Add(this);
        destination = transform.position;
    }

    void Update()
    {
        currentState = currentState.Process();
        
        if (Input.GetMouseButtonDown(0) && !isSelected )
        {
            //Debug.Log("Unit" + this.name + " selected");
        }

        if (Input.GetMouseButtonDown(0) && isSelected)
        {
            SetAction();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Deselected units");
            DeselectUnits();
        }
    }

    public void SetAction()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;

        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, Vector2.zero);


        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Flower"))
            {
                Move(hit.point);


            }
            else if (hit.collider.CompareTag("Nest"))
            {
                ReturnToHive();
            }
            else if (hit.collider.CompareTag("Walkable"))
            {
                Move(hit.point);
            }

            Debug.Log("Clicked on: " + hit.collider.name + " " + hit.point);

        }
    }

    private void Move(Vector3 destination)
    {
        agent.stoppingDistance = 0;
        agent.SetDestination(destination);
    }

    private void DeselectUnits()
    {
        foreach (WorkerBeeCtrl obj in moveableUnits)
        {

            obj.isSelected = false;
            //obj.gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        }
    }

    private void OnMouseDown()
    {
        isSelected = true;
        //gameObject.GetComponent<SpriteRender>().color = Color.green;

        foreach (WorkerBeeCtrl obj in moveableUnits)
        {
            if (obj != this)
            {
                obj.isSelected = false;
                //obj.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    private void Browsing(string resource, Vector2 areaTarget)
    {

    }

    private void Collect(string resource, GameObject plantTarget)
    {

    }

    private void ReturnToHive()
    {
        Vector3 nestPos = new Vector3(nest.transform.position.x, nest.transform.position.y, nest.transform.position.z);
        
        Move(nestPos);

        Debug.Log("The bee " + this.name + " is returning to its home");
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

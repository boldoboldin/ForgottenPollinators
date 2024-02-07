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

    private string currentAction;

    public static List<Bee> moveableUnits = new List<Bee>();
    public NavMeshAgent agent;
    public Vector3 destination;
    private bool isSelected;

    [SerializeField] private CircleCollider2D visionCol;
    [SerializeField] private Transform[] flowersPos;

    private float delay = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        //currentState = new Idle(gameObject, agent, visionCol, flowersPos);

        moveableUnits.Add(this);
        destination = transform.position;
    }

    void Update()
    {
        //currentState = currentState.Process();
        
        if (Input.GetMouseButtonDown(0) && !isSelected )
        {
            //info
        }
        else if (Input.GetMouseButtonDown(0) && isSelected)
        {
            Debug.Log("Unit" + this.name + " selected");
            SetAction();

        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Deselected units");
            DeselectUnits();
        }

        if (Input.GetKeyDown(KeyCode.B) && isSelected)
        {
            currentAction = "Browsing";
            Debug.Log("The bee " + this.name + " is browsing for flowers");
            
        }

        if (currentAction == "Browsing")
        {
            Browsing();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isSelected)
        {
            //currentState = new Idle(gameObject, agent, visionCol, flowersPos);
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


    private void Move(Vector3 destination)
    {
        agent.stoppingDistance = 0;
        agent.SetDestination(destination);
        currentAction = "Idle";
        agent.speed = 2f;

    }

    private void Browsing()
    {
        delay = delay - 0.1f;

        agent.isStopped = false;
        agent.speed = 1f;
        //Animator

        if (!agent.pathPending && agent.remainingDistance < 0.5f && delay <= 0)
        {
            SetRandomDestination();
            delay = Random.Range(100f, 500f);

        }

        //if (CanSeeFlowers())
        //{
        //    Debug.Log("The bee " + workerBee.name + " found a flower");
        //}

        //public bool CanSeeFlowers()
        //{
        //    Collider2D[] collisions = Physics2D.OverlapCircleAll(visionCol.transform.position, visionCol.radius);
        //    foreach (Collider2D col in collisions)
        //    {
        //        if (col.CompareTag("Flower"))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }

    private void SetRandomDestination()
    {
        float rndDestRange = 5f;

        Vector2 rndDirection = Random.insideUnitCircle.normalized * rndDestRange;
        Vector2 rndDest = (Vector2)transform.position + rndDirection;
        NavMeshHit hit;


        if (NavMesh.SamplePosition(rndDest, out hit, rndDestRange, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            return;
        }
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

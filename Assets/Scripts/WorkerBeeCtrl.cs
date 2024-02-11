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
    public bool isSelected;

    [SerializeField] private CircleCollider2D viewCol;
    private List<GameObject> flowersSeen;

    private float delay = 0f;

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

        if (CursorManager.cursorMode == "Default")
        {
            if (Input.GetMouseButtonDown(0) && !isSelected)
            {
                //info
            }
            else if (Input.GetMouseButtonDown(0) && isSelected)
            {
                Debug.Log("Unit" + this.name + " selected");
                SetAction();

            }
        }

        if (CursorManager.cursorMode == "CollectPollen")
        {
            if (Input.GetMouseButtonDown(0) && isSelected)
            {
                SetAction();
            }
        }

        if (CursorManager.cursorMode == "CollectNectar")
        {
            if (Input.GetMouseButtonDown(0) && isSelected)
            {
                SetAction();
            }
        }

        if (Input.GetMouseButtonDown(1) || (Input.GetKeyDown(KeyCode.Escape)))
        {
            Debug.Log("Deselected units");
            DeselectUnits();
        }

        if (Input.GetKeyDown(KeyCode.B) && isSelected)
        {
            currentAction = "Browsing";
            Debug.Log("The bee " + this.name + " is browsing for flowers");
        }

        if (Input.GetKeyDown(KeyCode.P) && isSelected)
        {
            CursorManager.instance.ChangeCursor("CollectPollen");
        }

        if (Input.GetKeyDown(KeyCode.N) && isSelected)
        {
            CursorManager.instance.ChangeCursor("CollectNectar");
        }

        if (currentAction == "Browsing")
        {
            Browsing();
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
                if (CursorManager.cursorMode == "CollectPollen")
                {
                    currentAction = "CollectPollen";
                    Debug.Log("The bee " + this.name + " is collecting pollen");
                }

                if (CursorManager.cursorMode == "CollectNectar")
                {
                    currentAction = "CollectNectar";
                    Debug.Log("The bee " + this.name + " is collecting Nectar");
                }


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
            CursorManager.somethingSelected = false;
            obj.isSelected = false;
            CursorManager.instance.ChangeCursor("Default");
            //obj.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void OnMouseDown()
    {
        CursorManager.somethingSelected = true;
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
            delay = Random.Range(100f, 600f);
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
        if (cropLoad < cropCapacity)
        {
            int rndTargetFlower = Mathf.RoundToInt(Random.Range(0f, flowersSeen.Count));
            agent.SetDestination(flowersSeen[rndTargetFlower].transform.position);
        }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Flower"))
        {
            Debug.Log("The bee " + this.name + " found flowers");
            flowersSeen.Add(other.gameObject);
        }
    }
}
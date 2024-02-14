using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerBeeCtrl : Bee
{
    [SerializeField] private int corbiculaCapacity;
    [SerializeField] private float corbiculaLoad;

    [SerializeField] private int cropCapacity;
    [SerializeField] private float cropLoad;

    [SerializeField] private GameObject nest;

    //private PlantCtrl targetPlantCtrl;

    private string currentAction;

    public static List<Bee> moveableUnits = new List<Bee>();
    public NavMeshAgent agent;

    [SerializeField] private GameObject targetPlant;
    [SerializeField] private Vector3 targetPlantPos;
    public bool isSelected;

    [SerializeField] private List<GameObject> flowersSeen;

    [SerializeField] private float delay = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;


        moveableUnits.Add(this);
    }

    void Update()
    {
        if (CursorManager.cursorMode == "Default")
        {
            if (Input.GetMouseButtonDown(0) && !isSelected)
            {
                //show info
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

        if (Input.GetKeyDown(KeyCode.F) && isSelected)
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
            agent.speed = 1f;
            //Animator

            Browsing();
        }

        if (currentAction == "CollectPollen")
        {
            Collect("Pollen");
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
                    Move(hit.point);
                    delay = 600f;
                    currentAction = "CollectPollen";
                    targetPlant = hit.collider.gameObject;
                    targetPlantPos = targetPlant.transform.position;

                    Debug.Log("The bee " + this.name + " is collecting pollen");
                }

                if (CursorManager.cursorMode == "CollectNectar")
                {
                    currentAction = "CollectNectar";
                }
            }
            else if (hit.collider.CompareTag("Nest"))
            {
                ReturnToHive();
            }
            else if (hit.collider.CompareTag("Walkable"))
            {
                Move(hit.point);
                agent.speed = 2f;

                currentAction = "Idle";
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

    private void Move(Vector3 destination)
    {
        agent.stoppingDistance = 0;
        agent.SetDestination(destination);
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
    }

    private void Collect(string resource)
    {
        PlantCtrl targetPlantCtrl = targetPlant.GetComponent<PlantCtrl>();

        if (!targetPlantCtrl.isOccupied)
        {
            targetPlantCtrl.isOccupied = true;
        }

        delay = delay - 0.1f;
        int rndTargetFlower;

        if (delay <= 0)
        {
            if (resource == "Pollen" && corbiculaLoad < corbiculaCapacity)
            {
                targetPlantCtrl.SpreadPollen();
                targetPlantCtrl.isOccupied = false;

                if (Random.Range(0f, 1f) < 0.5f || flowersSeen.Count == 0)
                {
                    Move(targetPlant.transform.position);
                }
                else
                {
                    rndTargetFlower = Mathf.RoundToInt(Random.Range(0, flowersSeen.Count - 1));

                    targetPlant = flowersSeen[rndTargetFlower];
                    targetPlantPos = flowersSeen[rndTargetFlower].transform.position;
                    targetPlantCtrl = flowersSeen[rndTargetFlower].GetComponent<PlantCtrl>();
                }

                Move(targetPlant.transform.position);
            }

            if (resource == "Nectar" && cropLoad < cropCapacity)
            {
                rndTargetFlower = Mathf.RoundToInt(Random.Range(0, flowersSeen.Count - 1));
                targetPlantCtrl.TransferNectar();
                targetPlantPos = flowersSeen[rndTargetFlower].transform.position;
                targetPlantCtrl = flowersSeen[rndTargetFlower].GetComponent<PlantCtrl>();


                Move(flowersSeen[rndTargetFlower].transform.position);
            }

            delay = Random.Range(100f, 600f);
        }
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

    public void Construct(string structure)
    {

    }

    // Select a unit
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

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Flower"))
        {
            PlantCtrl plantCtrl = other.gameObject.GetComponent<PlantCtrl>();

            if (plantCtrl.isOccupied == false && !flowersSeen.Contains(other.gameObject))
            {
                flowersSeen.Add(other.gameObject);
            }

            if (plantCtrl.isOccupied == true)
            {
                flowersSeen.Remove(other.gameObject);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Flower"))
        {
            flowersSeen.Remove(other.gameObject);
        }
    }

}
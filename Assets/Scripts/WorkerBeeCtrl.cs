using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerBeeCtrl : Bee
{
    [SerializeField] private int corbiculaCapacity;
    [SerializeField] private int corbiculaLoad;

    [SerializeField] private int cropCapacity;
    [SerializeField] private int cropLoad;

    private GameObject nest;
    private Vector3 nestPos;

    //private PlantCtrl targetPlantCtrl;

    private string currentAction;
    private string previusAction;

    [SerializeField] private SpriteRenderer sprt;
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

        nest = GameObject.FindGameObjectWithTag("Nest");
        nestPos = new Vector3(nest.transform.position.x, nest.transform.position.y, nest.transform.position.z);
    }

    void Update()
    {
        if (CursorManager.cursorMode == "Default")
        {
            if (Input.GetMouseButtonDown(0) && isSelected)
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

        if (currentAction == "CollectNectar")
        {
            Collect("Nectar");
        }

        if (currentAction == "DeliverRescorces")
        {
            Deliver();
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
                    Move(hit.point);
                    delay = 600f;
                    currentAction = "CollectNectar";
                    targetPlant = hit.collider.gameObject;
                    targetPlantPos = targetPlant.transform.position;

                    Debug.Log("The bee " + this.name + " is collecting nectar");
                }
            }
            else if (hit.collider.CompareTag("Nest"))
            {
                Move(nestPos);

                Debug.Log("The bee " + this.name + " is returning to its home"); ;
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
                targetPlantCtrl.TransferNectar();
                targetPlantCtrl.isOccupied = false;

                cropLoad++;

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

            delay = Random.Range(100f, 600f);
        }


        // Retorna para o ninho para entregar os recursos e restaurar a estamina
        if (currentAction == "CollectPollen" && corbiculaLoad >= corbiculaCapacity || currentAction == "CollectNectar" && cropLoad >= cropCapacity || IsExhausted)
        {
            Move(nestPos);
            previusAction = currentAction;
            currentAction = "DeliverResources";

            Debug.Log("The bee " + this.name + " is returning to its home");
        }
    }

    private void Deliver()
    {
        float nestDist = Vector3.Distance(this.transform.position, nestPos);

        if (nestDist <= 1) // Quando chegar ao ninho, "entra" e passa a entregar os recursos
        {
            sprt.enabled = false;
            Debug.Log("The bee " + this.name + " entered the nest");

            for (int i = corbiculaLoad; i > 0; i--)
            {
                corbiculaLoad--;
            }

            currentAction = previusAction;
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

    public void Guard(Vector2 siteTarget)
    {

    }

    public void Defend(GameObject enemyTarget)
    {

    }

    public void Construct(string structure)
    {

    }

    // Seleciona unidade
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
        if (collision.gameObject.tag == "Pollen" && corbiculaLoad <= corbiculaCapacity)
        {
            corbiculaLoad++;
            Destroy(collision.gameObject);
        }
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

            if (plantCtrl.isOccupied == true || currentAction == "CollectPollen" && plantCtrl.pollenLoad <= 0 || currentAction == "CollectNectar" && plantCtrl.nectarLoad <= 0)
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
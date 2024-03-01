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
    private string nextAction;

    [SerializeField] private SpriteRenderer sprt;
    private Rigidbody2D rb2D;
    public static List<Bee> moveableUnits = new List<Bee>();
    public NavMeshAgent agent;

    [SerializeField] private GameObject targetPlant;
    [SerializeField] private Vector3 targetPlantPos;
    public bool isSelected;

    [SerializeField] private List<GameObject> flowersSeen;

    private float forageDelay;
    private float collectDelay = 600f;
    private float deliveryDelay = 200f;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        
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

        if (CursorManager.cursorMode == "Forage")
        {
            if (Input.GetMouseButtonDown(0) && isSelected)
            {
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
            CursorManager.instance.ChangeCursor("Forage");
        }

        if (Input.GetKeyDown(KeyCode.P) && isSelected)
        {
            CursorManager.instance.ChangeCursor("CollectPollen");
        }

        if (Input.GetKeyDown(KeyCode.N) && isSelected)
        {
            CursorManager.instance.ChangeCursor("CollectNectar");
        }


        // Update behaviors 
        if (currentAction == "Forage")
        {
            Forage();
        }

        if (currentAction == "CollectPollen")
        {
            Collect("Pollen");
        }

        if (currentAction == "CollectNectar")
        {
            Collect("Nectar");
        }

        if (currentAction == "DeliverResources")
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
                collectDelay = 600f;
                targetPlant = hit.collider.gameObject;
                targetPlantPos = targetPlant.transform.position;

                agent.stoppingDistance = 0;
                Move(targetPlantPos);

                if (CursorManager.cursorMode == "CollectPollen")
                {
                    currentAction = "CollectPollen";
                    Debug.Log("The bee " + this.name + " is collecting pollen");
                }

                if (CursorManager.cursorMode == "CollectNectar")
                {
                    currentAction = "CollectNectar";
                    Debug.Log("The bee " + this.name + " is collecting nectar");
                }

                DeselectUnits();
            }
            else if (hit.collider.CompareTag("Nest"))
            {
                agent.stoppingDistance = 1;
                agent.speed = 1.5f;
                Move(nestPos);

                nextAction = currentAction;
                currentAction = "DeliverResources";

                DeselectUnits();

                Debug.Log("The bee " + this.name + " is returning to its home");
            }
            else if (hit.collider.CompareTag("Walkable"))
            {
                if (CursorManager.cursorMode == "Forage")
                {
                    currentAction = "Forage";

                    DeselectUnits();

                    Debug.Log("The bee " + this.name + " is browsing for flowers");
                }
                else
                {
                    currentAction = "Idle";
                }

                agent.stoppingDistance = 0;
                Move(hit.point);
            }

            CursorManager.instance.ChangeCursor("Default");
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
        agent.SetDestination(destination);
    }

    private void Forage()
    {
        forageDelay = forageDelay - 0.1f;
        agent.speed = 1f;
        //Animator

        if (!agent.pathPending && agent.remainingDistance < 0.5f && forageDelay <= 0)
        {
            SetRandomDestination();

            forageDelay = Random.Range(100f, 600f);
        }
    }
    private void Collect(string resource)
    {
        agent.speed = 1.5f;

        PlantCtrl targetPlantCtrl = targetPlant.GetComponent<PlantCtrl>();

        if (!targetPlantCtrl.isOccupied)
        {
            targetPlantCtrl.isOccupied = true;
        }

        collectDelay = collectDelay - 0.1f;
        int rndTargetFlower;

        if (collectDelay <= 0)
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

                if (targetPlantCtrl.nectarLoad > 0)
                {
                    cropLoad++;
                } 

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

            collectDelay = Random.Range(200f, 600f);
        }


        // Return to the nest to deliver resources and restore stamina
        if (currentAction == "CollectPollen" && corbiculaLoad >= corbiculaCapacity || currentAction == "CollectNectar" && cropLoad >= cropCapacity || IsExhausted || flowersSeen.Count < 0)
        {
            if (flowersSeen.Count < 0)
            {
                nextAction = "Idle";
            }
            else
            {
                nextAction = currentAction; // Defines the action you were already performing as the next action
            }

            Move(nestPos);
            currentAction = "DeliverResources";

            Debug.Log("The bee " + this.name + " is returning to its home");
        }
    }

    private void Deliver()
    {
        if (Arrived(nestPos, 1) == true) // Quando chegar ao ninho, "entra" e passa a entregar os recursos
        {
            NestCtrl nestCtrl = nest.GetComponent<NestCtrl>();

            sprt.enabled = false;
            rb2D.isKinematic = true;

            Debug.Log("The bee " + this.name + " entered the nest");

            for (int i = corbiculaLoad; i > 0; i--)
            {
                deliveryDelay = Mathf.Max(0, deliveryDelay - 1);

                if (deliveryDelay == 0)
                {
                    nestCtrl.AddResource("Pollen");
                    corbiculaLoad--;

                    deliveryDelay = 600f;
                }
            }

            for (int i = cropLoad; i > 0; i--)
            {
                deliveryDelay = Mathf.Max(0, deliveryDelay - 1);

                if (deliveryDelay == 0)
                {
                    nestCtrl.AddResource("Nectar");
                    cropLoad--;

                    deliveryDelay = 600f;
                }
            }

            if (corbiculaLoad <= 0 && cropLoad <= 0)
            {
                sprt.enabled = true;
                rb2D.isKinematic = true;

                if (nextAction == "CollectPollen" || nextAction == "CollectNectar")
                {
                    Move(targetPlant.transform.position);
                    currentAction = nextAction;
                }
            }
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

    bool Arrived(Vector3 destination, float minDist)
    {
        float dist = Vector3.Distance(this.transform.position, destination);

        if (dist <= minDist) // Quando chegou ao destino
        {
            return true;
        }
        else
        {
            return false;
        }

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
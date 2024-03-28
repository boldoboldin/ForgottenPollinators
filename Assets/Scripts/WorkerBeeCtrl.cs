using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.AI;

public class WorkerBeeCtrl : Bee
{
    [SerializeField] private VisualEffect vfxRenderer;

    [Header("UI attributes")]
    [SerializeField] private UI_Bee uiBee;
    [SerializeField] private GameObject uiInfos;
    [SerializeField] private GameObject uiCurrentAction;
    [SerializeField] private GameObject uiCorbiculaPollenBar;
    [SerializeField] private GameObject uiCorbiculaResinBar;
    [SerializeField] private GameObject uiCropBar;

    [Header("Resources")]
    [SerializeField] private int corbiculaPollenCapacity;
    [SerializeField] private int corbiculaPollenLoad;
    [SerializeField] private int corbiculaResinCapacity;
    [SerializeField] private int corbiculaResinLoad;
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

    public FogOfWarCtrl fogOfWarCtrl;
    public Transform secondaryFogOfWar;
    [Range(0, 5)]
    public float sightDistance;
    public float checkInterval;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        moveableUnits.Add(this);

        nest = GameObject.FindGameObjectWithTag("Nest");
        nestPos = new Vector3(nest.transform.position.x, nest.transform.position.y, nest.transform.position.z);

        currentStamina = maxStamina;

        StartCoroutine(CheckFogOfWar(checkInterval));
        secondaryFogOfWar.localScale = new Vector2(sightDistance*10, sightDistance * 10);
    }

    void Update()
    {
        uiBee.DrawHearts();
        
        vfxRenderer.SetVector3("ColliderPos", transform.position);


        uiInfos.SetActive(isSelected);
        uiCurrentAction.SetActive(!isSelected);


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

        if (CursorManager.cursorMode == "BuildPollenPot")
        {
            if (Input.GetMouseButtonDown(0) && isSelected)
            {
                SetAction();
            }
        }

        if (CursorManager.cursorMode == "BuildHoneyPot")
        {
            if (Input.GetMouseButtonDown(0) && isSelected)
            {
                SetAction();
            }
        }

        if (CursorManager.cursorMode == "BuildBroodCell")
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

        if (Input.GetKeyDown(KeyCode.R) && isSelected)
        {
            CursorManager.instance.ChangeCursor("CollectResin");
        }

        if (Input.GetKeyDown(KeyCode.B) && isSelected)
        {
            CursorManager.instance.ChangeCursor("BuildPollenPot");
        }

        if (Input.GetKeyDown(KeyCode.H) && isSelected)
        {
            CursorManager.instance.ChangeCursor("BuildHoneyPot");
        }

        if (Input.GetKeyDown(KeyCode.C) && isSelected)
        {
            CursorManager.instance.ChangeCursor("BuildBroodCell");
        }


        // Update behaviors
        // 
        if (currentAction == "Idle")
        {
            Forage();
            SpendStamina(0.01f);
        }

        if (currentAction == "Forage")
        {
            Forage();
            SpendStamina(0.01f);
        }

        if (currentAction == "CollectPollen")
        {
            Collect("Pollen");
            uiCorbiculaPollenBar.SetActive(true);
            uiCorbiculaResinBar.SetActive(false);
            uiCropBar.SetActive(false);
            SpendStamina(0.1f);
        }

        if (currentAction == "CollectNectar")
        {
            Collect("Nectar");
            uiCorbiculaPollenBar.SetActive(false);
            uiCorbiculaResinBar.SetActive(false);
            uiCropBar.SetActive(true);
            SpendStamina(0.1f);
        }

        if (currentAction == "BuildPollenPot")
        {
            Build("PollenPot");
            SpendStamina(1f);
        }

        if (currentAction == "BuildHoneyPot")
        {
            Build("HoneyPot");
            SpendStamina(1f);
        }

        if (currentAction == "BuildBroodCell")
        {
            Build("BroodCell");
            SpendStamina(1f);
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
            else if (hit.collider.CompareTag("Flower") || (hit.collider.CompareTag("Trunk")))
            {
                collectDelay = 600f;
                targetPlant = hit.collider.gameObject;
                targetPlantPos = targetPlant.transform.position;

                agent.stoppingDistance = 0;
                Move(targetPlantPos);

                if (CursorManager.cursorMode == "CollectResin")
                {
                    currentAction = "CollectResin";
                    Debug.Log("The bee " + this.name + " is collecting resin");
                }
            }
            else if (hit.collider.CompareTag("Nest"))
            {
                agent.stoppingDistance = 1;
                agent.speed = 1.5f;
                Move(nestPos);

                // Trazer o verificador de Arrive para cá

                if (CursorManager.cursorMode == "Default")
                {
                    nextAction = currentAction;
                    currentAction = "DeliverResources";

                    Debug.Log("The bee " + this.name + " is returning to its home");
                }

                if (CursorManager.cursorMode == "BuildPollenPot")
                {
                    currentAction = "BuildPollenPot";
                    Debug.Log("The bee " + this.name + " is building a pollen pot");
                }

                if (CursorManager.cursorMode == "BuildHoneyPot")
                {
                    currentAction = "BuildHoneyPot";
                    Debug.Log("The bee " + this.name + " is building a honey pot");
                }

                if (CursorManager.cursorMode == "BuildBroodCell")
                {
                    currentAction = "BuildBroodCell";
                    Debug.Log("The bee " + this.name + " is building a brood cell");
                }

                DeselectUnits();  
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

            uiBee.UpdateCurrentActionIcon(currentAction);

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
            uiInfos.SetActive(false);
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

        if (currentStamina <= 0)
        {
            agent.stoppingDistance = 1;
            agent.speed = 1.5f;
            Move(nestPos);

            nextAction = currentAction;
            currentAction = "DeliverResources";

            Debug.Log("The bee " + this.name + " is returning to its home");
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

        collectDelay -= 0.5f;
        int rndTargetFlower;
        uiBee.UpdateCurrentAction(collectDelay, 400);

        if (collectDelay <= 0)
        {
            if (resource == "Pollen" && corbiculaPollenLoad < corbiculaPollenCapacity)
            {
                targetPlantCtrl.TransferPollen();
                targetPlantCtrl.isOccupied = false;

                if (targetPlantCtrl.pollenLoad > 0)
                {
                    corbiculaPollenLoad++;
                    uiBee.UpdateCorbiculaPollenLoad(corbiculaPollenLoad, corbiculaPollenCapacity);
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

            if (resource == "Nectar" && cropLoad < cropCapacity)
            {
                targetPlantCtrl.TransferNectar();
                targetPlantCtrl.isOccupied = false;

                if (targetPlantCtrl.nectarLoad > 0)
                {
                    cropLoad++;
                    uiBee.UpdateCropLoad(cropLoad, cropCapacity);

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

            collectDelay = 400f;
        }


        // Return to the nest to deliver resources and restore stamina
        if (currentAction == "CollectPollen" && corbiculaPollenLoad >= corbiculaPollenCapacity || currentAction == "CollectNectar" && cropLoad >= cropCapacity || IsExhausted || flowersSeen.Count < 0 || currentStamina <= 0)
        {
            nextAction = currentAction; // Defines the action you were already performing as the next action
    
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

            for (int i = corbiculaPollenLoad; i > 0; i--)
            {
                deliveryDelay = Mathf.Max(0, deliveryDelay - 1);

                if (deliveryDelay == 0)
                {
                    nestCtrl.AddResource("Pollen");
                    corbiculaPollenLoad--;
                    uiBee.UpdateCorbiculaPollenLoad(corbiculaPollenLoad, corbiculaPollenCapacity);

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
                    uiBee.UpdateCropLoad(cropLoad, cropCapacity);

                    deliveryDelay = 600f;
                }
            }


            while (currentStamina <= maxStamina)
            {
                nestCtrl.Consume(1);
                currentStamina += 500;
            }

            if (corbiculaPollenLoad <= 0 && cropLoad <= 0 && currentStamina >= maxStamina - 10)
            {
                sprt.enabled = true;
                rb2D.isKinematic = true;

                if (nextAction == "Forage")
                {
                    agent.stoppingDistance = 0;
                    SetRandomDestination();

                    currentAction = nextAction;
                }
                if (nextAction == "CollectPollen" || nextAction == "CollectNectar" || nextAction == "Forage")
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

    public void Build(string structure)
    {
        if (Arrived(nestPos, 1) == true) // Quando chegar ao ninho, "entra" para construir a estrutura
        {
            NestCtrl nestCtrl = nest.GetComponent<NestCtrl>();

            sprt.enabled = false;
            rb2D.isKinematic = true;

            Debug.Log("The bee " + this.name + " entered the nest");
            nestCtrl.BuildStructure(structure);

            currentAction = "Idle";

            sprt.enabled = true;
            rb2D.isKinematic = false;
        }
    }

    void SpendStamina(float expenditure)
    {
        currentStamina = currentStamina - expenditure;
        uiBee.UpdateStaminaBar(currentStamina);
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
        if (collision.gameObject.tag == "Pollen" && corbiculaPollenLoad <= corbiculaPollenCapacity)
        {
            corbiculaPollenLoad++;
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

    private IEnumerator CheckFogOfWar(float checkInterval)
    {
        while (true)
        {
            fogOfWarCtrl.MakeHole(transform.position, sightDistance);
            yield return new WaitForSeconds(checkInterval);
        }
    }
}
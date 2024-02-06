using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForagingStates
{
    public enum STATE
    {
        IDLE, BROWSING, COLLECTING, RETURNING
    }

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    }

    public STATE stateName;
    protected EVENT stage;
    protected GameObject workerBee;
    protected NavMeshAgent agent;
    protected CircleCollider2D visionCol;
    //protected Animator anim;
    protected Transform[] flowersPos;
    protected ForagingStates nextState;

    public ForagingStates(GameObject workerBee, NavMeshAgent agent, CircleCollider2D visionCol, Transform[] flowersPos)
    {
        this.workerBee = workerBee;
        this.agent = agent;
        this.flowersPos = flowersPos;
    }

    public virtual void Enter()
    {
        stage = EVENT.UPDATE;
    }

    public virtual void Update()
    {
        stage = EVENT.UPDATE;
    }

    public virtual void Exit()
    {
        stage = EVENT.EXIT;
    }

    public ForagingStates Process()
    {
        if(stage == EVENT.ENTER)
        {
            Enter();
        }
        else if (stage == EVENT.UPDATE)
        {
            Update();
        }
        else
        {
            Exit();
            return nextState;
        }

        return this;
    }
}

public class Idle : ForagingStates
{
    public Idle(GameObject workerBee, NavMeshAgent agent, CircleCollider2D visionCol, Transform[] flowersPos) : base(workerBee, agent, visionCol, flowersPos)
    {
        stateName = STATE.IDLE;
    }

    public override void Enter()
    {
        agent.isStopped = true;
        //Animator
        Debug.Log("The bee " + workerBee.name + " is idle");
        base.Enter();
    }

    public override void Update()
    {
        
        nextState = new Browsing(workerBee, agent, visionCol, flowersPos);
        stage = EVENT.EXIT;
    }

    public override void Exit()
    {
        //Animator
        base.Exit();
    }
}

public class Browsing : ForagingStates
{
    private float rndDestRange = 5f;
    private float delay = 0f;
    //private float minDistanceToFlower = 2f; 

    public Browsing(GameObject workerBee, NavMeshAgent agent, CircleCollider2D visionCol, Transform[] flowersPos) : base(workerBee, agent, visionCol, flowersPos)
    {
        stateName = STATE.BROWSING;
    }

    public override void Enter()
    {
        agent.isStopped = false;
        agent.speed = 1f;
        //Animator
        Debug.Log("The bee " + workerBee.name + " is browsing for flowers");
        SetRandomDestination();
        base.Enter();
    }

    public override void Update()
    {
        delay = delay - 0.1f;

        if (!agent.pathPending && agent.remainingDistance < 0.5f && delay <= 0)
        {
            SetRandomDestination();
        }

        if (CanSeeFlowers())
        {
            Debug.Log("The bee " + workerBee.name + " found a flower");

            //nextState
            stage = EVENT.EXIT;
        }
        
    }
    public bool CanSeeFlowers()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(visionCol.transform.position, visionCol.radius);
        foreach (Collider2D col in collisions)
        {
            if (col.CompareTag("Flower"))
            {
                return true;
            }
        }
        return false;
    }

    private void SetRandomDestination()
    {
        

        Vector2 rndDirection = Random.insideUnitCircle.normalized * rndDestRange;
        Vector2 rndDest = (Vector2)workerBee.transform.position + rndDirection;
        NavMeshHit hit;


        if (NavMesh.SamplePosition(rndDest, out hit, rndDestRange, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            delay = Random.Range(100f,300f);
            return; 
        }
    }

    public override void Exit()
    {
        //Animator
        base.Exit();
    }
}



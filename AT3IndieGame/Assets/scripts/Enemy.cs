using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : FiniteStateMachine
{
    public Bounds bounds;
    public float viewRadius;
    public Transform player;

    public NavMeshAgent Agent { get; private set; }
    public Animator Anim { get; private set; }


    protected override void Awake()
    {
        entryState = new EnemyIdleState(this);
        if (TryGetComponent(out NavMeshAgent agent) == true)
        {
            Agent = agent;
        }
        //code below prevents AI from executing if it doesnt have a child
        //is there a way for the AI to check if it has a child?
        //GetChild(0) is the spesific child you want
        if(transform.GetChild(0).TryGetComponent(out Animator anim) == true)
        {
            Anim = anim;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        //base.start references the finitestate machines start function
        base.Start();
        //we can write custom code to be exectuted after the original start definition is run
    }

    // Update is called once per frame
    protected override void Update()
    {
        //NOTE: code bellow debugs what the states are returning
        //Debug.Log(CurrentState.GetType());
        base.Update();

    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}

public abstract class EnemyBehaviourState : IState
{
    protected Enemy Instance { get; private set; }
    protected NavMeshAgent Agent { get; private set; }

    public EnemyBehaviourState(Enemy instance)
    {
        Instance = instance;
        
    }
    public abstract void OnStateEnter();


    public abstract void OnStateExit();


    public abstract void OnStateUpdate();

    public virtual void DrawStateGizmos() { }

}

public class EnemyIdleState : EnemyBehaviourState
{
    private Vector2 idleTimeRange = new Vector2(3, 10);
    private float timer = -1;
    private float idleTime = 0;

    public EnemyIdleState(Enemy instance) : base(instance)
    {

    }

    public override void OnStateEnter()
    {
        Instance.Agent.isStopped = true;
        idleTime = Random.Range(idleTimeRange.x, idleTimeRange.y);
        timer = 0;
        //Debug.Log("idle state entered, waiting for " + idleTime + " seconds");
        //"isMoving" is your animation controller paramiter
        Instance.Anim.SetBool("isMoving", false);
    }

    public override void OnStateExit()
    {
        timer = -1;
        idleTime = 0;
        Debug.Log("exiting idle state");
    }

    public override void OnStateUpdate()
    {
        if (Vector3.Distance(Instance.transform.position, Instance.player.position) <= Instance.viewRadius)
        {
            Debug.Log("Player in range enter chase state");
            Instance.SetState(new EnemyChaseState(Instance));
        }


        if (timer >= 0)
        {
            timer += Time.deltaTime;
            if (timer >= idleTime)
            {
                Instance.SetState(new EnemyWanderState(Instance));
                Debug.Log("Exiting idle state after " + idleTime + " seconds");
            }
        }
    }
}

public class EnemyWanderState : EnemyBehaviourState
{
    private Vector3 targetPostion;
    private float wanderSpeed = 0.5f;

    public EnemyWanderState(Enemy instance) : base(instance)
    {

    }

    public override void OnStateEnter()
    {
        Instance.Agent.speed = wanderSpeed;
        Instance.Agent.isStopped = false;
        Vector3 randomPosInBounds = new Vector3
            (
            Random.Range(-Instance.bounds.extents.x, Instance.bounds.extents.y),
            Instance.transform.position.y,
            Random.Range(-Instance.bounds.extents.z, Instance.bounds.extents.z)
            );
        targetPostion = randomPosInBounds;
        Instance.Agent.SetDestination(targetPostion);
        Debug.Log("wander state entered with target postion of " + targetPostion);
        Instance.Anim.SetBool("isMoving", true);
        Instance.Anim.SetBool("isChasing", false);
    }

    public override void OnStateExit()
    {
        Debug.Log("wander state existed");
    }

    public override void OnStateUpdate()
    {

        Vector3 t = targetPostion;
        t.y = 0;
        // check if the AI is Close to its target postion
        if(Vector3.Distance(Instance.transform.position, targetPostion) <= Instance.Agent.stoppingDistance)
        {
            Instance.SetState(new EnemyIdleState(Instance));
        }

        //check if the player is within the view radius of the AI
        if (Vector3.Distance(Instance.transform.position, Instance.player.position) <= Instance.viewRadius)
        {
            Debug.Log("Player in range enter chase state");
            Instance.SetState(new EnemyChaseState(Instance));
        }
    }

    public override void DrawStateGizmos()
    {
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(targetPostion, 0.5f);
    }
}

public class EnemyChaseState : EnemyBehaviourState
{

    private float chaseSpeed = 1.5f;

    public EnemyChaseState(Enemy instance) : base(instance)
    {
    }

    public override void OnStateEnter()
    {
        Instance.Agent.isStopped = false;
        Instance.Agent.speed = chaseSpeed;
        Debug.Log("entered chase state.");
        Instance.Anim.SetBool("isMoving", true);
        Instance.Anim.SetBool("isChasing", true);
    }

    public override void OnStateExit()
    {
        Instance.Agent.speed /= chaseSpeed;
    }

    public override void OnStateUpdate()
    {
        Instance.Agent.SetDestination(Instance.player.position);

        if (Vector3.Distance(Instance.transform.position, Instance.player.position) > Instance.viewRadius)
        {
            Debug.Log("Player in range enter chase state");
            Instance.SetState(new EnemyWanderState(Instance));
        }
    }
}

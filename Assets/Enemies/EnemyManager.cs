using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    StateMachine<EnemyManager> fsm;
    public PatrolState patrolState;
    public AttackState attackState;
    public ChaseState chaseState;
    public FindState findState;
    public IdleState idleState;

    EnemyNavigation navigation;
    EnemyAttack attackScript;
    EnemyAnimationManager eam;

    [Tooltip("Enemy Sight Range Should not be less than the size of Sight Collider")]
    public float enemyAttackRange = 5f;
    public float enemyChaseRange = 10f;
    public float fireDelay = 0.25f;
    public float chaseDestinationUpdateDelay = 0.25f;
    public bool playerInCollider = false;
    public bool playerInAttackRange = false;
    public float lookAroundSpeed = 1f;


    void Awake()
    {
        eam = GetComponent<EnemyAnimationManager>();
        navigation = GetComponent<EnemyNavigation>();
        attackScript = GetComponent<EnemyAttack>();
        patrolState = new PatrolState();
        attackState = new AttackState();
        chaseState = new ChaseState();
        findState = new FindState();
        idleState = new IdleState();
        fsm = new StateMachine<EnemyManager>();
        playerInCollider = false;
        playerInAttackRange = false;
    }

    void Start()
    {
        fsm.Configure(this, patrolState);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f);
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + enemyChaseRange));
        Gizmos.color = new Color(1f, 0f, 0f);
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + enemyAttackRange));
       
    }
    
    void Update()
    {
        fsm.Update();
    }


    #region Idle
    public void GoIdle()
    {
        eam.SetToIdle();
        StopAllCoroutines();
        navigation.PauseAgent();
    }
    #endregion


    #region Patrolling

    public void SetLightColor(Color color)
    {
        attackScript.Alert(color);
    }
    
    public void BeginPatrol()
    {
        //print("initiating walk");
        eam.SetToWalk();
        navigation.ResumeAgent();
        navigation.Walk();
        navigation.PatrolThePoints();
    }

    public void EnableAutoBraking()
    {
        navigation.EnableAutoBraking();
    }

    public void StopAutoBraking()
    {
        navigation.StopAutoBraking();
    }
    #endregion


    #region Attacking

    public void PlayerInRange(Transform player)
    {
        //print("player in range called");
        attackState.player = player;
        transform.LookAt(player.position);
        //StopCoroutine("LookAroundC");
        StopAllCoroutines();
        ChangeState(attackState);
    }
    
    public void FindAndAttackPlayer(Transform player)
    {
        navigation.StopAllCoroutines();
        navigation.PauseAgent();
        StartCoroutine(CheckForPlayer(player));
    }

    IEnumerator CheckForPlayer(Transform target)
    {
        //print("Attack Coroutine Started");
        while (GameManager.instance.IsPlayerAlive && target)
        {
            Ray ray = new Ray(transform.position, Vector3.Normalize(target.position - transform.position));
            RaycastHit hit;

            float distance = Vector3.Distance(target.position, transform.position);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.transform == target && distance <= enemyAttackRange)
                {
                    eam.SetToAttack();
                    transform.LookAt(target.position);
                    attackScript.DealDamage(target);
                }
                else if (hit.collider.transform == target && distance > enemyAttackRange)
                {
                    //print("Player Out of Attack Range"); 
                    playerInAttackRange = false;
                    PlayerOutOfAttackRange(target);
                    yield break;
                }
                else
                {
                    //print("Player Out of Attack Range");
                    findState.target = target;
                    playerInAttackRange = false;
                    ChangeState(findState);
                    yield break;
                }
            }
            else
            {
                //print("Player Out of Attack Range");
                findState.target = target;
                playerInAttackRange = false;
                ChangeState(findState);
                yield break;
            }

            if (!GameManager.instance.IsPlayerAlive)
            {
                findState.target = target;
                playerInAttackRange = false; 
                playerInCollider = false;
                ChangeState(patrolState); 
                yield break;
            }

            yield return new WaitForSeconds(fireDelay);
        }
    }

    void PlayerOutOfAttackRange(Transform playerPosition)
    {
        chaseState.playerPosition = playerPosition;
        ChangeState(chaseState);
    }

    #endregion


    #region Chase State

    public void ChasePlayer(Transform target)
    {
        navigation.ResumeAgent();
        eam.SetToRun();
        navigation.Run();
        //StartCoroutine(ChaseThePlayer(target));
    }


    public void Chase(Transform target)
    {
        if (GameManager.instance.IsPlayerAlive)
        { 
            Ray ray = new Ray(transform.position, Vector3.Normalize(target.position - transform.position));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                float distance = Vector3.Distance(target.position, transform.position);
                if (hit.collider.transform == target && distance <= enemyChaseRange)
                {
                    navigation.GoTo(target.position);
                }
                else
                {
                    findState.target = target;
                    ChangeState(findState);
                    return;
                }
            }
            else
            {
                findState.target = target;
                ChangeState(findState);
                return;
            }

        }
        else
        {
            ChangeState(patrolState);
        }
    }

    #endregion


    #region Search State

    public void GotoLastLocation(Vector3 location)
    {
        eam.SetToRun();
        navigation.ResumeAgent();
        navigation.Run();
        navigation.GoTo(location);
    }


    public void CheckForRevertToPatrol(Transform player)    //change this to coroutine and move patrol state coroutine to first update
    {
        if (player == null)
            playerInCollider = false;

        if (playerInCollider)
        {
            Ray ra = new Ray(transform.position, Vector3.Normalize(player.position - transform.position));
            RaycastHit hit;
            if (Physics.Raycast(ra, out hit))
            {
                if (hit.collider.tag == "Player")
                {
                    PlayerInRange(hit.collider.transform);
                }
            }
        }
        else if (navigation.GetDistaneFromTarget() < 0.1f && !playerInCollider)
        {
            ChangeState(idleState);
            LookAround();
        }
    }

    public void LookAround()
    {
        StartCoroutine("LookAroundC");
    }

    IEnumerator LookAroundC()
    {
        Vector3 left = -transform.right;
        Vector3 right = transform.right;
        bool canLeft = !Physics.Raycast(transform.position, left, 1f);
        bool canRight = !Physics.Raycast(transform.position, right, 1f);
        if(canLeft)
        {
            navigation.Walk();
            navigation.ResumeAgent();
            navigation.FixPosition();
            navigation.GoTo(transform.position + left);
            yield return new WaitForSeconds(1.5f);
        }
        if (canRight)
        {
            navigation.Walk();
            navigation.ResumeAgent();
            navigation.FixPosition();
            navigation.GoTo(transform.position + right);
            yield return new WaitForSeconds(1.5f);
        }

        if(!canLeft && ! canRight)
            yield return new WaitForSeconds(1.5f);

        ChangeState(patrolState);
    }

    #endregion

    public State<EnemyManager> CurrentState()
    {
       return fsm.GetState();
    }

    public void ChangeState(State<EnemyManager> state)
    {
        print("Changing State to: " + state);
        fsm.ChangeState(state);
    }

}
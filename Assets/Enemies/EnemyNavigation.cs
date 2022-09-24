using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavigation : MonoBehaviour
{
    EnemyManager manager;

    NavMeshAgent navAgent;
    public List<Transform> PatrolPoints;
    int currentDestination;
    public float walkSpeed = 1.2f;
    public float runSpeed = 4f;

    void Awake()
    {
        manager = GetComponent<EnemyManager>();
        navAgent = GetComponent<NavMeshAgent>();
        currentDestination = 0;
    }

    public void FixPosition()
    {
        navAgent.updatePosition = false;
        navAgent.updatePosition = true;
    }

    public void ReleasePosition()
    {
        navAgent.updatePosition = true;
    }

    public void PatrolThePoints()
    {
        ResumeAgent();
        if(PatrolPoints.Count == 0)
            throw new UnityException("No Patrol Points Assigned");
        navAgent.SetDestination(PatrolPoints[currentDestination].position);
        StartCoroutine(FollowPath(PatrolPoints[currentDestination]));
    }

    public void StopAutoBraking()
    {
        navAgent.autoBraking = false;
    }

    public void EnableAutoBraking()
    {
        navAgent.autoBraking = true;
    }

    IEnumerator FollowPath(Transform target)
    {
        while (true)
        {
            if (!navAgent.pathPending)
            {
                //print("Reached" + Vector3.Distance(navAgent.destination, transform.position));
                if (navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    //if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
                    {
                        currentDestination = (currentDestination + 1) % PatrolPoints.Count;
                        navAgent.SetDestination(PatrolPoints[currentDestination].position);
                        //print(PatrolPoints[currentDestination].position);
                    }
                }
            }
            yield return null;
        }
    }

    public void GoTo(Vector3 target)
    {
        navAgent.SetDestination(target);
    }


    public void Run()
    {
        navAgent.speed = runSpeed;
    }

    public void Walk()
    {
        navAgent.speed = walkSpeed;
    }

    public void PauseAgent()
    {
        StopCoroutine("FollowTarget");
        StopCoroutine("FollowPath"); 
        navAgent.isStopped = true;
    }


    public float GetDistaneFromTarget() =>
         navAgent.remainingDistance;
    

    public void ResumeAgent()
    {
        ReleasePosition();
        navAgent.isStopped = false;
    }



}
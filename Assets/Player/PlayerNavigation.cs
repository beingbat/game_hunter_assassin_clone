using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PlayerNavigation : MonoBehaviour
{
    public LayerMask floor;
    public LayerMask enemyLayer;
    
    public GameObject targetLockPrefab;
    public float pathDrawDelay = 0.1f;
    public float targetFollowUpdateDelay = 0.2f;

    PlayerPathLine pathLine;
    NavMeshAgent navAgent;
    PlayerAnimationManager animManager;

    float playerBounds = 0;
    float targetBounds = 0;
    bool keyboardControls = false;

    void Start()
    {
        animManager = GetComponent<PlayerAnimationManager>();
        navAgent = GetComponent<NavMeshAgent>();
        pathLine = GetComponent<PlayerPathLine>();
        navAgent.updateRotation = false;
        playerBounds = GetComponent<CapsuleCollider>().bounds.size.x/2f;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            keyboardControls = !keyboardControls;
            if(keyboardControls)
            {
                StopAllCoroutines();
            }
        }

        if (keyboardControls)
            KeyboardMovement();
        else
            RaycastAndMove();
    }

    void KeyboardMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(h / 10, 0, 0);
        transform.Translate(0, 0, v / 10);
    }

    void RaycastAndMove()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit))
            {
                //Checking if the layer hit is enemy
                if ((1 << hit.collider.gameObject.layer & enemyLayer) != 0)
                {

                    animManager.Running();
                    StopAllCoroutines();
                    targetBounds = hit.collider.bounds.size.x/2f;
                    var obj = Instantiate(targetLockPrefab);
                    obj.transform.position = hit.collider.transform.position;
                    StartCoroutine(FollowTargetAndCreateLine(hit.collider.transform));
                }
                //Checking if the layer hit is floor or not
                else if ((floor.value & 1 << hit.transform.gameObject.layer) != 0)
                {
                    animManager.Running();
                    StopAllCoroutines();
                    navAgent.SetDestination(hit.point);
                    StartCoroutine(CreateLine(hit.point));
                }
            }
        }
    }

    IEnumerator FollowTargetAndCreateLine(Transform t)
    {
        while (t != null)
        {
            navAgent.SetDestination(t.position);
            transform.rotation = Quaternion.LookRotation(navAgent.velocity.normalized);
            pathLine.MakePathLine();
            float currentDistance = Vector3.Distance(transform.position, t.position);
            float leastDistance = navAgent.stoppingDistance + playerBounds + targetBounds;
            //print("Distance: " + currentDistance);
            //print("Nav Distance: " + navAgent.remainingDistance);
            //print("Min Distance: " + leastDistance);
            //print("Player: " + playerBounds);
            //print("Target: " + targetBounds);
            if (currentDistance <= leastDistance)
            {
                animManager.Idle();
                navAgent.destination = transform.position;
                navAgent.ResetPath();
                pathLine.ClearPathLine();
                yield break;
            }
            yield return new WaitForSeconds(targetFollowUpdateDelay);
        }

        animManager.Idle();
        navAgent.destination = transform.position;
        navAgent.ResetPath();
        pathLine.ClearPathLine();
    }

    IEnumerator CreateLine(Vector3 target)
    {
        while (true)
        {
            float currentDistance = Vector3.Distance(transform.position, target);
            float leastDistance = navAgent.stoppingDistance + playerBounds + 0.2f;
            //print("Distance: " + currentDistance);
            //print("Nav Distance: " + navAgent.remainingDistance);
            //print("Min Distance: " + leastDistance);
            if (currentDistance <= leastDistance)
            {
                animManager.Idle();
                navAgent.ResetPath();
                pathLine.ClearPathLine();
                //print("Line Finished");
                SwitchDirection();
                yield break;
            }
            transform.rotation = Quaternion.LookRotation(navAgent.velocity.normalized);
            pathLine.MakePathLine();
            yield return new WaitForSeconds(pathDrawDelay);
        }
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1));
    //}

    void SwitchDirection()
    {
        if (Physics.Raycast(transform.position, transform.forward, 1f))
        {
            transform.Rotate(transform.rotation.x, transform.rotation.y + 180f, transform.rotation.z); 
        }
    }



    
}

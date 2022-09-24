using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    //public LayerMask player;
    EnemyManager manager;
    Transform p;

    void Awake()
    {
        manager = GetComponentInParent<EnemyManager>();
    }

    void Update()
    {
        if (manager.playerInCollider && manager.CurrentState() != manager.attackState && p != null)
        {
            Ray ra = new Ray(transform.position, Vector3.Normalize(p.position - transform.position));
            RaycastHit hit;
            if (Physics.Raycast(ra, out hit))
            {
                if (hit.collider.tag == "Player" && !manager.playerInAttackRange)
                {
                    manager.playerInAttackRange = true;
                    manager.PlayerInRange(hit.transform);
                }
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (manager.playerInCollider || manager.playerInAttackRange)
            return;

        if (other.tag == "Player")
        {
            //print("Af");
            manager.playerInCollider = true;
            p = other.transform;
            Ray r = new Ray(transform.parent.position, Vector3.Normalize(other.transform.position - transform.parent.position));
            RaycastHit hit;
            if (Physics.Raycast(r, out hit))
            {
                if (hit.collider.transform == other.transform && !manager.playerInAttackRange)
                {
                    //print("Collider Calling PlayerInRange");
                    manager.playerInAttackRange = true;
                    manager.PlayerInRange(other.transform);
                }
            }
        }
    }



    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            p = null;
            //print("Player Exited Collider");
            manager.playerInCollider = false;
        }
    }

}

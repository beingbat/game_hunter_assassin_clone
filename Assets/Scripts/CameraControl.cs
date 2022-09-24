using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float cameraSpeed = 0.5f;

    [Tooltip("The Maximmum Distance allowed between camera and Player")]
    public float verticalOffset = 6.0f;
    public float horizontalOffset = 3.0f;

    //[Tooltip("Reset margin while resetting camera to player position")]
    //public float verticalResetMargin = 1.0f;
    //public float horizontalResetMargin = 1.0f;

    [Tooltip("Boundaries of the level to bound camera within it")]
    public float verticalBound = 5f;
    public float horizontalBound = 5f;

    //public LayerMask enemyLayer;

    Transform player;

    //bool moveForward = false,
    //    moveSideway = false;

    void Awake()
    {
        var p = FindObjectOfType<PlayerManager>();
        if (p)
            player = p.transform;
    }

    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if (player != null)
        {

            if (Mathf.Abs(transform.position.x - player.position.x) > horizontalOffset )
            {
                Vector3 dest = new Vector3(player.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, dest, cameraSpeed * Time.deltaTime);
            }

            if (Mathf.Abs(transform.position.z - player.position.z) > verticalOffset)
            {
                Vector3 dest = new Vector3(transform.position.x, transform.position.y, player.position.z);
                transform.position = Vector3.Lerp(transform.position, dest, cameraSpeed * Time.deltaTime);
            }

            if (Mathf.Abs(transform.position.x) > horizontalBound)
               transform.position = new Vector3(Mathf.Sign(transform.position.x) * horizontalBound, transform.position.y, transform.position.z);

            if (Mathf.Abs(transform.position.z) > verticalBound)
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Sign(transform.position.z) * verticalBound);
        }
    }
}

//    //void FollowPlayerAdvanced()
//    //{//{
//    //    if (player != null)
//    //    {
//    //        if (t != null)
//    //        {
//    //            var a = player.transform.position.normalized + t.transform.position.normalized;
//    //            pos = a.normalized;
//    //        }
//    //        else
//    //        {
//    //            pos = player.transform.position;
//    //        }

//    //        if (Mathf.Abs(transform.position.x - player.position.x) > horizontalLimit || move_sideway)
//    //        {
//    //            move_sideway = true;
//    //            Vector3 dest = new Vector3(pos.x, transform.position.y, transform.position.z);
//    //            transform.position = Vector3.Lerp(transform.position, dest, lerpSpeed * Time.deltaTime);
//    //            if (Mathf.Abs(transform.position.x - pos.x) < horizontalResetMargin || Mathf.Abs(transform.position.x) > 1.8f)
//    //                move_sideway = false;
//    //        }

//    //        if (Mathf.Abs(transform.position.z - pos.z) > verticalLimit || move_forward)
//    //        {
//    //            move_forward = true;
//    //            Vector3 dest = new Vector3(transform.position.x, transform.position.y, pos.z);
//    //            transform.position = Vector3.Lerp(transform.position, dest, lerpSpeed * Time.deltaTime);
//    //            if (Mathf.Abs(transform.position.z - pos.z) < verticalResetMargin || transform.position.z < -7.5 || transform.position.z > 8.35)
//    //                move_forward = false;
//    //        }
//    //    }
//    }


//    //Transform t;
//    //void OnTriggerEnter(Collider other)
//    //{
//    //    if(other.tag == "Enemy")
//    //    {
//    //        if(t == null)
//    //            t = other.transform;
//    //        else if(
//    //            Vector3.Distance(player.position, t.transform.position) > 
//    //            Vector3.Distance(player.position, other.transform.position))
//    //        {
//    //            t = other.transform;
//    //        }
//    //    }
//    //}

//    //void OnTriggerExit(Collider other)
//    //{
//    //    if (other == t)
//    //    {
//    //        Collider[] hitColliders = Physics.OverlapBox(new Vector3(0, 0, 40), new Vector3(8.5f, 14, 1), Quaternion.identity, e_layer);
//    //        float min_dist = 1000f;
//    //        foreach(var c in hitColliders)
//    //        {
//    //            float x = Vector3.Distance(player.transform.position, c.transform.position);
//    //            if ( x < min_dist)
//    //            {
//    //                t = c.transform;
//    //                min_dist = x; 
//    //            }
//    //        }
//    //    }
//    //}
//}
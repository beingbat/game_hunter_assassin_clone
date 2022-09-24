using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    public GameObject coinsInstantiator;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            if(coinsInstantiator != null)
            {
                var obj = Instantiate(coinsInstantiator, transform.position, Quaternion.identity);
                obj.transform.SetParent(GameObject.FindGameObjectWithTag("Container").transform);
            }
            GetComponent<PlayerAnimationManager>().Attack();
            Destroy(other.gameObject);
        }
    }

}

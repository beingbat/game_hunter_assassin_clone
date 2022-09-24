using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoins : MonoBehaviour
{
    public GameObject coinPrefab;
    Transform container;
    
    void Start()
    {
        container = GameObject.FindGameObjectWithTag("Container").transform;

        for (int i = 0; i < 10; i++)
        {
            var obj = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            obj.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(Random.Range(-1, 1), 0f, Random.Range(-1, 1)));
            obj.gameObject.transform.SetParent(container);
            Destroy(obj, 1.5f);
        }
        Destroy(gameObject, 2f);
    }
}
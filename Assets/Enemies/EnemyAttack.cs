using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackDamage = 5f;
    public GameObject fireSmoke;

    Light spotLight;
    ParticleSystem muzzleFlash;

    void Awake()
    {
        spotLight = GetComponentInChildren<Light>();
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
    }

    public void Alert(Color col)
    {
        spotLight.color = col;
    }

    public void DealDamage(Transform player)
    {
        Instantiate(fireSmoke, muzzleFlash.transform.position, Quaternion.identity);
        var main = muzzleFlash.main;
        main.startRotation = transform.rotation.y + 90f;
        muzzleFlash.Play();
        player.GetComponent<PlayerManager>().TakeDamage(attackDamage);
    }
}

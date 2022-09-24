using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float maxHealth = 100f;
    [SerializeField]
    float health;
    PlayerHealthBar healthBar;
    SpriteRenderer[] sprites;
    Color currentColor;
    PlayerAnimationManager animManager;


    void Awake()
    {
        GameManager.instance.IsPlayerAlive = true;
        sprites = GetComponentsInChildren<SpriteRenderer>();
        healthBar = GameObject.FindObjectOfType<PlayerHealthBar>();
        health = maxHealth;
        animManager = GetComponent<PlayerAnimationManager>();
        if (healthBar != null)
            healthBar.UpdateHealthBar(health);
    }

    public void TakeDamage(float damage)
    {
        float ratio = health / (float)maxHealth;
        currentColor = new Color(1f, ratio, ratio);
        foreach(var sprite in sprites)
            sprite.color = (currentColor);
        health -= damage;

        if(healthBar != null)
            healthBar.UpdateHealthBar(health);

        if (health <= 0)
        {
            GameManager.instance.IsPlayerAlive = false;
            Destroy(gameObject);
        }
    }

}

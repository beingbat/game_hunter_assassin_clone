using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthBar : MonoBehaviour
{

    Slider health;

    void Awake()
    {
        health = GetComponent<Slider>();
    }

    public void UpdateHealthBar(float val)
    {
        health.value = val;
    }

}

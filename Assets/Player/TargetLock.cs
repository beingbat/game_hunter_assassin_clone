using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLock : MonoBehaviour
{
    public float fadeTime = 2f;
    SpriteRenderer s;
    float def;
    void Start()
    {
        s = GetComponent<SpriteRenderer>();
        def = transform.localScale.x;
    }

    void Update()
    {

        if (transform.localScale.x > 1f + def)
            gameObject.SetActive(false);

        float temp = Time.deltaTime / fadeTime;
        Vector3 scale = new Vector3(temp, temp, temp);
        s.color = new Color(s.color.r, s.color.g, s.color.b, s.color.a - temp);
        transform.localScale += scale;
    }
}

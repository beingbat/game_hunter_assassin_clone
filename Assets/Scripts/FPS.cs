using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    Text t;

    void Start()
    {
        t = GetComponent<Text>();
        StartCoroutine(FP());
    }
    
    IEnumerator FP()
    {
        while (true)
        {
            t.text = (1f / Time.deltaTime).ToString("00.0");
            yield return new WaitForSeconds(0.3f);
        }
    }
}

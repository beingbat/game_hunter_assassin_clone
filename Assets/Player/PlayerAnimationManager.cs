using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{

    Animator[] anims;

    void Start()
    {
        anims = GetComponentsInChildren<Animator>(); 
        foreach (var anim in anims)
            anim.SetBool("Running", false);
    }

    public void Running()
    {
        foreach(var anim in anims)
            anim.SetBool("Running", true);
    }

    public void Idle()
    {
        foreach (var anim in anims)
            anim.SetBool("Running", false);
    }

    public void Attack()
    {
        foreach (var anim in anims)
            anim.SetTrigger("Attack");
    }

}

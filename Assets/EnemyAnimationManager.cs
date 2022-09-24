using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    Animator[] anims;

    void Awake()
    {
        anims = GetComponentsInChildren<Animator>();
    }


    public void SetToRun()
    {
        foreach (var anim in anims)
        {
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Idle");
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Run");
        }
    }

    public void SetToWalk()
    {
        foreach (var anim in anims)
        {
            anim.ResetTrigger("Run");
            anim.ResetTrigger("Idle");
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Walk");
        }
    }

    public void SetToIdle()
    {
        foreach (var anim in anims)
        {
            anim.ResetTrigger("Run");
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Idle");
        }
    }

    public void SetToAttack()
    {
        foreach (var anim in anims)
        {
            anim.ResetTrigger("Run");
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Idle");
            anim.SetTrigger("Attack");
        }
    }


}

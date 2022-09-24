using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public sealed class AttackState : State<EnemyManager>
{
    public Transform player;
    public override void Begin(EnemyManager e)
    {
        e.SetLightColor(new Color(1f, 0f, 0f));
        e.FindAndAttackPlayer(player);
    }
    public override void Execute(EnemyManager e)
    {
    }
    public override void End(EnemyManager e)
    {
        player = null;
    }
}

public sealed class IdleState : State<EnemyManager>
{
    public override void Begin(EnemyManager e)
    {
        e.SetLightColor(new Color(1f, 1f, 1f));
        e.GoIdle();
    }

    public override void Execute(EnemyManager e)
    {
    }

    public override void End(EnemyManager e)
    {
    }
}

public sealed class ChaseState : State<EnemyManager>
{
    public Transform playerPosition;
    
    public override void Begin(EnemyManager e)
    {
        e.SetLightColor(new Color(0f, 1f, 0f));
        e.ChasePlayer(playerPosition);
    }
    public override void Execute(EnemyManager e)
    {
        e.Chase(playerPosition);
    }
    public override void End(EnemyManager e)
    {
    }
}

public sealed class FindState : State<EnemyManager>
{
    public Transform target;
    
    public override void Begin(EnemyManager e)
    {
        e.SetLightColor(new Color(0f, 0f, 1f));
        e.GotoLastLocation(target.position); 
    }
    public override void Execute(EnemyManager e)
    {
        e.CheckForRevertToPatrol(target);
    }
    public override void End(EnemyManager e)
    {
    }
}

public sealed class PatrolState : State<EnemyManager>
{
    public override void Begin(EnemyManager e)
    {
        e.StopAutoBraking();
        e.SetLightColor(new Color(1f, 1f, 1f));
        e.BeginPatrol();
    }

    public override void Execute(EnemyManager e)
    {
    }

    public override void End(EnemyManager e)
    {
        e.EnableAutoBraking();
    }
}
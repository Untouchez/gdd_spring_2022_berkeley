using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : State
{
    public State attackState;
    public float attackRange;
    public override void AwakeCurrentState()
    {
        stateManager.updateRate = updateRate;  
    }

    public override State RunCurrentState()
    {
        if(Vector3.Distance(transform.position,player.position) < attackRange)
        {
            anim.SetTrigger("alarmed");
        } else {
            anim.SetFloat("speed", agent.velocity.magnitude / agent.speed);
            agent.SetDestination(player.position);
        }
        return this;
    }
}

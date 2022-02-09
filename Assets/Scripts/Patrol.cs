using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    public State chase;
    public float walkRadius;
    Vector3 startPos;
    public override void AwakeCurrentState()
    {
        isDone = true;
        startPos = me.transform.position;
    }

    public override State RunCurrentState()
    {
        if(IsPlayerInfrontOfMe() && CanSeePlayer())
        {
            chase.AwakeCurrentState();
            return chase;
        }

        if (isDone)
            StartCoroutine(MoveToRandomPos());

        anim.SetFloat("speed", agent.velocity.magnitude / agent.speed);
        return this;
    }

    public bool isDone;
    public IEnumerator MoveToRandomPos()
    {
        isDone = false;
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += startPos;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, Random.Range(-walkRadius, walkRadius), 1);
        Vector3 finalPosition = hit.position;
        agent.SetDestination(finalPosition);
        yield return new WaitForSeconds(Random.Range(0.2f,0.2f));
        isDone = true;
    }
}

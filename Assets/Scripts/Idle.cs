using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    public State patrol;
    public override void AwakeCurrentState()
    {
        
    }

    public override State RunCurrentState()
    {
        return patrol;
    }
}

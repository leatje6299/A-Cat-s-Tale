using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailLongState : TailBaseState
{
    public TailLongState()
    {
        stateID = 1;
        name = "Long";
    }
    public override void EnterState(TailStateManager tail)
    {
        stateID = 1;
        name = "Long";
    }
    public override void UpdateState(TailStateManager tail)
    {

    }
    public override List<float> GetPlayerSpeedAndGravity()
    {
        List<float> result = new List<float>(2);
        result.Add(-9.81f);
        result.Add(5f);
        result.Add(14f);
        return result;
    }
    public override float GetPuffyDuration()
    {
        return 0f;
    }
}

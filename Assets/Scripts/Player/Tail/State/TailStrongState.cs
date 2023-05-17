using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailStrongState : TailBaseState
{
    public TailStrongState()
    {
        stateID = 3;
        name = "Strong";
    }
    public override void EnterState(TailStateManager tail)
    {
        stateID = 3;
        name = "Strong";
    }
    public override void UpdateState(TailStateManager tail)
    {

    }
    public override List<float> GetPlayerSpeedAndGravity()
    {
        List<float> result = new List<float>(3);
        result.Add(-9.81f * 2); //gravity
        result.Add(3.0f); //speed
        result.Add(12.5f); //jump force
        return result;
    }

    public override float GetPuffyDuration()
    {
        return 0f;
    }
}

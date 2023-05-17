using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailPuffyState : TailBaseState
{
    public TailPuffyState()
    {
        stateID = 2;
        name = "Puffy";
    }
    public override void EnterState(TailStateManager tail)
    {
        stateID = 2;
        name = "Puffy";
    }
    public override void UpdateState(TailStateManager tail)
    {
        puffyDuration = 6f;
    }
    public override List<float> GetPlayerSpeedAndGravity()
    {
        List<float> result = new List<float>(2);
        result.Add(-9.81f);
        result.Add(7.0f);
        result.Add(18f);
        return result;
    }
    public override float GetPuffyDuration()
    {
        return puffyDuration;
    }
}

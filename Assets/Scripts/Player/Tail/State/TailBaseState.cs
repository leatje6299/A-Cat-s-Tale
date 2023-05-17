using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TailBaseState 
{
    public int stateID;
    public string name;
    public Vector3 playerPos;
    public float puffyDuration;

    public abstract void EnterState(TailStateManager tail);
    public abstract void UpdateState(TailStateManager tail);
    public abstract List<float> GetPlayerSpeedAndGravity();

    public abstract float GetPuffyDuration();

}

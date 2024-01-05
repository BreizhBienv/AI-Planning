using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveToChest_Action : BaseAction
{
    public MoveToChest_Action(Action pAction) : base(pAction)
    {
        _conditions.Add(EWorldState.NEAR_CHEST, false);
        _conditions.Add(EWorldState.HAS_INGOTS, true);
        _conditions.Add(EWorldState.STORE_INGOT, false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWS = new(pSimulated)
        {
            [EWorldState.NEAR_CHEST] = true
        };

        return newWS;
    }

    public override void StartAction(MinerAgent pAgent)
    {
        if (_hasStarted)
            return;

        _hasStarted = true;

        pAgent._perceivedWorldState[EWorldState.NEAR_CHUNK] = false;
        pAgent._perceivedWorldState[EWorldState.NEAR_FURNACE] = false;

        if (pAgent._target != null)
            return;

        Debug.Log("Moving To Chest");
        pAgent._target = World.Instance.GetRandomChest().gameObject;


        pAgent._navMeshAgent.SetDestination(pAgent._target.transform.position);
    }

    public override void Execute(MinerAgent pAgent)
    {
        Chest chest = pAgent._target?.GetComponent<Chest>();
        if (chest != null)
            return;

        pAgent._target = null;
        pAgent._navMeshAgent.isStopped = true;
    }

    public override bool IsComplete(MinerAgent pAgent)
    {
        if (pAgent._target == null || pAgent.CloseEnoughToTarget())
            return false;
        
        Debug.Log("Moved To " + pAgent._target.name);
        return true;
    }

    public override void OnFinished(MinerAgent pAgent)
    {
        pAgent._perceivedWorldState[EWorldState.NEAR_CHEST] = true;
    }
}

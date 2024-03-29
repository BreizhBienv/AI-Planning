using System;
using System.Collections.Generic;
using UnityEngine;

public class StoreIngot_Action : BaseAction
{
    public StoreIngot_Action()
    {
        _conditions.Add(EWorldState.NEAR_CHEST,         true);
        _conditions.Add(EWorldState.HAS_INGOTS,         true);
        _conditions.Add(EWorldState.STORE_INGOT,    false);
    }

    public override Dictionary<EWorldState, bool> ApplyEffect(Dictionary<EWorldState, bool> pSimulated)
    {
        Dictionary<EWorldState, bool> newWS = new(pSimulated)
        {
            [EWorldState.STORE_INGOT] = true
        };

        return newWS;
    }

    public override void StartAction(MinerAgent pAgent)
    {
        Chest chest = pAgent._target?.GetComponent<Chest>();
        if (chest == null)
        {
            pAgent._perceivedWorldState[EWorldState.NEAR_CHEST] = false;
            return;
        }

        Debug.Log("Storing Ingot");
    }

    public override void Execute(MinerAgent pAgent)
    {
        Chest chest = pAgent._target?.GetComponent<Chest>();
        if (chest == null)
            return;

        chest.StoreIngot(pAgent._ingotPossesed);
        pAgent._ingotPossesed = 0;
    }

    public override bool IsComplete(MinerAgent pAgent, float pTimeInAction)
    {
        return true;
    }

    public override void FinishAction(MinerAgent pAgent)
    {
        if (pAgent._ingotPossesed > 0)
            return;

        pAgent._perceivedWorldState[EWorldState.HAS_INGOTS] = false;
    }

    public override void AbortAction(MinerAgent pAgent)
    {
    }
}

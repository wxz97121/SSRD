using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Patchouli : AI
{

    override protected void Update()
    {
        base.Update();
    }

    override protected void Start()
    {
        base.Start();
    }

    public override void Action()
    {
        base.Action();
        _skillDictionary[skillSequence[actionID]].EffectFunction(this);

        actionID = (actionID + 1) % skillSequence.Count;
    }
}

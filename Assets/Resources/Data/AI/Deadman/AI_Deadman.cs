using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AI_Deadman : AI
{

    override protected void Update()
    {
        base.Update();
    }

    override protected void Start()
    {
        base.Start();
    }

    public override void Action(int beatnum)
    {
        base.Action(beatnum);
        //_skillDictionary[skillSequence[actionID]].EffectFunction(this);
        //actionID++;
        //if (actionID >= skillSequence.Count)
            //actionID = 0;
        Text text = GameObject.Find("flag2").GetComponent<Text>();
        text.text = (string)SoundController.Instance.timelineInfo.lastMarker;
    }
}

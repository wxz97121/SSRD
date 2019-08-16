using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_counter : Buff
{

    public string SGStr;
    public override void BuffAdded(Character p_chara,string str)
    {
        m_name = "counter";
        SGStr = str;
        base.BuffAdded(p_chara,str);




    }

    public void CounterAction()
    {
        Debug.Log("counter action!");
        (character as AI).SGSInsert(SGStr);
        Queue < string > playerskills= new Queue<string>();
        playerskills.Enqueue("ANI_player-damaged");
        playerskills.Enqueue("ANI_player-damaged");
        playerskills.Enqueue("ANI_player-damaged");
        playerskills.Enqueue("ANI_player-damaged");
        playerskills.Enqueue("ANI_player-idle");


        character.mTarget.GetComponent<Player>().IntoAutoMode(playerskills);
    }

    public override void BuffBeat(int beatNum)
    {
        base.BuffBeat(beatNum);


    }

    public override void BuffDecay()
    {
        base.BuffDecay();
        if (remainBeats == 0)
        {
            //Player.Instance.animator.Play("player-idle", 0);
        }
    }



}

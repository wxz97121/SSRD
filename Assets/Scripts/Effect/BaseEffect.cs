using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFF_DMG : CharacterEffect
{
    public int times;
    public Value value;
    public EFF_DMG (){
        times = 1;
        value = new VAL_CONST(0);
    }
    public override bool AreTargetAvailable(Skill skill, Character target)
    {
        return true;
    }
    public override void Resolve(Skill skill, List<Character> targets)
    {
        for (int i = 0; i < times;i++){
            foreach (var target in targets)
            {
                //TODO 普通伤害
                //target.controller.CreatureDamaged(source, value.GetValue(sourceCard, source));
            }
        }
    }
}

public class EFF_HEL : CharacterEffect
{
    public int times;
    public Value value;
    public EFF_HEL()
    {
        times = 1;
        value = new VAL_CONST(0);
    }
    public override bool AreTargetAvailable(Skill skill, Character target)
    {
        return true;
    }
    public override void Resolve(Skill skill, List<Character> targets)
    {
        for (int i = 0; i < times; i++)
        {
            foreach (var target in targets)
            {
               //VFXManager.Instance.VFXSequences.Add(new VFX(target, 0.5f, "+" + value.GetValue(sourceCard, source).ToString()));

               //target.Health = Function.Translate(target.Health, value.GetValue(sourceCard, source), target.stats.MaxHealth);
            //TODO 治疗效果
            }
        }
    }

}



public class EFF_SHD : CharacterEffect
{
    public int times;
    public Value value;
    public EFF_SHD()
    {
        times = 1;
        value = new VAL_CONST(0);
    }
    public override bool AreTargetAvailable(Skill skill, Character target)
    {
        return true;
    }
    public override void Resolve(Skill skill, List<Character> targets)
    {
        for (int i = 0; i < times; i++)
        {
            foreach (var target in targets)
            {
                //VFXManager.Instance.VFXSequences.Add(new VFX(target, 0.5f, "护甲+" + value.GetValue(sourceCard, source).ToString()));
               //target.Shield = Function.Translate(target.Shield, value.GetValue(sourceCard, source), 999);

            }
        }
    }
}


//演出？
public class EFF_ACT : CharacterEffect
{
    public int times;
    public Value value;
    public EFF_ACT()
    {
        times = 1;
        value = new VAL_CONST(0);
    }
    public override bool AreTargetAvailable(Skill skill, Character target)
    {
        return true;
    }
    public override void Resolve(Skill skill, List<Character> targets)
    {
        for (int i = 0; i < times; i++)
        {
            foreach (var target in targets)
            {
               // VFXManager.Instance.VFXSequences.Add(new VFX(target, 0.5f, "行动+" + value.GetValue(sourceCard, source).ToString()));

              // target.Act += value.GetValue(skill, target);
            }
        }
    }
}

public class EFF_BUFF : CharacterEffect
{
    public int buffduration;
    public int buffid;
    public EFF_BUFF()
    {
        buffduration = 1;
        buffid = 400000;
    }
    public override bool AreTargetAvailable(Skill skill, Character target)
    {
        return true;
    }
    public override void Resolve(Skill skill, List<Character> targets)
    {
        foreach (var target in targets)
        {
            //var libraryBuff = DB.GetBuff(buffid);
            //var instBuff = libraryBuff.CreateRuntimeInstance(buffduration);
           //instBuff.AddToCreature(target);
            
           // VFXManager.Instance.VFXSequences.Add(new VFX(target, 0.5f, "BUFF+" + libraryBuff.name));
        }
    }
}


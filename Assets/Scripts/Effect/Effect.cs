using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect{
    public virtual bool AreTargetAvailable (Skill skill, Character target){
        return true;
    }
    public virtual void Resolve(Skill skill, List<Character> targets)
    {
    }

}

public abstract class CharacterBaseEffect : Effect
{

}

public abstract class CardBaseEffect : Effect
{
}

public abstract class CharacterEffect : CharacterBaseEffect
{

}
public abstract class CardEffect : CardBaseEffect
{

}


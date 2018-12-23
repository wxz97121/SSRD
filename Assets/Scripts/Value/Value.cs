using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Value
{
    public abstract int GetValue(Skill skill, Character character);
    public abstract string GetValueView(Skill skill, Character character);
}
//常数
public class VAL_CONST : Value
{
    public VAL_CONST(int v)
    {
        value = v;
    }
    public int value;
    public override int GetValue(Skill skill, Character character)
    {
        return value;
    }
    public override string GetValueView(Skill skill, Character character)
    {
        return value.ToString();
    }
}
//随机数
public class VAL_RNG : Value {
    public int min;
    public int max;
    public VAL_RNG(int a, int b)
    {
        min = a;
        max = b;
    }
    public override int GetValue(Skill skill, Character character)
    {
        System.Random rng = new System.Random();
        return rng.Next(min, max);
    }
    public override string GetValueView(Skill skill, Character character)
    {
        return min.ToString()+"-"+max.ToString();
    }
}

//当前能量值
public class VAL_ACT : Value
{
    //倍数
    public int times;
    public VAL_ACT(int t)
    {
        times = t;
    }
    public override int GetValue(Skill skill, Character character)
    {
        int num = character.Mp;
        return times * num;
    }
    public override string GetValueView(Skill skill, Character character)
    {
        return "X";
    }
}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "Shop Data")]

public class ShopData : ScriptableObject
{
    public string m_name;
    public List<Goods> goods;
}

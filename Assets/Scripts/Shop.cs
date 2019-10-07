using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//商店类
public class Shop
{
    public List<Goods> goods=new List<Goods>();

    public ShopData data;

    //如果没加载过，就把DATA里的商品读进来
    public void Init()
    {
        if (goods.Count == 0)
        {
            foreach (Goods g in data.goods)
            {
                goods.Add(g);
            }
        }
    }








}



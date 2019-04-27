﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//战斗逻辑
public class DuelController : MonoBehaviour {

    public List<string> enemyList;
    //敌人总数
    public int enemyNum = 0;
    //当前敌人序号
    public int enemyIndex = 0;


    //记录玩家在行动拍是否输入过，其实是个锁
    public bool isInputedAtActionBeat = false;

    //ui控制
    public Transform UICanvas;

    //敌人的ui模板
    public GameObject enemyUIPrefab;

    #region 单例
    static DuelController _instance;
    private void Awake()
    {
        _instance = this;
    }
    public static DuelController Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion


    // Use this for initialization
    void Start () {
        UICanvas = GameObject.Find("Canvas").transform;

    }

    // Update is called once per frame
    void Update () {
		
	}




    public void EnemyRespawn()
    {

        if(Player.Instance.enemyList.Count==0)
            AddEnemy();

    }



    public void AddEnemy()
    {
        GameObject instEnemy = Instantiate((GameObject)Resources.Load("Data/AI/" + enemyList[enemyIndex] + "/Prefab", typeof(GameObject)), new Vector3(4, 0, 0), Quaternion.identity);
        SuperController.Instance.enemyBattleInfo.hPArea.chara = instEnemy.GetComponent<AI>() as Character;
        SuperController.Instance.enemyBattleInfo.init();
        instEnemy.GetComponent<AI>().m_name = enemyList[enemyIndex];
        instEnemy.GetComponent<AI>().Init();

        Player.Instance.enemyList.Add(instEnemy);
        Player.Instance.mTarget = instEnemy;
        enemyIndex = (enemyIndex + 1) % enemyList.Count;
    }



    public void ClearEnemy()
    {
        enemyIndex = 0;
        for (int i = Player.Instance.enemyList.Count - 1; i >= 0; i--)
        {
            var enemy = Player.Instance.enemyList[i];
            enemy.GetComponent<AI>().Die();
        }
    }

}

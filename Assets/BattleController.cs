using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {

    public BarController player;
    public BarController enemy;

    public bool resultLock = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if((player.mBeatTypeCurrent == beatType.Result) && (enemy.mBeatTypeCurrent == beatType.Result) && (!resultLock)){
            resultLock = true;
            SettleBattle();
        }
        if ((player.mBeatTypeCurrent == beatType.Battle) && (enemy.mBeatTypeCurrent == beatType.Battle) && (resultLock))
        {
            resultLock = false;
        }
    }

    public void SettleBattle (){

        actionType playerAction = player.mBeatActionCurrent;
        actionType enemyAction = enemy.mBeatActionCurrent;


        if (playerAction == actionType.Charge){
            if (enemyAction == actionType.Charge)
            {
                player.player.Charge();
                enemy.player.Charge();
            }
            else if (enemyAction == actionType.Hit)
            {
                if (enemy.player.Hit()){

                }
                else {
                    player.player.Charge();
                }
            }
            else if (enemyAction == actionType.Defense)
            {
                player.player.Charge();
            }
            else
            {
                player.player.Charge();
            }
        }
        else if (playerAction == actionType.Hit)
        {
            if (enemyAction == actionType.Charge)
            {
                if (player.player.Hit())
                {

                }
                else
                {
                   enemy.player.Charge();
                }
            }
            else if (enemyAction == actionType.Hit)
            {
                player.player.Hit();
                enemy.player.Hit();
            }
            else if (enemyAction == actionType.Defense)
            {

            }
            else {
                player.player.Hit();
            }
        }
        else if (playerAction == actionType.Defense)
        {
            if (enemyAction == actionType.Charge)
            {
                enemy.player.Charge();
            }
            else if (enemyAction == actionType.Hit)
            {

            }
            else if (enemyAction == actionType.Defense)
            {
            }
            else
            {

            }
        }
        else {
            if (enemyAction == actionType.Charge)
            {
                enemy.player.Charge();
            }
            else if (enemyAction == actionType.Hit)
            {
                enemy.player.Hit();
            }
            else if (enemyAction == actionType.Defense)
            {
            }
            else
            {

            }
        }
        Debug.Log(playerAction + "/" + enemyAction);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBarController : MonoBehaviour {

    //小节列表
    public List<UIBar> currentBarList;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //初始化
    public void InitBarArea()
    {

    }

    //读取一个新的BAR
    public UIBar InitBarByScore(List<Note> notes)
    {
        UIBar uIBar=new UIBar();


        return uIBar;
    }

    //小节整体上移
    public void BarMoving()
    {

    }

    //指针移动
    public void PinMoving()
    {

    }

    //移除NOTE
    public void RemoveNote()
    {

    }

}

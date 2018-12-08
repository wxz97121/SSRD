using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeViewTest : MonoBehaviour {
    public GameObject[] chargePoint;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        string sequence = "";
        foreach(var i in Player.Instance.mChargeList)
        {
            sequence += i.ToString() + "/";
        }
        GetComponent<Text>().text = sequence;
	}
}

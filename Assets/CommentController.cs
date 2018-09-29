using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentController : MonoBehaviour {

    public float aniTime = 0.2f;
    public Sprite[] comments;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator PopUp (){
        float timeCount = 0f;
        while (timeCount<aniTime){
            yield return new WaitForSeconds(Time.deltaTime);
            timeCount += Time.deltaTime;
            GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f - timeCount * timeCount * 25f);
        }
    }

    public void CallCommentUpdate (int type){
        GetComponent<Image>().sprite = comments[type];
        StartCoroutine("PopUp");
    }
}

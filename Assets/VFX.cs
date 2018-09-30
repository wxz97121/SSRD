using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VFX : MonoBehaviour {
    public float aniTime = 0.2f;
    public int type = 0;
    public Sprite[] frames;
    // Use this for initialization
    private void Awake()
    {

    }
    void Start () {
        if (type == 0){
            StartCoroutine("FadeOut");
        }
        if (type == 2){
            StartCoroutine("FrameAnimation");
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator FadeOut()
    {
        float timeCount = 0f;
        while (timeCount < aniTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timeCount += Time.deltaTime;
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - timeCount * timeCount * 25f);
        }
        Destroy(gameObject);
    }

    IEnumerator FrameAnimation()
    {
        int fCount = 0;
        while (fCount<frames.Length)
        {
            GetComponent<SpriteRenderer>().sprite = frames[fCount++];
            yield return new WaitForSeconds(Time.deltaTime * 2f);
        }
        Destroy(gameObject);
    }
}

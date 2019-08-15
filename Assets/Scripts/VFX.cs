using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//特效控制：0-淡出，1-静态，2-帧动画
public class VFX : MonoBehaviour {
    public float aniTime = 0.2f;
    public int type = 0;
    public Sprite[] frames;
    public float killtime;
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

        if (type == 1)
        {
            StartCoroutine("KillInTime");
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

    IEnumerator NoteInputBad()
    {
        Instantiate((GameObject)Resources.Load("VFX/BadX", typeof(GameObject)), transform);
        float timeCount = 0f;
        Vector3 originPos = transform.localPosition;
        
        while (timeCount < aniTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            System.Random rng = new System.Random();
            Vector3 glitch =new Vector3(rng.Next(-5, 5), rng.Next(-5, 5), rng.Next(-5, 5));
            transform.localPosition = originPos + glitch;
            timeCount += Time.deltaTime;
            
        }
        Destroy(gameObject);
    }


    IEnumerator FadeOutLarger()
    {
        float timeCount = 0f;
        while (timeCount < aniTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timeCount += Time.deltaTime;
            GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f - timeCount * timeCount * 25f);
            transform.localScale *= 1 + timeCount;
        }
        Destroy(gameObject);
    }

    IEnumerator SkillTipNoteRight()
    {
    //    Instantiate((GameObject)Resources.Load("VFX/RightO", typeof(GameObject)), transform);
        float timeCount = 0f;
    //    Vector3 originPos = transform.localPosition;

        while (timeCount < aniTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);

           //
            timeCount += Time.deltaTime;

        }
    }

    IEnumerator KillInTime()
    {
        yield return new WaitForSeconds(killtime);
        Destroy(gameObject);

    }


    public IEnumerator GetHit()
    {
        float timeCount = 0f;
        GetComponent<SpriteRenderer>().color = new Color(1f, 0f,0f);

        while (timeCount < aniTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timeCount += Time.deltaTime;
            var deltacolor = timeCount / aniTime;
            GetComponent<SpriteRenderer>().color = new Color(1f, deltacolor * deltacolor, deltacolor * deltacolor);
        }
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);


    }


    public void Kill()
    {
        Destroy(gameObject);

    }

    public static GameObject ShowVFX(string str,Vector3 vector,Character Char)
    {
       return Instantiate(Resources.Load("VFX/"+str),Char.transform.position+vector, Quaternion.identity,Char.transform) as GameObject;
    }

    public static GameObject ShowVFX(string str, Vector3 vector)
    {
        return Instantiate(Resources.Load("VFX/" + str),  vector, Quaternion.identity) as GameObject;
    }
}

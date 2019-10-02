using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour
{
    //要播放的prefab
    public GameObject storyAnim;

    //播放完成后触发的剧情
    public string NextStoryStep;



    public static void PlayStoryAnim(string name)
    {
        UIWindowController.Instance.arrow.transform.localScale = Vector3.zero;

        ShowStory();
        Debug.Log("Play : "+ "Prefab/Story/" + name);
        GameObject ins = Instantiate(Resources.Load<GameObject>("Prefab/Story/"+name),GameObject.Find("StoryBoard").transform) as GameObject;
        Story story = ins.GetComponent<Story>();
        story.storyAnim = ins;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ShowStory()
    {
        SuperController.Instance.StoryCanvas.localScale = Vector3.one;
    }
    public static void HideStory()
    {
        SuperController.Instance.StoryCanvas.localScale = Vector3.zero;
    }

    public void Kill()
    {
        SuperController.Instance.NextStep(NextStoryStep);
        Destroy(gameObject);
        Story.HideStory();

    }
}

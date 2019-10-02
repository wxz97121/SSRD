using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class UIWindow : MonoBehaviour
{
    public List<Button> buttons;
    public GameObject lastselect;


    // Start is called before the first frame update
    void Start()
    {
        lastselect = new GameObject();

        //Init();




    }

    public virtual void SetSelect()
    {

    }

    public virtual void Init()
    {
        buttons = new List<Button>(this.transform.GetComponentsInChildren<Button>());

        foreach (Button b in buttons)
        {
            b.onClick.AddListener(delegate () {
                OnClick(b);
            });
        }
        SetSelect();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastselect);
        }
        else
        {
            lastselect = EventSystem.current.currentSelectedGameObject;
        }
    }

    public virtual void OnClick(Button button)
    {
        Debug.Log("button click : "+button.name);
    }
}

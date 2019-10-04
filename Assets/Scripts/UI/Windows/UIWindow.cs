using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


public class UIWindow : MonoBehaviour
{
    public List<Button> buttons;
    public GameObject lastselect;
    public bool isFocus = false;

    // Start is called before the first frame update
    void Start()
    {


        //Init();




    }

    public virtual void SetSelect()
    {
        lastselect= EventSystem.current.currentSelectedGameObject;
    }

    public virtual void Init()
    {


        buttons = new List<Button>(this.transform.GetComponentsInChildren<Button>());

        foreach (Button b in buttons)
        {
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(delegate () {
                OnClick(b);
            });
            if (b.GetComponent<MyEventHandler>() != null)
            {
                b.GetComponent<MyEventHandler>().onSelect.RemoveAllListeners();
                b.GetComponent<MyEventHandler>().onSelect.AddListener(delegate () {
                    OnSelect(b);
                });
            }
        }


        UIWindowController.Instance.ClearFocus();
        isFocus = true;
        SetSelect();

        UIWindowController.Instance.arrow.transform.position =new Vector3(0, 0, 0);
        UIWindowController.Instance.StartCoroutine("ArrowMove");
    }

    // Update is called once per frame
   public void Update()
    {
        if (isFocus)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(lastselect);
            }
            else
            {
                if (lastselect != EventSystem.current.currentSelectedGameObject)
                {
                    OnChangeSelect();

                }


                lastselect = EventSystem.current.currentSelectedGameObject;
            }
        }

    }

    public virtual void OnClick(Button button)
    {
        Debug.Log("button click : "+button.name);
    }
    public virtual void OnSelect(Button button)
    {
        Debug.Log("button select : " + button.name);
    }
    public virtual void OnChangeSelect()
    {


        lastselect = EventSystem.current.currentSelectedGameObject;


        UIWindowController.Instance.StartCoroutine("ArrowMove");
        //UIWindowController.Instance.ArrowMoveDT();

    }

    public void AllButtonDisconnect()
    {
        foreach(Button b in buttons)
        {
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.None;
            b.navigation = nav;
        }
    }

    public void AllButtonConnect()
    {
        foreach (Button b in buttons)
        {
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.Automatic;
            b.navigation = nav;
        }
    }
}

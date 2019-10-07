using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


public class UIWindow : MonoBehaviour
{
    public List<Button> buttons;
    public bool isFocus = false;
    public UIWindow lastWindow=null;
    public bool canBeClosedByCancel=false;
    public GameObject tempselect;

    // Start is called before the first frame update
    void Start()
    {


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
        SetSelect();

        UIWindowController.Instance.StopCoroutine("ArrowMove");
        UIWindowController.Instance.StartCoroutine("ArrowMove");
    }

    // Update is called once per frame
   public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isFocus&&canBeClosedByCancel)
            {
                this.Close();
            }
        }
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    if (isFocus && canBeClosedByCancel)
        //    {
        //        this.Close();
        //    }
        //}

    }

    public virtual void OnClick(Button button)
    {
        Debug.Log("button click : "+button.name);
    }
    public virtual void OnSelect(Button button)
    {
        Debug.Log("button select : " + button.name);
        tempselect = button.gameObject;
    }



    public void AllButtonDisconnect()
    {
        foreach(Button b in buttons)
        {
            Navigation nav = new Navigation
            {
                mode = Navigation.Mode.None
            };
            b.navigation = nav;
        }
    }

    public void AllButtonConnect()
    {
        foreach (Button b in buttons)
        {
            Navigation nav = new Navigation
            {
                mode = Navigation.Mode.Automatic
            };
            b.navigation = nav;
        }
    }

    //窗口出现
    public void Open()
    {
        gameObject.SetActive(true);
        UIWindowController.Instance.arrow.transform.localScale = Vector3.one;

        if (UIWindowController.Instance.focusWindow != null)
        {
            lastWindow = UIWindowController.Instance.focusWindow;
            UIWindowController.Instance.focusWindow.Unfocus();
        }
        Init();
        Focus();

        transform.localScale = Vector3.one;
    }

    //窗口关闭
    public virtual void  Close()
    {
        transform.localScale = Vector3.zero;
        Debug.Log("lastwindow : " + lastWindow);
        if (lastWindow != null)
        {
            lastWindow.Focus();
        }
        else
        {
            Debug.Log("clear focusWindow");

            UIWindowController.Instance.focusWindow = null;
            Debug.Log("focusWindow : "+ UIWindowController.Instance.focusWindow);

            UIWindowController.Instance.arrow.transform.localScale = Vector3.zero;
        }
        isFocus = false;
        gameObject.SetActive(false);
    }

    //窗口失去焦点
    public void Unfocus()
    {
        UIWindowController.Instance.focusWindow = null;

        AllButtonDisconnect();

    }

    //窗口获得焦点
    public virtual void Focus()
    {
        isFocus = true;

        UIWindowController.Instance.focusWindow = this;
        AllButtonConnect();
        if (tempselect != null)
        {
            EventSystem.current.SetSelectedGameObject(tempselect);
        }
        else
        {
            SetSelect();
        }

    }
}

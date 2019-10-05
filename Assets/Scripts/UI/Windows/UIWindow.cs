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
        isFocus = true;
        SetSelect();

        //UIWindowController.Instance.arrow.transform.position =new Vector3(0, 0, 90.0f);
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

    }

    public virtual void OnClick(Button button)
    {
        Debug.Log("button click : "+button.name);
    }
    public virtual void OnSelect(Button button)
    {
        Debug.Log("button select : " + button.name);
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
        Focus();
        transform.localScale = Vector3.one;
    }

    //窗口关闭
    public void Close()
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
    public void Focus()
    {
        UIWindowController.Instance.focusWindow = this;
        AllButtonConnect();
        Init();

    }
}

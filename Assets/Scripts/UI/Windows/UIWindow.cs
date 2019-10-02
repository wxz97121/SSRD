using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class UIWindow : MonoBehaviour
{
    public List<Button> buttons;
    public GameObject lastselect;
    public bool isFocus = false;

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
        UIWindowController.Instance.ClearFocus();
        isFocus = true;

        UIWindowController.Instance.arrow.transform.localScale = Vector3.one;
        UIWindowController.Instance.StartCoroutine("ArrowMove");

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
            if(lastselect!= EventSystem.current.currentSelectedGameObject)
            {
                OnChangeSelect();

            }
            lastselect = EventSystem.current.currentSelectedGameObject;
        }
    }

    public virtual void OnClick(Button button)
    {
        Debug.Log("button click : "+button.name);
    }

    public virtual void OnChangeSelect()
    {
        UIWindowController.Instance.StartCoroutine("ArrowMove");

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

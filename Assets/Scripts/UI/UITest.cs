using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class UITest : MonoBehaviour
{
    public List<Button> buttons;
    public GameObject lastselect;


    // Start is called before the first frame update
    void Start()
    {
        lastselect = new GameObject();

        buttons = new List<Button>(FindObjectsOfType<Button>());
        GameObject.Find("Train").GetComponent<Button>().Select();
        foreach(Button b in buttons)
        {
            b.onClick.AddListener(delegate () {
                onClick(b);
            });
        }

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

    public void onClick(Button button)
    {
        Debug.Log(button.name);
    }
}

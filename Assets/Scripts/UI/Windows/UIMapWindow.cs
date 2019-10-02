using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIMapWindow : UIWindow
{

    public override void SetSelect()
    {
        base.SetSelect();
        MapController.Instance.currentMapArea.view.GetComponent<Button>().Select();
    }





}

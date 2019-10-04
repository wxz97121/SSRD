using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualNovelController : MonoBehaviour
{
    protected static VisualNovelController _instance;
    public static VisualNovelController Instance
    {
        get
        {
            return _instance;
        }
    }

    public VisualNovelLineView view;
    public NovelScript currentscript;
    public int currentLine;
    public bool isShowing;

    private void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isShowing)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                NextLine(false);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                NextLine(true);
            }
        }
    }

    public void InitScript(NovelScript script)
    {
        if (script == null)
        {
            MapController.Instance.ShowMap();
            return;
        }
        UIWindowController.Instance.arrow.transform.position = new Vector3(10000, 0, 0);

        currentscript = script;

        currentLine = 0;

        view.gameObject.SetActive(true);

        view.UpdateViewInfo(currentscript, currentLine);

        isShowing = true;
    }

    public void NextLine(bool choice)
    {
        var currentLineMsg = currentscript.lines[currentLine];
        if (currentLineMsg.options.Length > 0)
        {
            if (choice)
            {
                currentscript = currentLineMsg.options[1].result;
                currentLine = 0;
                view.UpdateViewInfo(currentscript, currentLine);
            }
            else
            {
                currentscript = currentLineMsg.options[0].result;
                currentLine = 0;
                view.UpdateViewInfo(currentscript, currentLine);
            }
            
        }
        else
        {
            
            if (currentLine+1 < currentscript.lines.Length)
            {
                view.UpdateViewInfo(currentscript, currentLine+1);
                foreach(var eff in currentLineMsg.effstring)
                {
                    EffectResolve(eff);
                }
                
            }
            else
            {
                view.gameObject.SetActive(false);
                isShowing = false;
                foreach (var eff in currentLineMsg.effstring)
                {
                    EffectResolve(eff);
                }
                //MapController.Instance.ShowMap();

            }

            currentLine++;
        }
        
    }

    public void EffectResolve(string eff)
    {
        switch (eff)
        {
            case "startfight":
                {
                    SuperController.Instance.BattleStart();
                }
                break;
            case "showmap":
                {
                    MapController.Instance.ShowMap();
                }
                break;
        }
    }
}

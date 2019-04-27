using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleInfo : MonoBehaviour
{
    public UIEnergyCells energyCells;
    public UIHPArea hPArea;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init()
    {
        if (energyCells != null)
        {
            energyCells.init();
        }
        hPArea.init();

    }

    public void Blink()
    {
        energyCells.Blink();
    }
}

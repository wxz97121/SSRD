using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleInfo : MonoBehaviour
{
    public Character chara;
    public UIEnergyCells energyCells;
    public UIHPArea hPArea;
    public UILifeArea lifeArea;
    public UISoulArea soulArea;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init(Character character)
    {
        chara = character;
        if (energyCells != null)
        {
            energyCells.init();
        }
        hPArea.init(chara);
        lifeArea.init(chara);
        soulArea.init(chara);


    }

    public void Blink()
    {
        if (energyCells != null)
        {
            energyCells.Blink();
        }
        soulArea.Blink();
    }
}

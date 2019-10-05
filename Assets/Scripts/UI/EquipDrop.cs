using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class EquipDrop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int index;
    private Player m_Player;
    [HideInInspector]
    public Image containerImage;
    private Color highlightColor = Color.yellow;
    private Color previousColor;
    private Text m_Text;
    private Text decText;

    private void Awake()
    {
        containerImage = transform.parent.GetComponent<Image>();
        m_Text = GetComponentInChildren<Text>();
    }
    private void Start()
    {
        m_Player = Player.Instance;
        decText = GameObject.Find("EquipDesc").GetComponents<Text>()[0] as Text;

    }
    private void Update()
    {
        switch (index)
        {
            case (0):
                if (Player.Instance.currentWeapon)
                {
                    GetComponent<Image>().sprite = Player.Instance.currentWeapon.Icon;
                    m_Text.text = Player.Instance.currentWeapon.equipName;
                    
                }
                else
                {
                    GetComponent<Image>().sprite = null;
                    m_Text.text = "";
                }
                break;
            case (1):
                if (Player.Instance.currentCloth)
                {
                    GetComponent<Image>().sprite = Player.Instance.currentCloth.Icon;
                    m_Text.text = Player.Instance.currentCloth.equipName;
                }
                else
                {
                    GetComponent<Image>().sprite = null;
                    m_Text.text = "";
                }
                break;
            case (2):
                if (Player.Instance.currentAmulet)
                {
                    GetComponent<Image>().sprite = Player.Instance.currentAmulet.Icon;
                    m_Text.text = Player.Instance.currentAmulet.equipName;
                }
                else
                {
                    GetComponent<Image>().sprite = null;
                    m_Text.text = "";
                }
                break;
            default:
                break;
        }
        /*
        if (Player.Instance.skillSlots[index].skill != null)
        {
            GetComponent<Image>().sprite = Player.Instance.skillSlots[index].skill.Icon;
            m_Text.text = Player.Instance.skillSlots[index].skill.m_name;
        }
        else
        {
            GetComponent<Image>().sprite = null;
            m_Text.text = "";
        }*/
    }

    public void OnDrop(PointerEventData data)
    { 
        var originalDrag = data.pointerDrag.GetComponent<EquipDrag>();
        var originalSkill = originalDrag.m_Equip;
        if (originalSkill == null) return;
        if (m_Player.CheckEquipSlot(index, originalSkill))
        {
            var tmp = m_Player.ChangeEquip(index, originalSkill);
            originalDrag.m_Equip = tmp;
        }
        if (m_Player.skillSlots[index].skill != null)
            decText.text = m_Player.skillSlots[index].skill.Desc;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (m_Player.skillSlots[index].skill != null)
            decText.text = m_Player.skillSlots[index].skill.Desc;
        if (containerImage == null) return;
        if (data.pointerDrag == null) return;
        var originalDrag = data.pointerDrag.GetComponent<EquipDrag>();
        var originalSkill = originalDrag.m_Equip;
        if (originalSkill == null) return;
        if (m_Player.CheckEquipSlot(index, originalSkill))
        {
            previousColor = containerImage.color;
            containerImage.color = highlightColor;
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        decText.text = "";

        if (containerImage == null) return;
        if (data.pointerDrag == null) return;
        var originalDrag = data.pointerDrag.GetComponent<EquipDrag>();
        var originalSkill = originalDrag.m_Equip;
        if (originalSkill == null) return;
        if (m_Player.CheckEquipSlot(index, originalSkill))
        {
            containerImage.color = previousColor;
        }
    }

}
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class SkillDrop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
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
        decText = GameObject.Find("SkillDesc").GetComponents<Text>()[0] as Text;

    }
    private void Update()
    {
        if (Player.Instance.skillSlots[index].skill != null)
        {
            GetComponent<Image>().sprite = Player.Instance.skillSlots[index].skill.Icon;
            m_Text.text = Player.Instance.skillSlots[index].skill.m_name;
        }
        else
        {
            GetComponent<Image>().sprite = null;
            m_Text.text = "";
        }
    }

    public void OnDrop(PointerEventData data)
    { 
        var originalDrag = data.pointerDrag.GetComponent<SkillDrag>();
        var originalSkill = originalDrag.m_Skill;
        if (originalSkill == null) return;
        if (m_Player.CheckSkillSlot(index, originalSkill))
        {
            var tmp = m_Player.ChangeSkill(index, originalSkill);
            originalDrag.m_Skill = tmp;
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
        var originalDrag = data.pointerDrag.GetComponent<SkillDrag>();
        var originalSkill = originalDrag.m_Skill;
        if (originalSkill == null) return;
        if (m_Player.CheckSkillSlot(index, originalSkill))
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
        var originalDrag = data.pointerDrag.GetComponent<SkillDrag>();
        var originalSkill = originalDrag.m_Skill;
        if (originalSkill == null) return;
        if (m_Player.CheckSkillSlot(index, originalSkill))
        {
            containerImage.color = previousColor;
        }
    }

}
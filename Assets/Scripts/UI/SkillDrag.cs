using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class SkillDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    private Color normalColor;
    public Skill m_Skill;
    public string SkillString;
    private Player m_Player;
    [HideInInspector]
    private GameObject m_DraggingIcons;
    private RectTransform m_DraggingPlanes;
    private SkillDrop[] AllDrop;
    private Text m_Text;
    private Text decText;
    [SerializeField]
    private Image m_SkillImage;
    private void Awake()
    {
        m_Text = GetComponentInChildren<Text>();
        AllDrop = FindObjectsOfType<SkillDrop>();
    }
    private void Update()
    {
        if (m_Skill!=null)
        {
            m_SkillImage.sprite = m_Skill.Icon;
            m_Text.text = m_Skill.m_name;
        }
        else
        {
            m_SkillImage.sprite = null;
            //GetComponent<Image>().
            m_Text.text = "";
        }
    }
    private void Start()
    {
        m_Player = Player.Instance;
        decText = GameObject.Find("SkillDesc").GetComponents<Text>()[0] as Text;
    }

    public void InitSkill(string id)
    {
        SkillString = id;
        m_Skill = new Skill(0, SkillString);
    }
    public void OnBeginDrag(PointerEventData data)
    {
        if (m_Skill == null) return;
        foreach(var m_Drop in AllDrop)
        {
            if (m_Player.CheckSkillSlot(m_Drop.index, m_Skill))
                m_Drop.containerImage.color = Color.green;
            else m_Drop.containerImage.color = Color.red;
        }
        var canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
            return;

        m_DraggingIcons = new GameObject("icon");

        m_DraggingIcons.transform.SetParent(canvas.transform, false);
        m_DraggingIcons.transform.SetAsLastSibling();

        var image = m_DraggingIcons.AddComponent<Image>();
        var group = m_DraggingIcons.AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;
        image.sprite = m_SkillImage.overrideSprite;
        image.SetNativeSize();

        m_DraggingPlanes = canvas.transform as RectTransform;
        SetDraggedPosition(data);
    }
    public void OnEndDrag(PointerEventData data)
    {
        foreach (var m_Drop in AllDrop)
            m_Drop.containerImage.color = Color.white;
        Destroy(m_DraggingIcons);
    }
    public void OnDrag(PointerEventData data)
    {
        if (m_DraggingIcons)
            SetDraggedPosition(data);
    }
    //IEnumerator CheckIfNull(Person PersonToCheck)
    //{
    //    yield return new WaitForEndOfFrame();
    //    if (PersonToCheck.m_Image.overrideSprite == null)
    //        PersonToCheck.ID = PersonToCheck.ID;
    //}
    private void SetDraggedPosition(PointerEventData eventData)
    {
        //if (dragOnSurfaces && eventData.pointerEnter != null && eventData.pointerEnter.transform as RectTransform != null)
        //    m_DraggingPlanes[eventData.pointerId] = eventData.pointerEnter.transform as RectTransform;

        var rt = m_DraggingIcons.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlanes, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = m_DraggingPlanes.rotation;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(m_Skill!=null)
            decText.text = m_Skill.Desc;
    }
   public void OnPointerExit(PointerEventData eventData)
    {
        decText.text = "";
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//选择要升级天赋的技能
public class UISkillUpgradeSelectSkill : UIWindow
{
    public Transform content;
    public Transform pos0;
    public Transform pos1;
    public Button targetButton;
    public List<GameObject> Items;

    [Header("scroll view 相关")]
    public Transform svUpPos;
    public Transform svBottomPos;

    public UISkillDesc skillDesc;

    public Text textDesc;

    private Vector2 tempContentSize = new Vector2(0f, 0f);

    public void Open(Button button)
    {
        gameObject.SetActive(true);
        targetButton = button;

        Open();

    }

    // Start is called before the first frame update
    public override void Init()
    {
        Items = new List<GameObject>();
        ShowSkillList();
        base.Init();
    }


    public override void SetSelect()
    {
        Items[0].GetComponent<Button>().Select();
        base.SetSelect();

    }



    private void ShowSkillList()
    {
        if (tempContentSize == new Vector2(0f, 0f))
        {
            tempContentSize = content.GetComponent<RectTransform>().sizeDelta;
        }
        else
        {
            content.GetComponent<RectTransform>().sizeDelta = tempContentSize;
        }

        foreach (Skill skill in Player.Instance.skillList)
        {
            var inst = Instantiate(Resources.Load<GameObject>("Prefab/UI/Buttons/UI_SkillToUpgrade"), content);
            inst.GetComponent<UISelectableItem>().skill = skill;
            inst.name = skill.m_name;
            inst.transform.Find("Image").GetComponent<Image>().sprite = skill.Icon;
            inst.transform.Find("Text").GetComponent<Text>().text = skill.m_name;
            inst.transform.Find("isEquiped").gameObject.SetActive(skill.isEquiped);

            if (skill.upgradeChoice1 == 0)
            {
                inst.transform.Find("Up1").transform.localScale = Vector3.zero;
            }else if (skill.upgradeChoice1 == 1)
            {
                inst.transform.Find("Up1").transform.localScale = Vector3.one;
                inst.transform.Find("Up1").GetComponent<Image>().sprite = skill.skillUpgrade11.sprite;
            }

            switch (skill.upgradeChoice2)
            {
                case 0:
                    inst.transform.Find("Up2").transform.localScale = Vector3.zero;
                    break;
                case 1:
                    inst.transform.Find("Up2").transform.localScale = Vector3.one;
                    inst.transform.Find("Up2").GetComponent<Image>().sprite = skill.skillUpgrade21.sprite;
                    break;
                case 2:
                    inst.transform.Find("Up2").transform.localScale = Vector3.one;
                    inst.transform.Find("Up2").GetComponent<Image>().sprite = skill.skillUpgrade22.sprite;
                    break;
            }

            switch (skill.upgradeChoice3)
            {
                case 0:
                    inst.transform.Find("Up3").transform.localScale = Vector3.zero;
                    break;
                case 1:
                    inst.transform.Find("Up3").transform.localScale = Vector3.one;
                    inst.transform.Find("Up3").GetComponent<Image>().sprite = skill.skillUpgrade31.sprite;
                    break;
                case 2:
                    inst.transform.Find("Up3").transform.localScale = Vector3.one;
                    inst.transform.Find("Up3").GetComponent<Image>().sprite = skill.skillUpgrade32.sprite;
                    break;
            }

            Items.Add(inst);
            inst.transform.localPosition = pos0.localPosition + (pos1.localPosition - pos0.localPosition) * (Items.Count - 1);
            Debug.Log("size delta = " + content.GetComponent<RectTransform>().sizeDelta);
            content.GetComponent<RectTransform>().sizeDelta += new Vector2(0f,  pos0.GetComponent<RectTransform>().anchoredPosition.y- pos1.GetComponent<RectTransform>().anchoredPosition.y);
        }

    }


    


    private void RefreshSkillList()
    {

        foreach (GameObject inst in Items)
        {
            Skill skill = inst.GetComponent<UISelectableItem>().skill;
            //var inst = Instantiate(Resources.Load<GameObject>("Prefab/UI/Buttons/UI_SkillToUpgrade"), content);
            inst.GetComponent<UISelectableItem>().skill = skill;
            inst.GetComponent<UISelectableItem>().type = 1;

            inst.transform.Find("Image").GetComponent<Image>().sprite = skill.Icon;
            inst.transform.Find("Text").GetComponent<Text>().text = skill.m_name;
            inst.transform.Find("isEquiped").gameObject.SetActive(skill.isEquiped);

            if (skill.upgradeChoice1 == 0)
            {
                inst.transform.Find("Up1").transform.localScale = Vector3.zero;
            }
            else if (skill.upgradeChoice1 == 1)
            {
                inst.transform.Find("Up1").transform.localScale = Vector3.one;
                inst.transform.Find("Up1").GetComponent<Image>().sprite = skill.skillUpgrade11.sprite;
            }

            switch (skill.upgradeChoice2)
            {
                case 0:
                    inst.transform.Find("Up2").transform.localScale = Vector3.zero;
                    break;
                case 1:
                    inst.transform.Find("Up2").transform.localScale = Vector3.one;
                    inst.transform.Find("Up2").GetComponent<Image>().sprite = skill.skillUpgrade21.sprite;
                    break;
                case 2:
                    inst.transform.Find("Up2").transform.localScale = Vector3.one;
                    inst.transform.Find("Up2").GetComponent<Image>().sprite = skill.skillUpgrade22.sprite;
                    break;
            }

            switch (skill.upgradeChoice3)
            {
                case 0:
                    inst.transform.Find("Up3").transform.localScale = Vector3.zero;
                    break;
                case 1:
                    inst.transform.Find("Up3").transform.localScale = Vector3.one;
                    inst.transform.Find("Up3").GetComponent<Image>().sprite = skill.skillUpgrade31.sprite;
                    break;
                case 2:
                    inst.transform.Find("Up3").transform.localScale = Vector3.one;
                    inst.transform.Find("Up3").GetComponent<Image>().sprite = skill.skillUpgrade32.sprite;
                    break;
            }

            //Items.Add(inst);
            //inst.transform.localPosition = pos0.localPosition - (pos1.localPosition - pos0.localPosition) * (Items.Count - 1);
        }

    }


    public override void OnClick(Button button)
    {
        if (isFocus)
        {
            base.OnClick(button);
            UIWindowController.Instance.upgradeSkill.Open(button);

        }

    }

    public override void Close(bool isChange = true)
    {
        foreach (GameObject go in Items)
        {
            Destroy(go);
        }
        Items.Clear();
        base.Close(isChange);
    }

    public override void Focus()
    {
        RefreshSkillList();
        base.Focus();
        UpdateDesc(tempselect.GetComponent<Button>());

    }


    public override void OnSelect(Button button)
    {
       // Debug.Log("button y:" + button.GetComponent<RectTransform>().anchoredPosition.y + "  content y:");
        //处理scrollview
        if (button.GetComponent<RectTransform>().anchoredPosition.y+ content.GetComponent<RectTransform>().anchoredPosition.y < svBottomPos.GetComponent<RectTransform>().anchoredPosition.y)
        {
            float distance = svBottomPos.GetComponent<RectTransform>().anchoredPosition.y - (button.GetComponent<RectTransform>().anchoredPosition.y+content.GetComponent<RectTransform>().anchoredPosition.y);
           // Debug.Log("OUT DOWN! + distance=" +distance);

            content.GetComponent<RectTransform>().anchoredPosition = new Vector2(content.GetComponent<RectTransform>().anchoredPosition.x, content.GetComponent<RectTransform>().anchoredPosition.y+distance);
        }



        if (button.GetComponent<RectTransform>().anchoredPosition.y + content.GetComponent<RectTransform>().anchoredPosition.y > svUpPos.GetComponent<RectTransform>().anchoredPosition.y)
        {
            float distance = svUpPos.GetComponent<RectTransform>().anchoredPosition.y - (button.GetComponent<RectTransform>().anchoredPosition.y + content.GetComponent<RectTransform>().anchoredPosition.y);
           // Debug.Log("OUT UP! + distance=" + distance);

            content.GetComponent<RectTransform>().anchoredPosition = new Vector2(content.GetComponent<RectTransform>().anchoredPosition.x, content.GetComponent<RectTransform>().anchoredPosition.y + distance);
        }
        base.OnSelect(button);
        UpdateDesc(button);

    }


    //更新右侧的详情
    public void UpdateDesc(Button button)
    {
        Debug.Log("111");
        if (button.GetComponent<UISelectableItem>().type == 1)
        {
            Debug.Log("222");

            if (button.GetComponent<UISelectableItem>().skill.Icon != null)
            {
                skillDesc.gameObject.SetActive(true);
                textDesc.gameObject.SetActive(false);

                skillDesc.Init(button.GetComponent<UISelectableItem>().skill);
            }
            else
            {
                Debug.Log("333");

                skillDesc.gameObject.SetActive(false);
                textDesc.gameObject.SetActive(false);
            }


        }
        if (button.GetComponent<UISelectableItem>().type == 2)
        {
            if (button.GetComponent<UISelectableItem>().equipment.equipDesc != null)
            {
                skillDesc.gameObject.SetActive(false);
                textDesc.gameObject.SetActive(true);
                textDesc.text = button.GetComponent<UISelectableItem>().equipment.equipDesc;

            }
            else
            {
                skillDesc.gameObject.SetActive(false);
                textDesc.gameObject.SetActive(false);
            }
        }
    }
}

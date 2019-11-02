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

        foreach (Skill skill in Player.Instance.skillList)
        {
            var inst = Instantiate(Resources.Load<GameObject>("Prefab/UI/Buttons/UI_SkillToUpgrade"), content);
            inst.GetComponent<UISelectableItem>().skill = skill;
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
            inst.transform.localPosition = pos0.localPosition - (pos1.localPosition - pos0.localPosition) * (Items.Count - 1);
        }

    }


    private void RefreshSkillList()
    {

        foreach (GameObject inst in Items)
        {
            Skill skill = inst.GetComponent<UISelectableItem>().skill;
            //var inst = Instantiate(Resources.Load<GameObject>("Prefab/UI/Buttons/UI_SkillToUpgrade"), content);
            inst.GetComponent<UISelectableItem>().skill = skill;
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

    public override void Close()
    {
        foreach (GameObject go in Items)
        {
            Destroy(go);
        }
        Items.Clear();
        base.Close();
    }

    public override void Focus()
    {
        RefreshSkillList();
        base.Focus();
    }
}

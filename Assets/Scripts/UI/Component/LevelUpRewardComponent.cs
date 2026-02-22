using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class LevelUpRewardComponent : MonoBehaviour
{
    [SerializeField]
    private Image SkillIcon;

    [SerializeField]
    private Image SkillBgImg;

    [SerializeField]
    private Button ChoiceBtn;


    [SerializeField]
    private List<Image> SkillBg2ImgList = new List<Image>();

    [SerializeField]
    private TextMeshProUGUI SkillNameText;


    [SerializeField]
    private TextMeshProUGUI BeforeLevelText;

    [SerializeField]
    private TextMeshProUGUI AfterLevelText;

    [SerializeField]
    private TextMeshProUGUI DecreaseValueText;

    [SerializeField]
    private TextMeshProUGUI SkillValueText;

    [SerializeField]
    private GameObject NewObj;

    private InGameUpgrade Upgrade = null;


    private System.Action<InGameUpgrade> Callback = null;

    private UpgradeTier Tier = UpgradeTier.Rare;


    void Awake()
    {
        ChoiceBtn.onClick.AddListener(OnClickChoice);
    }

    public void Set(InGameUpgrade upgrade,UpgradeTier tier  , System.Action<InGameUpgrade> callback = null)
    {
        Upgrade = upgrade;
        Tier = tier;
        Callback = callback;

        Setinfo();
    }



    public void Setinfo()
    {
        if (Upgrade == null) return;

        var td = Tables.Instance.GetTable<InGameUpgradeChoice>().GetData(Upgrade.UpgradeIdx);

        var finddata = GameRoot.Instance.PlayerSkillSystem.FindPlayerSkillValueData(Upgrade.UpgradeIdx);

        ProjectUtility.SetActiveCheck(NewObj, finddata == null);
        ProjectUtility.SetActiveCheck(SkillValueText.transform.parent.gameObject, finddata != null);

        if (td != null)
        {
            SkillNameText.text = Tables.Instance.GetTable<Localize>().GetString(td.choice_name);
            SetSkillValueText(Upgrade.SkillLevelStatType, Upgrade.UpgradeValue1, Upgrade.UpgradeValue2, SkillValueText);



            if (finddata != null)
            {
                BeforeLevelText.text = finddata.Skillevel.ToString();
                AfterLevelText.text = (finddata.Skillevel + 1).ToString();
            }

            foreach(var img in SkillBg2ImgList)
            {
                img.color = Config.Instance.GetImageColor($"Levelup_Grade_{(int)Tier}_2");
            }

            SkillBgImg.color = Config.Instance.GetImageColor($"Levelup_Grade_{(int)Tier}_1");

            SkillIcon.sprite = AtlasManager.Instance.GetSprite(Atlas.Atlas_UI_InGame, td.back_img);
        }
    }



    public void SetSkillValueText(int skilltype, int skillvalue, int skilldecreasevalue, TextMeshProUGUI text)
    {
        ProjectUtility.SetActiveCheck(DecreaseValueText.gameObject, skilltype == (int)UpgradeType.SkillCount);

        switch (skilltype)
        {
            case (int)UpgradeType.SkillUnlock:
                SkillValueText.text = Tables.Instance.GetTable<Localize>().GetString($"skill_type_value_desc{skilltype}");
                break;
            case (int)UpgradeType.SkillCount:
                {
                    SkillValueText.text = Tables.Instance.GetTable<Localize>().GetFormat($"skill_type_value_desc{skilltype}", skillvalue);
                    DecreaseValueText.text = Tables.Instance.GetTable<Localize>().GetFormat("skill_type_value_decrease", skilldecreasevalue);
                    break;
                }
            default:
                SkillValueText.text = Tables.Instance.GetTable<Localize>().GetFormat($"skill_type_value_desc{skilltype}", skillvalue);
                break;
        }
    }


    public void OnClickChoice()
    {
        Callback?.Invoke(Upgrade);
    }



}


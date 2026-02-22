using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;


[UIPath("UI/Popup/PopupLevelUpReward", true)]
public class PopupLevelUpReward : UIBase
{
    [SerializeField]
    private List<LevelUpRewardComponent> LevelUpRewardComponentList = new List<LevelUpRewardComponent>();

    [SerializeField]
    private TextMeshProUGUI TitleNameText;

    [SerializeField]
    private Image TitleImg;

    private bool UpgradeLock = false;

    public void Init(InGameUpgradeCategory category, UpgradeTier tier)
    {
        GameRoot.Instance.GameSpeedSystem.StopGameSpeed(true, false);

        UpgradeLock = false;

        var getUpgradelist = GameRoot.Instance.InGameUpgradeSystem.GetUpgrades(category, tier);

        for (int i = 0; i < getUpgradelist.Count; i++)
        {
            var findskilldata = GameRoot.Instance.PlayerSkillSystem.FindPlayerSkillValueData(getUpgradelist[i].UpgradeIdx);

            LevelUpRewardComponentList[i].Set(getUpgradelist[i], getUpgradelist[i].Tier, OnSelect);
        }

        TitleNameText.text = Tables.Instance.GetTable<Localize>().GetString($"skill_type_name_{category}");
        TitleImg.sprite = AtlasManager.Instance.GetSprite(Atlas.Atlas_UI_Common, $"Common_Frame_LevelupHeaderBg_{(int)tier}");
    }

    private void OnSelect(InGameUpgrade selected)
    {
        if (UpgradeLock) return;
        UpgradeLock = true;

        selected?.CallApply();

        GameRoot.Instance.GameSpeedSystem.StopGameSpeed(false, false);
        Hide();
    }

}


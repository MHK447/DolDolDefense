using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UniRx;

public class StatAreaLeftComponent : MonoBehaviour
{
    [SerializeField]
    private List<ImgeTextShow> ImgeTextShowList = new List<ImgeTextShow>();

    private CompositeDisposable disposables = new CompositeDisposable();

    private readonly Dictionary<int, int> statTypeToSlotIndex = new Dictionary<int, int>
    {
        { (int)SkillLevelStatTypeEnum.StatAttackIncrease, 0 },
        { (int)SkillLevelStatTypeEnum.StatHpIncrease, 1 },
        { (int)SkillLevelStatTypeEnum.StatCoolTimeIncrease, 2 },
        { (int)SkillLevelStatTypeEnum.StatCriticalDamageIncrease, 3 },
    };

    private Dictionary<int, int> slotToStatType = new Dictionary<int, int>();

    void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        disposables.Clear();
        slotToStatType.Clear();

        var statUpgrades = GameRoot.Instance.InGameUpgradeSystem.StatAllUpgrades;

        foreach (var statUpgrade in statUpgrades)
        {
            if (!statTypeToSlotIndex.TryGetValue(statUpgrade.SkillLevelStatType, out int slotIndex))
                continue;

            slotToStatType[slotIndex] = statUpgrade.SkillLevelStatType;
            InitSlot(slotIndex, statUpgrade);
        }

        foreach (var upgrade in GameRoot.Instance.InGameUpgradeSystem.ChoiceInGameUpgrades)
        {
            SubscribeUpgrade(upgrade);
        }

        GameRoot.Instance.InGameUpgradeSystem.ChoiceInGameUpgrades.ObserveAdd().Subscribe(addEvent =>
        {
            SubscribeUpgrade(addEvent.Value);
        }).AddTo(disposables);
    }

    private void SubscribeUpgrade(InGameUpgrade upgrade)
    {
        var td = Tables.Instance.GetTable<InGameUpgradeChoice>().GetData(upgrade.UpgradeIdx);
        if (td.category != 2) return;
        if (!statTypeToSlotIndex.TryGetValue(upgrade.SkillLevelStatType, out int slotIndex)) return;

        UpdateSlotPercent(slotIndex);

        upgrade.LevelProperty.Skip(1).Subscribe(_ =>
        {
            UpdateSlotPercent(slotIndex);
        }).AddTo(disposables);
    }

    private void InitSlot(int index, InGameUpgrade upgrade)
    {
        if (index >= ImgeTextShowList.Count) return;

        var imgtext = ImgeTextShowList[index];
        var td = Tables.Instance.GetTable<InGameUpgradeChoice>().GetData(upgrade.UpgradeIdx);
        var sprite = AtlasManager.Instance.GetSprite(Atlas.Atlas_UI_InGame, $"InGameUpgrade_Icon_{td.idx}");

        imgtext.Set(sprite, GetBuffText(upgrade.SkillLevelStatType));
        ProjectUtility.SetActiveCheck(imgtext.gameObject, true);
        ProjectUtility.SetActiveCheck(imgtext.OnRoot, true);
        ProjectUtility.SetActiveCheck(imgtext.OffRoot, false);
    }

    private void UpdateSlotPercent(int index)
    {
        if (index >= ImgeTextShowList.Count) return;
        if (!slotToStatType.TryGetValue(index, out int statType)) return;

        ImgeTextShowList[index].Text.text = GetBuffText(statType);
    }

    private string GetBuffText(int statType)
    {
        var info = GameRoot.Instance.UserData.InGamePlayerData.PlayerUnitInfoData;

        switch (statType)
        {
            case (int)SkillLevelStatTypeEnum.StatAttackIncrease:
                return $"{info.AttackDamageBuffValue}%";
            case (int)SkillLevelStatTypeEnum.StatHpIncrease:
            {
                var upgrade = GameRoot.Instance.InGameUpgradeSystem.ChoiceInGameUpgrades
                    .ToList().Find(x => x.SkillLevelStatType == statType);
                int val = upgrade != null ? upgrade.UpgradeValue1 * upgrade.LevelProperty.Value : 0;
                return $"{val}%";
            }
            case (int)SkillLevelStatTypeEnum.StatCoolTimeIncrease:
                return $"{(int)info.AttackCooltimeBuffValue}%";
            case (int)SkillLevelStatTypeEnum.StatCriticalDamageIncrease:
                return $"{info.CriticalDamageBuffValue}%";
            default:
                return "0%";
        }
    }

    void OnDestroy()
    {
        disposables.Clear();
    }

    void OnDisable()
    {
        disposables.Clear();
    }
}

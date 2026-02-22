using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UniRx;


public class SkillAreaRightComponent : MonoBehaviour
{
    [SerializeField]
    private List<ImgeTextShow> ImgeTextShowList = new List<ImgeTextShow>();

    private CompositeDisposable disposables = new CompositeDisposable();


    void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        disposables.Clear();

        foreach (var imgtext in ImgeTextShowList)
        {
            ProjectUtility.SetActiveCheck(imgtext.OnRoot, false);
            ProjectUtility.SetActiveCheck(imgtext.OffRoot, true);
        }

        var skillList = GameRoot.Instance.UserData.InGamePlayerData.PlayerSkillList;

        int visibleIndex = 0;
        for (int i = 0; i < skillList.Count; i++)
        {
            var skill = skillList[i];
            var td = Tables.Instance.GetTable<InGameUpgradeChoice>().GetData(skill.SkillIdx);

            if (td.category == 2)
            {
                continue;
            }

            int slotIndex = visibleIndex;
            SetupSlot(slotIndex, skill);

            skill.SkillLevel.Skip(1).Subscribe(_ =>
            {
                UpdateSlotLevel(slotIndex, skill);
            }).AddTo(disposables);

            visibleIndex++;
        }

        skillList.ObserveAdd().Subscribe(addEvent =>
        {
            var td = Tables.Instance.GetTable<InGameUpgradeChoice>().GetData(addEvent.Value.SkillIdx);

            if(td.category == 2)
            {
                return;
            }

            int index = GetVisibleSkillIndex(addEvent.Value);
            var upgrade = addEvent.Value;

            SetupSlot(index, upgrade);

            upgrade.SkillLevel.Skip(1).Subscribe(_ =>
            {
                UpdateSlotLevel(index, upgrade);
            }).AddTo(disposables);

        }).AddTo(disposables);
    }

    private int GetVisibleSkillIndex(PlayerSkillBase target)
    {
        var skillList = GameRoot.Instance.UserData.InGamePlayerData.PlayerSkillList;
        int visibleIndex = 0;
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i] == target)
                return visibleIndex;

            var td = Tables.Instance.GetTable<InGameUpgradeChoice>().GetData(skillList[i].SkillIdx);
            if (td.category != 2)
            {
                visibleIndex++;
            }
        }
        return visibleIndex;
    }

    private void SetupSlot(int index, PlayerSkillBase upgrade)
    {
        if (index >= ImgeTextShowList.Count) return;

        var imgtext = ImgeTextShowList[index];
        var td = Tables.Instance.GetTable<PlayerSkillInfo>().GetData(upgrade.SkillIdx);
        var sprite = AtlasManager.Instance.GetSprite(Atlas.Atlas_UI_InGame, $"InGameUpgrade_Icon_{td.skill_idx}");

        imgtext.Set(sprite, upgrade.SkillLevel.Value.ToString());
        ProjectUtility.SetActiveCheck(imgtext.gameObject, true);
        ProjectUtility.SetActiveCheck(imgtext.OnRoot, true);
        ProjectUtility.SetActiveCheck(imgtext.OffRoot, false);
    }

    private void UpdateSlotLevel(int index, PlayerSkillBase upgrade)
    {
        if (index >= ImgeTextShowList.Count) return;

        ImgeTextShowList[index].Text.text = upgrade.SkillLevel.Value.ToString();
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


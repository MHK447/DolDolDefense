using System.Collections.Generic;
using System.Linq;
using BanpoFri;
using UnityEngine;

public enum InGameUpgradeCategory
{
    AddSKill = 1,
    AddStat = 2,
    AddCoin = 3,
}

public enum UpgradeTier
{
    Rare = 1,
    Epic = 2,
    Legendary = 3,
}

public class InGameUpgradeSystem
{
    public List<InGameUpgrade> ChoiceInGameUpgrades = new();

    public int UpgradeCount = 0;

    public List<InGameUpgrade> SkillAllUpgrades = new();
    public List<InGameUpgrade> StatAllUpgrades = new();

    public void Create()
    {
        var tdlist = Tables.Instance.GetTable<InGameUpgradeChoice>().DataList.FindAll(x=> x.category == 2).ToList();

        foreach (var td in tdlist)
        {
            StatAllUpgrades.Add(new InGameUpgrade(
                td.idx,
                UpgradeTier.Rare,
                1,
                td
            ));
        }

        //temp
        if (GameRoot.Instance.UserData.Skillcarddatas.Count == 0)
        {
            GameRoot.Instance.UserData.Skillcarddatas.Add(new SkillCardData()
            {
                Skillidx = 1,
                Skillevel = 1,
            });
            GameRoot.Instance.UserData.Skillcarddatas.Add(new SkillCardData()
            {
                Skillidx = 2,
                Skillevel = 1,
            });
            GameRoot.Instance.UserData.Skillcarddatas.Add(new SkillCardData()
            {
                Skillidx = 3,
                Skillevel = 1,
            });
            GameRoot.Instance.UserData.Skillcarddatas.Add(new SkillCardData()
            {
                Skillidx = 4,
                Skillevel = 1,
            });
            GameRoot.Instance.UserData.Skillcarddatas.Add(new SkillCardData()
            {
                Skillidx = 5,
                Skillevel = 1,
            });
        }
    }


    public void GameStartCheck()
    {
        SkillAllUpgrades.Clear();



        var tdlist = Tables.Instance.GetTable<InGameUpgradeChoice>().DataList;


        var skilldatas = GameRoot.Instance.UserData.Skillcarddatas;
        var tdByIdx = tdlist.ToDictionary(data => data.idx, data => data);

        foreach (var skilldata in skilldatas)
        {
            if (!tdByIdx.TryGetValue(skilldata.Skillidx, out var choiceData))
            {
                continue;
            }

            SkillAllUpgrades.Add(new InGameUpgrade(
                skilldata.Skillidx,
                UpgradeTier.Rare,
                skilldata.Skillevel,
                choiceData
            ));
        }
    }


    public List<InGameUpgrade> GetUpgrades(InGameUpgradeCategory category, UpgradeTier minimumTier = UpgradeTier.Rare)
    {
        //티어설정
        UpgradeTier tierToApply;

        if (category == InGameUpgradeCategory.AddSKill)
        {
            tierToApply = SelectTierByWeight(minimumTier);

            var selectData = GetSelectInfoData();
            if (selectData == null)
            {
                return SkillNaturalChoices(SkillAllUpgrades, tierToApply);
            }
            else
            {
                return SkillNaturalChoices(SkillAllUpgrades, tierToApply);
            }
        }
        else
        {
            tierToApply = SelectTierByWeight(minimumTier);

            return StatNaturalChoices(StatAllUpgrades, tierToApply);
        }
    }

    public List<InGameUpgrade> StatNaturalChoices(List<InGameUpgrade> upgrades, UpgradeTier tier)
    {
        List<InGameUpgrade> choicelist = new();

        // 중복 없이 최대 3개 랜덤 선택
        int countToSelect = Mathf.Min(3, upgrades.Count);
        var shuffled = upgrades.OrderBy(x => Random.Range(0, int.MaxValue)).Take(countToSelect).ToList();

        foreach (var upgrade in shuffled)
        {
            upgrade.Tier = tier;
            upgrade.RandSelectType();
            choicelist.Add(upgrade);
        }


        return choicelist;
    }



    public List<InGameUpgrade> SkillNaturalChoices(List<InGameUpgrade> upgrades, UpgradeTier tier)
    {
        List<InGameUpgrade> choicelist = new();

        // 중복 없이 최대 3개 랜덤 선택
        int countToSelect = Mathf.Min(3, upgrades.Count);
        var shuffled = upgrades.OrderBy(x => Random.Range(0, int.MaxValue)).Take(countToSelect).ToList();

        foreach (var upgrade in shuffled)
        {
            upgrade.RandSelectType();
            choicelist.Add(upgrade);
        }


        return choicelist;
    }


    public void Reset()
    {
        ChoiceInGameUpgrades.Clear();
        UpgradeCount = 0;
    }


    public void RandUpgradeChoice()
    {
        var skillcarddatas = GameRoot.Instance.UserData.Skillcarddatas;

        if (skillcarddatas.Count == 0) return;

    }

    public WayChoicesSelectInfoData GetSelectInfoData()
    {

        WayChoicesSelectInfoData data = Tables.Instance.GetTable<WayChoicesSelectInfo>().DataList.FirstOrDefault(x =>
        {
            return x.stage == GameRoot.Instance.UserData.Stageidx.Value
            && x.challenge_count == GameRoot.Instance.UserData.GetRecordCount(Config.RecordCountKeys.StageFailedCount, GameRoot.Instance.UserData.Stageidx.Value)
            && x.choice_count == UpgradeCount;
        });

        return data;
    }



    private UpgradeTier SelectTierByWeight(UpgradeTier minimumTier = UpgradeTier.Rare)
    {
        int stageIndex = GameRoot.Instance.UserData.Stageidx.Value;
        int tryCount = GameRoot.Instance.UserData.GetRecordCount(Config.RecordCountKeys.StageFailedCount, stageIndex);
        //티어별 웨이트 테이블값 사용

        List<int> tierWeights = new(){
                    Tables.Instance.GetTable<ChoicesGroup>().GetData((int)UpgradeTier.Rare).value,
                    Tables.Instance.GetTable<ChoicesGroup>().GetData((int)UpgradeTier.Epic).value,
                    Tables.Instance.GetTable<ChoicesGroup>().GetData((int)UpgradeTier.Legendary).value,
                };
        return (UpgradeTier)Mathf.Max((int)minimumTier, ProjectUtility.GetWeightedRandomGrade(tierWeights));

        //else return (UpgradeTier)Mathf.Max((int)minimumTier, wayChoiceData.choices[UpgradeCount]);
    }




}



using System.Collections.Generic;
using System.Linq;
using BanpoFri;
using UnityEngine;

public enum InGameUpgradeCategory
{
    AddSKill = 1,
    AddStat = 2,
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

    public List<InGameUpgrade> AllUpgrades = new();


    public void Create()
    {

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
        AllUpgrades.Clear();



        var tdlist = Tables.Instance.GetTable<InGameUpgradeChoice>().DataList;


        var skilldatas = GameRoot.Instance.UserData.Skillcarddatas;
        var tdByIdx = tdlist.ToDictionary(data => data.idx, data => data);

        foreach (var skilldata in skilldatas)
        {
            if (!tdByIdx.TryGetValue(skilldata.Skillidx, out var choiceData))
            {
                continue;
            }

            AllUpgrades.Add(new InGameUpgrade(
                skilldata.Skillidx,
                UpgradeTier.Rare,
                skilldata.Skillevel,
                choiceData
            ));
        }
    }


    public List<InGameUpgrade> GetUpgrades(UpgradeTier minimumTier = UpgradeTier.Rare)
    {
        //티어설정
        UpgradeTier tierToApply;


        tierToApply = SelectTierByWeight(minimumTier);

        var selectData = GetSelectInfoData();
        if (selectData == null)
        {
            return NaturalChoices(AllUpgrades, tierToApply);
        }
        else
        {
            return NaturalChoices(AllUpgrades, tierToApply);
        }
    }


    public List<InGameUpgrade> NaturalChoices(List<InGameUpgrade> upgrades, UpgradeTier tier)
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



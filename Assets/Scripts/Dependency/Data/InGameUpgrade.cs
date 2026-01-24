using UnityEngine;
using BanpoFri;
using System.Linq;



public enum SkillLevelStatTypeEnum
{
    SKillAdd = 1,
    SKillCountAdd = 2,
    SkillUpScale = 3,
    SKillAttackUp = 4,
    SkillDurationIncrease = 5,
    StatAttackIncrease = 101,
    StatHpIncrease = 102,
    StatCoolTimeIncrease = 103,
    StatCriticalDamageIncrease = 104,
}


public class InGameUpgrade
{
    public int UpgradeIdx = 0;
    public UpgradeTier Tier = UpgradeTier.Rare;
    public int Level = 0;
    public InGameUpgradeChoiceData UpgradeChoiceData = null;

    public bool IsRecommend = false;

    public int SkillLevelStatType = 0;

    public int UpgradeValue1 = 0;

    public int UpgradeValue2 = 0;



    public InGameUpgrade(int upgradeidx, UpgradeTier tier, int level, InGameUpgradeChoiceData choiceData, bool isrecommend = false)
    {
        UpgradeIdx = upgradeidx;
        Tier = tier;
        Level = level;
        UpgradeChoiceData = choiceData;
        IsRecommend = isrecommend;

        var findskilldata = GameRoot.Instance.InGameUpgradeSystem.ChoiceInGameUpgrades.Find(x => x.UpgradeIdx == upgradeidx);


        if (findskilldata == null)
        {
            SkillLevelStatType = (int)SkillLevelStatTypeEnum.SKillAdd;

            UpgradeValue1 = 0;
            UpgradeValue2 = 0;
        }
        else
        {
            if(choiceData.category == 1)
            {
                RandSelectType();
            }
            else
            {
                SkillLevelStatType = (int)choiceData.skill_level_stat_type.First();
                UpgradeValue1 = choiceData.upgrade_value_1.First();
                UpgradeValue2 = choiceData.upgrade_value_2.First();
            }
        }
    }

    public void RandSelectType()
    {
        var findskilldata = GameRoot.Instance.InGameUpgradeSystem.ChoiceInGameUpgrades.Find(x => x.UpgradeIdx == UpgradeIdx);


        if (findskilldata == null)
        {
            SkillLevelStatType = (int)SkillLevelStatTypeEnum.SKillAdd;

            UpgradeValue1 = 0;
            UpgradeValue2 = 0;
        }
        else
        {
            var randvalue = Random.Range(0, UpgradeChoiceData.skill_level_stat_type.Count);

            SkillLevelStatType = UpgradeChoiceData.skill_level_stat_type[randvalue];

            int findindex = UpgradeChoiceData.skill_level_stat_type.FindIndex(x => x == randvalue);

            UpgradeValue1 = UpgradeChoiceData.upgrade_value_1[findindex] * (int)Tier;

            UpgradeValue2 = Tier > UpgradeTier.Epic ? 0 : UpgradeChoiceData.upgrade_value_2[findindex];
        }
    }


    public virtual void CallApply()
    {
        GameRoot.Instance.InGameUpgradeSystem.UpgradeCount++;

        switch (SkillLevelStatType)
        {
            case (int)SkillLevelStatTypeEnum.SKillAdd:
                {
                    GameRoot.Instance.UserData.InGamePlayerData.AddPlayerSkill(new PlayerSkill_BlackBall());
                }
                break;
            case (int)SkillLevelStatTypeEnum.SKillCountAdd:
                {
                }
                break;
            case (int)SkillLevelStatTypeEnum.SkillUpScale:
                {

                }
                break;
            case (int)SkillLevelStatTypeEnum.SKillAttackUp:
                {

                }
                break;
            case (int)SkillLevelStatTypeEnum.SkillDurationIncrease:
                {

                }
                break;
            case (int)SkillLevelStatTypeEnum.StatAttackIncrease:
                {

                }
                break;
            case (int)SkillLevelStatTypeEnum.StatHpIncrease:
                {

                }
                break;
            case (int)SkillLevelStatTypeEnum.StatCoolTimeIncrease:
                {

                }
                break;
            case (int)SkillLevelStatTypeEnum.StatCriticalDamageIncrease:
                {

                }
                break;
        }
    }

}

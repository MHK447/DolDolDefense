using UnityEngine;
using BanpoFri;
using System.Linq;
using UniRx;


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
    public IReactiveProperty<int> LevelProperty = new ReactiveProperty<int>(0);
    public InGameUpgradeChoiceData UpgradeChoiceData = null;

    public bool IsRecommend = false;

    public int SkillLevelStatType = 0;

    public int UpgradeValue1 = 0;

    public int UpgradeValue2 = 0;



    public InGameUpgrade(int upgradeidx, UpgradeTier tier, int level, InGameUpgradeChoiceData choiceData, bool isrecommend = false)
    {
        UpgradeIdx = upgradeidx;
        Tier = tier;
        LevelProperty.Value = level;
        UpgradeChoiceData = choiceData;
        IsRecommend = isrecommend;

        var findskilldata = GameRoot.Instance.InGameUpgradeSystem.ChoiceInGameUpgrades.ToList().Find(x => x.UpgradeIdx == upgradeidx);


        if (findskilldata == null)
        {
            var td = Tables.Instance.GetTable<InGameUpgradeChoice>().GetData(upgradeidx);

            SkillLevelStatType = td.skill_level_stat_type.First();

            UpgradeValue1 = td.upgrade_value_1.First();
            UpgradeValue2 = td.upgrade_value_2.First();
        }
        else
        {
            if (choiceData.category == 1)
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
        var findskilldata = GameRoot.Instance.InGameUpgradeSystem.ChoiceInGameUpgrades.ToList().Find(x => x.UpgradeIdx == UpgradeIdx);

        var td = Tables.Instance.GetTable<InGameUpgradeChoice>().GetData(UpgradeIdx);

        if (td == null)
        {
            BpLog.LogError(" InGameUpgrade RandSelectType Error : UpgradeIdx = " + UpgradeIdx);
            return;
        }


        if (findskilldata == null)
        {
            SkillLevelStatType = td.skill_level_stat_type.First();

            UpgradeValue1 = td.upgrade_value_1.First();
            UpgradeValue2 = td.upgrade_value_2.First();
        }
        else
        {
            if (td.category == 1)
            {
                var randvalue = Random.Range(0, td.skill_level_stat_type.Count);

                SkillLevelStatType = td.skill_level_stat_type[randvalue];

                UpgradeValue1 = td.upgrade_value_1[randvalue] * (int)Tier;

                UpgradeValue2 = Tier >= UpgradeTier.Epic ? 0 : td.upgrade_value_2[randvalue];
            }
            else
            {

                SkillLevelStatType = td.skill_level_stat_type[0];

                UpgradeValue1 = td.upgrade_value_1[0] * (int)Tier;


            }
        }
    }


    public virtual void CallApply()
    {
        GameRoot.Instance.InGameUpgradeSystem.UpgradeCount++;


        var findskilldata = GameRoot.Instance.UserData.InGamePlayerData.FindSkill(UpgradeIdx);

        if (findskilldata != null)
        {
            findskilldata.SkillLevel.Value += 1;
        }


        switch (SkillLevelStatType)
        {
            case (int)SkillLevelStatTypeEnum.SKillAdd:
                {
                    AddSkill(UpgradeIdx);
                }
                break;
            case (int)SkillLevelStatTypeEnum.SKillCountAdd:
                {
                    if (findskilldata != null)
                    {
                        findskilldata.SkillCount += 1;
                        findskilldata.AttackDamage -= (int)ProjectUtility.PercentCalc(findskilldata.AttackDamage, UpgradeValue2);
                    }

                }
                break;
            case (int)SkillLevelStatTypeEnum.SkillUpScale:
                {
                    if (findskilldata != null)
                    {
                        findskilldata.SkillSizeProperty.Value += ProjectUtility.PercentCalc(findskilldata.SkillSizeProperty.Value, UpgradeValue1);
                    }
                }
                break;
            case (int)SkillLevelStatTypeEnum.SKillAttackUp:
                {
                    findskilldata.AttackDamage += (int)ProjectUtility.PercentCalc(findskilldata.AttackDamage, UpgradeValue2);
                }
                break;
            case (int)SkillLevelStatTypeEnum.SkillDurationIncrease:
                {
                    findskilldata.SkillCoolTime -= ProjectUtility.PercentCalc(findskilldata.SkillCoolTime, UpgradeValue1);
                }
                break;
            case (int)SkillLevelStatTypeEnum.StatAttackIncrease:
                {
                    GameRoot.Instance.UserData.InGamePlayerData.PlayerUnitInfoData.AttackDamageBuffValue += UpgradeValue1;
                    TrackStatUpgrade();
                }
                break;
            case (int)SkillLevelStatTypeEnum.StatHpIncrease:
                {
                    GameRoot.Instance.UserData.InGamePlayerData.PlayerUnitInfoData.CurHpProperty.Value += (int)ProjectUtility.PercentCalc(GameRoot.Instance.UserData.InGamePlayerData.PlayerUnitInfoData.CurHpProperty.Value, UpgradeValue1);
                    GameRoot.Instance.UserData.InGamePlayerData.PlayerUnitInfoData.StartHpProperty.Value += (int)ProjectUtility.PercentCalc(GameRoot.Instance.UserData.InGamePlayerData.PlayerUnitInfoData.StartHpProperty.Value, UpgradeValue1);
                    TrackStatUpgrade();
                }
                break;
            case (int)SkillLevelStatTypeEnum.StatCoolTimeIncrease:
                {
                    GameRoot.Instance.UserData.InGamePlayerData.PlayerUnitInfoData.AttackCooltimeBuffValue += UpgradeValue1;
                    TrackStatUpgrade();
                }
                break;
            case (int)SkillLevelStatTypeEnum.StatCriticalDamageIncrease:
                {
                    GameRoot.Instance.UserData.InGamePlayerData.PlayerUnitInfoData.CriticalDamageBuffValue += UpgradeValue1;
                    TrackStatUpgrade();
                }
                break;
        }
    }


    private void TrackStatUpgrade()
    {
        var existing = GameRoot.Instance.InGameUpgradeSystem.ChoiceInGameUpgrades.ToList().Find(x => x.UpgradeIdx == UpgradeIdx);
        if (existing == null)
        {
            GameRoot.Instance.InGameUpgradeSystem.AddInGameupgrade(this);
        }
        else
        {
            existing.LevelProperty.Value += 1;
        }
    }

    public void AddSkill(int skillidx)
    {
        GameRoot.Instance.InGameUpgradeSystem.AddInGameupgrade(this);

        switch (skillidx)
        {
            case (int)PlayerSkillSystem.PlayerSkillType.BlackBall:
                GameRoot.Instance.UserData.InGamePlayerData.AddPlayerSkill(new PlayerSkill_BlackBall());
                break;
            case (int)PlayerSkillSystem.PlayerSkillType.Lightning:
                GameRoot.Instance.UserData.InGamePlayerData.AddPlayerSkill(new PlayerSkill_Lightning());
                break;
            case (int)PlayerSkillSystem.PlayerSkillType.IceOrb:
                GameRoot.Instance.UserData.InGamePlayerData.AddPlayerSkill(new PlayerSkill_Poison());
                break;
            case (int)PlayerSkillSystem.PlayerSkillType.DarkGear:
                GameRoot.Instance.UserData.InGamePlayerData.AddPlayerSkill(new PlayerSkill_DarkGear());
                break;
            case (int)PlayerSkillSystem.PlayerSkillType.PoisonBullet:
                GameRoot.Instance.UserData.InGamePlayerData.AddPlayerSkill(new PlayerSkill_Poison());
                break;
            case (int)PlayerSkillSystem.PlayerSkillType.Fireball:
                GameRoot.Instance.UserData.InGamePlayerData.AddPlayerSkill(new PlayerSkill_FireBall());
                break;
            case (int)PlayerSkillSystem.PlayerSkillType.Amageddon:
                GameRoot.Instance.UserData.InGamePlayerData.AddPlayerSkill(new PlayerSkill_Amagetdon());
                break;
            case (int)PlayerSkillSystem.PlayerSkillType.LaserCannon:
                GameRoot.Instance.UserData.InGamePlayerData.AddPlayerSkill(new PlayerSkill_Laser());
                break;
            case (int)PlayerSkillSystem.PlayerSkillType.ShadowKnife:
                GameRoot.Instance.UserData.InGamePlayerData.AddPlayerSkill(new PlayerSkill_ShadowKnife());
                break;
            case (int)PlayerSkillSystem.PlayerSkillType.LifeSteal:
                GameRoot.Instance.UserData.InGamePlayerData.AddPlayerSkill(new PlayerSkill_Lifesteal());
                break;
            case (int)PlayerSkillSystem.PlayerSkillType.BombKnockBack:
                GameRoot.Instance.UserData.InGamePlayerData.AddPlayerSkill(new PlayerSkill_BombKonckBack());
                break;
            case (int)PlayerSkillSystem.PlayerSkillType.LogThrow:
                GameRoot.Instance.UserData.InGamePlayerData.AddPlayerSkill(new PlayerSkill_LogThrow());
                break;
            case (int)PlayerSkillSystem.PlayerSkillType.BlueSoul:
                GameRoot.Instance.UserData.InGamePlayerData.AddPlayerSkill(new PlayerSkill_BlueSoul());
                break;
        }

    }

}

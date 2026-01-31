using System;
using Google.FlatBuffers;
using UniRx;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public partial class UserDataSystem
{

}

public class PlayerSkillData
{
    public int SkillIdx = 0;
    public int SkillLevel = 0;


    public float SKillCoolTime = 0f;
    public float SkilldeltaTime = 0f;

    public float AttackDamage = 0f;


    public PlayerSkillData(int skillidx , int skillevel , float skillcooltime , float attackdamage)
    {
        SkillIdx = skillidx;
        SkillLevel = skillevel;
        SKillCoolTime = skillcooltime;
        AttackDamage = attackdamage;
        SkilldeltaTime = 0f;
    }
}



public class InGamePlayerData
{
    public string Playername { get; set; } = "Player";

    public int Playerlevel = 1;
    public int Playerexp = 0;


    public IReactiveProperty<int> InGameExpProperty { get; private set; } = new ReactiveProperty<int>(0);
    public IReactiveProperty<int> InGameUpgradeCountProperty { get; private set; } = new ReactiveProperty<int>(1);

    public IReactiveProperty<int> KillCountProperty = new ReactiveProperty<int>(0);

    public IReactiveProperty<bool> IsGameStartProperty = new ReactiveProperty<bool>(false);

    public IReactiveProperty<int> WaveTimePorperty = new ReactiveProperty<int>(0);

    public IReactiveProperty<int> InGameMoneyProperty { get; private set; } = new ReactiveProperty<int>(0);
    public int InGameReRollCount = 0;

    public PlayerUnitInfoData PlayerUnitInfoData { get; private set; } = new PlayerUnitInfoData();

    public ReactiveCollection<PlayerSkillBase> PlayerSkillList = new ReactiveCollection<PlayerSkillBase>();


    public void SetPlayerLevel(int level)
    {
        level = Mathf.Max(level, Playerlevel);
        Playerlevel = level;
    }

    public void StageClear()
    {
        PlayerSkillList.Clear();
        InGameExpProperty.Value = 0;
        InGameUpgradeCountProperty.Value = 1;
        InGameMoneyProperty.Value = 0;
        KillCountProperty.Value = 0;
        IsGameStartProperty.Value = false;
        WaveTimePorperty.Value = 0;
    }


    public void SetPlayerHp(int hp)
    {
        PlayerUnitInfoData.StartHpProperty.Value = hp;
    }


    public void AddPlayerSkill(PlayerSkillBase skill)
    {
        var finddata = PlayerSkillList.ToList().Find(x => x.SkillIdx == skill.SkillIdx);
        if (finddata != null)
        {
            finddata.SkillLevel += 1;
        }
        else
        {
            PlayerSkillList.Add(skill);
        }
    }

    public void SkillUpdate()
    {
        for(int i = PlayerSkillList.Count - 1; i >= 0; i--)
        {
            PlayerSkillList[i].Update();
        }
    }
}

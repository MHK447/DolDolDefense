using System;
using Google.FlatBuffers;
using UniRx;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

    public IReactiveProperty<int> StartHpProperty { get; private set; } = new ReactiveProperty<int>(0);
    public IReactiveProperty<int> CurHpProperty { get; private set; } = new ReactiveProperty<int>(0);

    public IReactiveProperty<int> InGameExpProperty { get; private set; } = new ReactiveProperty<int>(0);
    public IReactiveProperty<int> InGameUpgradeCountProperty { get; private set; } = new ReactiveProperty<int>(1);

    public IReactiveProperty<int> KillCountProperty = new ReactiveProperty<int>(0);

    public IReactiveProperty<bool> IsGameStartProperty = new ReactiveProperty<bool>(false);

    public IReactiveProperty<int> WaveTimePorperty = new ReactiveProperty<int>(0);

    public IReactiveProperty<int> InGameMoneyProperty { get; private set; } = new ReactiveProperty<int>(0);
    public int InGameReRollCount = 0;

    public ReactiveCollection<PlayerSkillData> PlayerSkillDataList = new ReactiveCollection<PlayerSkillData>();


    public void SetPlayerLevel(int level)
    {
        level = Mathf.Max(level, Playerlevel);
        Playerlevel = level;
    }

    public void StageClear()
    {
        PlayerSkillDataList.Clear();
        InGameExpProperty.Value = 0;
        InGameUpgradeCountProperty.Value = 1;
        InGameMoneyProperty.Value = 0;
        KillCountProperty.Value = 0;
        IsGameStartProperty.Value = false;
        WaveTimePorperty.Value = 0;
    }


    public void SetPlayerHp(int hp)
    {
        StartHpProperty.Value = hp;
    }


    public void AddPlayerSkill(int skillidx , int skillevel, float skillcooltime , float attackdamage)
    {
        var finddata = PlayerSkillDataList.ToList().Find(x => x.SkillIdx == skillidx);
        if (finddata != null)
        {
            finddata.SkillLevel += skillevel;
        }
        else
        {
            PlayerSkillDataList.Add(new PlayerSkillData(skillidx , skillevel , skillcooltime , attackdamage));
        }
    }
}

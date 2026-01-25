using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PlayerSkill_Laser : PlayerSkillBase
{
    public override void OnInitalize()
    {
        SkillIdx = (int)PlayerSkillSystem.PlayerSkillType.LaserCannon;
        base.OnInitalize();
    }


    public override void Update()
    {
        if (InGameStage == null) return;

        if (InGameStage.PlayerUnit == null) return;

        if (!GameRoot.Instance.UserData.InGamePlayerData.IsGameStartProperty.Value) return;

        if (SkillCoolTime <= 0f) return;


        Skilldeltatime += Time.deltaTime;

        if (Skilldeltatime >= SkillCoolTime)
        {
            OnSkillUse();
        }
    }

    override public void OnSkillUse()
    {
        base.OnSkillUse();


        if (InGameStage.PlayerUnit.BulletCreate(SkillIdx))
        {
            Skilldeltatime = 0f;
        }
    }


}


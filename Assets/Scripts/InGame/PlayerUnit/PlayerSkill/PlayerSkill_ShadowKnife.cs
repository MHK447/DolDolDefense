using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PlayerSkill_ShadowKnife : PlayerSkillBase
{
    public override void OnInitalize()
    {
        SkillIdx = (int)PlayerSkillSystem.PlayerSkillType.ShadowKnife;
        base.OnInitalize();
    }

    public override void Update()
    {
        base.Update();
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


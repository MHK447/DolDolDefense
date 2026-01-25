using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PlayerSkill_Amagetdon : PlayerSkillBase
{
    public override void OnInitalize()
    {
        SkillIdx = (int)PlayerSkillSystem.PlayerSkillType.Amageddon;
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

        var findenemy = InGameStage.EnemyUnitGroup.FindEnemyTarget(InGameStage.PlayerUnit.transform.position, AttackRange);

        if (findenemy != null)
        {
            GameRoot.Instance.EffectSystem.MultiPlay<AmagetdonEffect>(findenemy.transform.position, (x) =>
            {
                x.Init(findenemy.transform, null);
                x.SetAutoRemove(true, 5f);
            });
        }
    }
}


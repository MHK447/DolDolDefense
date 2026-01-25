using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PlayerSkill_DarkGear : PlayerSkillBase
{


    public override void OnInitalize()
    {
        SkillIdx = (int)PlayerSkillSystem.PlayerSkillType.DarkGear;
        base.OnInitalize();

        AddDarkGear();
    }

    

    public void AddDarkGear()
    {
        InGameStage.PlayerUnit.BulletCreate((int)PlayerSkillSystem.PlayerSkillType.DarkGear , false);
    }
}


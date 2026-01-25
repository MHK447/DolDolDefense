using UnityEngine;

public class PlayerSkill_LogThrow : PlayerSkillBase
{
    public override void OnInitalize()
    {
        SkillIdx = (int)PlayerSkillSystem.PlayerSkillType.LogThrow;
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

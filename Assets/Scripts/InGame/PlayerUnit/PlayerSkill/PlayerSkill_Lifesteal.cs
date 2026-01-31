using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerSkill_Lifesteal : PlayerSkillBase
{
    public override void OnInitalize()
    {
        SkillIdx = (int)PlayerSkillSystem.PlayerSkillType.LifeSteal;
        base.OnInitalize();


        GameRoot.Instance.EffectSystem.MultiPlay<LifeStealEffect>(InGameStage.PlayerUnit.transform.position, (x) =>
        {
            x.Init(GameRoot.Instance.UserData.InGamePlayerData.PlayerUnitInfoData.AttackDamage);
        });
    }

    
}

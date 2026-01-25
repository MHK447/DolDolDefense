using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PlayerSkill_Lightning : PlayerSkillBase
{

    public override void OnInitalize()
    {
        SkillIdx = (int)PlayerSkillSystem.PlayerSkillType.Lightning;
        base.OnInitalize();
    }
    public override void OnSkillUse()
    {
        if (LigihtningAttack())
        {
            Skilldeltatime = 0f;
        }
    }


    public bool LigihtningAttack()
    {
        List<EnemyUnit> targetedEnemies = new List<EnemyUnit>();

        for (int i = 0; i < SkillCount; i++)
        {
            // range 내의 적들 중 아직 타겟팅되지 않은 적을 찾기
            var findenemy = InGameStage.EnemyUnitGroup.ActiveUnits
                .Where(x => !x.IsDead) // 살아있는 적만
                .Where(x => Vector3.Distance(x.transform.position, InGameStage.PlayerUnit.transform.position) <= AttackRange) // range 내에 있는 적만
                .Where(x => !targetedEnemies.Contains(x)) // 이미 타겟팅된 적 제외
                .OrderBy(x => Vector3.Distance(x.transform.position, InGameStage.PlayerUnit.transform.position)) // 가까운 순으로 정렬
                .FirstOrDefault(); // 가장 가까운 적 선택

            // 새로운 적을 찾지 못하면 이미 타겟팅된 적 중 하나를 선택
            if (findenemy == null)
            {
                findenemy = InGameStage.EnemyUnitGroup.FindEnemyTarget(InGameStage.PlayerUnit.transform.position, AttackRange);
            }

            // 여전히 적을 찾지 못하면 스킬 사용 중단
            if (findenemy == null)
            {
                return false;
            }

            // 타겟팅된 적을 리스트에 추가
            targetedEnemies.Add(findenemy);

            GameRoot.Instance.EffectSystem.Play<LightningEffect>(new Vector3(findenemy.transform.position.x, findenemy.transform.position.y - 0.5f, findenemy.transform.position.z), x =>
                  {
                      SoundPlayer.Instance.PlaySound("effect_bomb");
                      findenemy.Damage(10);
                      x.SetAutoRemove(true, 3f);
                  });

        }

        return true;
    }

}


using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Poison_Aliment : AlimentData
{
    private List<Color> originalColors = new List<Color>();
    private bool isApplied = false;

    public override void Set(AlimentType type, float time, EnemyUnit target, double damage, float damagedelay, int maxStackCount = 1)
    {
        base.Set(type, time, target, damage, damagedelay, maxStackCount);

        deltatime = 0f;

        // 독 효과 적용
        ApplyPoisonEffect();
    }

    private void ApplyPoisonEffect()
    {
        if (Target == null || isApplied) return;

        isApplied = true;
        originalColors.Clear();

        // 원래 색상 저장하고 초록색으로 변경
        var spriteList = Target.GetComponent<EnemyUnit>()?.GetUnitImg;
        if (spriteList != null)
        {
            // 원래 색상 저장
            originalColors.Add(spriteList.color);

            // 초록색으로 변경 (밝은 초록색)
            spriteList.color = new Color(0.5f, 1f, 0.5f, spriteList.color.a);
        }
    }

    public override void Update()
    {
        if (Time <= 0f)
        {
            // 독 효과 해제
            RemovePoisonEffect();
            return;
        }

        // 시간 감소
        deltatime += UnityEngine.Time.deltaTime;
        if (deltatime >= DamageDelay)
        {
            Time -= DamageDelay;
            deltatime = 0f;
            OnDamage();
        }

        if (Target != null && Target.IsDead)
        {
            var findenemy = GameRoot.Instance.InGameSystem.GetInGame<InGameBase>().Stage.EnemyUnitGroup.FindEnemyTarget(Target.transform.position, int.MaxValue);

            if (findenemy != null)
            {
                GameRoot.Instance.EffectSystem.MultiPlay<InfectionEffect>(Target.transform.position, (x) =>
                {
                    x.SetAutoRemove(true, 1.5f);
                });
            }
        }
    }

    private void RemovePoisonEffect()
    {
        if (Target == null || !isApplied) return;

        // 원래 색상으로 복구
        var spriteList = Target.GetComponent<EnemyUnit>()?.GetUnitImg;
        if (spriteList != null)
        {
            spriteList.color = originalColors[0];
        }

        isApplied = false;
        originalColors.Clear();
    }

    public override void OnDamage()
    {
        base.OnDamage();
        // 중첩 수에 따라 데미지 증가
        double stackedDamage = Damage * StackCount;
        Target.Damage(stackedDamage);


    }



}

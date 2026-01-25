using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

[EffectPath("Effect/AmagetdonEffect", false, false)]
public class AmagetdonEffect : Effect
{
    private bool IsTracking = false;

    private Transform Target;

    private float AttackRange = 3;

    public void Init(Transform target, System.Action endaction)
    {
        Target = target;
        IsTracking = false;
        this.transform.DOMove(target.transform.position, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            endaction?.Invoke();
            IsTracking = true;
            OnAttackDamage();
        });
    }

    private void Update()
    {
        if (IsTracking && Target != null)
        {
            this.transform.position = Target.position;
        }
    }


    public void OnAttackDamage()
    {
        var findenemylist = GameRoot.Instance.InGameSystem.GetInGame<InGameBase>().Stage.EnemyUnitGroup.ActiveUnits;

        // AttackRange 내의 살아있는 모든 적에게 데미지
        var enemiesInRange = findenemylist
            .Where(x => !x.IsDead)
            .Where(x => Vector3.Distance(x.transform.position, this.transform.position) <= AttackRange)
            .ToList();

        foreach (var enemy in enemiesInRange)
        {
            enemy.Damage(10);
        }
    }
}


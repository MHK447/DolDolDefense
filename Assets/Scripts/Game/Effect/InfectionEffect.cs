using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[EffectPath("Effect/InfectionEffect", false, false)]
public class InfectionEffect : Effect
{
    private bool IsTracking = false;

    private Transform Target;

    public void Init(Transform target, System.Action endaction)
    {
        Target = target;
        IsTracking = false;
        this.transform.DOMove(target.transform.position, 0.5f).SetEase(Ease.Linear).OnComplete(() => {
            endaction?.Invoke();
            IsTracking = true;
        });
    }

    private void Update()
    {
        if(IsTracking && Target != null)
        {
            this.transform.position = Target.position;
        }
    }
}

using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UniRx;

public class PlayerUnitInfoData
{
    public IReactiveProperty<int> StartHpProperty { get; private set; } = new ReactiveProperty<int>(0);
    public IReactiveProperty<int> CurHpProperty { get; private set; } = new ReactiveProperty<int>(0);

    public float AttackRange = 0f;
    public int AttackDamage = 1;





}


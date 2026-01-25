using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[EffectPath("Effect/LifeStealEffect", false, false)]
public class LifeStealEffect : Effect
{

    public enum State
    {
        Attack,
        Move,
        Idle,
    }

    private InGameBaseStage InGameStage = null;

    private int Damage = 0;

    private State CurState = State.Idle;

    private EnemyUnit CurrentTarget = null;

    private float AttackDuration = 2f; // 공격 지속 시간
    private float DamageInterval = 0.1f; // 데미지 간격
    private float RestDuration = 1f; // 휴식 시간

    private int TotalDamageDealt = 0; // 총 입힌 데미지
    private float FollowSpeed = 5f; // 타겟 따라가는 속도

    public void Init(int damage)
    {
        Damage = damage;

        InGameStage = GameRoot.Instance.InGameSystem.GetInGame<InGameBase>().Stage;

        StartCoroutine(AttackCycle());
    }

    void Update()
    {
        if (InGameStage == null) return;

        // 공격 상태일 때 타겟을 따라감
        if (CurState == State.Attack && CurrentTarget != null)
        {
            transform.position = Vector3.Lerp(transform.position, CurrentTarget.transform.position, Time.deltaTime * FollowSpeed);
        }
    }

    private IEnumerator AttackCycle()
    {
        while (true)
        {
            // 1. 적 찾기
            CurrentTarget = InGameStage.EnemyUnitGroup.FindEnemyTarget(InGameStage.PlayerUnit.transform.position);

            if (CurrentTarget != null)
            {
                // 2. 공격 상태로 전환
                CurState = State.Attack;
                TotalDamageDealt = 0;

                // 3. 2초 동안 0.1초 간격으로 데미지 주기
                float elapsedTime = 0f;
                while (elapsedTime < AttackDuration)
                {
                    if (CurrentTarget != null && !CurrentTarget.IsDead)
                    {
                        // 데미지 적용
                        CurrentTarget.Damage(Damage);
                        TotalDamageDealt += Damage;
                    }
                    else
                    {
                        // 타겟이 죽었으면 새로운 타겟 찾기
                        CurrentTarget = InGameStage.EnemyUnitGroup.FindEnemyTarget(InGameStage.PlayerUnit.transform.position);
                        if (CurrentTarget == null)
                            break;
                    }

                    yield return new WaitForSeconds(DamageInterval);
                    elapsedTime += DamageInterval;
                }

                // 4. 체력 회복 (총 입힌 데미지만큼)
                if (TotalDamageDealt > 0)
                {
                    InGameStage.PlayerUnit.PlayerUnitInfoData.CurHp += TotalDamageDealt;

                    // 최대 체력 넘지 않도록 제한
                    if (InGameStage.PlayerUnit.PlayerUnitInfoData.CurHp > InGameStage.PlayerUnit.PlayerUnitInfoData.StartHp)
                    {
                        InGameStage.PlayerUnit.PlayerUnitInfoData.CurHp = InGameStage.PlayerUnit.PlayerUnitInfoData.StartHp;
                    }

                    // UI 업데이트
                    InGameStage.PlayerUnit.SetHpProgress(InGameStage.PlayerUnit.PlayerUnitInfoData.CurHp);
                }

                // 5. 휴식 상태로 전환
                CurState = State.Idle;
                CurrentTarget = null;

                // 6. 1초 휴식
                yield return new WaitForSeconds(RestDuration);
            }
            else
            {
                // 적이 없으면 대기
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();

        SetAutoRemove(true, 0f);
    }

    void OnDisable()
    {
        StopAllCoroutines();

        SetAutoRemove(true, 0f);
    }
}

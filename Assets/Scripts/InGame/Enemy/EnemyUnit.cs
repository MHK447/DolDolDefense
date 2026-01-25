using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;
using System.Numerics;

public enum KnocBackDirection
{
    Back,
    Left,
    Right,
}

public class EnemyUnit : MonoBehaviour
{
    public enum EnemyState
    {
        Dead,
        Move,
        Sturn,
    }

    protected InGameHpProgress InGameHpProgress;


    [HideInInspector]
    public EnemyInfoData EnemyInfoData = new EnemyInfoData();

    [SerializeField]
    private SpriteRenderer UnitImg;

    public SpriteRenderer GetUnitImg { get { return UnitImg; } }


    private int EnemyIdx = 0;

    private EnemyState CurState = EnemyState.Move;

    protected InGameBaseStage BaseStage;
    public bool IsDead { get { return CurState == EnemyState.Dead; } }


    private PlayerUnit PlayerUnit;




    public void Set(int enemyidx, int hp)
    {
        EnemyIdx = enemyidx;

        EnemyInfoData.StartHp = hp;
        EnemyInfoData.CurHp = hp;
        EnemyInfoData.MoveSpped = 0.5f;

        BaseStage = GameRoot.Instance.InGameSystem.GetInGame<InGameBase>().Stage;


        SetState(EnemyState.Move);


        SetHpprogress(hp);

        this.transform.DOKill();
        this.transform.localScale = UnityEngine.Vector3.zero;
        this.transform.DOScale(UnityEngine.Vector3.one, 0.3f).SetEase(Ease.OutBack);

        UnitImg.DisableHitEffect();


        PlayerUnit = BaseStage.PlayerUnit;
    }


    public void PlayerUnitDamage(int damage)
    {
        BaseStage.PlayerUnit.Damage(damage);
    }



    public void SetHpprogress(int hp)
    {
        if (InGameHpProgress == null)
        {
            GameRoot.Instance.UISystem.LoadFloatingUI<InGameHpProgress>(hpprogress =>
                    {
                        InGameHpProgress = hpprogress;
                        // 먼저 비활성화하여 잘못된 위치에서 보이지 않도록 함
                        ProjectUtility.SetActiveCheck(hpprogress.gameObject, true);
                        hpprogress.Init(transform);
                        hpprogress.SetHpText(hp, EnemyInfoData.StartHp);
                    });
        }
        else
        {
            InGameHpProgress.SetHpText(hp, EnemyInfoData.StartHp);
            ProjectUtility.SetActiveCheck(InGameHpProgress.gameObject, true);
        }
    }


    void Update()
    {
        Move();
    }


    public virtual void Damage(double damage)
    {
        GameRoot.Instance.DamageTextSystem.ShowDamage(damage,
        new UnityEngine.Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Color.white);

        EnemyInfoData.CurHp -= (int)damage;
        InGameHpProgress?.SetHpText((int)EnemyInfoData.CurHp, EnemyInfoData.StartHp);

        DamageColorEffect();

        if (EnemyInfoData.CurHp <= 0)
        {
            Dead();
        }
    }


    public virtual void Dead()
    {
        SetState(EnemyState.Dead);
        ProjectUtility.SetActiveCheck(InGameHpProgress.gameObject, false);

        this.transform.DOKill();

        this.transform.localScale = UnityEngine.Vector3.one;

        GameRoot.Instance.EffectSystem.MultiPlay<TextEffectMoney>(transform.position, (x) =>
        {
            GameRoot.Instance.UserData.InGamePlayerData.InGameMoneyProperty.Value += 1;
            x.SetText(1);
        });

        this.transform.DOScale(UnityEngine.Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            BaseStage.EnemyUnitGroup.DeadUnits.Add(this);
            ProjectUtility.SetActiveCheck(this.gameObject, false);
        });
    }



    public virtual void SetState(EnemyState state)
    {
        if (CurState == state) return;

        CurState = state;
    }

    public void Move()
    {
        if (CurState != EnemyState.Move) return;


        // PlayerUnit 방향으로 이동
        if (PlayerUnit != null)
        {
            UnityEngine.Vector3 direction = (PlayerUnit.transform.position - transform.position).normalized;
            transform.position += direction * EnemyInfoData.MoveSpped * Time.deltaTime;
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            BaseStage.PlayerUnit.Damage((int)EnemyInfoData.CurHp);
            Dead();
        }
    }

    private bool IsDamageDirect = false;

    public virtual void DamageColorEffect()
    {
        if (!IsDamageDirect)
        {
            IsDamageDirect = true;

            UnitImg.EnableHitEffect();

            // 피격 효과 적용


            GameRoot.Instance.WaitTimeAndCallback(0.15f, () =>
            {
                if (this != null)
                {
                    // 효과 종료 후 원래 머티리얼로 복귀
                    UnitImg.DisableHitEffect();

                    IsDamageDirect = false;
                }
            });
        }
    }


    public void KnockBack(KnocBackDirection direction , float power = 5f)
    {
        if (PlayerUnit == null) return;

        UnityEngine.Vector3 knockbackDirection = UnityEngine.Vector3.zero;

        switch (direction)
        {
            case KnocBackDirection.Back:
                // 현재 이동 방향의 반대편으로 넉백
                knockbackDirection = (transform.position - PlayerUnit.transform.position).normalized;
                break;
            case KnocBackDirection.Left:
                // 왼쪽으로 넉백
                knockbackDirection = UnityEngine.Vector3.left;
                break;
            case KnocBackDirection.Right:
                // 오른쪽으로 넉백
                knockbackDirection = UnityEngine.Vector3.right;
                break;
        }

        // 넉백 효과 적용
        transform.position += knockbackDirection * power * Time.deltaTime;
    }
}


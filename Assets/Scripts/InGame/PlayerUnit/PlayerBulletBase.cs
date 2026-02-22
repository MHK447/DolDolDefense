using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PlayerBulletBase : MonoBehaviour
{
    [SerializeField]
    private ColliderAction ColAction;

    [SerializeField]
    private float MoveSpeed = 4f;

    [SerializeField]
    [Tooltip("포물선 궤적의 최대 높이 (위로 올라갔다 떨어지는 정도)")]
    private float ArcHeight = 12f;

    public TrailComponent TrailComponent;

    private int BulletIdx = 0;

    public int GetBulletIdx { get { return BulletIdx; } }

    protected PlayerUnit PlayerUnit;
    protected System.Action<PlayerBulletBase> DeleteAction;

    private Vector3 StartPosition;
    private Vector3 TargetPosition;
    private float TravelDuration;
    private float ElapsedTime;

    protected bool IsDamageOn = false;

    public virtual void Awake()
    {
        if (TrailComponent != null)
        {
            TrailComponent.InitTrail();
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        // {
        //     var enemy = collision.gameObject.GetComponent<EnemyUnit>();
        //     if (enemy != null && !IsDamageOn)
        //     {
        //         IsDamageOn = true;
        //         enemy.Damage(BulletIdx);
        //         DeleteAction?.Invoke(this);
        //     }
        // }
    }

    public void TargetDamage()
    {
        const float range = 1f;
        var stage = GameRoot.Instance.InGameSystem.GetInGame<InGameBase>().Stage;
        if (stage?.EnemyUnitGroup == null) return;

        foreach (var enemy in stage.EnemyUnitGroup.ActiveUnits)
        {
            if (enemy == null || enemy.IsDead) continue;
            if (Vector3.Distance(transform.position, enemy.transform.position) > range) continue;
            enemy.Damage(1);
        }
    }

    public virtual void Set(int bulletidx, PlayerUnit unit, Vector3 targetposition, System.Action<PlayerBulletBase> deleteaction)
    {
        IsDamageOn = false;
        BulletIdx = bulletidx;
        PlayerUnit = unit;
        StartPosition = transform.position;
        TargetPosition = targetposition;
        DeleteAction = deleteaction;
        //ColAction.TriggerEnterAction = OnTriggerEnter2D;

        // 포물선 궤적의 실제 경로 길이로 이동 시간 계산 → 거리와 무관하게 속도 일정
        const int arcSamples = 32;
        float arcLength = 0f;
        Vector3 prev = GetPositionOnArc(0f);
        for (int i = 1; i <= arcSamples; i++)
        {
            float t = (float)i / arcSamples;
            Vector3 pos = GetPositionOnArc(t);
            arcLength += Vector3.Distance(prev, pos);
            prev = pos;
        }
        TravelDuration = arcLength / MoveSpeed;
        ElapsedTime = 0f;
    }

    private Vector3 GetPositionOnArc(float t)
    {
        Vector3 linear = Vector3.Lerp(StartPosition, TargetPosition, t);
        float parabola = 4f * ArcHeight * t * (1f - t);
        return linear + Vector3.up * parabola;
    }

    public virtual void Update()
    {
        Move();
    }


    public virtual void Move()
    {
        ElapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(ElapsedTime / TravelDuration);

        // 포물선: 직선 보간 + 위로 볼록한 높이 (t=0.5에서 최고점)
        transform.position = GetPositionOnArc(t);

        if (t >= 1f)
        {
            DeleteAction?.Invoke(this);
            TargetDamage();
        }
    }

}


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
    private float MoveSpeed = 10f;

    private int BulletIdx = 0;

    public int GetBulletIdx { get { return BulletIdx; } }

    protected PlayerUnit PlayerUnit;
    protected System.Action<PlayerBulletBase> DeleteAction;

    private Vector3 TargetPosition;
    private Vector3 Direction;

    protected bool IsDamageOn = false;

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyUnit>();
            if (enemy != null && !IsDamageOn)
            {
                IsDamageOn = true;
                enemy.Damage(BulletIdx);
                DeleteAction?.Invoke(this);
            }
        }
    }

    public virtual void Set(int bulletidx, PlayerUnit unit, Vector3 targetposition, System.Action<PlayerBulletBase> deleteaction)
    {
        IsDamageOn = false;
        BulletIdx = bulletidx;
        PlayerUnit = unit;
        TargetPosition = targetposition;
        DeleteAction = deleteaction;
        ColAction.TriggerEnterAction = OnTriggerEnter2D;
        Direction = (TargetPosition - transform.position).normalized;
    }

    public virtual void Update()
    {
        Move();
    }


    public virtual void Move()
    {
        transform.position += Direction * MoveSpeed * Time.deltaTime;
    }

}


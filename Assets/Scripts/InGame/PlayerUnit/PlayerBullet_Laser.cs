using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

public class PlayerBullet_Laser : PlayerBulletBase
{

    [HideInInspector]
    public float EndTime = 3f;

    [SerializeField]
    private SpriteRenderer BulletLaserRoot;

    private EnemyUnit TargetEnemy;


    public override void Set(int bulletidx, PlayerUnit unit, Vector3 targetposition, Action<PlayerBulletBase> deleteaction)
    {
        base.Set(bulletidx, unit, targetposition, deleteaction);

        // 타겟 위치에 있는 적 유닛 찾기
        FindTargetEnemy();


        GameRoot.Instance.WaitTimeAndCallback(EndTime , ()=>
        {
            DeleteAction?.Invoke(this);
        });
    }

    private void FindTargetEnemy()
    {
        // 가장 가까운 적 유닛을 찾습니다
        var enemies = GameRoot.Instance.InGameSystem.GetInGame<InGameBase>().Stage.EnemyUnitGroup.FindEnemyTarget(this.transform.position, 10f);
        if (enemies != null)
        {
            TargetEnemy = enemies;
        }
    }

    public override void Update()
    {
        base.Update();

        // BulletLaserRoot가 타겟을 바라보게 회전
        if (BulletLaserRoot != null && TargetEnemy != null)
        {
            Vector3 direction = TargetEnemy.transform.position - BulletLaserRoot.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            BulletLaserRoot.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }


    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyUnit>();
            if (enemy != null && !IsDamageOn)
            {
                IsDamageOn = true;
                enemy.Damage(5);
            }
        }
    }
}


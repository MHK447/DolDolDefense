using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

public class PlayerBullet_ThrowLog : PlayerBulletBase
{

    public override void Set(int bulletidx, PlayerUnit unit, Vector3 targetposition, Action<PlayerBulletBase> deleteaction)
    {
        base.Set(bulletidx, unit, targetposition, deleteaction);


        GameRoot.Instance.WaitTimeAndCallback(3f , ()=> {
            DeleteAction?.Invoke(this);
        });
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyUnit>();
            if (enemy != null && !IsDamageOn)
            {
                IsDamageOn = true;
                enemy.Damage(3);
                enemy.KnockBack(KnocBackDirection.Back, 2f);
            }
        }
    }
}


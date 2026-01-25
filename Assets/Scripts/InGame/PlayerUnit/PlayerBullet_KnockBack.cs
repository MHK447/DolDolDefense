using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PlayerBullet_KnockBack : PlayerBulletBase
{
    override public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
                enemy.KnockBack(KnocBackDirection.Back, 5f);
                enemy.Damage(10);
            }
        }
    }
}


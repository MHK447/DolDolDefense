using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PlayerBullet_BlueSoul : PlayerBullet_Orb
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
                enemy.Damage(1);
                enemy.KnockBack(KnocBackDirection.Back, 1f);
            }
        }
    }
}


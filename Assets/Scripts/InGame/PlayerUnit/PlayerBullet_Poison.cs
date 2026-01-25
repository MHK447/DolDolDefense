using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PlayerBullet_Poison : PlayerBulletBase
{
    public override void Move()
    {
        base.Move();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyUnit>();
            if (enemy != null && !IsDamageOn)
            {
                IsDamageOn = true;
                enemy.Damage((int)PlayerUnit.PlayerUnitInfoData.AttackDamage);

                GameRoot.Instance.AlimentSystem.AddAliment(AlimentType.Poison, 1f, enemy, 0, 1f,1 );
            }
        }
    }
}

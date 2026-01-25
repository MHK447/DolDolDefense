using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

public class PlayerBullet_ShadowKnife : PlayerBulletBase
{
    private float elapsedTime = 0f;
    private float outDuration = 3f;
    private float returnDuration = 3f;
    private bool isReturning = false;
    private Vector3 startPosition;
    private Vector3 targetDirection;

    public override void Set(int bulletidx, PlayerUnit unit, Vector3 targetposition, Action<PlayerBulletBase> deleteaction)
    {
        base.Set(bulletidx, unit, targetposition, deleteaction);
        elapsedTime = 0f;
        isReturning = false;
        startPosition = transform.position;
        targetDirection = (targetposition - transform.position).normalized;
    }

    override public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
                enemy.Damage(10);

                var randvalue = UnityEngine.Random.Range(0, 3);
                enemy.KnockBack((KnocBackDirection)randvalue, 5f);
            }
        }
    }

    public override void Move()
    {
        elapsedTime += Time.deltaTime;

        if (!isReturning && elapsedTime < outDuration)
        {
            // 3초 동안 앞으로 발사
            transform.position += targetDirection * 10f * Time.deltaTime;
        }
        else if (!isReturning && elapsedTime >= outDuration)
        {
            // 3초가 지나면 돌아오기 시작
            isReturning = true;
            elapsedTime = 0f;
        }
        else if (isReturning && elapsedTime < returnDuration)
        {
            // 3초 동안 시작 위치로 돌아옴
            Vector3 returnDirection = (startPosition - transform.position).normalized;
            transform.position += returnDirection * 10f * Time.deltaTime;
        }
        else if (isReturning && elapsedTime >= returnDuration)
        {
            // 돌아온 후 삭제
            DeleteAction?.Invoke(this);
        }
    }
}


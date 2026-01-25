using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PlayerBullet_Orb : PlayerBulletBase
{
    [SerializeField] private float rotationSpeed = 100f; // 회전 속도 (degree per second)
    [SerializeField] private float orbitRadius = 1.5f; // 궤도 반지름
    private float currentAngle = 0f; // 현재 각도


    public override void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<EnemyUnit>();
        if (enemy != null)
        {
            enemy.Damage(GetBulletIdx);
        }
    }
    override public void Move()
    {
        if (PlayerUnit == null) return;

        // 각도 증가
        currentAngle += rotationSpeed * Time.deltaTime;
        
        // 각도를 0~360 범위로 유지
        if (currentAngle >= 360f)
        {
            currentAngle -= 360f;
        }

        // 플레이어 유닛 위치를 중심으로 원형 궤도 계산
        float radian = currentAngle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radian) * orbitRadius;
        float y = Mathf.Sin(radian) * orbitRadius;

        // 플레이어 유닛의 위치 + 궤도 오프셋
        transform.position = PlayerUnit.transform.position + new Vector3(x, y, 0f);

        // 오브 자체도 회전시키기 (선택사항)
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}


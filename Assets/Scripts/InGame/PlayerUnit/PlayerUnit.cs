using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.AddressableAssets;

public class PlayerUnit : MonoBehaviour
{
    public enum PlayerUnitState
    {
        Idle,
        Attack,
        Dead,
    }


    [SerializeField]
    private SpriteRenderer PlayerUnitImg;

    [SerializeField]
    private SpriteRenderer PlayerWeaponImg;


    private PlayerUnitState UnitState = PlayerUnitState.Idle;

    public bool IsDead { get { return UnitState == PlayerUnitState.Dead; } }

    [HideInInspector]
    public PlayerUnitInfoData PlayerUnitInfoData = new PlayerUnitInfoData();

    [SerializeField]
    private LineRenderer AttackRangeRenderer;

    private CastleHpProgress CastleHpProgress;

    private int PlayerUnitIdx = 0;

    private Coroutine AttackCo = null;



    private List<PlayerBulletBase> PlayerBulletList = new List<PlayerBulletBase>();



    public void Set(int unitidx)
    {
        PlayerUnitIdx = unitidx;

        var td = Tables.Instance.GetTable<PlayerUnitInfo>().GetData(unitidx);



        if (td != null)
        {
            PlayerUnitInfoData.StartHp = td.base_hp;
            PlayerUnitInfoData.CurHp = td.base_hp;
            PlayerUnitInfoData.AttackRange = 500 * 0.01f;
            AddPlayerSkill(td.base_skill, 1);

            UnitState = PlayerUnitState.Idle;

            SetHpProgress(PlayerUnitInfoData.CurHp);

            StartAttack();

            CreateCircle();
        }


    }

    public void SetHpProgress(int hp)
    {
        if (CastleHpProgress == null)
        {
            GameRoot.Instance.UISystem.LoadFloatingUI<CastleHpProgress>(x =>
            {
                CastleHpProgress = x;
                CastleHpProgress.SetHpText(hp, PlayerUnitInfoData.StartHp);
                CastleHpProgress.Init(transform);
            });
        }
        else
        {
            CastleHpProgress.SetHpText(hp, PlayerUnitInfoData.StartHp);
        }
    }



    public void Damage(int damage)
    {
        PlayerUnitInfoData.CurHp -= damage;
        CastleHpProgress.SetHpText(PlayerUnitInfoData.CurHp, PlayerUnitInfoData.StartHp);
        DamageColorEffect();

        if (PlayerUnitInfoData.CurHp <= 0)
        {
            UnitState = PlayerUnitState.Dead;
        }
    }




    private bool IsDamageDirect = false;
    public virtual void DamageColorEffect()
    {
        if (!IsDamageDirect)
        {
            IsDamageDirect = true;

            PlayerUnitImg.EnableHitEffect();

            // 피격 효과 적용


            GameRoot.Instance.WaitTimeAndCallback(0.15f, () =>
            {
                if (this != null)
                {
                    // 효과 종료 후 원래 머티리얼로 복귀
                    PlayerUnitImg.DisableHitEffect();

                    IsDamageDirect = false;
                }
            });
        }
    }


    public virtual void StartAttack()
    {
        if (AttackCo != null)
        {
            GameRoot.Instance.StopCoroutine(AttackCo);
        }

        AttackCo = StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        while (!IsDead)
        {
            var playerSkillList = GameRoot.Instance.UserData.InGamePlayerData.PlayerSkillDataList;

            foreach (var skill in playerSkillList)
            {
                skill.SkilldeltaTime += Time.deltaTime;

                if (skill.SkilldeltaTime >= skill.SKillCoolTime)
                {
                    // 공격이 성공했을 때만 쿨타임 리셋
                    if (Attack(skill))
                    {
                        skill.SkilldeltaTime = 0f;
                    }
                }
            }

            yield return null;
        }
    }

    public virtual bool Attack(PlayerSkillData skill)
    {
        var prefab = Tables.Instance.GetTable<PlayerSkillInfo>().GetData(skill.SkillIdx).bullet_prefab;

        var finddata = PlayerBulletList.Find(x => x.GetBulletIdx == skill.SkillIdx && !x.gameObject.activeSelf);

        var findenemy = GameRoot.Instance.InGameSystem.GetInGame<InGameBase>().Stage.EnemyUnitGroup.FindEnemyTarget(transform.position, PlayerUnitInfoData.AttackRange);

        // 적이 없으면 공격 실패 (false 반환하여 쿨타임 리셋 안 함)
        if (findenemy == null)
        {
            return false;
        }

        if (finddata != null)
        {
            ProjectUtility.SetActiveCheck(finddata.gameObject, true);

            finddata.transform.position = transform.position;
            finddata.Set(skill.SkillIdx, this, findenemy.transform.position, BulletColisionAction);
        }
        else
        {
            var bullet = Addressables.InstantiateAsync(prefab, transform).WaitForCompletion().GetComponent<PlayerBulletBase>();
            bullet.Set(skill.SkillIdx, this, findenemy.transform.position, BulletColisionAction);

            ProjectUtility.SetActiveCheck(bullet.gameObject, true);


            PlayerBulletList.Add(bullet);
        }

        return true; // 공격 성공
    }


    public void AddPlayerSkill(int skillidx, int skilllevel)
    {
        var td = Tables.Instance.GetTable<PlayerSkillInfo>().GetData(skillidx);

        if (td != null)
        {
            GameRoot.Instance.UserData.InGamePlayerData.AddPlayerSkill(skillidx, skilllevel, td.base_atk_speed / 100f, td.base_atk);
        }
    }



    public void Clear()
    {
        if (PlayerBulletList != null && PlayerBulletList.Count > 0)
        {
            for (int i = PlayerBulletList.Count - 1; i >= 0; i--)
            {
                if (PlayerBulletList[i] != null && PlayerBulletList[i].gameObject != null)
                {
                    Destroy(PlayerBulletList[i].gameObject);
                }
            }
            PlayerBulletList.Clear();
        }

        if (AttackCo != null)
        {
            StopCoroutine(AttackCo);
            AttackCo = null;
        }
    }


    public void BulletColisionAction(PlayerBulletBase bullet)
    {
        ProjectUtility.SetActiveCheck(bullet.gameObject, false);
    }


    private int segments = 200;

    void CreateCircle()
    {
        
        AttackRangeRenderer.positionCount = segments + 1;
        AttackRangeRenderer.useWorldSpace = false; // 로컬 좌표계 사용 (유닛을 따라다님)
        AttackRangeRenderer.loop = true; // 원의 시작점과 끝점을 연결

        // Width 설정 - 반지름에 비례하게 조정
        AttackRangeRenderer.startWidth = 0.1f;
        AttackRangeRenderer.endWidth = 0.1f;
        
        // Color 설정 (알파값 포함하여 더 잘 보이게)
        AttackRangeRenderer.startColor = new Color(1f, 1f, 1f, 0.5f); // 흰색
        AttackRangeRenderer.endColor = new Color(1f, 1f, 1f, 0.5f);
        
      
        float angle = 0f;
        float radius = 3f; // 반지름을 명시적으로 지정

        for (int i = 0; i < segments + 1; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * PlayerUnitInfoData.AttackRange;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * PlayerUnitInfoData.AttackRange;

            // 로컬 좌표계로 설정 (유닛 중심 기준)
            AttackRangeRenderer.SetPosition(i, new Vector3(x, y, 0f));
            angle += (360f / segments);
        }
        
        Debug.Log($"Circle created at {transform.position} with radius {radius}, LineRenderer enabled: {AttackRangeRenderer.enabled}, Material: {AttackRangeRenderer.material?.name}");
    }

}


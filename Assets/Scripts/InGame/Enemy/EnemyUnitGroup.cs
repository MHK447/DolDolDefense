using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class EnemyUnitGroup : MonoBehaviour
{

    [SerializeField]
    private List<Transform> EnemySpawnTrList = new List<Transform>();

    public HashSet<EnemyUnit> ActiveUnits = new HashSet<EnemyUnit>();
    public HashSet<EnemyUnit> DeadUnits = new HashSet<EnemyUnit>();

    [HideInInspector]
    public int SpawnOrder = 0;

    public void AddEnemyUnit(int enemyidx, int hp)
    {
        SpawnOrder++;

        var find = DeadUnits.FirstOrDefault();

        EnemyUnit instance;

        // EnemySpawnTrList 카운트 순서대로 순환
        int spawnIndex = SpawnOrder % EnemySpawnTrList.Count;

        if (find != null)
        {
            // 재활용
            instance = find;
            DeadUnits.Remove(find);
            ActiveUnits.Add(instance);
        }
        else
        {
            // 새로 생성
            var handle = Addressables.InstantiateAsync("EnemyUnit_Base", transform);
            var result = handle.WaitForCompletion();
            instance = result.GetComponent<EnemyUnit>();
            ActiveUnits.Add(instance);
        }

        // 위치 설정 및 초기화
        instance.transform.position = EnemySpawnTrList[spawnIndex].position;
        instance.Set(enemyidx , hp);

    }


    public EnemyUnit FindEnemyTarget(Vector3 position, float range)
    {
        var findenemytarget = ActiveUnits.FirstOrDefault(x => x.transform.position.x >= position.x - range && x.transform.position.x <= position.x + range && x.transform.position.y >= position.y - range && x.transform.position.y <= position.y + range);
        return findenemytarget;
    }

}


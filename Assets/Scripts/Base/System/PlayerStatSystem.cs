using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PlayerStatSystem
{

    public enum PlayerStatType
    {
        ATTACK = 1,
        HEALTH = 2,
        COOLTIME = 3,
        CRITICALDAMAGE = 4,
    }


    public Dictionary<int, int> PlayerStatDic = new Dictionary<int, int>();


    public void Create()
    {
        if (PlayerStatDic.Count == 0)
        {
            PlayerStatDic.Add((int)PlayerStatType.ATTACK, 0);
            PlayerStatDic.Add((int)PlayerStatType.HEALTH, 0);
            PlayerStatDic.Add((int)PlayerStatType.COOLTIME, 0);
            PlayerStatDic.Add((int)PlayerStatType.CRITICALDAMAGE, 0);
        }
    }

    public void AddStat(PlayerStatType statType, int value)
    {
        PlayerStatDic[(int)statType] += value;
    }


    
}


using UnityEngine;
using System.Collections.Generic;

namespace BanpoFri
{
    [System.Serializable]
    public class PlayerSkillInfoData
    {
        [SerializeField]
		private int _skill_idx;
		public int skill_idx
		{
			get { return _skill_idx;}
			set { _skill_idx = value;}
		}
		[SerializeField]
		private int _base_atk;
		public int base_atk
		{
			get { return _base_atk;}
			set { _base_atk = value;}
		}
		[SerializeField]
		private int _base_atk_speed;
		public int base_atk_speed
		{
			get { return _base_atk_speed;}
			set { _base_atk_speed = value;}
		}
		[SerializeField]
		private string _bullet_prefab;
		public string bullet_prefab
		{
			get { return _bullet_prefab;}
			set { _bullet_prefab = value;}
		}

    }

    [System.Serializable]
    public class PlayerSkillInfo : Table<PlayerSkillInfoData, int>
    {
    }
}


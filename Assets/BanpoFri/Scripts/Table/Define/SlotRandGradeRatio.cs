using UnityEngine;
using System.Collections.Generic;
using BanpoFri;
using System.Linq;
using UnityEngine.UI;

namespace BanpoFri
{
    [System.Serializable]
    public class SlotRandGradeRatioData
    {
        [SerializeField]
		private int _idx;
		public int idx
		{
			get { return _idx;}
			set { _idx = value;}
		}
		[SerializeField]
		private float _ratio;
		public float ratio
		{
			get { return _ratio;}
			set { _ratio = value;}
		}

    }

    [System.Serializable]
    public class SlotRandGradeRatio : Table<SlotRandGradeRatioData, int>
    {
    }
}


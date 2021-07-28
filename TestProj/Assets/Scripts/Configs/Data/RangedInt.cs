using System;
using UnityEngine;

namespace Configs.Data
{
	[Serializable]
	public struct RangedInt
	{
		public int MinValue;
		public int MaxValue;

		public RangedInt(int minValue, int maxValue)
		{
			MinValue = minValue;
			MaxValue = maxValue;
		}

		private int Min => Mathf.Min(MinValue, MaxValue);

		private int Max => Mathf.Max(MinValue, MaxValue);

		public int GetRandom()
		{
			return UnityEngine.Random.Range(Min, Max);
		}

		public bool CheckInRange(int value)
		{
			return value <= Max && value >= Min;
		}
	}
}

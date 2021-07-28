using System;
using Configs.Data;
using UnityEngine;

namespace Configs
{
	[CreateAssetMenu(fileName = "CardConfig", menuName = "CardConfig")]
	[Serializable]
	public class CardConfig : ScriptableObject, ICardConfig
	{
		[SerializeField]
		private string IconPath;
		[SerializeField]
		private string Title;
		[SerializeField]
		private string Description;

		[SerializeField]
		private RangedInt _manaRange;
		[SerializeField]
		private RangedInt _attackRange;
		[SerializeField]
		private RangedInt _healthRange;

		public string GetTitle()
		{
			return Title;
		}

		public string GetDescription()
		{
			return Description;
		}

		public string GetIconPath()
		{
			return IconPath;
		}

		public RangedInt GetCounterRange(CardCounterType counterType)
		{
			switch (counterType)
			{
				case CardCounterType.Attack:
					return _attackRange;

				case CardCounterType.Health:
					return _healthRange;

				default:
					return _manaRange;
			}
		}
	}
}
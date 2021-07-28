using System;
using Configs.Data;
using UnityEngine;

namespace Configs
{
	[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig")]
	[Serializable]
	public class GameConfig : ScriptableObject, IGameConfig
	{
		[Header("UI")]
		[SerializeField]
		private GameObject _levelScreen;

		[SerializeField]
		private GameObject _cardItem;


		[Header("Gameplay")]
		[SerializeField]
		[Tooltip("MinValue inclusive, MaxValue exclusive")]
		private RangedInt _cardCountRange;

		[SerializeField]
		[Tooltip("MinValue inclusive, MaxValue exclusive")]
		private RangedInt _counterPlayRange;

		public GameObject GetLevelScreen()
		{
			return _levelScreen;
		}

		public GameObject GetCardItem()
		{
			return _cardItem;
		}

		public RangedInt GetCardCountRange()
		{
			return _cardCountRange;
		}

		public RangedInt GetCounterPlayRange()
		{
			return _counterPlayRange;
		}
	}
}
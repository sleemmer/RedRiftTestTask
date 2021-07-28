using Configs.Data;
using UnityEngine;

namespace Configs
{
	public interface IGameConfig
	{
		GameObject GetLevelScreen();

		GameObject GetCardItem();

		RangedInt GetCardCountRange();

		RangedInt GetCounterPlayRange();
	}
}
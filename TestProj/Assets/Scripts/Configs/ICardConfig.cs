using Configs.Data;

namespace Configs
{
	public interface ICardConfig
	{
		string GetTitle();

		string GetDescription();

		string GetIconPath();

		RangedInt GetCounterRange(CardCounterType counterType);
	}
}
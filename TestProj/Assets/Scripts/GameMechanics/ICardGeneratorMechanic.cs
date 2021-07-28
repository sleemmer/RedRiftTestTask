using System.Collections.Generic;
using Configs;

namespace GameMechanics
{
	public interface ICardGeneratorMechanic
	{
		List<CardData> GenerateCards(int cardsCount, ICardConfig cardConfig);
	}
}
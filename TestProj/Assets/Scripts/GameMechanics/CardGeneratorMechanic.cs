using System.Collections.Generic;
using Configs;
using Configs.Data;
using UnityEngine;

namespace GameMechanics
{
	public class CardGeneratorMechanic : ICardGeneratorMechanic
	{
		public List<CardData> GenerateCards(int cardsCount, ICardConfig cardConfig)
		{
			var cardDataList = new List<CardData>();
			for (int i = 0; i < cardsCount; i++)
			{
				var cardData = GenerateCard(i, cardConfig);
				cardDataList.Add(cardData);
			}

			return cardDataList;
		}

		private CardData GenerateCard(int id, ICardConfig cardConfig)
		{
			var title = string.Format("{0} {1}", cardConfig.GetTitle(), id);
			var description = string.Format("{0} {1}", cardConfig.GetDescription(), id);
			var iconPath = cardConfig.GetIconPath();

			var mana = cardConfig.GetCounterRange(CardCounterType.Mana).GetRandom();
			var attack = cardConfig.GetCounterRange(CardCounterType.Attack).GetRandom();
			var health = cardConfig.GetCounterRange(CardCounterType.Health).GetRandom();

			var cardData = new CardData(id, iconPath, title, description, mana, attack, health);

			return cardData;
		}
	}
}
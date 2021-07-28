using System.Collections.Generic;
using Configs;
using GameMechanics;
using UnityEngine;

namespace UI.Items
{
	public class TableComponent : MonoBehaviour
	{
		[SerializeField]
		private RectTransform _cardRoot;

		List<CardItem> _cardItems = new List<CardItem>();

		public void CreateDroppedCard(CardData cardData, IGameConfig gameConfig)
		{
			var cardObject = GameObject.Instantiate(gameConfig.GetCardItem(), _cardRoot);
			var cardItem = cardObject.GetComponent<CardItem>();
			cardItem.Init(false);
			cardItem.SetData(cardData);

			_cardItems.Add(cardItem);
		}

		public void AddDroppedCard(CardItem cardItem)
		{
			cardItem.Init(false);
			cardItem.transform.parent = _cardRoot;
			cardItem.transform.localScale = Vector3.one;
			cardItem.transform.localRotation = Quaternion.identity;

			_cardItems.Add(cardItem);
		}
	}
}
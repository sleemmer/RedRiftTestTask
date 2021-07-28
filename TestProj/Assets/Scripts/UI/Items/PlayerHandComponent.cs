using System;
using System.Collections.Generic;
using Configs;
using GameMechanics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Items
{
	public class PlayerHandComponent : MonoBehaviour
	{
		public event Action<int> StartDragEvent;
		public event Action<PointerEventData> EndDragEvent;
		public event Action DeathEndEvent;

		[SerializeField]
		private RectTransform _cardRoot;

		[SerializeField]
		private int _cardCircleRadius;

		[SerializeField]
		private int _cardAngleStep;

		List<CardItem> _cardItems = new List<CardItem>();

		public void SetData(List<CardData> cardDataList, IGameConfig gameConfig)
		{
			foreach (var card in cardDataList)
			{
				var cardObject = GameObject.Instantiate(gameConfig.GetCardItem(), _cardRoot);
				var cardItem = cardObject.GetComponent<CardItem>();
				cardItem.Init();
				cardItem.SetData(card);
				cardItem.StartDragEvent += OnCardStartDrag;
				cardItem.EndDragEvent += OnCardEndDrag;

				_cardItems.Add(cardItem);
			}

			UpdateCardPositions();
		}

		private void UpdateCardPositions(bool isAnimated = false)
		{
			Vector3 center = _cardRoot.position;

			var cardsCount = _cardItems.Count;
			var halfCount = cardsCount / 2;
			bool isOdd = (cardsCount % 2) == 1;

			float angleStep = Mathf.Max(5, _cardAngleStep - cardsCount);

			if (isOdd)
			{
				var angle = (cardsCount / 2) * -angleStep;
				for (int i = 0; i < cardsCount; i++)
				{
					SetCardPosition(i, angle, _cardCircleRadius, center, isAnimated);
					angle += angleStep;
				}
			}
			else
			{
				var angle = angleStep * halfCount - angleStep * 0.5f;
				for (int i = 0; i < halfCount; i++)
				{
					SetCardPosition(i, -angle, _cardCircleRadius, center, isAnimated);
					SetCardPosition(cardsCount - 1 - i, angle, _cardCircleRadius, center, isAnimated);
					angle -= angleStep;
				}
			}
		
		}

		private void SetCardPosition(int index, float angle, float radius, Vector3 center, bool isAnimated = false)
		{
			var pos = new Vector3(
				center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad),
				center.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad),
				center.z);

			var rot = Quaternion.FromToRotation(Vector3.down, center - pos);
			_cardItems[index].transform.rotation = rot;

			var targetPos = pos - new Vector3(0, radius - 80, 0);
			if (isAnimated)
			{
				_cardItems[index].Move(0, 0.5f, targetPos);
			}
			else
			{
				_cardItems[index].GetRect().localPosition = targetPos;
			}
		}

		public void ChangeCardData(int index, CardData data, Action callback = null)
		{
			_cardItems[index].UpdatePlayableData(data, true, callback);
		}

		public void PlayDeath(int index)
		{
			_cardItems[index].PlayDeath( () =>
			{
				DestroyCard(_cardItems[index]);
				_cardItems.RemoveAt(index);

				UpdateCardPositions(true);

				DeathEndEvent?.Invoke();
			});
		}

		private void DestroyCard(CardItem cardItem)
		{
			cardItem.StartDragEvent -= OnCardStartDrag;
			cardItem.EndDragEvent -= OnCardEndDrag;
			cardItem.Dispose();
			DestroyImmediate(cardItem.gameObject);
		}

		public void GetDroppedCardIcon(int index)
		{
			DestroyCard(_cardItems[index]);
			_cardItems.RemoveAt(index);

			UpdateCardPositions(true);
		}

		public CardItem DropCard(int index)
		{
			var cardItem = _cardItems[index];
			cardItem.StartDragEvent -= OnCardStartDrag;
			cardItem.EndDragEvent -= OnCardEndDrag;
			cardItem.Dispose();

			_cardItems.RemoveAt(index);

			UpdateCardPositions(true);

			return cardItem;
		}

		private void OnCardStartDrag(CardItem draggedItem)
		{
			for (int i = 0; i < _cardItems.Count; i++)
			{
				if (draggedItem == _cardItems[i])
				{
					StartDragEvent?.Invoke(i);
					break;
				}
			}
		}

		private void OnCardEndDrag(PointerEventData eventData)
		{
			EndDragEvent?.Invoke(eventData);
		}
	}
}
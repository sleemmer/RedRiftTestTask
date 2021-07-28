using System;
using System.Collections.Generic;
using Configs;
using GameMechanics;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Items
{
	public class LevelScreen : MonoBehaviour
	{
		public event Action PlayButtonClickEvent;
		public event Action ExitButtonClickEvent;
		public event Action<int> StartDragEvent;
		public event Action<PointerEventData, RectTransform> EndDragEvent;

		[SerializeField]
		private Button _actionButton;

		[SerializeField]
		private PlayerHandComponent _playerHand;

		[SerializeField]
		private TableComponent _table;

		[SerializeField]
		private RectTransform _dropZone;

		private IGameConfig _gameConfig;
		private bool isActionButtonAvailable = true;

        #region Event Handlers

        [UsedImplicitly]
		public void OnActionButtonClick()
		{
			if (!isActionButtonAvailable)
				return;

			PlayButtonClickEvent?.Invoke();
		}

		[UsedImplicitly]
		public void OnExitButtonClick()
		{
			ExitButtonClickEvent?.Invoke();
		}

		private void OnCardStartDrag(int draggedCardIndex)
		{
			StartDragEvent?.Invoke(draggedCardIndex);
		}

		private void OnCardEndDrag(PointerEventData eventData)
		{
			EndDragEvent?.Invoke(eventData, _dropZone);
		}

		private void OnDeathEnd()
		{
			isActionButtonAvailable = true;
		}

		#endregion

		public void Init(IGameConfig gameConfig)
		{
			_gameConfig = gameConfig;

			_playerHand.StartDragEvent += OnCardStartDrag;
			_playerHand.EndDragEvent += OnCardEndDrag;
			_playerHand.DeathEndEvent += OnDeathEnd;
		}

		public void SetData(List<CardData> cardDataList)
		{
			_playerHand.SetData(cardDataList, _gameConfig);
		}

		public void ChangeCardData(int index, CardData data, Action callback = null)
		{
			isActionButtonAvailable = false;
			_playerHand.ChangeCardData(index, data, () =>
			{
				isActionButtonAvailable = true;
				callback();
			});
		}

		public void PlayDeath(int index)
		{
			isActionButtonAvailable = false;
			_playerHand.PlayDeath(index);
		}

		public void DropCard(int index, CardData cardData)
		{
			var dropppedCard = _playerHand.DropCard(index);
			_table.AddDroppedCard(dropppedCard);
		}
	}
}
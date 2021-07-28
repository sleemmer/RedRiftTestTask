using System.Collections.Generic;
using Configs;
using GameMechanics;
using UI.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameControllers
{
	public class GameController : IGameController
	{
		private readonly IGameConfig _gameConfig;
		private readonly ICardConfig _cardConfig;
		private readonly Transform _uiRoot;
		private readonly Camera _uiCamera;
		private readonly ICardGeneratorMechanic _cardGenerator;

		private List<CardData> _cardDataList = new List<CardData>();
		private LevelScreen _levelScreen;
		private int _currentCardIndex;
		private int _draggedCardIndex;
		public GameController(IGameConfig gameConfig, ICardConfig cardConfig, Transform uiRoot, Camera uiCamera)
		{
			_gameConfig = gameConfig;
			_cardConfig = cardConfig;
			_uiRoot = uiRoot;
			_uiCamera = uiCamera;

			_cardGenerator = new CardGeneratorMechanic();
		}

		public void StartGame()
		{
			var cardCountRange = _gameConfig.GetCardCountRange();
			var cardCount = Random.Range(cardCountRange.MinValue, cardCountRange.MaxValue);

			_cardDataList = _cardGenerator.GenerateCards(cardCount, _cardConfig);

			_levelScreen = CreateLevelScreen();
			_levelScreen.Init(_gameConfig);
			_levelScreen.SetData(_cardDataList);
			_levelScreen.PlayButtonClickEvent += OnPlayButtonClick;
			_levelScreen.ExitButtonClickEvent += OnExitButtonClick;
			_levelScreen.StartDragEvent += OnCardStartDrag;
			_levelScreen.EndDragEvent += OnCardEndDrag;
		}

		#region Event Handlers

		private void OnPlayButtonClick()
		{
			if (_cardDataList.Count == 0)
				return;

			if (_currentCardIndex >= _cardDataList.Count)
				_currentCardIndex = 0;

			ChangeCardData(_currentCardIndex);

			_currentCardIndex++;
		}

		private void OnExitButtonClick()
		{
			Application.Quit();
		}

		private void OnCardStartDrag(int draggedCardIndex)
		{
			_draggedCardIndex = draggedCardIndex;
		}

		private void OnCardEndDrag(PointerEventData eventData, RectTransform dropZone)
		{
			if (RectTransformUtility.RectangleContainsScreenPoint(dropZone, eventData.position, _uiCamera))
			{
				if (_draggedCardIndex != -1)
				{
					var cardData = _cardDataList[_draggedCardIndex];
					_cardDataList.RemoveAt(_draggedCardIndex);
					_levelScreen.DropCard(_draggedCardIndex, cardData);
				}
			}

			_draggedCardIndex = -1;
		}

		#endregion

		private LevelScreen CreateLevelScreen()
		{
			var levelScreenObject = GameObject.Instantiate(_gameConfig.GetLevelScreen(), _uiRoot);
			levelScreenObject.transform.localPosition = Vector3.zero;
			levelScreenObject.transform.localRotation = Quaternion.identity;
			var levelScreen = levelScreenObject.GetComponent<LevelScreen>();

			return levelScreen;
		}

		private void ChangeCardData(int currentCardIndex)
		{
			var cardData = _cardDataList[currentCardIndex];

			cardData.Attack = _gameConfig.GetCounterPlayRange().GetRandom();
			cardData.Health = _gameConfig.GetCounterPlayRange().GetRandom();
			cardData.Mana = _gameConfig.GetCounterPlayRange().GetRandom();

			if (cardData.IsDead())
				_levelScreen.ChangeCardData(currentCardIndex, cardData, () => { PlayDeath(currentCardIndex); });
			else
				_levelScreen.ChangeCardData(currentCardIndex, cardData, null);
		}

		private void PlayDeath(int deadCardIndex)
		{
			_cardDataList.RemoveAt(deadCardIndex);
			_levelScreen.PlayDeath(deadCardIndex);
		}
	}
}
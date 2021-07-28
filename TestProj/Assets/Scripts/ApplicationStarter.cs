using Configs;
using GameControllers;
using UnityEngine;

public class ApplicationStarter : MonoBehaviour
{
    [SerializeField]
    private GameConfig _gameConfig;
    [SerializeField]
    private CardConfig _cardConfig;

    [SerializeField]
    private Transform _uiRoot;

    [SerializeField]
    private Camera _uiCamera;

    private IGameController _gameController;

    public void Start()
    {
        _gameController = new GameController(_gameConfig, _cardConfig, _uiRoot, _uiCamera);
        _gameController.StartGame();
    }
}

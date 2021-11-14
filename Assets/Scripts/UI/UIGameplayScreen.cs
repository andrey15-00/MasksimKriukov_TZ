using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGame
{
    public class UIGameplayScreen : UIScreen
    {
        [SerializeField] private Button _pauseGame;
        [SerializeField] private TMP_Text _currentScore;
        [SerializeField] private TMP_Text _bestScore;
        [SerializeField] private TMP_Text _life;
        [SerializeField] private EventDispatcher _eventListener;
        [SerializeField] private DataHandler _dataHandler;

        private void Awake()
        {
            _pauseGame.onClick.AddListener(OnPauseClicked);

            _eventListener.Subscribe<GameUnpausedMessage>(OnGameUnpaused);
            _eventListener.Subscribe<GameStartedMessage>(OnGameStarted);

            _dataHandler.SubscribeDataChanged<CurrentScore>(OnCurrentScoreChanged);
            _dataHandler.SubscribeDataChanged<BestScore>(OnBestScoreChanged);
            _dataHandler.SubscribeDataChanged<CurrentLife>(OnLifeChanged);
        }

        private void OnGameUnpaused(GameUnpausedMessage message)
        {
            Show();
        }

        private void OnGameStarted(GameStartedMessage message)
        {
            Show();
        }

        private void OnCurrentScoreChanged(CurrentScore data)
        {
            _currentScore.text = string.Format("Current score: {0}", data.value.ToString());
        }

        private void OnBestScoreChanged(BestScore data)
        {
            _bestScore.text = string.Format("Best score: {0}", data.value.ToString());
        }

        private void OnLifeChanged(CurrentLife data)
        {
            _life.text = string.Format("Life: {0}", Mathf.Max(0, data.value).ToString());

            if(data.value <= 0)
            {
                _eventListener.Publish(new GameDefeatMessage());
            }
        }

        private void OnPauseClicked()
        {
            _eventListener.Publish(new GamePausedMessage());
        }
    }
}


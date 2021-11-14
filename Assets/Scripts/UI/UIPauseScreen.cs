using UnityEngine;
using UnityEngine.UI;

namespace UnityGame
{
    public class UIPauseScreen : UIScreen
    {
        [SerializeField] private Button _restartGame;
        [SerializeField] private Button _continueGame;
        [SerializeField] private EventDispatcher _eventListener;

        private void Awake()
        {
            _restartGame.onClick.AddListener(OnRestartClicked);
            _continueGame.onClick.AddListener(OnContinueClicked);

            _eventListener.Subscribe<GamePausedMessage>(OnGamePaused);
        }

        private void OnGamePaused(GamePausedMessage message)
        {
            Show();
        }

        private void OnRestartClicked()
        {
            _eventListener.Publish(new GameStartedMessage());
            Hide();
        }

        private void OnContinueClicked()
        {
            _eventListener.Publish(new GameUnpausedMessage());
            Hide();
        }
    }
}


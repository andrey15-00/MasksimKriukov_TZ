using UnityEngine;
using UnityEngine.UI;

namespace UnityGame
{
    public class UIDefeatScreen : UIScreen
    {
        [SerializeField] private Button _restartGame;
        [SerializeField] private EventDispatcher _eventListener;

        private void Awake()
        {
            _restartGame.onClick.AddListener(OnRestartClicked);

            _eventListener.Subscribe<GameDefeatMessage>(OnDefeat);
        }

        private void OnDefeat(GameDefeatMessage message)
        {
            Show();
        }

        private void OnRestartClicked()
        {
            _eventListener.Publish(new GameStartedMessage());
            Hide();
        }
    }
}


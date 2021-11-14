using UnityEngine;

namespace UnityGame
{
    public class GameplayStarter : MonoBehaviour
    {
        [SerializeField] private EventDispatcher _eventListener;

        private void Start()
        {
            _eventListener.Publish(new GameStartedMessage());
        }
    }
}


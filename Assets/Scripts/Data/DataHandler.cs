using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGame
{
    public class DataHandler : MonoBehaviour
    {
        [SerializeField] private EventDispatcher _eventListener;
        private DataStorage _dataStorage = new DataStorage();
        private BestScore _bestScore;
        private CurrentScore _currentScore;
        private CurrentLife _currentLife;
        private Dictionary<Type, List<Action<IData>>> _dataChangedSubscribers = new Dictionary<Type, List<Action<IData>>>();

        private void Awake()
        {
            _bestScore = _dataStorage.Get<BestScore>();
            _currentScore = _dataStorage.Get<CurrentScore>();
            _currentLife = _dataStorage.Get<CurrentLife>();

            _eventListener.Subscribe<BallDestroyedMessage>(OnBallDestroyed);
            _eventListener.Subscribe<GameStartedMessage>(OnGameStarted);
        }

        private void OnGameStarted(GameStartedMessage message)
        {
            _currentScore.value = 0;
            _currentLife.value = 500;
            PublishDataChanged(_currentScore);
            PublishDataChanged(_bestScore);
            PublishDataChanged(_currentLife);
        }

        private void OnBallDestroyed(BallDestroyedMessage message)
        {
            if (message.destroyedByUser)
            {
                _currentScore.value += 1;
                _dataStorage.AddOrChange(_currentScore);
                PublishDataChanged(_currentScore);
                
                if(_currentScore.value > _bestScore.value)
                {
                    _bestScore.value = _currentScore.value;
                    _dataStorage.AddOrChange(_bestScore);
                    PublishDataChanged(_bestScore);
                }
            }
            else
            {
                _currentLife.value -= message.model.damage;
                _dataStorage.AddOrChange(_currentLife);
                PublishDataChanged(_currentLife);
            }
        }

        public void SubscribeDataChanged<T>(Action<T> callback) where T: IData
        {
            Type key = typeof(T);

            Action<IData> changed = (data) =>
            {
                callback.Invoke((T)data);
            };

            if (_dataChangedSubscribers.ContainsKey(key))
            {
                _dataChangedSubscribers[key].Add(changed);
            }
            else
            {
                _dataChangedSubscribers[key] = new List<Action<IData>>()
                {
                    changed
                };
            }
        }

        private void PublishDataChanged<T>(T data) where T: IData
        {
            Type key = typeof(T);
            List<Action<IData>> subscribers;
            if (_dataChangedSubscribers.TryGetValue(key, out subscribers))
            {
                foreach(var subscriber in subscribers)
                {
                    subscriber?.Invoke(data);
                }
            }
        }
    }
}


using System.Collections.Generic;
using UnityEngine;

namespace UnityGame
{
    public class BallSpawner : MonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField] private EventDispatcher _eventListener;
        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private Transform _spawnedBallsParent;
        [SerializeField] private float _spawnIntervalSeconds;
        [Header("Damage")]
        [SerializeField] private Vector2Int _minMaxDamage;
        [Header("Speed")]
        [SerializeField] private Vector2 _minMaxBaseSpeed;
        [SerializeField] private float _speedChangeIntervalSeconds;
        [SerializeField][Range(0,1)] private float _speedChangeProcents;
        private bool _spawning;
        private float _timePassedSinceLastSpawn;
        private float _timePassedSinceLastSpeedChange;
        private float _currentSpeedScale = 1f;
        private Dictionary<int, Ball> _spawnedBalls = new Dictionary<int, Ball>();


        private void Awake()
        {
            _eventListener.Subscribe<GamePausedMessage>(OnGamePaused);
            _eventListener.Subscribe<GameUnpausedMessage>(OnGameUnpaused);
            _eventListener.Subscribe<GameStartedMessage>(OnGameStarted);
            _eventListener.Subscribe<BallDestroyedMessage>(OnBallDestroyed);
            _eventListener.Subscribe<GameDefeatMessage>(Ondefeat);
        }

        private void OnGamePaused(GamePausedMessage message)
        {
            _spawning = false;
            FreezeBalls();
        }

        private void OnGameUnpaused(GameUnpausedMessage message)
        {
            _spawning = true;
            UnfreezeBalls();
        }

        private void Ondefeat(GameDefeatMessage message)
        {
            _spawning = false;
            FreezeBalls();
        }

        private void OnBallDestroyed(BallDestroyedMessage message)
        {
            _spawnedBalls.Remove(message.instanceId);
        }

        private void OnGameStarted(GameStartedMessage message)
        {
            _timePassedSinceLastSpawn = 0f;
            _timePassedSinceLastSpeedChange = 0f;
            _currentSpeedScale = 1f;
            RemoveBalls();
            _spawning = true;
        }

        private void Update()
        {
            if (_spawning)
            {
                float deltaTime = Time.deltaTime;

                _timePassedSinceLastSpeedChange += deltaTime;
                _timePassedSinceLastSpawn += deltaTime;

                if (_timePassedSinceLastSpawn >= _spawnIntervalSeconds)
                {
                    _timePassedSinceLastSpawn -= _spawnIntervalSeconds;
                    SpawnBall();
                }

                if (_timePassedSinceLastSpeedChange >= _speedChangeIntervalSeconds)
                {
                    _timePassedSinceLastSpeedChange -= _speedChangeIntervalSeconds;
                    _currentSpeedScale += _currentSpeedScale * _speedChangeProcents;
                }
            }
        }

        private void SpawnBall()
        {
            Ball ball = Instantiate(_ballPrefab, _spawnedBallsParent);
            float r = Random.Range(0f, 1f);
            float g = Random.Range(0f, 1f);
            float b = Random.Range(0f, 1f);
            BallModel model = new BallModel()
            {
                color = new Color(r, g, b),
                damage = Random.Range(_minMaxDamage.x, _minMaxDamage.y),
                speed = Random.Range(_minMaxBaseSpeed.x, _minMaxBaseSpeed.y) * _currentSpeedScale,
                position = new Vector2(Random.Range(50, Screen.width-50), 0f)
            };
            ball.Init(_eventListener, model);
            ball.StartFall();

            _spawnedBalls.Add(ball.GetInstanceID(), ball);
        }

        private void RemoveBalls()
        {
            foreach (var pair in _spawnedBalls)
            {
                if(pair.Value != null)
                {
                    Destroy(pair.Value.gameObject);
                }
            }
            _spawnedBalls.Clear();
        }

        private void FreezeBalls()
        {
            foreach(var pair in _spawnedBalls)
            {
                pair.Value.StopFall();
            }
        }

        private void UnfreezeBalls()
        {
            foreach (var pair in _spawnedBalls)
            {
                pair.Value.StartFall();
            }
        }
    }
}


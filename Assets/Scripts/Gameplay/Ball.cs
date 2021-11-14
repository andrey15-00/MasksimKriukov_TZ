using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityGame
{
    public class Ball : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] protected Image _body;
        [SerializeField] protected RectTransform _rect;
        [SerializeField] protected Image _explosion;
        [SerializeField] protected int _explosionDurationMilliseconds;
        private BallModel _model;
        private EventDispatcher _eventListener;
        private bool _falling = false;

        public void Init(EventDispatcher eventListener, BallModel model)
        {
            _eventListener = eventListener;
            _model = model;
            _body.color = model.color;
            _explosion.color = model.color;
            _rect.anchoredPosition = model.position;
        }

        public void StartFall()
        {
            _falling = true;
        }

        public void StopFall()
        {
            _falling = false;
        }

        private void Update()
        {
            if (!_falling)
            {
                return;
            }

            _rect.anchoredPosition += new Vector2(0, Time.deltaTime * -_model.speed);

            if (_rect.anchoredPosition.y <= -Screen.height)
            {
                OnNeedDestroy(false);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnNeedDestroy(true);
        }

        private async void OnNeedDestroy(bool destroyedByUser)
        {
            if (!_falling)
            {
                return;
            }

            _falling = false;

            _eventListener.Publish(new BallDestroyedMessage(_model, destroyedByUser, GetInstanceID()));

            _body.gameObject.SetActive(false);
            _explosion.gameObject.SetActive(true);

            await Task.Delay(_explosionDurationMilliseconds);

            if (this != null)
            {
                Destroy(gameObject);
            }
        }
    }
}


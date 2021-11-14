using UnityEngine;

namespace UnityGame
{
    public class UIScreen : MonoBehaviour
    {
        [SerializeField] protected GameObject _visual;

        protected void Show()
        {
            _visual.SetActive(true);
        }

        protected void Hide()
        {
            _visual.SetActive(false);
        }
    }
}


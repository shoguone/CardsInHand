using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Game
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CardDragDrop : MonoBehaviour,
        IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private RectTransform _cardTrf;
        private CanvasGroup _canvasGroup;
        private Canvas _canvas;

        private Vector2 _startingAnchoredPosition;
        private bool _wasDropped = false;

        private void Awake()
        {
            _cardTrf = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();

            if (_canvas == null)
            {
                _canvas = GetComponentInParent<Canvas>();
                if (_canvas == null)
                {
                    Debug.LogWarning($"{nameof(_canvas)} is not set!");
                    enabled = false;
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startingAnchoredPosition = _cardTrf.anchoredPosition;
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _cardTrf.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            if (!_wasDropped)
            {
                _cardTrf.anchoredPosition = _startingAnchoredPosition;
            }
        }

        public void Drop()
        {
            _wasDropped = true;
        }
    }
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardsInHand.Scripts.Game
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CardDragDrop : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler,
        IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private RectTransform _cardTrf;
        private CanvasGroup _canvasGroup;
        private Canvas _canvas;

        private Vector2 _startingAnchoredPosition;
        private float _startingRotation;
        private bool _wasDropped = false;
        private bool _wasReleased = false;

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
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _cardTrf.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ReleaseCard();
        }

        public void Drop()
        {
            _wasDropped = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _wasReleased = false;

            _startingAnchoredPosition = _cardTrf.anchoredPosition;
            _startingRotation = _cardTrf.rotation.eulerAngles.z;
            _cardTrf.DORotateQuaternion(Quaternion.identity, .5f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ReleaseCard();
        }

        private void ReleaseCard()
        {
            if (_wasReleased)
            {
                return;
            }

            _wasReleased = true;
            _canvasGroup.blocksRaycasts = true;
            if (!_wasDropped)
            {
                //_cardTrf.anchoredPosition = _startingAnchoredPosition;
                DOTween.Sequence()
                    .Append(_cardTrf.DOAnchorPos(_startingAnchoredPosition, .5f))
                    .Join(_cardTrf.DORotateQuaternion(Quaternion.Euler(0, 0, _startingRotation), .5f));
            }
        }
    }
}

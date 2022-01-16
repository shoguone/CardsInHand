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
        private BackgroundGlow _cardGlow;
        private RectTransform _cardTrf;
        private CanvasGroup _canvasGroup;
        private Canvas _canvas;

        private Vector2 _startingAnchoredPosition;
        private float _startingRotation;
        private bool _isDragging = false;

        private void Awake()
        {
            _cardGlow = GetComponent<BackgroundGlow>();
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
            _isDragging = true;
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _cardTrf.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            ReleaseCard();
        }

        public void DropToTable()
        {
            _isDragging = false;
            ReleaseCard(false);
            enabled = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _startingAnchoredPosition = _cardTrf.anchoredPosition;
            _startingRotation = _cardTrf.rotation.eulerAngles.z;
            _cardTrf.DORotateQuaternion(Quaternion.identity, .5f);

            _cardGlow.Glow();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isDragging)
            {
                ReleaseCard();
            }
        }

        private void ReleaseCard(bool animate = true)
        {
            _canvasGroup.blocksRaycasts = true;
            if (animate)
            {
                DOTween.Sequence()
                    .Append(_cardTrf.DOAnchorPos(_startingAnchoredPosition, .5f))
                    .Join(_cardTrf.DORotateQuaternion(Quaternion.Euler(0, 0, _startingRotation), .5f));
            }

            _cardGlow.Glow(false);
        }
    }
}

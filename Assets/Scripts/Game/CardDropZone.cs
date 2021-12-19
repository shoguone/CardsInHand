using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Game
{
    public class CardDropZone : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
            {
                return;
            }

            var cardGo = eventData.pointerDrag;
            var cardDrag = cardGo.GetComponent<CardDragDrop>();
            if (cardDrag != null)
            {
                cardDrag.enabled = false;
            }

            var cardRectTrf = cardGo.GetComponent<RectTransform>();
            if (cardRectTrf == null)
            {
                return;
            }

            var rectTransform = GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                return;
            }

            cardRectTrf.SetParent(rectTransform);
            cardRectTrf.rotation = Quaternion.Euler(Vector3.zero);
        }
    }
}

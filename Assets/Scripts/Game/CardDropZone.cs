using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardsInHand.Scripts.Game
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
                cardDrag.DropToTable();
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
            cardRectTrf.DORotateQuaternion(Quaternion.identity, .5f);
        }
    }
}
